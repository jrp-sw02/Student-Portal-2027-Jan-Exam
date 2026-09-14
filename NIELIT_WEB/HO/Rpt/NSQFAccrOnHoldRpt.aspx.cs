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
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web;
using System.Configuration;
using System.Collections.Generic;

using System.Transactions;
using EConnect.Utils.Common;
using System.Drawing;

public partial class NSQFAccrOnHoldRpt : BasePage
    {
    Table tbl = new Table();

    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 applicantTypeID = 0;
    Int32 currentRoleId = 0, UserTypeId;

    protected void Page_Load(object sender, EventArgs e)
        {
        try
            {
            if (!IsSessionAlive())
                {
                Response.Redirect("~/index.aspx");
                }

            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);  // centreId
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);

            if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/Rpt/NSQFAccrOnHoldRpt.aspx"))
                {
                //Response.Write("Sorry! You don't have rights  to view this page");
                //Response.End();
                }

            if (!Page.IsPostBack)
                {
                //ddlcoursecategory.SelectedValue = "1";

                //vishal
                //FillCategories();
                //FillCourses();
               // BindNSQFCourse();
               // ddlCourseName.Enabled = false;
                txtPaymentFromDate.Text = System.DateTime.Today.ToString("dd-MMM-yyyy");
               // txPaymentToDate.Enabled = true;
                //vishal                
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

            tc1.ColumnSpan = 9;
           
                tc1.Text = "Report  NSQF Accredited Institutes OnHold As On " + txtPaymentFromDate.Text ;
               

            tc1.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tc1);

            tbl.Rows.Add(th1);

            TableHeaderRow th2 = new TableHeaderRow();
            th2.CssClass = "head1";

            //TableHeaderCell tc2 = new TableHeaderCell();
            //tc2.Width = Unit.Percentage(100);

            //tc2.ColumnSpan = 8;
            //tc2.Text = "Report Generated on: " + System .DateTime .Now.ToLongDateString ();
            //tc2.HorizontalAlign = HorizontalAlign.Right;
            // th2.Cells.Add(tc2);

            tbl.Rows.Add(th2);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "#";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(6);
            tcCol2.Text = "Request ID";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(25);
            tcCol6.Text = "Course Name";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(6);
            tcCol4.Text = "Accr No";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol66 = new TableHeaderCell();
            tcCol66.Width = Unit.Percentage(15);
            tcCol66.Text = "Institute Name";
            tcCol66.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol66);

            TableCell tdRow113 = new TableCell();
            tdRow113.Width = Unit.Percentage(25);
            tdRow113.Text = "Institute Address";
            tdRow113.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tdRow113);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(15);
            tcCol3.Text = "Remarks";
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol3);

            ////Date by Vishal
            //TableHeaderCell tcCol5 = new TableHeaderCell();
            //tcCol5.Width = Unit.Percentage(10);
            //tcCol5.Text = "Application Date";
            //tcCol5.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol5);

            //TableHeaderCell tcCol7 = new TableHeaderCell();
            //tcCol7.Width = Unit.Percentage(10);
            //tcCol7.Text = "OnHold Date";
            //tcCol7.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol7);
            ////Date by Vishal





            //TableHeaderCell tcCol4 = new TableHeaderCell();
            //tcCol4.Width = Unit.Percentage(10);
            //tcCol4.Text = "Validity of NSQF course";
            //tcCol4.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol4);

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

            if (txtPaymentFromDate.Text.Length == 0)
            {
                ShowAlert("Please, Enter the Valid From Date.");
                return;
            }




            DateTime FromDate = Convert.ToDateTime(txtPaymentFromDate.Text);
            //DateTime ToDate = Convert.ToDateTime(txPaymentToDate.Text);

            Int64 gtot = 0;
            //int choice = Convert.ToInt32(RdoNSQFCourseRptChoice.SelectedValue.ToString());
            // EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            //SqlCommand scCommand = new SqlCommand("getGridNSQFFreeCoursesGrantReport", new SqlConnection(con.ConnectionString));  //getGridNSQFFreeCoursesOnHoldReport
            SqlCommand scCommand = new SqlCommand("getGridNSQFFreeCoursesOnHoldReport", new SqlConnection(con.ConnectionString));  //getGridNSQFFreeCoursesOnHoldReport
            scCommand.CommandType = CommandType.StoredProcedure;
            //scCommand.Parameters.Add("@Choice", SqlDbType.Int).Value = choice;
            //scCommand.Parameters.Add("@CourseID", SqlDbType.Int).Value = 0; // Convert.ToInt32(ddlCourseName.SelectedValue);
            scCommand.Parameters.Add("@FromDate", SqlDbType.Date).Value = Convert.ToDateTime(txtPaymentFromDate.Text);
            scCommand.Parameters.Add("@ToDate", SqlDbType.Date).Value = Convert.ToDateTime(txtPaymentFromDate.Text);
            scCommand.CommandTimeout = 50000;
            if (scCommand.Connection.State == ConnectionState.Closed)
            {
                scCommand.Connection.Open();
            }
            if ((txtPaymentFromDate.Text == "01-01-1900"))
            {
                txtPaymentFromDate.Text = "";
                // txPaymentToDate.Text = "";
            }
            SqlDataAdapter da = new SqlDataAdapter(scCommand);
            DataSet ds = new DataSet();

            da.Fill(ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gtot = Convert.ToInt64(ds.Tables[0].Rows.Count);
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
                    tdRow2.Width = Unit.Percentage(6);
                    tdRow2.Text = ds.Tables[0].Rows[i]["reqId"].ToString();
                    tdRow2.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow2);

                    TableCell tdRow3 = new TableCell();
                    tdRow3.Width = Unit.Percentage(25);
                    tdRow3.Text = ds.Tables[0].Rows[i]["cName"].ToString();
                    tdRow3.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow3);

                    TableCell tdRow30 = new TableCell();
                    tdRow30.Width = Unit.Percentage(6);
                    tdRow30.Text = ds.Tables[0].Rows[i]["AccrNo"].ToString();
                    tdRow30.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow30);

                    TableCell tdRow33 = new TableCell();
                    tdRow33.Width = Unit.Percentage(15);
                    tdRow33.Text = ds.Tables[0].Rows[i]["iName"].ToString();
                    //string s = ds.Tables[0].Rows[i]["iName"].ToString();
                    //int index = s.LastIndexOf(',');   // to remove the last semicolen
                    //tdRow33.Text = s.Remove(index, 1);
                    tdRow33.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow33);

                    TableCell tdRow16 = new TableCell();
                    tdRow16.Width = Unit.Percentage(25);
                    tdRow16.Text = ds.Tables[0].Rows[i]["InstAddress"].ToString();
                    tdRow16.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow16);

                    TableCell tdRow4 = new TableCell();
                    tdRow4.Width = Unit.Percentage(15);
                    tdRow4.Text = ds.Tables[0].Rows[i]["Remarks"].ToString();
                    tdRow4.HorizontalAlign = HorizontalAlign.Center;

                    tr.Cells.Add(tdRow4);

                    ////Date By Vishal
                    //TableCell tdRow7 = new TableCell();
                    //tdRow7.Width = Unit.Percentage(10);
                    //tdRow7.Text = Convert.ToDateTime(ds.Tables[0].Rows[i]["ApplDate"]).ToString("dd/MMM/yyyy");
                    //tdRow7.HorizontalAlign = HorizontalAlign.Center;
                    //tr.Cells.Add(tdRow7);

                    //TableCell tdRow8 = new TableCell();
                    //tdRow8.Width = Unit.Percentage(10);
                    //tdRow8.Text = Convert.ToDateTime(ds.Tables[0].Rows[i]["OnHoldDate"]).ToString("dd/MMM/yyyy");
                    //tdRow8.HorizontalAlign = HorizontalAlign.Center;
                    //tr.Cells.Add(tdRow8);
                    ////Date By Vishal




                    //TableCell tdRow5 = new TableCell();
                    //tdRow5.Width = Unit.Percentage(10);
                    //tdRow5.Text = Convert.ToDateTime(ds.Tables[0].Rows[i]["ApplicationDate"]).ToString("dd/MMM/yyyy");
                    //tdRow5.HorizontalAlign = HorizontalAlign.Center;
                    //tr.Cells.Add(tdRow5);

                    tbl.Rows.Add(tr);
                }
                lblError.Visible = false;
                imPrint.Visible = true;
                ibExport.Visible = true;
                imgPDF.Visible = true;
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "No Record Found !";
                lblheading.Visible = true;
                lblheadingCandDetails.Visible = false;
                throw new Exception("No Record Found !");
            }
            scCommand.Connection.Close();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
       
    protected void RdoNSQFCourseRptChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
        //try
        //{
        //    lblheading.Visible = false;
        //    lblError.Visible = false;
        //     if (RdoNSQFCourseRptChoice.SelectedValue == "4")
        //     {
        //         ddlCourseName.Enabled = true;
        //        // ddlcourse.SelectedValue = "0";
        //         txtPaymentFromDate.Enabled = false;
        //         txPaymentToDate.Enabled = false;
        //         txtPaymentFromDate.Text = "";
        //         txPaymentToDate.Text = "";
        //     }
        //     else if (RdoNSQFCourseRptChoice.SelectedValue == "5")
        //     {
        //         ddlCourseName.Enabled = false;
        //         BindNSQFCourse();        
        //         txtPaymentFromDate.Enabled = true;
        //         txPaymentToDate.Enabled = true;               
        //     }
        //     else
        //     {
        //         ddlCourseName.Enabled = false;
        //         BindNSQFCourse(); 
        //         txtPaymentFromDate.Enabled = false;
        //         txPaymentToDate.Enabled = false;
        //         txtPaymentFromDate.Text = "";
        //         txPaymentToDate.Text = "";
        //     }                              
        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message.ToString() + ex.Source.ToString());
        //}
        }
  
    protected void btnReportChoice_Click(object sender, EventArgs e)
        {
        lblheading.Visible = true;
        tbl.CssClass = "sample3";
        tbl.CellPadding = 2;
        tbl.CellSpacing = 1;
        tbl.Width = Unit.Percentage(100);
        ShowData();
        divReportData.Controls.Add(tbl);
        divReportData.Visible = true;
        }
    protected void btnReset_Click(object sender, EventArgs e)
        {
        Response.Redirect("NSQFAccrOnHoldRpt.aspx");
        }
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
        {
        try
            {
            //vishal
        
                if (txtPaymentFromDate.Text.Length == 0)
                    {
                    ShowAlert("Please, Enter the Valid From Date.");
                    return;
                    }
               

           
            //vishal


            //if (ddlcourse.SelectedValue.ToString().Equals("0"))
            //    return;

            //if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
            //    return;

            //try
            //{
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;

            divReportData.Visible = true;
            //getData();
            ShowData();
            divReportData.Controls.Add(tbl);
            Response.Clear();

            Response.AddHeader("content-disposition", "attachment;filename=NSQFAccrOnHoldReport.xls");
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
        try
            {
            //vishal
         
                if (txtPaymentFromDate.Text.Length == 0)
                    {
                    ShowAlert("Please, Enter the Valid From Date.");
                    return;
                    }
               

         
            //vishal


            //if (ddlcourse .SelectedValue.ToString ().Equals ("0"))
            //    return;

            //if (ddlcoursecategory.SelectedValue.ToString().Equals("0"))
            //    return;

            //  try
            //{        
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
            // Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=NSQFAccrOnHoldReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Write(pdfDoc);
            Response.End();
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }

    #region other code

    protected void FillCategories()
        {
        //try
        //{
        //    using (EConnectContext context = new EConnectContext())
        //    {
        //        Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationCourse );
        //        System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");

        //        var Category = (from s in context.CourseCategories
        //                        join c in context.Courses on s.ID equals c.CourseCategoryID
        //                        where c.CourseTypeID == CourseType && s.ID==1
        //                        orderby (s.Name)
        //                        select new { ValueField = s.ID, TextField = s.Name }).Distinct();
        //        EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category, lst);
        //    };
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
        }
    protected void FillCourses()
        {
        //try
        //{
        //    using (EConnectContext context = new EConnectContext())
        //    {
        //        Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationExam);
        //        System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
        //        int id = Convert.ToInt32(ddlcoursecategory.SelectedValue);
        //        var CourseList = from p in context.Courses
        //                         where p.CourseCategoryID == id                                 
        //                         select new { ValueField = p.ID, TextField = p.Name };
        //        EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourse, CourseList, lst);
        //    };
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
        }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
        {
        //ddlcourse.Items.Clear();

        //FillCourses();
        //divReportData.Visible = false;       
        }
    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
        {

        //divReportData.Visible = false;
        //lblheading.Visible = false;
        //lblheadingCandDetails.Visible = false;
        //lblError.Text = "";
        //lblError.Visible = false;
        }

    //public override void VerifyRenderingInServerForm(Control control)
    //{
    //    /* Verifies that the control is rendered */
    //}    
    #endregion
    }