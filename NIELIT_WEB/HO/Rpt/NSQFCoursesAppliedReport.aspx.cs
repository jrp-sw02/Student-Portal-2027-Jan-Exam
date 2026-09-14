using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Data;
using System.Data .SqlClient ;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Transactions;
using EConnect.Utils.Common;
using System.Drawing;

public partial class NSQFCoursesAppliedReport : BasePage
{
    Table tbl = new Table(); 
    UserType loginUserType;
    Int64 entityID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!Page.IsPostBack)
            {               
                txtto.Text = System.DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
                txtto.Enabled = false;
                txtto.BackColor = Color.LightCyan;
                tbl.CssClass = "sample3";
                tbl.BorderStyle  = BorderStyle .Solid ;  
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(100);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
  
    protected void ShowTableHeader()
    {
        if (txtFrom.Text == "")
        {
            ShowAlert("Please Select the Date From!!");
            return;
        }  
        try
        {
            DateTime FromDate = Convert.ToDateTime(txtFrom.Text);
            DateTime ToDate = Convert.ToDateTime(txtto.Text);
            string FromDate1 = FromDate.ToString("dd-MM-yyyy");
            string ToDate1 = ToDate.ToString("dd-MM-yyyy");
            TableHeaderRow th2 = new TableHeaderRow();
            th2.CssClass = "head1";

            TableHeaderRow th1 = new TableHeaderRow();
            th1.CssClass = "head1";

            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);
            tc1.ColumnSpan = 6;
            tc1.Text =  "<font size = '4'><b>National Institute of Electronics and Information Technology (NIELIT)</b></font><br/>"+"<font size = '2'><b>Institutes Applied For Free NSQF Accreditation From " + FromDate1 + " To " + ToDate1 + "</b></font><p style='text-align:right;'>" + "Date: " + System.DateTime.Now.ToLongDateString() + "</p>";
               
            tc1.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tc1);
            tbl.Rows.Add(th1);
            
           
          
           
            TableHeaderRow th = new TableHeaderRow();
           // th.CssClass = "head2";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "#";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

        
            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(2);
            tcCol2.Text = "Request ID";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol2a = new TableHeaderCell();
            tcCol2a.Width = Unit.Percentage(4);
            tcCol2a.Text = "Application Date";
            tcCol2a.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2a);
           
            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(5);
            tcCol3.Text = "Instt Name- Accr No.";
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol3);

            TableHeaderCell tdRow113 = new TableHeaderCell();
            tdRow113.Width = Unit.Percentage(5);
            tdRow113.Text = "Instt Address";
            tdRow113.HorizontalAlign = HorizontalAlign.Center;

            th.Cells.Add(tdRow113);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(10);
            tcCol4.Text = "Course";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);            

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
  
    public DataTable FillGridViewRecord()
    {

        DateTime FromDate = Convert.ToDateTime(txtFrom.Text);
        DateTime ToDate = Convert.ToDateTime(txtto.Text);
        string FromDate1 = FromDate.ToString("dd-MM-yyyy");
        string ToDate1 = ToDate.ToString("dd-MM-yyyy");
        //string FromDate1 = FromDate.ToString("yyyy-MM-dd");
        //string ToDate1 = ToDate.ToString("yyyy-MM-dd");
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("getGridNSQFFreeCoursesApplied", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@vRequestDateFrom", SqlDbType.Date));
                cmd.Parameters["@vRequestDateFrom"].Value = FromDate;
                cmd.Parameters.Add(new SqlParameter("@vRequestdateTo", SqlDbType.Date));
                cmd.Parameters["@vRequestdateTo"].Value = ToDate;
               
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
   
        
   protected void getData()
    {      
        try
        {
           
            ShowTableHeader();

            if (txtFrom.Text=="")
            {
                ShowAlert("Please Select the Date From!!");
                return;
            }  
            
            DateTime FromDate = Convert.ToDateTime(txtFrom.Text);
            DateTime ToDate = Convert.ToDateTime(txtto.Text);
            string FromDate1 = FromDate.ToString("dd-MM-yyyy");
            string ToDate1 = ToDate.ToString("dd-MM-yyyy");

           // EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand scCommand = new SqlCommand("getGridNSQFFreeCoursesApplied", new SqlConnection(con.ConnectionString));
            scCommand.CommandType = CommandType.StoredProcedure;

            scCommand.Parameters.Add(new SqlParameter("@vRequestDateFrom", SqlDbType.Date));
            scCommand.Parameters["@vRequestDateFrom"].Value = FromDate;
            scCommand.Parameters.Add(new SqlParameter("@vRequestdateTo", SqlDbType.Date));
            scCommand.Parameters["@vRequestdateTo"].Value = ToDate;
            scCommand.CommandTimeout = 50000;
            if (scCommand.Connection.State == ConnectionState.Closed)
            {
                scCommand.Connection.Open();
            }

            SqlDataAdapter da = new SqlDataAdapter(scCommand);
            DataSet ds = new DataSet();

            da.Fill(ds);
            int TotalRecord = 0;
            TotalRecord = Convert.ToInt32(ds.Tables[0].Rows.Count);
            if (ds.Tables[0].Rows.Count > 0)
            {
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
                tdRow.Text = (i+1).ToString();
                tdRow.HorizontalAlign = HorizontalAlign.Center;
                tr.Cells.Add(tdRow);


                TableCell tdRow2 = new TableCell();
                tdRow2.Width = Unit.Percentage(2);
                tdRow2.Text = ds.Tables[0].Rows[i]["ID"].ToString();
                tdRow2.HorizontalAlign = HorizontalAlign.Center ;

                tr.Cells.Add(tdRow2);

                TableCell tdRow2a = new TableCell();
                tdRow2a.Width = Unit.Percentage(2);
                tdRow2a.Text = ds.Tables[0].Rows[i]["AppDate"].ToString();
                tdRow2a.HorizontalAlign = HorizontalAlign.Left;

                tr.Cells.Add(tdRow2a);

                
                TableCell tdRow3 = new TableCell();
                tdRow3.Width = Unit.Percentage(5);
                tdRow3.Text = ds.Tables[0].Rows[i]["InstName"].ToString() + " ( " + ds.Tables[0].Rows[i]["Accreditation_Number"].ToString()+" )";
                tdRow3.HorizontalAlign = HorizontalAlign.Center;

               tr.Cells.Add(tdRow3);
              

               TableCell tdRow16 = new TableCell();
               tdRow16.Width = Unit.Percentage(5);
               tdRow16.Text = ds.Tables[0].Rows[i]["InstAddress"].ToString();
               tdRow16.HorizontalAlign = HorizontalAlign.Center;

               tr.Cells.Add(tdRow16); 

               TableCell tdRow4 = new TableCell();
               tdRow4.Width = Unit.Percentage(20);
               tdRow4.Text = ds.Tables[0].Rows[i]["Name"].ToString();
               tdRow4.HorizontalAlign = HorizontalAlign.Center;

               tr.Cells.Add(tdRow4);              

                tbl.Rows.Add(tr);             
            }
                //TableRow tr1 = new TableRow();
                //TableCell tdRow44 = new TableCell();
                //tdRow44.Width = Unit.Percentage(25);
                //tdRow44.Text = "Total :" +TotalRecord.ToString();
                //tdRow44.HorizontalAlign = HorizontalAlign.Center;

                //tr1.Cells.Add(tdRow44);

                //tbl.Rows.Add(tr1);   
                 TableRow trNew = new TableRow();
            if (i % 2 == 0)
                trNew.CssClass = "gdalternate1";
            else
                trNew.CssClass = "gdrow1";        

            TableCell tdNewRow2 = new TableCell();
            tdNewRow2.Width = Unit.Percentage(8);
            tdNewRow2.Height = Unit.Percentage(15);
            tdNewRow2.Text = "<font size = '2'> <b>Total : </b>" + TotalRecord.ToString() +"</font>";
            tdNewRow2.ColumnSpan = 5;
            tdNewRow2.HorizontalAlign = HorizontalAlign.Left;
            tdNewRow2.BorderWidth = 1;
            tdNewRow2.BorderColor = System.Drawing.Color.White;
            trNew.Cells.Add(tdNewRow2);                       
            
        

            tbl.Rows.Add(trNew);
        
          }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
                      
   }
  
     protected void btnGenerate_Click(object sender, EventArgs e)
    {
        if (txtFrom.Text == "")
        {
            ShowAlert("Please Select the Date From!!");
            return;
        }  
        tbl.CssClass = "sample3";
        tbl.CellPadding = 2;
        tbl.CellSpacing = 1;
        tbl.Width = Unit.Percentage(100); 
        BindGridView();
    }
    
     protected void BindGridView()
     {
         try
         {
             using (DataTable dt = FillGridViewRecord())
             {
                 if (dt.Rows.Count > 0)
                 {     
                     var ApplicationData = (from p in dt.AsEnumerable()
                                            select new
                                            {
                                                ID = p.Field<Int64>("ID"),
                                                InstName = p.Field<string>("InstName"),
                                                InstAddress = p.Field<string>("InstAddress"),
                                                CourseName = p.Field<string>("Name"),
                                                Accreditation_Number=p.Field <string >("Accreditation_Number"),
                                                AppDate=p.Field <DateTime  >("AppDate"),
                                            });
                    
                     PagingBar1.Bind(ApplicationData, ref gvMain);
                     uPnlGrid.Update();
                     uPnlNavigation.Update();
                     lblError.Visible = false;
                     lblheading.Visible = true;
                     gvMain.Visible = true;
		     PagingBar1.Visible = true;
                     if (gvMain.Rows.Count <= 0)
                     {
                         lblError.Text = "No record found.";
                         lblError.Visible = true;
                         gvMain.Visible = false;
                         lblheading.Visible = true;
                         PagingBar1.Visible = false;
                     }
                 }
                 else
                 {
                     lblError.Text = "No record found.";
                     lblError.Visible = true;
                     gvMain.Visible = false;
                     lblheading.Visible = true;
        	     PagingBar1.Visible = false;
                 }
             }
         }
         catch (Exception ex)
         {
             throw ex;
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
     protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
     {
         try
         {

             if (e.Row.RowType == DataControlRowType.DataRow)
             {
                 //Encryption url of hypelink field
                 HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
                 string href = hl.NavigateUrl;
                 if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
                 {
                     href += "&ID=" + Request.QueryString["ID"].ToString();
                 }              
                 e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

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
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
                   

        try
        {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
           
                divReportData.Visible = true;
                getData();
                divReportData.Controls.Add(tbl);
                Response.Clear();
            
            Response.AddHeader("content-disposition", "attachment;filename=StudentRegdReport.xls");
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
    protected void imgPDF_Click(object sender, ImageClickEventArgs e)
    {
        if (txtFrom.Text == "")
        {
            ShowAlert("Please Select the Date From!!");
            return;
        }                 
     
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
                getData();
                divReportData.Controls.Add(tbl);            
            divReportData.RenderControl(hw);
            hw.RenderEndTag();
            Response.Clear();
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=NSQFCoursesAccrReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);            
            Response.Write(pdfDoc);
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