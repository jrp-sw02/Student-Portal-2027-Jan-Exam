using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.OleDb;
using System.IO;
using EConnect.Utils.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;

public partial class HO_Rpt_StatisticsReportProjectWise : BasePage
{
    Table tbl = new Table(); 
    UserType loginUserType; 
    Int32 currentRoleId = 0;  

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/StatisticsReportFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            if (!Page.IsPostBack)
            {
                 if ((!string.IsNullOrEmpty(Decrypt(HttpUtility.UrlDecode(Request.QueryString["DateFrom"])))) && (!string.IsNullOrEmpty(Decrypt(HttpUtility.UrlDecode(Request.QueryString["DateTo"])))) && (!string.IsNullOrEmpty(Decrypt(HttpUtility.UrlDecode(Request.QueryString["projectID"])))))

                {
                    tbl.CssClass = "sample3";
                    tbl.CellPadding = 2;
                    tbl.CellSpacing = 1;
                    tbl.Width = Unit.Percentage(100);
                    ShowTableData();
                    divReportData.Controls.Add(tbl);
                }
                else
                {
                    Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Home.aspx")));
                    Response.End();
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowTableHeaderPDF()
    {
        try
        {
            DateTime datefrom, dateto;
            //datefrom = Convert.ToDateTime(Request.QueryString["DateFrom"]); 
            //dateto = Convert.ToDateTime(Request.QueryString["DateTo"]);
            datefrom = Convert.ToDateTime(Decrypt(HttpUtility.UrlDecode(Request.QueryString["DateFrom"])));
            dateto = Convert.ToDateTime(Decrypt(HttpUtility.UrlDecode(Request.QueryString["DateTo"])));
            using (EConnectContext context = new EConnectContext())
            {
                TableHeaderRow th5 = new TableHeaderRow();
                th5.CssClass = "head1";
                TableHeaderCell tcCol151 = new TableHeaderCell();
                tcCol151.Width = Unit.Percentage(19);
                tcCol151.Height = Unit.Pixel(24);
               // tcCol151.BackColor = System.Drawing.Color.Blue;
                tcCol151.HorizontalAlign = HorizontalAlign.Center;
                tcCol151.BorderWidth = 1;
                tcCol151.BorderColor = System.Drawing.Color.Chocolate;
                tcCol151.Text = "<b><b><b>National Institute of Electronics and Information Technology (NIELIT)</b></b> </b>";
                tcCol151.ColumnSpan = 10;
                th5.Cells.Add(tcCol151);

                tbl.Rows.Add(th5);

                TableHeaderRow th4 = new TableHeaderRow();
                th4.CssClass = "head1";
                TableHeaderCell tcCol141 = new TableHeaderCell();
                tcCol141.Width = Unit.Percentage(19);
                // tcCol141.Height = Unit.Pixel(14);
                tcCol141.HorizontalAlign = HorizontalAlign.Center;
                tcCol141.BorderWidth = 1;
                tcCol141.BorderColor = System.Drawing.Color.Chocolate;
                tcCol141.Text = "<b>Report Period : </b>" + datefrom.ToString("dd-MMM-yyyy") + " to " + dateto.ToString("dd-MMM-yyyy")
                  + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                  + "<b><b> Traninig Programme Wise Summary</b></b> "
                  + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; "
                  + "<b> Report Generated on:</b> " + DateTime.Now.ToString("dd-MMM-yyyy");
                tcCol141.ColumnSpan = 10;
                th4.Cells.Add(tcCol141);

                tbl.Rows.Add(th4);

                TableHeaderRow th3 = new TableHeaderRow();
                th3.CssClass = "head1";
                TableHeaderCell tcCol131 = new TableHeaderCell();
                tcCol131.Width = Unit.Percentage(19);
                tcCol131.Height = Unit.Pixel(34);
                tcCol131.BorderWidth = 1;
                tcCol131.BorderColor = System.Drawing.Color.Chocolate;
                tcCol131.Text = "&nbsp;";
                tcCol131.ColumnSpan = 10;
                th3.Cells.Add(tcCol131);

                tbl.Rows.Add(th3);

                TableHeaderRow th2 = new TableHeaderRow();
                th2.CssClass = "head1";
                
                //TableHeaderCell tc2 = new TableHeaderCell();
                //tc2.Width = Unit.Percentage(100);

                TableHeaderCell tcCol11 = new TableHeaderCell();
                tcCol11.Width = Unit.Percentage(1);
               tcCol11.RowSpan = 2;
               tcCol11.HorizontalAlign = HorizontalAlign.Center;
               tcCol11.Text = "S. No.";
               tcCol11.BorderWidth = 1;
               tcCol11.BorderColor = System.Drawing.Color.Chocolate;
                th2.Cells.Add(tcCol11);

                TableHeaderCell tcCol12 = new TableHeaderCell();
                tcCol12.Width = Unit.Percentage(15);
                tcCol12.RowSpan = 2;
                tcCol12.HorizontalAlign = HorizontalAlign.Center;
                tcCol12.Text = "Traninig Programme";
                tcCol12.BorderWidth = 1;
                tcCol12.BorderColor = System.Drawing.Color.Chocolate;
                th2.Cells.Add(tcCol12);

                TableHeaderCell tcCol13 = new TableHeaderCell();
                tcCol13.ColumnSpan = 2;
                tcCol13.Width = Unit.Percentage(15);
                tcCol13.HorizontalAlign = HorizontalAlign.Center;
                tcCol13.Text = "Own Centre";
                tcCol13.BorderWidth = 1;
                tcCol13.BorderColor = System.Drawing.Color.Chocolate;
                th2.Cells.Add(tcCol13);

                TableHeaderCell tcCol14 = new TableHeaderCell();
                tcCol14.ColumnSpan = 2;
                tcCol14.Width = Unit.Percentage(15);
                tcCol14.HorizontalAlign = HorizontalAlign.Center;
                tcCol14.Text = "Accredited Institutes";
                tcCol14.BorderWidth = 1;
                tcCol14.BorderColor = System.Drawing.Color.Chocolate;
                th2.Cells.Add(tcCol14);

                TableHeaderCell tcCol15 = new TableHeaderCell();
                tcCol15.ColumnSpan = 2;
                tcCol15.Width = Unit.Percentage(15);
                tcCol15.HorizontalAlign = HorizontalAlign.Center;
                tcCol15.Text = "Direct Candidate";
                tcCol15.BorderWidth = 1;
                tcCol15.BorderColor = System.Drawing.Color.Chocolate;
                th2.Cells.Add(tcCol15);

                TableHeaderCell tcCol16 = new TableHeaderCell();
                tcCol16.ColumnSpan = 2;
                tcCol16.Width = Unit.Percentage(15);
                tcCol16.HorizontalAlign = HorizontalAlign.Center;
                tcCol16.Text = "Total";
                tcCol16.BorderWidth = 1;
                tcCol16.BorderColor = System.Drawing.Color.Chocolate;
                th2.Cells.Add(tcCol16);

                tbl.Rows.Add(th2);
                //tc2.ColumnSpan = 6;
                //tc2.Text = "Report Generated on: " + System.DateTime.Now.ToLongDateString();
                //tc2.HorizontalAlign = HorizontalAlign.Right;
                //th2.Cells.Add(tc2);

                //tbl.Rows.Add(th2);

                TableHeaderRow th = new TableHeaderRow();
                th.CssClass = "head1";

                //TableHeaderCell tcCol1 = new TableHeaderCell();
                //tcCol1.Width = Unit.Percentage(3);
               
                //tcCol1.Text = "S. No.";
                //th.Cells.Add(tcCol1);

                //TableHeaderCell tcCol2 = new TableHeaderCell();
                //tcCol2.Width = Unit.Percentage(15); 
                //tcCol2.Text = "Traninig Programme";
                //th.Cells.Add(tcCol2);

                TableHeaderCell tcCol3 = new TableHeaderCell();
                tcCol3.Width = Unit.Percentage(8);
                tcCol3.HorizontalAlign = HorizontalAlign.Center;
                tcCol3.Text = "Cumulative Undergoing Training/ Trained" + "<br/> / Appeared ";
                tcCol3.BorderWidth = 1;
                tcCol3.BorderColor = System.Drawing.Color.Chocolate;
                th.Cells.Add(tcCol3);

                TableHeaderCell tcCol6 = new TableHeaderCell();
                tcCol6.Width = Unit.Percentage(8);
                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                tcCol6.BorderWidth = 1;
                tcCol6.BorderColor = System.Drawing.Color.Chocolate;
                tcCol6.Text = "Cumulative Certified";
                th.Cells.Add(tcCol6);

                TableHeaderCell tcCol7 = new TableHeaderCell();
                tcCol7.Width = Unit.Percentage(8);
                tcCol7.HorizontalAlign = HorizontalAlign.Center;
                tcCol7.BorderWidth = 1;
                tcCol7.BorderColor = System.Drawing.Color.Chocolate;
                tcCol7.Text = "Cumulative Undergoing Training/ Trained/ Apperaed";
                th.Cells.Add(tcCol7);

                TableHeaderCell tcCol9 = new TableHeaderCell();
                tcCol9.Width = Unit.Percentage(8);
                tcCol9.HorizontalAlign = HorizontalAlign.Center;
                tcCol9.BorderWidth = 1;
                tcCol9.BorderColor = System.Drawing.Color.Chocolate;
                tcCol9.Text = "Cumulative Certified";
                th.Cells.Add(tcCol9);

                TableHeaderCell tcCol17 = new TableHeaderCell();
                tcCol17.Width = Unit.Percentage(8);
                tcCol17.HorizontalAlign = HorizontalAlign.Center;
                tcCol17.BorderWidth = 1;
                tcCol17.BorderColor = System.Drawing.Color.Chocolate;
                tcCol17.Text = "Cumulative Undergoing Training/ Trained/ Apperaed";
                th.Cells.Add(tcCol17);

                TableHeaderCell tcCol19 = new TableHeaderCell();
                tcCol19.Width = Unit.Percentage(8);
                tcCol19.HorizontalAlign = HorizontalAlign.Center;
                tcCol19.BorderWidth = 1;
                tcCol19.BorderColor = System.Drawing.Color.Chocolate;
                tcCol19.Text = "Cumulative Certified";
                th.Cells.Add(tcCol19);


                TableHeaderCell tcCol10 = new TableHeaderCell();
                tcCol10.Width = Unit.Percentage(8);
                tcCol10.HorizontalAlign = HorizontalAlign.Center;
                tcCol10.BorderWidth = 1;
                tcCol10.BorderColor = System.Drawing.Color.Chocolate;
                tcCol10.Text = "Total Undergoing Training/ Trained/ Appeared";
                th.Cells.Add(tcCol10);

                TableHeaderCell tcCol5 = new TableHeaderCell();
                tcCol5.Width = Unit.Percentage(8);
                tcCol5.HorizontalAlign = HorizontalAlign.Center;
                tcCol5.BorderWidth = 1;
                tcCol5.BorderColor = System.Drawing.Color.Chocolate;
                tcCol5.Text = "Total Certified";
                th.Cells.Add(tcCol5);

                tbl.Rows.Add(th);
            };
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
            using (EConnectContext context = new EConnectContext())
            {
                // TableHeaderRow th5 = new TableHeaderRow();
                // th5.CssClass = "head1";
                // TableHeaderCell tcCol151 = new TableHeaderCell();
                // tcCol151.Width = Unit.Percentage(19);
                //  tcCol151.Height = Unit.Pixel(24);
                // tcCol151.HorizontalAlign = HorizontalAlign.Center;
                // tcCol151.Text = "<b>National Institute of Electronics and Information Technology (NIELIT) </b>";
                // tcCol151.ColumnSpan = 10;
                // th5.Cells.Add(tcCol151);

                // tbl.Rows.Add(th5);

                // TableHeaderRow th4 = new TableHeaderRow();
                // th4.CssClass = "head1";
                // TableHeaderCell tcCol141 = new TableHeaderCell();
                // tcCol141.Width = Unit.Percentage(19);
                //// tcCol141.Height = Unit.Pixel(14);
                // tcCol141.HorizontalAlign = HorizontalAlign.Right;
                // tcCol141.Text = "From to till";
                // tcCol141.ColumnSpan = 10;
                // th4.Cells.Add(tcCol141);

                // tbl.Rows.Add(th4);

                // TableHeaderRow th3 = new TableHeaderRow();
                // th3.CssClass = "head1";
                // TableHeaderCell tcCol131 = new TableHeaderCell();
                // tcCol131.Width = Unit.Percentage(19);
                // tcCol131.Height = Unit.Pixel(34);
                // tcCol131.Text = "&nbsp;";
                // tcCol131.ColumnSpan = 10;
                // th3.Cells.Add(tcCol131);

                // tbl.Rows.Add(th3);

                TableHeaderRow th2 = new TableHeaderRow();
                th2.CssClass = "head1";

                //TableHeaderCell tc2 = new TableHeaderCell();
                //tc2.Width = Unit.Percentage(100);

                TableHeaderCell tcCol11 = new TableHeaderCell();
                tcCol11.Width = Unit.Percentage(1);
                tcCol11.RowSpan = 2;
                tcCol11.HorizontalAlign = HorizontalAlign.Center;
                tcCol11.Text = "S. No.";
                tcCol11.BorderWidth = 1;
                tcCol11.BorderColor = System.Drawing.Color.White;
                th2.Cells.Add(tcCol11);

                TableHeaderCell tcCol12 = new TableHeaderCell();
                tcCol12.Width = Unit.Percentage(15);
                tcCol12.RowSpan = 2;
                tcCol12.HorizontalAlign = HorizontalAlign.Center;
                tcCol12.Text = "Traninig Programme";
                tcCol12.BorderWidth = 1;
                tcCol12.BorderColor = System.Drawing.Color.White;
                th2.Cells.Add(tcCol12);

                TableHeaderCell tcCol13 = new TableHeaderCell();
                tcCol13.ColumnSpan = 2;
                tcCol13.Width = Unit.Percentage(15);
                tcCol13.HorizontalAlign = HorizontalAlign.Center;
                tcCol13.Text = "Own Centre";
                tcCol13.BorderWidth = 1;
                tcCol13.BorderColor = System.Drawing.Color.White;
                th2.Cells.Add(tcCol13);

                TableHeaderCell tcCol14 = new TableHeaderCell();
                tcCol14.ColumnSpan = 2;
                tcCol14.Width = Unit.Percentage(15);
                tcCol14.HorizontalAlign = HorizontalAlign.Center;
                tcCol14.Text = "Accredited Institutes";
                tcCol14.BorderWidth = 1;
                tcCol14.BorderColor = System.Drawing.Color.White;
                th2.Cells.Add(tcCol14);

                TableHeaderCell tcCol15 = new TableHeaderCell();
                tcCol15.ColumnSpan = 2;
                tcCol15.Width = Unit.Percentage(15);
                tcCol15.HorizontalAlign = HorizontalAlign.Center;
                tcCol15.Text = "Direct Candidate";
                tcCol15.BorderWidth = 1;
                tcCol15.BorderColor = System.Drawing.Color.White;
                th2.Cells.Add(tcCol15);

                TableHeaderCell tcCol16 = new TableHeaderCell();
                tcCol16.ColumnSpan = 2;
                tcCol16.Width = Unit.Percentage(15);
                tcCol16.HorizontalAlign = HorizontalAlign.Center;
                tcCol16.Text = "Total";
                tcCol16.BorderWidth = 1;
                tcCol16.BorderColor = System.Drawing.Color.White;
                th2.Cells.Add(tcCol16);

                tbl.Rows.Add(th2);
                //tc2.ColumnSpan = 6;
                //tc2.Text = "Report Generated on: " + System.DateTime.Now.ToLongDateString();
                //tc2.HorizontalAlign = HorizontalAlign.Right;
                //th2.Cells.Add(tc2);

                //tbl.Rows.Add(th2);

                TableHeaderRow th = new TableHeaderRow();
                th.CssClass = "head1";

                //TableHeaderCell tcCol1 = new TableHeaderCell();
                //tcCol1.Width = Unit.Percentage(3);

                //tcCol1.Text = "S. No.";
                //th.Cells.Add(tcCol1);

                //TableHeaderCell tcCol2 = new TableHeaderCell();
                //tcCol2.Width = Unit.Percentage(15); 
                //tcCol2.Text = "Traninig Programme";
                //th.Cells.Add(tcCol2);

                TableHeaderCell tcCol3 = new TableHeaderCell();
                tcCol3.Width = Unit.Percentage(8);
                tcCol3.HorizontalAlign = HorizontalAlign.Center;
                tcCol3.Text = "Cumulative Undergoing Training/ Trained" + "<br/> / Appeared ";
                tcCol3.BorderWidth = 1;
                tcCol3.BorderColor = System.Drawing.Color.White;
                th.Cells.Add(tcCol3);

                TableHeaderCell tcCol6 = new TableHeaderCell();
                tcCol6.Width = Unit.Percentage(8);
                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                tcCol6.BorderWidth = 1;
                tcCol6.BorderColor = System.Drawing.Color.White;
                tcCol6.Text = "Cumulative Certified";
                th.Cells.Add(tcCol6);

                TableHeaderCell tcCol7 = new TableHeaderCell();
                tcCol7.Width = Unit.Percentage(8);
                tcCol7.HorizontalAlign = HorizontalAlign.Center;
                tcCol7.BorderWidth = 1;
                tcCol7.BorderColor = System.Drawing.Color.White;
                tcCol7.Text = "Cumulative Undergoing Training/ Trained/ Apperaed";
                th.Cells.Add(tcCol7);

                TableHeaderCell tcCol9 = new TableHeaderCell();
                tcCol9.Width = Unit.Percentage(8);
                tcCol9.HorizontalAlign = HorizontalAlign.Center;
                tcCol9.BorderWidth = 1;
                tcCol9.BorderColor = System.Drawing.Color.White;
                tcCol9.Text = "Cumulative Certified";
                th.Cells.Add(tcCol9);

                TableHeaderCell tcCol17 = new TableHeaderCell();
                tcCol17.Width = Unit.Percentage(8);
                tcCol17.HorizontalAlign = HorizontalAlign.Center;
                tcCol17.BorderWidth = 1;
                tcCol17.BorderColor = System.Drawing.Color.White;
                tcCol17.Text = "Cumulative Undergoing Training/ Trained/ Apperaed";
                th.Cells.Add(tcCol17);

                TableHeaderCell tcCol19 = new TableHeaderCell();
                tcCol19.Width = Unit.Percentage(8);
                tcCol19.HorizontalAlign = HorizontalAlign.Center;
                tcCol19.BorderWidth = 1;
                tcCol19.BorderColor = System.Drawing.Color.White;
                tcCol19.Text = "Cumulative Certified";
                th.Cells.Add(tcCol19);


                TableHeaderCell tcCol10 = new TableHeaderCell();
                tcCol10.Width = Unit.Percentage(8);
                tcCol10.HorizontalAlign = HorizontalAlign.Center;
                tcCol10.BorderWidth = 1;
                tcCol10.BorderColor = System.Drawing.Color.White;
                tcCol10.Text = "Total Undergoing Training/ Trained/ Appeared";
                th.Cells.Add(tcCol10);

                TableHeaderCell tcCol5 = new TableHeaderCell();
                tcCol5.Width = Unit.Percentage(8);
                tcCol5.HorizontalAlign = HorizontalAlign.Center;
                tcCol5.BorderWidth = 1;
                tcCol5.BorderColor = System.Drawing.Color.White;
                tcCol5.Text = "Total Certified";
                th.Cells.Add(tcCol5);

                tbl.Rows.Add(th);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void ShowTableData()
    {
        try
        {
            ShowTableHeader();
            DateTime datefrom, dateto;
            Int64 projectID = 0;
            //datefrom = Convert.ToDateTime(Request.QueryString["DateFrom"]); 
            //dateto = Convert.ToDateTime(Request.QueryString["DateTo"]);
            //projectID = Convert.ToInt64(Request.QueryString["projectID"]);
            datefrom = Convert.ToDateTime(Decrypt(HttpUtility.UrlDecode(Request.QueryString["DateFrom"])));
            dateto = Convert.ToDateTime(Decrypt(HttpUtility.UrlDecode(Request.QueryString["DateTo"])));
            projectID = Convert.ToInt64(Decrypt(HttpUtility.UrlDecode(Request.QueryString["projectID"])));
            lbldatefromto.Text = "Report Period : " + datefrom.ToString("dd-MMM-yyyy") + " to " + dateto.ToString("dd-MMM-yyyy");
            string strHead = "";
            int i = 0, j = 0;           
            Int64 totUnderTrained = 0, totCertified = 0;
            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand scCommand = new SqlCommand("GetStatisticsReportDataProjectWise", new SqlConnection(con.ConnectionString));
            scCommand.CommandType = CommandType.StoredProcedure;

            scCommand.Parameters.Add("@datefrom", SqlDbType.Date).Value = datefrom;
            scCommand.Parameters.Add("@dateto", SqlDbType.Date).Value = dateto;
            scCommand.Parameters.Add("@projectID", SqlDbType.BigInt).Value = projectID;   
            scCommand.CommandTimeout = 50000;

            if (scCommand.Connection.State == ConnectionState.Closed)
            {
                scCommand.Connection.Open();
            }

            SqlDataAdapter da = new SqlDataAdapter(scCommand);
            DataSet ds = new DataSet();

            da.Fill(ds);
          
            for (j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                TableRow tr = new TableRow();
                if (i % 2 == 0)
                    tr.CssClass = "gdalternate1";
                else
                    tr.CssClass = "gdrow1";
                i++;
                TableCell tdRow = new TableCell();
                tdRow.Width = Unit.Percentage(1);
                tdRow.Text = (j + 1).ToString();
                tdRow.HorizontalAlign = HorizontalAlign.Center;
                tdRow.BorderWidth = 1;
                tdRow.BorderColor = System.Drawing.Color.White;
                tr.Cells.Add(tdRow);

                TableCell tdRow1 = new TableCell();
                tdRow1.Width = Unit.Percentage(17);               
                tdRow1.Text = ds.Tables[0].Rows[j]["TrainingProgramme"].ToString();
                tdRow1.HorizontalAlign = HorizontalAlign.Left;
                tdRow1.BorderWidth = 1;
                tdRow1.BorderColor = System.Drawing.Color.White;
                tr.Cells.Add(tdRow1);

                TableCell tdRow2 = new TableCell();
                tdRow2.Width = Unit.Percentage(8);
                tdRow2.Text = ds.Tables[0].Rows[j]["OwnUndrgoingTraining"].ToString();
                tdRow2.HorizontalAlign = HorizontalAlign.Right;
                tdRow2.BorderWidth = 1;
                tdRow2.BorderColor = System.Drawing.Color.White;
                tr.Cells.Add(tdRow2);

                TableCell tdRow3 = new TableCell();
                tdRow3.Width = Unit.Percentage(8);
                tdRow3.Text = ds.Tables[0].Rows[j]["OwnCumulativeCertified"].ToString();               
                tdRow3.HorizontalAlign = HorizontalAlign.Right;
                tdRow3.BorderWidth = 1;
                tdRow3.BorderColor = System.Drawing.Color.White;
                tr.Cells.Add(tdRow3);


                TableCell tdRow15 = new TableCell();
                tdRow15.Width = Unit.Percentage(8);
                tdRow15.Text = ds.Tables[0].Rows[j]["AccreditedUndrgoingTraining"].ToString();
                tdRow15.HorizontalAlign = HorizontalAlign.Right;
                tdRow15.BorderWidth = 1;
                tdRow15.BorderColor = System.Drawing.Color.White;
                tr.Cells.Add(tdRow15);

                TableCell tdRow14 = new TableCell();
                tdRow14.Width = Unit.Percentage(8);
                tdRow14.Text = ds.Tables[0].Rows[j]["AccreditedCumulativeCertified"].ToString();
                tdRow14.HorizontalAlign = HorizontalAlign.Right;
                tdRow14.BorderWidth = 1;
                tdRow14.BorderColor = System.Drawing.Color.White;
                tr.Cells.Add(tdRow14);

                TableCell tdRow16 = new TableCell();
                tdRow16.Width = Unit.Percentage(8);
                tdRow16.Text = ds.Tables[0].Rows[j]["DirectCanUndrgoingTraining"].ToString();
                tdRow16.HorizontalAlign = HorizontalAlign.Right;
                tdRow16.BorderWidth = 1;
                tdRow16.BorderColor = System.Drawing.Color.White;
                tr.Cells.Add(tdRow16);

                TableCell tdRow4 = new TableCell();
                tdRow4.Width = Unit.Percentage(8);
                tdRow4.Text = ds.Tables[0].Rows[j]["DirectCanCumulativeCertified"].ToString();
                tdRow4.HorizontalAlign = HorizontalAlign.Right;
                tdRow4.BorderWidth = 1;
                tdRow4.BorderColor = System.Drawing.Color.White;
                tr.Cells.Add(tdRow4);

                TableCell tdRow21 = new TableCell();
                tdRow21.Width = Unit.Percentage(8);
                Int64 totcountUndrgoingTraining = Convert.ToInt64(ds.Tables[0].Rows[j]["OwnUndrgoingTraining"].ToString())+
                     Convert.ToInt64(ds.Tables[0].Rows[j]["AccreditedUndrgoingTraining"].ToString())
                     + Convert.ToInt64(ds.Tables[0].Rows[j]["DirectCanUndrgoingTraining"].ToString());
                tdRow21.Text = totcountUndrgoingTraining.ToString();
                tdRow21.HorizontalAlign = HorizontalAlign.Right;
                tdRow21.BorderWidth = 1;
                tdRow21.BorderColor = System.Drawing.Color.White;
                tr.Cells.Add(tdRow21);
                totUnderTrained = totUnderTrained + totcountUndrgoingTraining;

                Int64 totcountCumulativeCertified = Convert.ToInt64(ds.Tables[0].Rows[j]["OwnCumulativeCertified"].ToString()) +
                    Convert.ToInt64(ds.Tables[0].Rows[j]["AccreditedCumulativeCertified"].ToString())
                    + Convert.ToInt64(ds.Tables[0].Rows[j]["DirectCanCumulativeCertified"].ToString());
                TableCell tdRow22 = new TableCell();
                tdRow22.Width = Unit.Percentage(8);
                tdRow22.Text = totcountCumulativeCertified.ToString();
                tdRow22.HorizontalAlign = HorizontalAlign.Right;
                tdRow22.BorderWidth = 1;
                tdRow22.BorderColor = System.Drawing.Color.White;
                tr.Cells.Add(tdRow22);
                totCertified = totCertified + totcountCumulativeCertified;

                tbl.Rows.Add(tr);
                LblRptSubHeader.Text = strHead;
            }           
            TableRow trNew = new TableRow();
            if (i % 2 == 0)
                trNew.CssClass = "gdalternate1";
            else
                trNew.CssClass = "gdrow1";
            TableCell tdNewRow1 = new TableCell();
            tdNewRow1.Width = Unit.Percentage(1);
            tdNewRow1.Height = Unit.Percentage(15);
            tdNewRow1.Text = "";
            tdNewRow1.HorizontalAlign = HorizontalAlign.Right;
            tdNewRow1.BorderWidth = 1;
            tdNewRow1.BorderColor = System.Drawing.Color.White;
            trNew.Cells.Add(tdNewRow1);

            TableCell tdNewRow2 = new TableCell();
            tdNewRow2.Width = Unit.Percentage(8);
            tdNewRow2.Height = Unit.Percentage(15);
            tdNewRow2.Text = "<b>Total candidates trained/appeared in Formal, Non-Formal, Short Term and Digital Literacy Courses</b>";
            tdNewRow2.ColumnSpan = 7;
            tdNewRow2.HorizontalAlign = HorizontalAlign.Right;
            tdNewRow2.BorderWidth = 1;
            tdNewRow2.BorderColor = System.Drawing.Color.White;
            trNew.Cells.Add(tdNewRow2);                       
            
            TableCell tdNewRow21 = new TableCell();
            tdNewRow21.Width = Unit.Percentage(8);
            tdNewRow21.Height = Unit.Percentage(15);
            tdNewRow21.Text = "<b>Total :</b> &nbsp;&nbsp;" + totUnderTrained.ToString();
            tdNewRow21.HorizontalAlign = HorizontalAlign.Right;
            tdNewRow21.BorderWidth = 1;
            tdNewRow21.BorderColor = System.Drawing.Color.White;
            trNew.Cells.Add(tdNewRow21);

            TableCell tdNewRow22 = new TableCell();
            tdNewRow22.Width = Unit.Percentage(8);
            tdNewRow22.Height = Unit.Percentage(15);
            tdNewRow22.Text = "<b>Total :</b> &nbsp;&nbsp;" + totCertified.ToString();
            tdNewRow22.HorizontalAlign = HorizontalAlign.Right;
            tdNewRow22.BorderWidth = 1;
            tdNewRow22.BorderColor = System.Drawing.Color.White;
            trNew.Cells.Add(tdNewRow22);

            tbl.Rows.Add(trNew);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowTableDataPDF()
    {
        try
        {
            ShowTableHeaderPDF();
            DateTime datefrom, dateto;
            Int64 projectID = 0;
            //datefrom = Convert.ToDateTime(Request.QueryString["DateFrom"]);
            //dateto = Convert.ToDateTime(Request.QueryString["DateTo"]);
            //projectID = Convert.ToInt64(Request.QueryString["projectID"]);
            datefrom = Convert.ToDateTime(Decrypt(HttpUtility.UrlDecode(Request.QueryString["DateFrom"])));
            dateto = Convert.ToDateTime(Decrypt(HttpUtility.UrlDecode(Request.QueryString["DateTo"])));
            projectID = Convert.ToInt64(Decrypt(HttpUtility.UrlDecode(Request.QueryString["projectID"])));
            lbldatefromto.Text = "From " + datefrom.ToString("dd-MMM-yyyy") + " to " + dateto.ToString("dd-MMM-yyyy");
            string strHead = "";
            int i = 0, j = 0;
            Int64 totUnderTrained = 0, totCertified = 0;
            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand scCommand = new SqlCommand("GetStatisticsReportDataProjectWise", new SqlConnection(con.ConnectionString));
            scCommand.CommandType = CommandType.StoredProcedure;

            scCommand.Parameters.Add("@datefrom", SqlDbType.Date).Value = datefrom;
            scCommand.Parameters.Add("@dateto", SqlDbType.Date).Value = dateto;
            scCommand.Parameters.Add("@projectID", SqlDbType.BigInt).Value = projectID;
            scCommand.CommandTimeout = 50000;

            if (scCommand.Connection.State == ConnectionState.Closed)
            {
                scCommand.Connection.Open();
            }

            SqlDataAdapter da = new SqlDataAdapter(scCommand);
            DataSet ds = new DataSet();

            da.Fill(ds);

            for (j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                TableRow tr = new TableRow();
                if (i % 2 == 0)
                    tr.CssClass = "gdalternate1";
                else
                    tr.CssClass = "gdrow1";
                i++;
                TableCell tdRow = new TableCell();
                tdRow.Width = Unit.Percentage(1);
                tdRow.Text = (j + 1).ToString();
                tdRow.HorizontalAlign = HorizontalAlign.Center;
                tdRow.BorderWidth = 1;
                tdRow.BorderColor = System.Drawing.Color.Chocolate;
                tr.Cells.Add(tdRow);

                TableCell tdRow1 = new TableCell();
                tdRow1.Width = Unit.Percentage(17);
                tdRow1.Text = ds.Tables[0].Rows[j]["TrainingProgramme"].ToString();
                tdRow1.HorizontalAlign = HorizontalAlign.Left;
                tdRow1.BorderWidth = 1;
                tdRow1.BorderColor = System.Drawing.Color.Chocolate;
                tr.Cells.Add(tdRow1);

                TableCell tdRow2 = new TableCell();
                tdRow2.Width = Unit.Percentage(8);
                tdRow2.Text = ds.Tables[0].Rows[j]["OwnUndrgoingTraining"].ToString();
                tdRow2.HorizontalAlign = HorizontalAlign.Right;
                tdRow2.BorderWidth = 1;
                tdRow2.BorderColor = System.Drawing.Color.Chocolate;
                tr.Cells.Add(tdRow2);

                TableCell tdRow3 = new TableCell();
                tdRow3.Width = Unit.Percentage(8);
                tdRow3.Text = ds.Tables[0].Rows[j]["OwnCumulativeCertified"].ToString();
                tdRow3.HorizontalAlign = HorizontalAlign.Right;
                tdRow3.BorderWidth = 1;
                tdRow3.BorderColor = System.Drawing.Color.Chocolate;
                tr.Cells.Add(tdRow3);


                TableCell tdRow15 = new TableCell();
                tdRow15.Width = Unit.Percentage(8);
                tdRow15.Text = ds.Tables[0].Rows[j]["AccreditedUndrgoingTraining"].ToString();
                tdRow15.HorizontalAlign = HorizontalAlign.Right;
                tdRow15.BorderWidth = 1;
                tdRow15.BorderColor = System.Drawing.Color.Chocolate;
                tr.Cells.Add(tdRow15);

                TableCell tdRow14 = new TableCell();
                tdRow14.Width = Unit.Percentage(8);
                tdRow14.Text = ds.Tables[0].Rows[j]["AccreditedCumulativeCertified"].ToString();
                tdRow14.HorizontalAlign = HorizontalAlign.Right;
                tdRow14.BorderWidth = 1;
                tdRow14.BorderColor = System.Drawing.Color.Chocolate;
                tr.Cells.Add(tdRow14);

                TableCell tdRow16 = new TableCell();
                tdRow16.Width = Unit.Percentage(8);
                tdRow16.Text = ds.Tables[0].Rows[j]["DirectCanUndrgoingTraining"].ToString();
                tdRow16.HorizontalAlign = HorizontalAlign.Right;
                tdRow16.BorderWidth = 1;
                tdRow16.BorderColor = System.Drawing.Color.Chocolate;
                tr.Cells.Add(tdRow16);

                TableCell tdRow4 = new TableCell();
                tdRow4.Width = Unit.Percentage(8);
                tdRow4.Text = ds.Tables[0].Rows[j]["DirectCanCumulativeCertified"].ToString();
                tdRow4.HorizontalAlign = HorizontalAlign.Right;
                tdRow4.BorderWidth = 1;
                tdRow4.BorderColor = System.Drawing.Color.Chocolate;
                tr.Cells.Add(tdRow4);

                TableCell tdRow21 = new TableCell();
                tdRow21.Width = Unit.Percentage(8);
                Int64 totcountUndrgoingTraining = Convert.ToInt64(ds.Tables[0].Rows[j]["OwnUndrgoingTraining"].ToString()) +
                     Convert.ToInt64(ds.Tables[0].Rows[j]["AccreditedUndrgoingTraining"].ToString())
                     + Convert.ToInt64(ds.Tables[0].Rows[j]["DirectCanUndrgoingTraining"].ToString());
                tdRow21.Text = totcountUndrgoingTraining.ToString();
                tdRow21.HorizontalAlign = HorizontalAlign.Right;
                tdRow21.BorderWidth = 1;
                tdRow21.BorderColor = System.Drawing.Color.Chocolate;
                tr.Cells.Add(tdRow21);
                totUnderTrained = totUnderTrained + totcountUndrgoingTraining;

                Int64 totcountCumulativeCertified = Convert.ToInt64(ds.Tables[0].Rows[j]["OwnCumulativeCertified"].ToString()) +
                    Convert.ToInt64(ds.Tables[0].Rows[j]["AccreditedCumulativeCertified"].ToString())
                    + Convert.ToInt64(ds.Tables[0].Rows[j]["DirectCanCumulativeCertified"].ToString());
                TableCell tdRow22 = new TableCell();
                tdRow22.Width = Unit.Percentage(8);
                tdRow22.Text = totcountCumulativeCertified.ToString();
                tdRow22.HorizontalAlign = HorizontalAlign.Right;
                tdRow22.BorderWidth = 1;
                tdRow22.BorderColor = System.Drawing.Color.Chocolate;
                tr.Cells.Add(tdRow22);
                totCertified = totCertified + totcountCumulativeCertified;

                tbl.Rows.Add(tr);
                LblRptSubHeader.Text = strHead;
            }
            TableRow trNew = new TableRow();
            if (i % 2 == 0)
                trNew.CssClass = "gdalternate1";
            else
                trNew.CssClass = "gdrow1";
            TableCell tdNewRow1 = new TableCell();
            tdNewRow1.Width = Unit.Percentage(1);
            tdNewRow1.Height = Unit.Percentage(15);
            tdNewRow1.Text = "";
            tdNewRow1.HorizontalAlign = HorizontalAlign.Right;
            tdNewRow1.BorderWidth = 1;
            tdNewRow1.BorderColor = System.Drawing.Color.Chocolate;
            trNew.Cells.Add(tdNewRow1);

            TableCell tdNewRow2 = new TableCell();
            tdNewRow2.Width = Unit.Percentage(8);
            tdNewRow2.Height = Unit.Percentage(15);
            tdNewRow2.Text = "<b>Total candidates trained/appeared in Formal, Non-Formal, Short Term and Digital Literacy Courses</b>";
            tdNewRow2.ColumnSpan = 7;
            tdNewRow2.HorizontalAlign = HorizontalAlign.Right;
            tdNewRow2.BorderWidth = 1;
            tdNewRow2.BorderColor = System.Drawing.Color.Chocolate;
            trNew.Cells.Add(tdNewRow2);

            TableCell tdNewRow21 = new TableCell();
            tdNewRow21.Width = Unit.Percentage(8);
            tdNewRow21.Height = Unit.Percentage(15);
            tdNewRow21.Text = "<b>Total :</b> &nbsp;&nbsp;" + totUnderTrained.ToString();
            tdNewRow21.HorizontalAlign = HorizontalAlign.Right;
            tdNewRow21.BorderWidth = 1;
            tdNewRow21.BorderColor = System.Drawing.Color.Chocolate;
            trNew.Cells.Add(tdNewRow21);

            TableCell tdNewRow22 = new TableCell();
            tdNewRow22.Width = Unit.Percentage(8);
            tdNewRow22.Height = Unit.Percentage(15);
            tdNewRow22.Text = "<b>Total :</b> &nbsp;&nbsp;" + totCertified.ToString();
            tdNewRow22.HorizontalAlign = HorizontalAlign.Right;
            tdNewRow22.BorderWidth = 1;
            tdNewRow22.BorderColor = System.Drawing.Color.Chocolate;
            trNew.Cells.Add(tdNewRow22);

            tbl.Rows.Add(trNew);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
   
    protected void imgPDF_Click(object sender, ImageClickEventArgs e)
    {  
        try
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            hw.AddAttribute("border", "1");
            hw.RenderBeginTag(HtmlTextWriterTag.Font);
            hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "10");
            
            divReportData.Visible = true;
            tbl.BorderStyle = BorderStyle.Solid;
            //tbl.CssClass = "sample3";
            tbl.CssClass = "head1";            

            tbl.Width = Unit.Percentage(400);
         
            ShowTableDataPDF();
            divReportData.Controls.Add(tbl);

            divReportData.RenderControl(hw);
            hw.RenderEndTag();
            Response.Clear();
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=StatisticsReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Write(pdfDoc);
            Response.End();
                      
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    private string Decrypt(string cipherText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        cipherText = cipherText.Replace(" ", "+");
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            //Html32TextWriter htmlWrite;
            //divReportData.Visible = true;
            //ShowTableDataPDF();
            //divReportData.Controls.Add(tbl);
            //Response.Clear();
            //Response.AddHeader("content-disposition", "attachment;filename=StatisticsReport.xls");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.xls";
            //htmlWrite = new Html32TextWriter(StringWrite);
            //divReportData.RenderControl(htmlWrite);
            //Response.Write(StringWrite.ToString());
            //Response.End();

            //Response.ContentType = "application/x-msexcel";
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.xls";
            Response.AddHeader("Content-Disposition", "attachment;filename=StatisticsReport.xls");
            Response.ContentEncoding = Encoding.UTF8; 
            StringWriter tw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(tw);
            ShowTableDataPDF();
            tbl.RenderControl(hw);
            Response.Write(tw.ToString());
            Response.End();
            }
         catch (Exception ex)
         {
            ShowAlert(ex.Message);
         }
    }
}