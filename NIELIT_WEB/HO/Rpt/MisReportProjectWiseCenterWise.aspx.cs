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

public partial class MisReportProjectWiseCenterWise : BasePage
{
    Table tbl = new Table();
    
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 applicantTypeID = 0;
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
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/MisReportProjectWiseCenterWiseFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            if (!Page.IsPostBack)
            {
                if ((!string.IsNullOrEmpty(Decrypt(HttpUtility.UrlDecode(Request.QueryString["startDate"])))) && (!string.IsNullOrEmpty(Decrypt(HttpUtility.UrlDecode(Request.QueryString["endDate"])))) && (!string.IsNullOrEmpty(Decrypt(HttpUtility.UrlDecode(Request.QueryString["ProjID"])))) && (!string.IsNullOrEmpty(Decrypt(HttpUtility.UrlDecode(Request.QueryString["centreID"])))) && (!string.IsNullOrEmpty(Decrypt(HttpUtility.UrlDecode(Request.QueryString["status"])))))
                {
                    tbl.CssClass = "sample3";
                    tbl.CellPadding = 2;
                    tbl.CellSpacing = 1;
                    tbl.Width = Unit.Percentage(100);
                    ShowData();
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
   
    protected void ShowData()
    {      
        try
        {
            DateTime startDate, endDate;
            Int64 ProjID = 0, centreID = 0, status=0;
      
            startDate = Convert.ToDateTime(Decrypt(HttpUtility.UrlDecode(Request.QueryString["startDate"])));
            endDate = Convert.ToDateTime(Decrypt(HttpUtility.UrlDecode(Request.QueryString["endDate"])));
            ProjID = Convert.ToInt64(Decrypt(HttpUtility.UrlDecode(Request.QueryString["ProjID"])));
            centreID = Convert.ToInt64(Decrypt(HttpUtility.UrlDecode(Request.QueryString["centreID"])));
            status = Convert.ToInt64(Decrypt(HttpUtility.UrlDecode(Request.QueryString["status"])));

            lbldatefromto.Text = "Report Period : " + startDate.ToString("dd-MMM-yyyy") + " to " + endDate.ToString("dd-MMM-yyyy");
            string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand scCommand = new SqlCommand("GetStudentsDetailsProjectWiseCenterWise", new SqlConnection(con.ConnectionString));
            scCommand.CommandType = CommandType.StoredProcedure;
            scCommand.Parameters.Add("@ProjID", SqlDbType.BigInt).Value = ProjID;
            scCommand.Parameters.Add("@centreID", SqlDbType.BigInt).Value = centreID;
            scCommand.Parameters.Add("@startDate", SqlDbType.Date).Value = startDate;
            scCommand.Parameters.Add("@endDate", SqlDbType.Date).Value = endDate;
            scCommand.Parameters.Add("@status", SqlDbType.Int).Value = status;
           
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
                ShowTableHeader();
                int i;
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    TableRow tr = new TableRow();
                    if (i % 2 == 0)
                        tr.CssClass = "gdalternate1";
                    else
                        tr.CssClass = "gdrow1";
         
                    TableCell tdRow = new TableCell();
                    tdRow.Width = Unit.Percentage(1);
                    tdRow.Text = (i + 1).ToString();
                    tdRow.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow);

                    TableCell tdRow2 = new TableCell();
                    tdRow2.Width = Unit.Percentage(4);
                    tdRow2.Text = ds.Tables[0].Rows[i]["projectName"].ToString();
                    tdRow2.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow2);

                    TableCell tdRow3 = new TableCell();
                    tdRow3.Width = Unit.Percentage(5);
                    tdRow3.Text = ds.Tables[0].Rows[i]["CentreName"].ToString();
                    tdRow3.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow3);

                    TableCell tdRow16 = new TableCell();
                    tdRow16.Width = Unit.Percentage(5);
                    tdRow16.Text = ds.Tables[0].Rows[i]["CourseName"].ToString();
                    tdRow16.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow16);                   

                    TableCell tdRow12 = new TableCell();
                    tdRow12.Width = Unit.Percentage(4);
                    tdRow12.Text = ds.Tables[0].Rows[i]["StudentName"].ToString();
                    tdRow12.HorizontalAlign = HorizontalAlign.Left;

                    tr.Cells.Add(tdRow12);

                    TableCell tdRow13 = new TableCell();
                    tdRow13.Width = Unit.Percentage(5);                   
                    tdRow13.Text = ds.Tables[0].Rows[i]["Father_Name"].ToString();
                    tdRow13.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow13);

                    TableCell tdRow4 = new TableCell();
                    tdRow4.Width = Unit.Percentage(20);
                    tdRow4.Text = Convert.ToDateTime(ds.Tables[0].Rows[i]["Dob"]).ToString("dd-MMM-yyyy");
                    tdRow4.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow4);

                    TableCell tdRow5 = new TableCell();
                    tdRow5.Width = Unit.Percentage(15);
		     if(ProjID ==1 && ds.Tables[0].Rows[i]["Gender"].ToString()=="Female" && ds.Tables[0].Rows[i]["Is_EWS"].ToString()=="True")
                        tdRow5.Text = "EWS(W)";
                    else
	                    tdRow5.Text = ds.Tables[0].Rows[i]["CastCategory"].ToString();
                    tdRow5.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow5);

                    TableCell tdRow7 = new TableCell();
                    tdRow7.Width = Unit.Percentage(5);
                    tdRow7.Text = ds.Tables[0].Rows[i]["Gender"].ToString();
                    tdRow7.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow7);

                    TableCell tdRow18 = new TableCell();
                    tdRow18.Width = Unit.Percentage(5);
                    tdRow18.Text = Convert.ToDateTime(ds.Tables[0].Rows[i]["BatchFrom"]).ToString("dd/MMM/yyyy");
                    tdRow18.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow18);

                    TableCell tdRow17 = new TableCell();
                    tdRow17.Width = Unit.Percentage(5);
                    tdRow17.Text = Convert.ToDateTime(ds.Tables[0].Rows[i]["BatchTo"]).ToString("dd/MMM/yyyy");
                    tdRow17.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow17);

                    TableCell tdRow8 = new TableCell();
                    tdRow8.Width = Unit.Percentage(5);
                    tdRow8.Text = ds.Tables[0].Rows[i]["Status"].ToString();
                    tdRow8.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow8);
                   
                    tbl.Rows.Add(tr);

                }               
                lblError.Visible = false;
                //lblheading.Visible = true; 
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "No Record Found !";
                //lblheading.Visible = true;               
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
           TableHeaderRow th1 = new TableHeaderRow();
           th1.CssClass = "head1";

           TableHeaderCell tc1 = new TableHeaderCell();
           tc1.Width = Unit.Percentage(100);

           DateTime datefrom, dateto;
           datefrom = Convert.ToDateTime(Decrypt(HttpUtility.UrlDecode(Request.QueryString["startDate"])));
           dateto = Convert.ToDateTime(Decrypt(HttpUtility.UrlDecode(Request.QueryString["endDate"])));          

           tc1.ColumnSpan = 12;
           tc1.Text = "MIS Report of Student Details of ProjectWise CenterWise From " + datefrom.ToString("dd-MMM-yyyy") + " To " + dateto.ToString("dd-MMM-yyyy") + ""; //" + ddlProjectName.SelectedItem.Text + "";

           tc1.HorizontalAlign = HorizontalAlign.Center;
           th1.Cells.Add(tc1);

           tbl.Rows.Add(th1);

           TableHeaderRow th2 = new TableHeaderRow();
           th2.CssClass = "head1";

           TableHeaderCell tc2 = new TableHeaderCell();
           tc2.Width = Unit.Percentage(100);

           tc2.ColumnSpan = 12;
           tc2.Text = "Report Generated on: " + System.DateTime.Now.ToLongDateString();
           tc2.HorizontalAlign = HorizontalAlign.Right;
           th2.Cells.Add(tc2);

           tbl.Rows.Add(th2);

           TableHeaderRow th = new TableHeaderRow();
           th.CssClass = "head1";

           TableHeaderCell tcCol = new TableHeaderCell();
           tcCol.Width = Unit.Percentage(1);
           tcCol.Text = "#";
           tcCol.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol);


           TableHeaderCell tcCol2 = new TableHeaderCell();
           tcCol2.Width = Unit.Percentage(10);
           tcCol2.Text = "Project Name";
           tcCol2.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol2);

           TableCell tdRow113 = new TableCell();
           tdRow113.Width = Unit.Percentage(15);
           tdRow113.Text = "Course Name";
           tdRow113.HorizontalAlign = HorizontalAlign.Center;

           TableHeaderCell tcCol6 = new TableHeaderCell();
           tcCol6.Width = Unit.Percentage(15);
           tcCol6.Text = "Centre Name";
           tcCol6.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol6);

           th.Cells.Add(tdRow113);
           TableHeaderCell tcCol3 = new TableHeaderCell();
           tcCol3.Width = Unit.Percentage(15);
           tcCol3.Text = "Student Name";
           tcCol3.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol3);

           TableHeaderCell tcCol4 = new TableHeaderCell();
           tcCol4.Width = Unit.Percentage(15);
           tcCol4.Text = "Father / Guardian Name";
           tcCol4.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol4);

           TableHeaderCell tcCol5 = new TableHeaderCell();
           tcCol5.Width = Unit.Percentage(20);
           tcCol5.Text = "Date Of Birth ";
           tcCol5.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol5);

           TableHeaderCell tcCol15 = new TableHeaderCell();
           tcCol15.Width = Unit.Percentage(20);
           tcCol15.Text = "Category ";
           tcCol15.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol15);

           TableHeaderCell tcCol7 = new TableHeaderCell();
           tcCol7.Width = Unit.Percentage(5);
           tcCol7.Text = "Gender";
           tcCol7.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol7);

           TableHeaderCell tcCol8 = new TableHeaderCell();
           tcCol8.Width = Unit.Percentage(5);
           tcCol8.Text = "Batch From ";
           tcCol8.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol8);

           TableHeaderCell tcCol9 = new TableHeaderCell();
           tcCol9.Width = Unit.Percentage(5);
           tcCol9.Text = "Batch To ";
           tcCol9.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol9);

           TableHeaderCell tcCol10 = new TableHeaderCell();
           tcCol10.Width = Unit.Percentage(5);
           tcCol10.Text = "Status  ";
           tcCol10.HorizontalAlign = HorizontalAlign.Center;
           th.Cells.Add(tcCol10);

           tbl.Rows.Add(th);
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
              hw.RenderBeginTag(HtmlTextWriterTag.Font );
              hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize , "9");

            divReportData.Visible = true;
            tbl.BorderStyle = BorderStyle .Solid ;
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
           // Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
              
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=MisReportProjectWiseCenterWise.pdf");
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
            ShowData();
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;

            divReportData.Visible = true;
            //getData();
            divReportData.Controls.Add(tbl);
            Response.Clear();

            Response.AddHeader("content-disposition", "attachment;filename=ProjectCentreWise.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            htmlWrite = new Html32TextWriter(StringWrite);
            htmlWrite.AddAttribute("border", "2");
            divReportData.RenderControl(htmlWrite);
            Response.Write(StringWrite.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
  
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }    
}