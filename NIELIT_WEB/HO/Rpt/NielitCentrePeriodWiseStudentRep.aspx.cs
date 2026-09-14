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
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data.Objects;
using System.Data.OleDb;
using EConnect.Utils.Data;
using System.Security.Cryptography;

public partial class NielitCentrePeriodWiseStudentRep : BasePage
{
    Table tbl = new Table();
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0, UserTypeId;
    Int64 centreID = 0;
    DataSet ds = new DataSet();
    string centreName = "", castCat = "", gender = "", reportType = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }

            currentRoleId = Convert.ToInt32(Session["RoleID"]);

            if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/Rpt/NielitCentrePeriodWiseStudentFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            loginUserType = (UserType)Session["UserType"]; 
            entityID = Convert.ToInt64(Session["EntityID"]);  // centreId
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);            

            if (UserTypeId != 6)  // For Non Ho Users
            {
                centreID = Convert.ToInt64(Request.QueryString["centreID"]);
                if (entityID != centreID)
                {
                    Response.Write("Sorry! You don't have rights  to view this page");
                    Response.End();
                }
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

        }
    }
    
    #region vCode

    protected void ShowTableHeader()
    {
        try
        {
            Int64 centreID, categoryId; //courseCat = 0, courseID = 0;
            DateTime FromDate, ToDate;
            reportType = Convert.ToString(Request.QueryString["TypeYear"]);
            centreID = Convert.ToInt64(Request.QueryString["centreID"]);
            FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            categoryId = Convert.ToInt64(Request.QueryString["categoryId"]);
            gender = Convert.ToString(Request.QueryString["genderId"]);

            using (NIELITMISContext context = new NIELITMISContext())
            {
                if (centreID != 0)
                {
                    centreName = (from s in context.NielitCentres
                                  where s.ID == centreID
                                  select new { CentreName = s.Name }).FirstOrDefault().CentreName;
                }
            }
            using (EConnectContext vContext = new EConnectContext())
            {
                if (categoryId != 0)
                {
                    castCat = (from c in vContext.CastCategories
                               where c.ID == categoryId
                               select new { CatName = c.Name }).FirstOrDefault().CatName;
                }
            }

            int colSpanNo = 6;
            TableHeaderRow th11 = new TableHeaderRow();
            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(18);
            tcCol1.Text = "Centre Name : " + centreName;
            tcCol1.ColumnSpan = colSpanNo;
            tcCol1.HorizontalAlign = HorizontalAlign.Left;
            th11.Cells.Add(tcCol1);
            tbl.Rows.Add(th11);

            TableHeaderRow th01 = new TableHeaderRow();
            TableHeaderCell tcCol02 = new TableHeaderCell();
            tcCol02.Width = Unit.Percentage(18);
            tcCol02.Text = "Batch From Date : " + FromDate.ToString("dd-MMM-yyyy");
            tcCol02.ColumnSpan = colSpanNo;
            tcCol02.HorizontalAlign = HorizontalAlign.Left;
            th01.Cells.Add(tcCol02);
            tbl.Rows.Add(th01);

            TableHeaderRow th02 = new TableHeaderRow();
            TableHeaderCell tcCol03 = new TableHeaderCell();
            tcCol03.Width = Unit.Percentage(18);
            tcCol03.Text = "Batch To Date : " + ToDate.ToString("dd-MMM-yyyy") + "<br/>";
            tcCol03.ColumnSpan = colSpanNo;
            tcCol03.HorizontalAlign = HorizontalAlign.Left;
            th02.Cells.Add(tcCol03);
            tbl.Rows.Add(th02);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            if (reportType == "N")
            {
                TableHeaderRow th02n = new TableHeaderRow();
                TableHeaderCell tcCol03n = new TableHeaderCell();
                tcCol03n.Width = Unit.Percentage(18);
                tcCol03n.Text = "Cast Category : All";
                tcCol03n.HorizontalAlign = HorizontalAlign.Left;
                tcCol03n.ColumnSpan = colSpanNo;
                th02n.Cells.Add(tcCol03n);
                tbl.Rows.Add(th02n);

                TableHeaderRow th02c = new TableHeaderRow();
                TableHeaderCell tcCol03c = new TableHeaderCell();
                tcCol03c.Width = Unit.Percentage(18);
                tcCol03c.Text = "Gender : All";
                tcCol03c.HorizontalAlign = HorizontalAlign.Left;
                tcCol03c.ColumnSpan = colSpanNo;
                th02c.Cells.Add(tcCol03c);
                tbl.Rows.Add(th02c);

                TableHeaderCell tcCol = new TableHeaderCell();
                tcCol.Width = Unit.Percentage(1);
                tcCol.Text = "#";
                tcCol.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol);

                TableHeaderCell tcCol2 = new TableHeaderCell();
                tcCol2.Width = Unit.Percentage(20);
                tcCol2.HorizontalAlign = HorizontalAlign.Center;
                tcCol2.Text = " Course ";
                th.Cells.Add(tcCol2);

                TableHeaderCell tcCol3 = new TableHeaderCell();
                tcCol3.Width = Unit.Percentage(8);
                tcCol3.HorizontalAlign = HorizontalAlign.Center;
                tcCol3.Text = "Course Type";
                th.Cells.Add(tcCol3);


                TableHeaderCell tcCol4 = new TableHeaderCell();
                tcCol4.Width = Unit.Percentage(8);
                tcCol4.Text = "Cast Category";
                tcCol4.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol4);

                TableHeaderCell tcCol5 = new TableHeaderCell();
                tcCol5.Width = Unit.Percentage(6);
                tcCol5.Text = "Gender";
                tcCol5.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol5);

                TableHeaderCell tcCol6 = new TableHeaderCell();
                tcCol6.Width = Unit.Percentage(8);
                tcCol6.Text = "Total Trained";
                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol6);
            }

            if (reportType == "C")
            {
                TableHeaderRow th02n = new TableHeaderRow();
                TableHeaderCell tcCol03n = new TableHeaderCell();
                tcCol03n.Width = Unit.Percentage(18);
                tcCol03n.Text = "Cast Category : " + castCat;
                tcCol03n.ColumnSpan = colSpanNo;
                tcCol03n.HorizontalAlign = HorizontalAlign.Left;
                th02n.Cells.Add(tcCol03n);
                tbl.Rows.Add(th02n);

                TableHeaderRow th02c = new TableHeaderRow();
                TableHeaderCell tcCol03c = new TableHeaderCell();
                tcCol03c.Width = Unit.Percentage(18);
                tcCol03c.Text = "Gender : All";
                tcCol03c.ColumnSpan = colSpanNo;
                tcCol03c.HorizontalAlign = HorizontalAlign.Left;
                th02c.Cells.Add(tcCol03c);
                tbl.Rows.Add(th02c);

                TableHeaderCell tcCol = new TableHeaderCell();
                tcCol.Width = Unit.Percentage(1);
                tcCol.Text = "#";
                tcCol.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol);

                TableHeaderCell tcCol2 = new TableHeaderCell();
                tcCol2.Width = Unit.Percentage(20);
                tcCol2.HorizontalAlign = HorizontalAlign.Center;
                tcCol2.Text = " Course ";
                th.Cells.Add(tcCol2);

                TableHeaderCell tcCol3 = new TableHeaderCell();
                tcCol3.Width = Unit.Percentage(8);
                tcCol3.HorizontalAlign = HorizontalAlign.Center;
                tcCol3.Text = "Course Type";
                th.Cells.Add(tcCol3);

                TableHeaderCell tcCol5 = new TableHeaderCell();
                tcCol5.Width = Unit.Percentage(6);
                tcCol5.Text = "Gender";
                tcCol5.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol5);

                TableHeaderCell tcCol6 = new TableHeaderCell();
                tcCol6.Width = Unit.Percentage(8);
                tcCol6.Text = "Total Trained";
                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol6);

                TableHeaderCell tcCol4 = new TableHeaderCell();
                tcCol4.Width = Unit.Percentage(8);
                tcCol4.Text = "Total Certified";
                tcCol4.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol4);
            }

            if (reportType == "G")
            {
                TableHeaderRow th02n = new TableHeaderRow();
                TableHeaderCell tcCol03n = new TableHeaderCell();
                tcCol03n.Width = Unit.Percentage(18);
                tcCol03n.Text = "Cast Category : All";
                tcCol03n.ColumnSpan = colSpanNo;
                tcCol03n.HorizontalAlign = HorizontalAlign.Left;
                th02n.Cells.Add(tcCol03n);
                tbl.Rows.Add(th02n);

                TableHeaderRow th02c = new TableHeaderRow();
                TableHeaderCell tcCol03c = new TableHeaderCell();
                tcCol03c.Width = Unit.Percentage(18);
                tcCol03c.Text = "Gender : " + gender;
                tcCol03c.ColumnSpan = colSpanNo;
                tcCol03c.HorizontalAlign = HorizontalAlign.Left;
                th02c.Cells.Add(tcCol03c);
                tbl.Rows.Add(th02c);

                TableHeaderCell tcCol = new TableHeaderCell();
                tcCol.Width = Unit.Percentage(1);
                tcCol.Text = "#";
                tcCol.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol);

                TableHeaderCell tcCol2 = new TableHeaderCell();
                tcCol2.Width = Unit.Percentage(20);
                tcCol2.HorizontalAlign = HorizontalAlign.Center;
                tcCol2.Text = " Course ";
                th.Cells.Add(tcCol2);

                TableHeaderCell tcCol3 = new TableHeaderCell();
                tcCol3.Width = Unit.Percentage(8);
                tcCol3.HorizontalAlign = HorizontalAlign.Center;
                tcCol3.Text = "Course Type";
                th.Cells.Add(tcCol3);

                TableHeaderCell tcCol5 = new TableHeaderCell();
                tcCol5.Width = Unit.Percentage(8);
                tcCol5.Text = "Cast Category";
                tcCol5.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol5);

                TableHeaderCell tcCol6 = new TableHeaderCell();
                tcCol6.Width = Unit.Percentage(8);
                tcCol6.Text = "Total Trained";
                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol6);

                TableHeaderCell tcCol4 = new TableHeaderCell();
                tcCol4.Width = Unit.Percentage(8);
                tcCol4.Text = "Total Certified";
                tcCol4.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol4);
            }

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

            Int64 categoryId;
            DateTime FromDate, ToDate;

            centreID = Convert.ToInt64(Request.QueryString["centreID"]);
            FromDate = Convert.ToDateTime(Request.QueryString["FromDate"]);
            ToDate = Convert.ToDateTime(Request.QueryString["ToDate"]);
            reportType = Convert.ToString(Request.QueryString["TypeYear"]);
            categoryId = Convert.ToInt64(Request.QueryString["categoryId"]);
            gender = Convert.ToString(Request.QueryString["genderId"]);

            using (ds = PeriodWiseStudent(centreID, FromDate, ToDate, categoryId, gender))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ShowTableHeader();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        TableRow tr = new TableRow();
                        if (i % 2 == 0)
                            tr.CssClass = "gdalternate1";
                        else
                            tr.CssClass = "gdrow1";
                        ds.Tables[0].NewRow();
                        TableCell tdRow = new TableCell();
                        tdRow.Width = Unit.Percentage(1);
                        tdRow.Text = (i + 1).ToString();
                        tdRow.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow);

                        if (reportType == "N")
                        {
                            TableCell tdRow1 = new TableCell();
                            tdRow1.Width = Unit.Percentage(20);
                            tdRow1.Text = ds.Tables[0].Rows[i]["CourseName"].ToString();
                            tdRow1.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow1);

                            TableCell tdRow2 = new TableCell();
                            tdRow2.Width = Unit.Percentage(10);
                            tdRow2.Text = ds.Tables[0].Rows[i]["CourseType"].ToString();
                            tdRow2.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow2);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(12);
                            tdRow3.Text = ds.Tables[0].Rows[i]["CastCategoty"].ToString();
                            tdRow3.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow3);

                            TableCell tdRow4 = new TableCell();
                            tdRow4.Width = Unit.Percentage(6);
                            tdRow4.Text = ds.Tables[0].Rows[i]["Gender"].ToString();
                            tdRow4.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow4);

                            TableCell tdRow5 = new TableCell();
                            tdRow5.Width = Unit.Percentage(10);
                            tdRow5.Text = ds.Tables[0].Rows[i]["TotalTrained"].ToString();
                            tdRow5.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow5);
                        }

                        if (reportType == "C")
                        {
                            TableCell tdRow1 = new TableCell();
                            tdRow1.Width = Unit.Percentage(20);
                            tdRow1.Text = ds.Tables[0].Rows[i]["CourseName"].ToString();
                            tdRow1.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow1);

                            TableCell tdRow2 = new TableCell();
                            tdRow2.Width = Unit.Percentage(10);
                            tdRow2.Text = ds.Tables[0].Rows[i]["CourseType"].ToString();
                            tdRow2.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow2);

                            //TableCell tdRow3 = new TableCell();
                            //tdRow3.Width = Unit.Percentage(12);
                            //tdRow3.Text = ds.Tables[0].Rows[i]["CastCategoty"].ToString();
                            //tdRow3.HorizontalAlign = HorizontalAlign.Center;
                            //tr.Cells.Add(tdRow3);

                            TableCell tdRow4 = new TableCell();
                            tdRow4.Width = Unit.Percentage(6);
                            tdRow4.Text = ds.Tables[0].Rows[i]["Gender"].ToString();
                            tdRow4.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow4);

                            TableCell tdRow5 = new TableCell();
                            tdRow5.Width = Unit.Percentage(10);
                            tdRow5.Text = ds.Tables[0].Rows[i]["TotalTrained"].ToString();
                            tdRow5.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow5);

                            TableCell tdRow6 = new TableCell();
                            tdRow6.Width = Unit.Percentage(10);
                            tdRow6.Text = ds.Tables[0].Rows[i]["TotalCertified"].ToString();
                            tdRow6.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow6);
                        }

                        if (reportType == "G")
                        {
                            TableCell tdRow1 = new TableCell();
                            tdRow1.Width = Unit.Percentage(20);
                            tdRow1.Text = ds.Tables[0].Rows[i]["CourseName"].ToString();
                            tdRow1.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow1);

                            TableCell tdRow2 = new TableCell();
                            tdRow2.Width = Unit.Percentage(10);
                            tdRow2.Text = ds.Tables[0].Rows[i]["CourseType"].ToString();
                            tdRow2.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow2);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(12);
                            tdRow3.Text = ds.Tables[0].Rows[i]["CastCategoty"].ToString();
                            tdRow3.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow3);

                            //TableCell tdRow4 = new TableCell();
                            //tdRow4.Width = Unit.Percentage(6);
                            //tdRow4.Text = ds.Tables[0].Rows[i]["Gender"].ToString();
                            //tdRow4.HorizontalAlign = HorizontalAlign.Center;
                            //tr.Cells.Add(tdRow4);

                            TableCell tdRow5 = new TableCell();
                            tdRow5.Width = Unit.Percentage(10);
                            tdRow5.Text = ds.Tables[0].Rows[i]["TotalTrained"].ToString();
                            tdRow5.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow5);

                            TableCell tdRow6 = new TableCell();
                            tdRow6.Width = Unit.Percentage(10);
                            tdRow6.Text = ds.Tables[0].Rows[i]["TotalCertified"].ToString();
                            tdRow6.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow6);
                        }

                        tbl.Rows.Add(tr);
                        // i++;
                        //lblCount.Visible = true;
                        //lblCount.Text = "Total Records : " + ds.Tables[0].Rows.Count.ToString();
                    }
                }

                else
                {
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
    public DataSet PeriodWiseStudent(Int64 centreID, DateTime FromDate, DateTime ToDate, Int64 categoryId, string genderId)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("RepPeriodWiseStudentDetails", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                if (categoryId == 0 && genderId == "0")
                {
                    cmd.Parameters.Add(new SqlParameter("@centreId", SqlDbType.BigInt));
                    cmd.Parameters["@centreId"].Value = centreID;
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.Date));
                    cmd.Parameters["@FromDate"].Value = FromDate;
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.Date));
                    cmd.Parameters["@ToDate"].Value = ToDate;
                    cmd.Parameters.Add(new SqlParameter("@pView", SqlDbType.BigInt));
                    cmd.Parameters["@pView"].Value = 1; // for all records
                    //
                    cmd.Parameters.Add(new SqlParameter("@genderId", SqlDbType.VarChar));
                    cmd.Parameters["@genderId"].Value = genderId;
                    cmd.Parameters.Add(new SqlParameter("@categoryId", SqlDbType.BigInt));
                    cmd.Parameters["@categoryId"].Value = categoryId; // for all records
                }

                else if (categoryId != 0 && genderId == "0")
                {
                    cmd.Parameters.Add(new SqlParameter("@centreId", SqlDbType.BigInt));
                    cmd.Parameters["@centreId"].Value = centreID;
                    //cmd.Parameters.Add(new SqlParameter("@yearType", SqlDbType.VarChar));
                    //cmd.Parameters["@yearType"].Value = yearType;
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.Date));
                    cmd.Parameters["@FromDate"].Value = FromDate;
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.Date));
                    cmd.Parameters["@ToDate"].Value = ToDate;
                    cmd.Parameters.Add(new SqlParameter("@categoryId", SqlDbType.BigInt));
                    cmd.Parameters["@categoryId"].Value = categoryId;
                    cmd.Parameters.Add(new SqlParameter("@pView", SqlDbType.BigInt));
                    cmd.Parameters["@pView"].Value = 2; // for category records
                    cmd.Parameters.Add(new SqlParameter("@genderId", SqlDbType.VarChar));
                    cmd.Parameters["@genderId"].Value = genderId;
                }

                else if (categoryId == 0 && genderId != "0")
                {
                    cmd.Parameters.Add(new SqlParameter("@centreId", SqlDbType.BigInt));
                    cmd.Parameters["@centreId"].Value = centreID;
                    //cmd.Parameters.Add(new SqlParameter("@yearType", SqlDbType.VarChar));
                    //cmd.Parameters["@yearType"].Value = yearType;
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.Date));
                    cmd.Parameters["@FromDate"].Value = FromDate;
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.Date));
                    cmd.Parameters["@ToDate"].Value = ToDate;
                    cmd.Parameters.Add(new SqlParameter("@genderId", SqlDbType.VarChar));
                    cmd.Parameters["@genderId"].Value = genderId;
                    cmd.Parameters.Add(new SqlParameter("@pView", SqlDbType.BigInt));
                    cmd.Parameters["@pView"].Value = 3; // for gender records
                    //
                    cmd.Parameters.Add(new SqlParameter("@categoryId", SqlDbType.BigInt));
                    cmd.Parameters["@categoryId"].Value = categoryId; // for all records
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
            Response.AddHeader("content-disposition", "attachment;filename=NielitCentrePeriodWiseStudentDetails.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            //  ShowAlert(ex.Message);
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
            Response.AddHeader("content-disposition", "attachment;filename=NielitCentrePeriodWiseStudentDetails.xls");
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

    #endregion
}