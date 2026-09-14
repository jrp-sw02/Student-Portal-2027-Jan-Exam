using EConnect.DAL;
using EConnect.URM;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HO_Rpt_CentreWiseBatchFeeDetails : System.Web.UI.Page
{
    Table tbl = new Table();
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 UserTypeId = 0;
    DataSet ds = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {


            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/CentreWiseBatchFeeDetailsFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            //UserTypeId = 10;
            //currentRoleId = 21;
           // loginUserNo = 285716;

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

        }
    }
    protected void ShowTableHeader()
    {
        try
        {
            int centreId = Convert.ToInt32(Request.QueryString["centreId"]);
            string reportId = Convert.ToString(Request.QueryString["reportId"]);

            if (reportId == "C")
            {
                int courseId = Convert.ToInt32(Request.QueryString["courseId"]);
                int batchId = Convert.ToInt32(Request.QueryString["batchId"]);
            }
            else if (reportId == "D")
            {
                DateTime batchFromDate = Convert.ToDateTime(Request.QueryString["batchFromDate"]);
                DateTime batchToDate = Convert.ToDateTime(Request.QueryString["batchToDate"]);
            }
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "SrNo.";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(8);
            tcCol1.Text = "Sub Centre";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);


            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(8);
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            tcCol2.Text = "Course Name";
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(8);
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Text = "Batch Name";
            th.Cells.Add(tcCol3);


            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(8);
            tcCol4.Text = "Start Date";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(10);
            tcCol5.Text = "End Date";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);


            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(5);
            tcCol6.Text = "Fee Type";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);


            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(3);
            tcCol7.Text = "Amount (Rs)";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);          

            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(3);
            tcCol9.Text = "Effective From ";
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol9);

            TableHeaderCell tcCol10 = new TableHeaderCell();
            tcCol10.Width = Unit.Percentage(5);
            tcCol10.Text = "Effective To" ;
            tcCol10.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol10);

            //TableHeaderCell tcCol15 = new TableHeaderCell();
            //tcCol15.Width = Unit.Percentage(6);
            //tcCol15.Text = "Remarks";
            //tcCol15.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol15);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            //ShowAlert(ex.Message);
        }

    }

    protected void ShowData()
    {
        try
        {

            lblError.Visible = false;

            NIELITMISContext context = new NIELITMISContext();

            int centreId = Convert.ToInt32(Request.QueryString["centreId"]);
            string reportId = Convert.ToString(Request.QueryString["reportId"]);


            int courseId = Convert.ToInt32(Request.QueryString["courseId"]);
            int batchId = Convert.ToInt32(Request.QueryString["batchId"]);

            DateTime batchFromDate = Convert.ToDateTime(Request.QueryString["batchFromDate"]);
            DateTime batchToDate = Convert.ToDateTime(Request.QueryString["batchToDate"]);

            StringBuilder mySql = new StringBuilder();
           
            using (ds= GetBatchFeesData(centreId, reportId, courseId, batchId, batchFromDate, batchToDate))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ShowTableHeader();

                   
                   
                    int i;
                    int z=0;
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        TableRow tr = new TableRow();                        
                        if (i % 2 == 0)
                            tr.CssClass = "gdalternate1";
                        else
                            tr.CssClass = "gdrow1";
                        ds.Tables[0].NewRow();                       
                        TableCell tdRow = new TableCell();
                        tdRow.Width = Unit.Percentage(1);
                        tdRow.Text = (z + 1).ToString();
                        tdRow.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow);


                        TableCell tdRow1 = new TableCell();
                        tdRow1.Width = Unit.Percentage(10);
                        tdRow1.Text = ds.Tables[0].Rows[i]["subName"].ToString();
                        tdRow1.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow1);

                        TableCell tdRow2 = new TableCell();
                        tdRow2.Width = Unit.Percentage(10);
                        tdRow2.Text = ds.Tables[0].Rows[i]["courseName"].ToString();
                        tdRow2.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow2);

                        TableCell tdRow3 = new TableCell();
                        tdRow3.Width = Unit.Percentage(12);
                        tdRow3.Text = ds.Tables[0].Rows[i]["BatchName"].ToString();
                        tdRow3.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow3);


                        TableCell tdRow4 = new TableCell();
                        tdRow4.Width = Unit.Percentage(8);
                        if (ds.Tables[0].Rows[i]["startDate"] != null)
                        {
                            DateTime dt1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["startDate"]);
                            tdRow4.Text = dt1.ToString("dd-MMM-yyyy");
                        }

                      //  tdRow4.Text = ds.Tables[0].Rows[i]["startDate"].ToString();
                        tdRow4.HorizontalAlign = HorizontalAlign.Center;
                        tdRow4.Wrap = false;
                        tr.Cells.Add(tdRow4);

                        TableCell tdRow5 = new TableCell();
                        tdRow5.Width = Unit.Percentage(10);
                        if (ds.Tables[0].Rows[i]["endDate"] != null)
                        {
                            DateTime dt1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["endDate"]);
                            tdRow5.Text = dt1.ToString("dd-MMM-yyyy");
                        }
                      //  tdRow5.Text = ds.Tables[0].Rows[i]["endDate"].ToString();
                        tdRow5.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow5);


                        TableCell tdRow13 = new TableCell();
                        tdRow13.Width = Unit.Percentage(10);
                        //tdRow13.Text = ds.Tables[0].Rows[i]["feeType"].ToString();
                        tdRow13.HorizontalAlign = HorizontalAlign.Center;
                        tdRow13.Wrap = false;
                        tr.Cells.Add(tdRow13);

                        TableCell tdRow14 = new TableCell();
                        tdRow14.Width = Unit.Percentage(5);
                        //tdRow14.Text = ds.Tables[0].Rows[i]["feeAmount"].ToString();
                        tdRow14.HorizontalAlign = HorizontalAlign.Center;
                        tdRow14.Wrap = false;
                        tr.Cells.Add(tdRow14);
                     

                        TableCell tdRow6 = new TableCell();
                        tdRow6.Width = Unit.Percentage(5);                       
                        tdRow6.HorizontalAlign = HorizontalAlign.Center;
                        tdRow6.Wrap = false;
                        tr.Cells.Add(tdRow6);

                        TableCell tdRow7 = new TableCell();
                        tdRow7.Width = Unit.Percentage(3); 
                        tdRow7.HorizontalAlign = HorizontalAlign.Center;
                        tdRow7.Wrap = false;
                        tr.Cells.Add(tdRow7);

                        string Feetypes = "", Feeamount = "", Edates, ETdates,c="",b="";
                        DateTime EffectiveDates, EffectiveToDates;
                        c = ds.Tables[0].Rows[i]["courseId"].ToString();
                        b = ds.Tables[0].Rows[i]["batchId"].ToString();

                        courseId = int.Parse(c.ToString());
                        batchId = int.Parse(b.ToString());
                        using (DataTable dt1 = GetBatchFeesDetailsFeetypeWise(centreId, courseId, batchId))
                        {
                            if (dt1.Rows.Count > 0)
                            {
                                int j;
                                for (j = 0; j < dt1.Rows.Count; j++)
                                {
                                    Feetypes = dt1.Rows[j].Field<string>("feeType").ToString();
                                    Feeamount = dt1.Rows[j].Field<int>("feeAmount").ToString();                                  
                                    EffectiveDates = dt1.Rows[j].Field<DateTime>("effectiveFromDate");
                                    Edates = EffectiveDates.ToString("dd-MMM-yyyy");
                                    EffectiveToDates = dt1.Rows[j].Field<DateTime>("effectiveToDate");
                                    ETdates = EffectiveToDates.ToString("dd-MMM-yyyy");
                                    if (ETdates != "01-Jan-1900")
                                    { //ETdates = ETdates;
                                    }
                                    else
                                    {
                                        ETdates = "NA";
                                    }
                                         if (j == 0)
                                        {
                                            tdRow13.Text = "# " + Feetypes;
                                            tdRow14.Text =  Feeamount;
                                            tdRow6.Text = Edates;
                                            tdRow7.Text = ETdates;
                                        }
                                        else
                                        {
                                            tdRow13.Text = tdRow13.Text + "<br> " + "----------" + "<br># " + Feetypes;
                                            tdRow14.Text = tdRow14.Text + "<br> " + "----------" + "<br>" + Feeamount;
                                            tdRow6.Text = tdRow6.Text + "<br> " + "----------" + "<br>" + Edates;
                                            tdRow7.Text = tdRow7.Text + "<br> " + "----------" + "<br>" + ETdates;
                                        } 
                                }

                            }

                        }                        

                        //TableCell tdRow6 = new TableCell();
                        //tdRow6.Width = Unit.Percentage(5);
                        ////if (ds.Tables[0].Rows[i]["effectiveFromDate"] != null)
                        ////{
                        ////    DateTime dt1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["effectiveFromDate"]);

                        ////    tdRow6.Text = dt1.ToString("dd-MMM-yyyy"); 
                        ////}
                        //tdRow6.HorizontalAlign = HorizontalAlign.Center;
                        //tdRow6.Wrap = false;
                        //tr.Cells.Add(tdRow6);

                        //TableCell tdRow7 = new TableCell();
                        //tdRow7.Width = Unit.Percentage(3);
                        //DateTime dt2 = Convert.ToDateTime(ds.Tables[0].Rows[i]["effectiveToDate"]);
                        //string dates = dt2.ToString("dd-MMM-yyyy");
                        //if (dates != "01-Jan-1900")
                        //{
                        //    tdRow7.Text = dt2.ToString("dd-MMM-yyyy");
                        //}
                        //else
                        //{
                        //    tdRow7.Text = "--";
                        //}

                        //tdRow7.HorizontalAlign = HorizontalAlign.Center;
                        //tdRow7.Wrap = false;
                        //tr.Cells.Add(tdRow7);

                        //TableCell tdRow7 = new TableCell();
                        //tdRow7.Width = Unit.Percentage(3);

                        //if (ds.Tables[0].Rows[i]["effectiveToDate"].ToString() != "")
                        //{
                        //    DateTime dt1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["effectiveToDate"]);

                        //    tdRow7.Text = dt1.ToString("dd-MMM-yyyy"); ;
                        //}
                        //tdRow7.HorizontalAlign = HorizontalAlign.Center;
                        //tdRow7.Wrap = false;
                        //tr.Cells.Add(tdRow7);                      


                        //TableCell tdRow15 = new TableCell();
                        //tdRow15.Width = Unit.Percentage(6);
                        //tdRow15.Text = ds.Tables[0].Rows[i]["Remarks"].ToString();
                        //tdRow15.HorizontalAlign = HorizontalAlign.Center;
                        //tdRow15.Wrap = false;
                        //tr.Cells.Add(tdRow15);
                        using (DataTable dt1 = GetBatchFeesDetailsFeetypeWise(centreId, courseId, batchId))
                        {
                            if (dt1.Rows.Count > 0)
                            {                              
                                tbl.Rows.Add(tr);
                                z = z + 1;
                            }                           
                        }
                       // tbl.Rows.Add(tr);
                        // i++;
                        lblCount.Visible = true;
                        //lblCount.Text = "Total Records : " + ds.Tables[0].Rows.Count.ToString();
                        lblCount.Text = "Total Records : " + z.ToString();
                    }                  

                }

                else
                {
                    lblCount.Visible = false;
                    lblError.Visible = true;
                    lblError.Text = "No Record Found";
                }
                    // LblRptSubHeader.Text = strHead;
                }
            }
        catch (Exception ex)
        {
            // ShowAlert(ex.Message);
        }
        finally
        {
        }
    }
    public DataTable GetBatchFeesDetailsFeetypeWise(Int64 centreId, Int64 courseId, Int64 batchId)
    {

        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetBatchFeesDataFeeTypeDetails", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@centreId", SqlDbType.BigInt));
                cmd.Parameters["@centreId"].Value = centreId;              
               
                    cmd.Parameters.Add(new SqlParameter("@courseId", SqlDbType.BigInt));
                    cmd.Parameters["@courseId"].Value = courseId;

                    cmd.Parameters.Add(new SqlParameter("@batchId", SqlDbType.BigInt));
                    cmd.Parameters["@batchId"].Value = batchId;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
    public DataSet GetBatchFeesData(Int64 centreId, string reportId, Int64 courseId, Int64 batchId, DateTime batchFromDate, DateTime batchToDate)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetBatchFeesDataReport", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@centreId", SqlDbType.BigInt));
                cmd.Parameters["@centreId"].Value = centreId;
                cmd.Parameters.Add(new SqlParameter("@reporttype", SqlDbType.VarChar));
                cmd.Parameters["@reporttype"].Value = reportId;
                if (reportId == "C")
                {
                    cmd.Parameters.Add(new SqlParameter("@courseId", SqlDbType.BigInt));
                    cmd.Parameters["@courseId"].Value = courseId;

                    cmd.Parameters.Add(new SqlParameter("@batchId", SqlDbType.BigInt));
                    cmd.Parameters["@batchId"].Value = batchId;
                }
                else if (reportId == "D")
                {
                    cmd.Parameters.Add(new SqlParameter("@BatchFromDate", SqlDbType.Date));
                    cmd.Parameters["@BatchFromDate"].Value = batchFromDate;
                    cmd.Parameters.Add(new SqlParameter("@BatchToDate", SqlDbType.Date));
                    cmd.Parameters["@BatchToDate"].Value = batchToDate;
                }
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    

                    sda.Fill(ds);
                }
            }
        }
        return ds;
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

            divpdf.Visible = true;
            tbl.BorderStyle = BorderStyle.Solid;
            tbl.CssClass = "sample3";
            tbl.CellPadding = 2;
            tbl.CellSpacing = 1;
            tbl.Width = Unit.Percentage(200);

            ShowData();
            divpdf.Controls.Add(tbl);

            divpdf.RenderControl(hw);
            hw.RenderEndTag();
            Response.Clear();
            StringReader sr = new StringReader(sw.ToString());
            // Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=CentreWiseBatchFeeDetails.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            //  ShowAlert(ex.Message);
        }
    }
}