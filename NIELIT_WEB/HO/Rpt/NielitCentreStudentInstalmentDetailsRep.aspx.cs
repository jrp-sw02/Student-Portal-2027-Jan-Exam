using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.Utils.Data;
using System.Data;
using EConnect.NIELIT;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Transactions;
using EConnect.Utils.Common;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Data.OleDb;
using System.IO;

public partial class ReportPgae : BasePage
{   
    Table tbl = new Table();  
    StringBuilder str = new StringBuilder();
    Int32 currentRoleId = 0;
    Int64 InstituteID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/Rpt/NielitCentreStudentInstalmentDetailsRepFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }          
            if (!Page.IsPostBack)
            {
                tbl.CssClass = "sample3";
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(100);
                ShowData();
                divReportData.Controls.Add(tbl);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
     
    protected void ShowTableHeader()
    {
        try
        {
           int CourseCatId = Convert.ToInt32(Request.QueryString["CourseCatId"]);
           Int32 CourseType = CourseCatId.ToString().Length;
           Int64 CourseId = Convert.ToInt64(Request.QueryString["CourseId"]);
           Int64 BatchId = Convert.ToInt64(Request.QueryString["BatchId"]); 
            String CourseName ="",batchcodeName="",feeDetails="";
            DateTime BatchFromDate = Convert.ToDateTime("01-01-1900");
            DateTime BatchToDate = Convert.ToDateTime("01-01-1900");
            using (EConnectContext context = new EConnectContext())
            {
                if (CourseType <= 2)
                {
                    if (CourseCatId != 0)
                    {
                        Course cName = context.Courses.Where(s => s.ID == CourseId).FirstOrDefault();
                        CourseName = cName.Name; 
                    }
                } 
                else
                {
                    NIELITMISContext context1 = new NIELITMISContext();
                    if (CourseCatId != 0)
                    {
                        NielitCentreCourse cName = context1.NielitCentreCourses.Where(s => s.ID == CourseId).FirstOrDefault();
                        CourseName = cName.Name;
                        NielitCentreBatch batchcode = context1.NielitCentreBatchs.Where(s => s.ID == BatchId).FirstOrDefault();
                        BatchFromDate = batchcode.startDate;
                        BatchToDate = batchcode.endDate;
                        batchcodeName = batchcode.BatchCode;
                    }
                }
            }
            using (DataTable dt = GetFeeDetailsBatchIdWise(InstituteID, CourseId, BatchId,0))
            {
                if (dt.Rows.Count > 0)
                {
                    var fdetails = (from a in dt.AsEnumerable()
                                    select new
                                    {
                                        description = a.Field<string>("description"),
                                        feeAmount = a.Field<int>("feeAmount")
                                    }).ToList();
                    if (fdetails.Count() > 0)
                    {
                        foreach (var fd in fdetails)
                        {
                            if (feeDetails == "")
                            {
                                feeDetails = fd.description.ToString() + " : " + fd.feeAmount.ToString() +"/-";
                            }
                            else
                            {
                                feeDetails += " <br/>" + fd.description.ToString() + " : " + fd.feeAmount.ToString() + "/-";
                            }
                        }

                    }
                }
            }
            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);

            TableHeaderRow th1 = new TableHeaderRow();
            tc1.ColumnSpan = 6;
            tc1.Text = "Report of Instalment details of Students for Batch  ( " + batchcodeName + "  )";
            tc1.HorizontalAlign = HorizontalAlign.Center;
            tc1.Font.Bold = true;
            th1.Cells.Add(tc1);           
            tbl.Rows.Add(th1);

            TableHeaderCell tc2 = new TableHeaderCell();
            tc2.Width = Unit.Percentage(100);
            TableHeaderRow th2 = new TableHeaderRow();
            tc2.ColumnSpan = 6;
            tc2.Text = " <left> Course : " + CourseName + "  <br/> Batch Start Date : " + BatchFromDate.ToString("dd-MMM-yyyy") + "   &    Batch End Date : " + BatchToDate.ToString("dd-MMM-yyyy") + "  <br/>  Details of  Fee to be Paid :  " + feeDetails + "<left/>";
            
            tc2.HorizontalAlign = HorizontalAlign.Left;
            th2.Cells.Add(tc2);
            tbl.Rows.Add(th2);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";
            TableHeaderCell tc = new TableHeaderCell();
            tc.Width = Unit.Percentage(1);
            tc.Text = "<b>#</b>";
            tc.HorizontalAlign = HorizontalAlign.Left;
            th.Cells.Add(tc);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(6);
            tcCol1.Text = "<b>Online Reference Number</b>";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(8);
            tcCol2.Text = "<b>Name</b>";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(5);
            tcCol3.Text = "<b>Father/Guardian Name</b>";
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(5);
            tcCol4.Text = "<b>Mother Name</b>";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(15);
            tcCol7.Text = "<b>Installments Details</b>";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowData()
    {
        try
        {
            User objUser;
            Int32 loginUserNo = 0;
            //Int64 InstituteID = 0;
            lblError.Visible = false;
            string strHead = "";
            
            NIELITMISContext context = new NIELITMISContext();
            string InstituteIDd = Request.QueryString["InstId"];
            if (InstituteIDd == "undefined")
            {
                using (EConnectContext context1 = new EConnectContext())
                {
                    loginUserNo = Convert.ToInt32(Session["UserID"]);
                    objUser = new EConnect.URM.User();
                    User loginUser = context1.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    Int64 NielitCentreId = Convert.ToInt32(loginUser.UserRefNumber);
                    InstituteID = NielitCentreId;
                }
            }
            else
            {
                 InstituteID = Convert.ToInt64(Request.QueryString["InstId"]);
            }
            Int64 CourseId = Convert.ToInt64(Request.QueryString["CourseId"]);
            int CourseCatId = Convert.ToInt32(Request.QueryString["CourseCatId"]);           
            Int64 BatchId = Convert.ToInt64(Request.QueryString["BatchId"]); 
            StringBuilder mySql = new StringBuilder();

            using (DataTable dt = GetInstallmentStudentData(InstituteID, CourseId, BatchId,0))
            {
                if (dt.Rows.Count > 0)
                {
                    var application = (from a in dt.AsEnumerable()
                                       select new
                                       {
                                           studentID= a.Field<Int64>("studenid"),
                                           OnlineReferenceNumber = a.Field<string>("OnlineReferenceNumber"),
                                           StudentName = a.Field<string>("StudentName"),
                                           Fathername = a.Field<string>("Fathername"),
                                           Guardianname = a.Field<string>("Guardianname"),
                                           Mothername = a.Field<string>("Mothername"),
                                           batchID = a.Field<Int64>("batchID")                                         
                                       }).ToList();
                   
                    if (application.Count() > 0)
                    {
                        ShowTableHeader();                      
                        int i = 1;
                        foreach (var app in application)
                        {
                            TableRow tr = new TableRow();
                            if (i % 2 == 0)
                                tr.CssClass = "gdalternate1";
                            else
                                tr.CssClass = "gdrow1";

                            TableCell tdRow = new TableCell();
                            tdRow.Width = Unit.Percentage(1);
                            tdRow.Text = i.ToString();
                            tdRow.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow);

                            TableCell tdRow2 = new TableCell();
                            tdRow2.Width = Unit.Percentage(1);
                            NielitCentreStudent NielitCentreStudentRefNo = context.NielitCentreStudent.Where(s => s.Number == app.OnlineReferenceNumber).FirstOrDefault();
                            if (NielitCentreStudentRefNo != null)
                                tdRow2.Text = NielitCentreStudentRefNo.Number.ToString();
                            else
                                tdRow2.Text = "";
                            tdRow2.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow2);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(1);
                            tdRow3.Text = app.StudentName.ToString();
                            tdRow3.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow3);

                            if (string.IsNullOrEmpty(app.Guardianname) == true && string.IsNullOrWhiteSpace(app.Guardianname) == true)
                            {
                                TableCell tdRow45 = new TableCell();
                                tdRow45.Width = Unit.Percentage(8);
                                if (app.Fathername != null)
                                    tdRow45.Text = app.Fathername.ToUpper();
                                tdRow45.HorizontalAlign = HorizontalAlign.Left;
                                tdRow45.Wrap = false;
                                tr.Cells.Add(tdRow45);

                                TableCell tdRow35 = new TableCell();
                                tdRow35.Width = Unit.Percentage(8);
                                if (app.Mothername != null)
                                    tdRow35.Text = app.Mothername.ToUpper();
                                tdRow35.HorizontalAlign = HorizontalAlign.Left;
                                tdRow35.Wrap = false;
                                tr.Cells.Add(tdRow35);
                            }
                            else
                            {
                                TableCell tdRow45 = new TableCell();
                                tdRow45.Width = Unit.Percentage(8);
                                if (app.Guardianname != null)
                                    tdRow45.Text = app.Guardianname.ToUpper();
                                tdRow45.HorizontalAlign = HorizontalAlign.Left;
                                tdRow45.Wrap = false;
                                tr.Cells.Add(tdRow45);

                                TableCell tdRow46 = new TableCell();
                                tdRow46.Width = Unit.Percentage(8);
                                tdRow46.Text = "NA";
                                tdRow46.HorizontalAlign = HorizontalAlign.Left;
                                tdRow46.Wrap = false;
                                tr.Cells.Add(tdRow46);
                            }
                            TableCell tdRow7 = new TableCell();
                            tdRow7.Width = Unit.Percentage(8);
                           // tdRow7.Text = "NA";
                            tdRow7.HorizontalAlign = HorizontalAlign.Left;
                            tdRow7.Wrap = false;
                           // tr.Cells.Add(tdRow7);

                            Int64 stID = 0, batchID=0;
                            string descriptionF = "", descriptionF1 = "", lastAmountPaid = "", InstallmentNumber = "", LastAmtRecieved = "", descridescriptionForLastAmtRec = "", LastInstallmentsAmount = "",  AmountPaid = "", Pinst = "";
                            string DefRc = "", DefRc1 = "", DefRc2 = "", descridescriptionForLastAmtRec1 = "";
                            string paymentdate = "",paymentdates = "";
                            stID = app.studentID;
                            batchID = app.batchID;                           
                            Int64 feetypeid = 0;
                           
                            using (DataTable dt1 = GetInstallmentDetailsBatchIdStudentIdWise(1, 1, batchID, stID))
                            {
                                int slno = 1;
                                Int32 lastAmount1 = 0, lastAmount2 = 0, lastAmount3 = 0;
                                if (dt1.Rows.Count > 0)
                                {
                                    int j;
                                    for (j = 0; j < dt1.Rows.Count; j++)
                                    {
                                        InstallmentNumber = dt1.Rows[j].Field<string>("InstallmentsNumber").ToString();
                                        lastAmountPaid = dt1.Rows[j].Field<int>("LastAmtPaid").ToString();
                                        AmountPaid = dt1.Rows[j].Field<int>("AmtPaid").ToString();
                                        descriptionF = dt1.Rows[j].Field<string>("description");
                                        descriptionF1 = dt1.Rows[j].Field<string>("description");
                                        feetypeid = dt1.Rows[j].Field<Int64>("feeTypeID");                                       
                                        paymentdate = dt1.Rows[j].Field<DateTime>("paymentDate").ToString("dd-MMM-yyyy");
                                      
                                        if (descriptionF != DefRc)
                                        {    // For total fee received amount batchwise and student id wise
                                            using (DataTable dt3 = GetTotalPaymentAmountView(feetypeid, stID, batchID))
                                            {
                                                if (dt3.Rows.Count > 0)
                                                {
                                                    int m;
                                                    descridescriptionForLastAmtRec1 = "";
                                                    for (m = 0; m < dt3.Rows.Count; m++)
                                                    {
                                                        LastAmtRecieved = dt3.Rows[m].Field<int>("TotalAmtRecieved").ToString();                                                        
                                                        DefRc2 = dt3.Rows[m].Field<string>("description").ToString();
                                                        descridescriptionForLastAmtRec1 += DefRc2 + " :  " + LastAmtRecieved + "/-  "  + "<br>";
                                                     }
                                                 }
                                             }
                                         }

                                         if (descriptionF != DefRc)
                                         {   // For Current Installments batchwise and student id wise
                                             using (DataTable dt2 = GetTotalPaymentAmountByInstallaments(feetypeid, stID, batchID))
                                                {
                                                    if (dt2.Rows.Count > 0)
                                                    {
                                                        int m;
                                                        for (m = 0; m < dt2.Rows.Count; m++)
                                                        {                                                           
                                                            LastInstallmentsAmount = dt2.Rows[m].Field<int>("LastInstallmentsAmount").ToString();
                                                            paymentdates = dt2.Rows[m].Field<DateTime>("paymentDate").ToString("dd-MMM-yyyy");
                                                            DefRc = dt2.Rows[m].Field<string>("description").ToString();
                                                            descridescriptionForLastAmtRec += DefRc + " :  " + LastInstallmentsAmount + "/-  " + paymentdates + "<br>";
                                                        }
                                                    }                                                   
                                                }
                                            }
                                        
                                        if (j == 0)
                                        {
                                            if (InstallmentNumber != "0")
                                            {                                               
                                                      tdRow7.Text = tdRow7.Text + "<br>" + " <b> # " + slno.ToString() + "</b>" + "  " + descriptionF.ToString() + "  " + lastAmountPaid.ToString() + "/-" + "  " + paymentdate.ToString();                                                                                                  
                                            }
                                        }
                                        else
                                        {       
                                            if (InstallmentNumber != "0")
                                            {
                                                if (descriptionF == DefRc1)
                                                {
                                                    lastAmount2 = Convert.ToInt32(lastAmountPaid);
                                                    lastAmount3 = lastAmount2 - lastAmount1;

                                                    slno =slno+ 1;
                                                    tdRow7.Text = tdRow7.Text + "<br> <b> # " + slno.ToString() + "</b>" + "  " + descriptionF.ToString() + "  " + lastAmount3.ToString() + "/-" + "  " + paymentdate.ToString();
                                                    lastAmount1 = Convert.ToInt32(lastAmountPaid);
                                                }
                                                else
                                                {
                                                    lastAmount1 = 0; lastAmount2 = 0; lastAmount3 = 0;
                                                    slno = 1;
                                                    Pinst = "Previous Installments :";
                                                    tdRow7.Text = tdRow7.Text + "<br> <b> # " + slno.ToString() + "</b>" + "  " + descriptionF.ToString() + "  " + lastAmountPaid.ToString() + "/-" + "  " + paymentdate.ToString();
                                                    lastAmount1 = Convert.ToInt32(lastAmountPaid);
                                                }                                               
                                            }
                                        }
                                        DefRc1 = dt1.Rows[j].Field<string>("description");
                                        DefRc2 = dt1.Rows[j].Field<string>("description");
                                    }
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                  tdRow7.Text = "<b> " + "Total Fee Received :" + "</b>" + " <br> " + descridescriptionForLastAmtRec1 + "<br>" +  "<b> " + "Current Installments :" + "</b>" + "<br> " + descridescriptionForLastAmtRec  + "<br>" + "<b>" + Pinst + "</b>" + tdRow7.Text ;
                                }
                                else
                                { tdRow7.Text = ""; }
                            }
                            tdRow7.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow7); // Installments details student id wise and batch id  add  row record

                            tbl.Rows.Add(tr);
                            i++;
                        } 
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }                    
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "No Record Found";
                }
            } 
            LblRptSubHeader.Text = strHead;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally { 
        }
    }
    public DataTable GetInstallmentStudentData(Int64 InstituteID, Int64 CourseId, Int64 BatchId, Int64 stID)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetFeeInstallmentsDetailsStudentIDWise", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;                
                cmd.Parameters.Add(new SqlParameter("@InstituteID", SqlDbType.BigInt));
                cmd.Parameters["@InstituteID"].Value = InstituteID;
                cmd.Parameters.Add(new SqlParameter("@CourseId", SqlDbType.BigInt));
                cmd.Parameters["@CourseId"].Value = CourseId;
                cmd.Parameters.Add(new SqlParameter("@BatchId", SqlDbType.BigInt));
                cmd.Parameters["@BatchId"].Value = BatchId;
                cmd.Parameters.Add(new SqlParameter("@feeTypeID", SqlDbType.BigInt));
                cmd.Parameters["@feeTypeID"].Value = 1;
                cmd.Parameters.Add(new SqlParameter("@stID", SqlDbType.BigInt));
                cmd.Parameters["@stID"].Value = 1;
                cmd.Parameters.Add(new SqlParameter("@View", SqlDbType.Int));
                cmd.Parameters["@View"].Value = 2;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
    public DataTable GetTotalPaymentAmountByInstallaments( Int64 FeetypeId, Int64 stID, Int64 BatchId)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetFeeInstallmentsDetailsStudentIDWise", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@InstituteID", SqlDbType.BigInt));
                cmd.Parameters["@InstituteID"].Value = 1;
                cmd.Parameters.Add(new SqlParameter("@CourseId", SqlDbType.BigInt));
                cmd.Parameters["@CourseId"].Value = 1;
                cmd.Parameters.Add(new SqlParameter("@BatchId", SqlDbType.BigInt));
                cmd.Parameters["@BatchId"].Value = BatchId;
                cmd.Parameters.Add(new SqlParameter("@feeTypeID", SqlDbType.BigInt));
                cmd.Parameters["@feeTypeID"].Value = FeetypeId;
                cmd.Parameters.Add(new SqlParameter("@stID", SqlDbType.BigInt));
                cmd.Parameters["@stID"].Value = stID;
                cmd.Parameters.Add(new SqlParameter("@View", SqlDbType.Int));
                cmd.Parameters["@View"].Value = 5;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
    public DataTable GetTotalPaymentAmountView(Int64 FeetypeId, Int64 stID, Int64 BatchId)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetFeeInstallmentsDetailsStudentIDWise", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@InstituteID", SqlDbType.BigInt));
                cmd.Parameters["@InstituteID"].Value = 1;
                cmd.Parameters.Add(new SqlParameter("@CourseId", SqlDbType.BigInt));
                cmd.Parameters["@CourseId"].Value = 1;
                cmd.Parameters.Add(new SqlParameter("@BatchId", SqlDbType.BigInt));
                cmd.Parameters["@BatchId"].Value = BatchId;
                cmd.Parameters.Add(new SqlParameter("@feeTypeID", SqlDbType.BigInt));
                cmd.Parameters["@feeTypeID"].Value = FeetypeId;
                cmd.Parameters.Add(new SqlParameter("@stID", SqlDbType.BigInt));
                cmd.Parameters["@stID"].Value = stID;
                cmd.Parameters.Add(new SqlParameter("@View", SqlDbType.Int));
                cmd.Parameters["@View"].Value = 6;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
    public DataTable GetFeeDetailsBatchIdWise(Int64 InstituteID, Int64 CourseId, Int64 BatchId, Int64 stID)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetFeeInstallmentsDetailsStudentIDWise", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@InstituteID", SqlDbType.BigInt));
                cmd.Parameters["@InstituteID"].Value = InstituteID;
                cmd.Parameters.Add(new SqlParameter("@CourseId", SqlDbType.BigInt));
                cmd.Parameters["@CourseId"].Value = CourseId;
                cmd.Parameters.Add(new SqlParameter("@BatchId", SqlDbType.BigInt));
                cmd.Parameters["@BatchId"].Value = BatchId;
                cmd.Parameters.Add(new SqlParameter("@feeTypeID", SqlDbType.BigInt));
                cmd.Parameters["@feeTypeID"].Value = 1;
                cmd.Parameters.Add(new SqlParameter("@stID", SqlDbType.BigInt));
                cmd.Parameters["@stID"].Value = 1;
                cmd.Parameters.Add(new SqlParameter("@View", SqlDbType.Int));
                cmd.Parameters["@View"].Value = 1;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }

    public DataTable GetInstallmentDetailsBatchIdStudentIdWise(Int64 InstituteID, Int64 CourseId, Int64 BatchId, Int64  stID)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetFeeInstallmentsDetailsStudentIDWise", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@InstituteID", SqlDbType.BigInt));
                cmd.Parameters["@InstituteID"].Value = InstituteID;
                cmd.Parameters.Add(new SqlParameter("@CourseId", SqlDbType.BigInt));
                cmd.Parameters["@CourseId"].Value = CourseId;
                cmd.Parameters.Add(new SqlParameter("@BatchId", SqlDbType.BigInt));
                cmd.Parameters["@BatchId"].Value = BatchId;
                cmd.Parameters.Add(new SqlParameter("@feeTypeID", SqlDbType.BigInt));
                cmd.Parameters["@feeTypeID"].Value = 1;
                cmd.Parameters.Add(new SqlParameter("@stID", SqlDbType.BigInt));
                cmd.Parameters["@stID"].Value = stID;
                cmd.Parameters.Add(new SqlParameter("@View", SqlDbType.Int));
                cmd.Parameters["@View"].Value = 3;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
    protected void imgPDF_Click(object sender, ImageClickEventArgs e)
    {  
        try
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            hw.AddAttribute("border", "1");
            hw.RenderBeginTag(HtmlTextWriterTag.Font);
            hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "9");
            divReportData.Visible = true;
            tbl.BorderStyle = BorderStyle.Solid;
            tbl.CssClass = "sample3";
            tbl.CellPadding = 2;
            tbl.CellSpacing = 1;
            tbl.Width = Unit.Percentage(200);
            ShowData();
            divReportData.Controls.Add(tbl);
            divReportData.RenderControl(hw);
            hw.RenderEndTag();
            Response.Clear();
            StringReader sr = new StringReader(sw.ToString());
             Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);
           // Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=StudentBatchWiseReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
            divReportData.Visible = true;
            ShowData();
            divReportData.Controls.Add(tbl);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=StudentBatchWiseReport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            htmlWrite = new Html32TextWriter(StringWrite);
            divReportData.RenderControl(htmlWrite);
            Response.Write(StringWrite.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}