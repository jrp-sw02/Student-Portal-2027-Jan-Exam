using AjaxControlToolkit;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Metadata;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using ListItem = System.Web.UI.WebControls.ListItem;

public partial class HO_Rpt_ProjectCompleteData : BasePage
{
    Int64 entityID = 0;
    UserType loginUserType;
    Int32 currentRoleId = 0;
    Table tbl = new Table();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //if (!IsSessionAlive())
            //{
            //    Response.Redirect("~/Index.aspx");
            //    return;
            //}

            //currentRoleId = Convert.ToInt32(Session["RoleID"]);

            //if (!UserManager.HasRight(currentRoleId, enmRight.View))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}

          

            //loginUserType = (UserType)Session["UserType"];
            //entityID = Convert.ToInt64(Session["EntityID"]);
            //entityID = 45000003;
            entityID = 2492;

            if (!checkUser())
            {
                Response.Write("Sorry! You don't have rights to view this page");
                Response.End();
            }

            //entityID = 2492;
            if (!IsPostBack)
            {
                //entityID = Convert.ToInt64(Session["EntityID"]);
           //entityID = 45000003;

                tbl.Width = Unit.Percentage(100);

               entityID = 2492;

                // for O level
                //int courseID = enm

                FillProjects();
                //FillInstitutes();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected bool checkUser()
    {
        bool isHO = false;
        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))
        {
            connection.Open();
            using (var command = new SqlCommand("checkuserlogin", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@entity_id", entityID);

                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    isHO = Convert.ToBoolean(result);
                }

            }
            connection.Close();
        }
        return isHO;
    }

    protected bool isValidForm()
    {
        if (ddlproject.SelectedValue == "0")
        {
            lblerror.Visible = true;
            lblerror.Text = "Kindly choose a Project";
            return false;
        }

        return true;
    }
    protected void FillProjects()
    {
        ListItem lst1 = new ListItem(" --Select Project-- ", "0");
        try
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("nfillprojects", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();
                 //   command.Parameters.AddWithValue("@entityid", entityID);


                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        DataSet ds = new DataSet();

                        da.Fill(ds);

                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlproject, ds.Tables[0], lst1);
                    }
                }
            }
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindGridView()
    {
        try
        {
            // put validation

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("getProjectDataDownload", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@Project_Id", Convert.ToInt64(ddlproject.SelectedValue));


                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(dt);
                    }

                    uPnlGrid.Update();
                    uPnlNavigation.Update();

                    if (dt.Rows.Count > 0)
                    {
                        gvMain.Visible = true;
                        PagingBar1.Visible = true;
                        //gvMain.DataSource = dt;
                        //gvMain.DataBind();

                        PagingBar1.Bind(dt, ref gvMain);

                        // Apply alternating row styles
                        for (int i = 0; i < gvMain.Rows.Count; i++)
                        {
                            gvMain.Rows[i].CssClass = (i % 2 == 0) ? "gdalternate1" : "gdrow1";
                        }


                        btnDownload.Visible = true;
                        btnDownloadpdf.Visible = true;
                        btnShowData.Visible = false;
                        lblerror.Visible = false;
                        lblMessage.Visible = false;
                    }
                    else
                    {
                        btnDownload.Visible = false;
                        btnDownloadpdf.Visible = false ;
                        lblMessage.Visible = true;
                        lblMessage.Text = dt.Rows.Count == 0 ? "No record found." : "";
                    }
                }
                connection.Close();
            }
        }
        catch (Exception ex)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "An error occurred: " + ex.Message;
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
    protected void btnShowData_Click(object sender, EventArgs e)
    {
        
        if (!isValidForm())
            return;

        BindGridView();

    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {

        //Query to fetch data
        System.IO.StringWriter StringWrite = new System.IO.StringWriter();
        Html32TextWriter htmlWrite;

        ShowData();

        Response.AddHeader("content-disposition", "attachment;filename=Student_Report.xls");
        Response.Charset = "";
        //Response.ContentType = "application/vnd.xls";
        Response.ContentType = "application/vnd.ms-excel";
        htmlWrite = new Html32TextWriter(StringWrite);
        tbl.RenderControl(htmlWrite);
        Response.Write(StringWrite.ToString());

        Response.End();
    }
    protected void btnDownloadpdf_Click(object sender, EventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("getProjectDataDownload", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Project_Id", ddlproject.SelectedValue);
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(ds);
                    }
                }
            }

            if (ds.Tables[0].Rows.Count == 0)
            {
                ShowAlert("No data to export.");
                return;
            }

            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
            pdfDoc.Open();

            
            // important to change, if number of columns changes
            PdfPTable pdfTable = new PdfPTable(32);
            pdfTable.WidthPercentage = 100;
            //float[] widths = { 2, 5, 10, 5, 3, 4, 5, 5, 5, 10, 10, 10, 8, 8, 4, 5, 4, 4, 15, 15, 5, 10, 5,5, 5,10, 5, 10, 5, 5, 5, 5 ,5};
            int[] widths = { 1, 3, 3, 2, 3, 5, 2, 2, 2, 2, 2, 2, 2, 2, 3, 2, 3, 3, 3, 3, 2, 4, 2, 2, 2, 4, 2, 2, 2, 2, 2, 2 };
            pdfTable.SetWidths(widths);



       
            Font titleFont = new Font(Font.FontFamily.HELVETICA, 8f, Font.BOLD, BaseColor.WHITE);
            Font headerFont = new Font(Font.FontFamily.HELVETICA, 4f, Font.BOLD, BaseColor.WHITE);
            Font cellFont = new Font(Font.FontFamily.HELVETICA, 4f, Font.NORMAL, BaseColor.BLACK);

            string imagePath = Server.MapPath("../../images/NIELIT-Logo.png"); 


            PdfPTable headertable = new PdfPTable(2);
            headertable.WidthPercentage = 100;
            headertable.SetWidths(new float[] { 1f, 3f });

            // Wrap the image in a cell
            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagePath);
            logo.ScaleToFit(100f, 100f);
            logo.Alignment = Element.ALIGN_CENTER;

            PdfPCell logoCell = new PdfPCell(logo);
            logoCell.Border = PdfPCell.NO_BORDER;
            logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headertable.AddCell(logoCell);

            // TEXT CODE START
            Font font1 = new Font(Font.FontFamily.HELVETICA, 10f, Font.BOLD, BaseColor.BLACK);
            Font font2 = new Font(Font.FontFamily.HELVETICA, 10f, Font.NORMAL, BaseColor.BLACK);
            Paragraph text = new Paragraph();
            text.Add(new Chunk("National Institute of Electronics & Information Technology\n", font1));
            text.Add(new Chunk("Ministry of Electronics & Information Technology\n", font1));
            text.Add(new Chunk("Government of India", font1));

            PdfPCell textCell = new PdfPCell(text);
            textCell.Border = PdfPCell.NO_BORDER;
            textCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headertable.AddCell(textCell);


            pdfDoc.Add(headertable);
            pdfDoc.Add(new Paragraph("\n"));


         

            BaseColor headerBg = new BaseColor(39, 59, 110);
            BaseColor alternateRowBg = new BaseColor(230, 240, 240); // #E6F0F0 light blue-ish
            BaseColor rowBg = new BaseColor(201, 215, 226); // #c9d7e2 light blue

            // Add main title header
            PdfPCell cell = new PdfPCell(new Phrase("Project Complete Data", titleFont));
            cell.Colspan = 32;  
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = headerBg;
            pdfTable.AddCell(cell);

            // Add generated date header
            cell = new PdfPCell(new Phrase("As on: " + System.DateTime.Now.ToLongDateString(), titleFont));
            cell.Colspan = 32;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = headerBg;
            pdfTable.AddCell(cell);

            // Add column headers
            string[] headers = { "#", "NIELIT Centre", "Online Ref. No.", "Reg. No.", "Batch Name", "Candidate Name", "Father Name", "Mother Name", "Caste Category", "EWS", "Handicapped", "Ex ServiceMan", "Gender", "Date of Birth", "Course", "Batch Start Date", "Batch End Date", "District", "Training Partner", "Candidate Mobile", "Candidate Email", "Correspondence Address", "District", "State", "Pin Code", "Permanent Address", "District", "State", "Pin Code", "Course Complete", "Certificate Issued", "Drop Out" };
            int ans = headers.Length;
            foreach (string header in headers)
            {
                cell = new PdfPCell(new Phrase(header, headerFont));
                cell.BackgroundColor = headerBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);
            }

            int testing = 10;

            loadingtxt.Text = "setirnsdf"; //test


            // Add data rows with alternating blue shades
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {


                testing = i;

                BaseColor currentRowBg = (i % 2 == 0) ? alternateRowBg : rowBg; 
                // Sr. No.
                cell = new PdfPCell(new Phrase((i + 1).ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Assuming 'ds' is a DataSet, 'pdfTable' is a PdfPTable, 'cellFont' is a Font, 'currentRowBg' is a BaseColor, and 'i' is the row index
               

                // NIELIT Centre
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["NIELIT Centre"].ToString()) ? "-" : ds.Tables[0].Rows[i]["NIELIT Centre"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Online Reference Number
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Online Reference Number"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Online Reference Number"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Registration Number
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Registration Number"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Registration Number"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Batch Name
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Batch Name"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Batch Name"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Candidate Name
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Candidate Name"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Candidate Name"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Father Name
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Father Name"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Father Name"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Mother Name
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Mother Name"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Mother Name"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);


                // Caste Category
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Caste Category"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Caste Category"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // EWS
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["EWS"].ToString()) ? "-" : ds.Tables[0].Rows[i]["EWS"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Handicapped
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Handicapped"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Handicapped"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Ex ServiceMan
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Ex ServiceMan"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Ex ServiceMan"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);


                // Gender
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Gender"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Gender"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Date of Birth
                cell = new PdfPCell(new Phrase(ds.Tables[0].Rows[i]["Date of Birth"] == DBNull.Value ? "-" : Convert.ToDateTime(ds.Tables[0].Rows[i]["Date of Birth"]).ToString("dd-MMM-yyyy"), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);
              

                // Course
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Course"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Course"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Batch Start Date
                cell = new PdfPCell(new Phrase(ds.Tables[0].Rows[i]["Batch Start Date"] == DBNull.Value ? "-" : Convert.ToDateTime(ds.Tables[0].Rows[i]["Batch Start Date"]).ToString("dd-MMM-yyyy"), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Batch End Date
                cell = new PdfPCell(new Phrase(ds.Tables[0].Rows[i]["Batch End Date"] == DBNull.Value ? "-" : Convert.ToDateTime(ds.Tables[0].Rows[i]["Batch End Date"]).ToString("dd-MMM-yyyy"), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // District
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["District"].ToString()) ? "-" : ds.Tables[0].Rows[i]["District"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Training Partner
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Training Partner"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Training Partner"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Candidate Mobile
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Candidate Mobile"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Candidate Mobile"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Candidate Email
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Candidate Email"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Candidate Email"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Correspondence Address
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Correspondence Address"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Correspondence Address"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Correspondence District
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["C_District"].ToString()) ? "-" : ds.Tables[0].Rows[i]["C_District"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Correspondence State
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["C_State"].ToString()) ? "-" : ds.Tables[0].Rows[i]["C_State"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Correspondence Pin Code
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["C_Pin Code"].ToString()) ? "-" : ds.Tables[0].Rows[i]["C_Pin Code"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Permanent Address
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Permanent Address"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Permanent Address"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Permanent District
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["P_District"].ToString()) ? "-" : ds.Tables[0].Rows[i]["P_District"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Permanent State
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["P_State"].ToString()) ? "-" : ds.Tables[0].Rows[i]["P_State"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Permanent Pin Code
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["P_Pin Code"].ToString()) ? "-" : ds.Tables[0].Rows[i]["P_Pin Code"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Course Complete
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Course Complete"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Course Complete"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Certificate Issued
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Certficate Issued"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Certficate Issued"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // Drop Out
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Drop Out"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Drop Out"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

     
            }

            // Add table to document
            pdfDoc.Add(pdfTable);
            pdfDoc.Close();
            writer.Close();

            // Write to response
            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=student_report.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(memoryStream.ToArray());
            Response.Flush();

            // loading code
            loadingtxt.Text = Convert.ToString(testing) + '%';

            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ShowData()
    {
        try
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("getProjectDataDownload", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@Project_Id", ddlproject.SelectedValue);

                    DataSet ds = new DataSet();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(ds);
                    }

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

                            // Sr. No.
                            TableCell tdSrNo = new TableCell();
                            tdSrNo.Width = Unit.Percentage(3);
                            tdSrNo.Text = (i + 1).ToString();
                            tdSrNo.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdSrNo);
                            // Assuming 'ds' is a DataSet, 'tbl' is a Table control, 'tr' is a TableRow, and 'i' is the row index

                            // NIELIT Centre
                            TableCell tdNIELITCentre = new TableCell();
                            tdNIELITCentre.Width = Unit.Percentage(5);
                            tdNIELITCentre.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["NIELIT Centre"].ToString()) ? "-" : ds.Tables[0].Rows[i]["NIELIT Centre"].ToString();
                            tdNIELITCentre.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdNIELITCentre);

                            // Online Reference Number
                            TableCell tdOnlineReferenceNumber = new TableCell();
                            tdOnlineReferenceNumber.Width = Unit.Percentage(5);
                            tdOnlineReferenceNumber.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Online Reference Number"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Online Reference Number"].ToString();
                            tdOnlineReferenceNumber.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdOnlineReferenceNumber);

                            // Registration Number
                            TableCell tdRegistrationNumber = new TableCell();
                            tdRegistrationNumber.Width = Unit.Percentage(5);
                            tdRegistrationNumber.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Registration Number"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Registration Number"].ToString();
                            tdRegistrationNumber.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRegistrationNumber);

                            // Batch Name
                            TableCell tdBatchName = new TableCell();
                            tdBatchName.Width = Unit.Percentage(5);
                            tdBatchName.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Batch Name"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Batch Name"].ToString();
                            tdBatchName.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdBatchName);

                            // Candidate Name
                            TableCell tdCandidateName = new TableCell();
                            tdCandidateName.Width = Unit.Percentage(8);
                            tdCandidateName.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Candidate Name"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Candidate Name"].ToString();
                            tdCandidateName.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdCandidateName);

                            // Father Name
                            TableCell tdFatherName = new TableCell();
                            tdFatherName.Width = Unit.Percentage(8);
                            tdFatherName.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Father Name"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Father Name"].ToString();
                            tdFatherName.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdFatherName);

                            // Mother Name
                            TableCell tdMotherName = new TableCell();
                            tdMotherName.Width = Unit.Percentage(8);
                            tdMotherName.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Mother Name"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Mother Name"].ToString();
                            tdMotherName.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdMotherName);


                            // Caste Category
                            TableCell tdCasteCategory = new TableCell();
                            tdCasteCategory.Width = Unit.Percentage(5);
                            tdCasteCategory.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Caste Category"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Caste Category"].ToString();
                            tdCasteCategory.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdCasteCategory);

                            // EWS
                            TableCell tdEWS = new TableCell();
                            tdEWS.Width = Unit.Percentage(4);
                            tdEWS.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["EWS"].ToString()) ? "-" : ds.Tables[0].Rows[i]["EWS"].ToString();
                            tdEWS.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdEWS);

                            // Handicapped
                            TableCell tdHandicapped = new TableCell();
                            tdHandicapped.Width = Unit.Percentage(4);
                            tdHandicapped.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Handicapped"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Handicapped"].ToString();
                            tdHandicapped.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdHandicapped);

                            // Ex-Servicemen
                            TableCell tdExServiceMan = new TableCell();
                            tdExServiceMan.Width = Unit.Percentage(4);
                            tdExServiceMan.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Ex ServiceMan"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Ex ServiceMan"].ToString();
                            tdExServiceMan.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdExServiceMan);

                            // Gender
                            TableCell tdGender = new TableCell();
                            tdGender.Width = Unit.Percentage(4);
                            tdGender.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Gender"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Gender"].ToString();
                            tdGender.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdGender);



                            // Date of Birth
                            TableCell tdDOB = new TableCell();
                            tdDOB.Width = Unit.Percentage(5);
                            tdDOB.Text = ds.Tables[0].Rows[i]["Date of Birth"] == DBNull.Value ? "-" : Convert.ToDateTime(ds.Tables[0].Rows[i]["Date of Birth"]).ToString("dd-MMM-yyyy");
                            tdDOB.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdDOB);

                 
                          

                            // Course Enrolled
                            TableCell tdCourse = new TableCell();
                            tdCourse.Width = Unit.Percentage(6);
                            tdCourse.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Course"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Course"].ToString();
                            tdCourse.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdCourse);

                            // Batch Start Date
                            TableCell tdBatchStartDate = new TableCell();
                            tdBatchStartDate.Width = Unit.Percentage(5);
                            tdBatchStartDate.Text = ds.Tables[0].Rows[i]["Batch Start Date"] == DBNull.Value ? "-" : Convert.ToDateTime(ds.Tables[0].Rows[i]["Batch Start Date"]).ToString("dd-MMM-yyyy");
                            tdBatchStartDate.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdBatchStartDate);

                            // Batch End Date
                            TableCell tdBatchEndDate = new TableCell();
                            tdBatchEndDate.Width = Unit.Percentage(5);
                            tdBatchEndDate.Text = ds.Tables[0].Rows[i]["Batch End Date"] == DBNull.Value ? "-" : Convert.ToDateTime(ds.Tables[0].Rows[i]["Batch End Date"]).ToString("dd-MMM-yyyy");
                            tdBatchEndDate.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdBatchEndDate);

                            // District
                            TableCell tdDistrict = new TableCell();
                            tdDistrict.Width = Unit.Percentage(5);
                            tdDistrict.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["District"].ToString()) ? "-" : ds.Tables[0].Rows[i]["District"].ToString();
                            tdDistrict.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdDistrict);

                            // Training Partner
                            TableCell tdTrainingPartner = new TableCell();
                            tdTrainingPartner.Width = Unit.Percentage(6);
                            tdTrainingPartner.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Training Partner"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Training Partner"].ToString();
                            tdTrainingPartner.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdTrainingPartner);

                            // Candidate Mobile
                            TableCell tdCandidateMobile = new TableCell();
                            tdCandidateMobile.Width = Unit.Percentage(5);
                            tdCandidateMobile.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Candidate Mobile"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Candidate Mobile"].ToString();
                            tdCandidateMobile.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdCandidateMobile);

                            // Candidate Email
                            TableCell tdCandidateEmail = new TableCell();
                            tdCandidateEmail.Width = Unit.Percentage(6);
                            tdCandidateEmail.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Candidate Email"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Candidate Email"].ToString();
                            tdCandidateEmail.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdCandidateEmail);

                            // Correspondence Address
                            TableCell tdCorrespondenceAddress = new TableCell();
                            tdCorrespondenceAddress.Width = Unit.Percentage(8);
                            tdCorrespondenceAddress.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Correspondence Address"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Correspondence Address"].ToString();
                            tdCorrespondenceAddress.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdCorrespondenceAddress);

                            // Correspondence District
                            TableCell tdCDistrict = new TableCell();
                            tdCDistrict.Width = Unit.Percentage(5);
                            tdCDistrict.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["C_District"].ToString()) ? "-" : ds.Tables[0].Rows[i]["C_District"].ToString();
                            tdCDistrict.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdCDistrict);

                            // Correspondence State
                            TableCell tdCState = new TableCell();
                            tdCState.Width = Unit.Percentage(5);
                            tdCState.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["C_State"].ToString()) ? "-" : ds.Tables[0].Rows[i]["C_State"].ToString();
                            tdCState.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdCState);

                            // Correspondence Pin Code
                            TableCell tdCPinCode = new TableCell();
                            tdCPinCode.Width = Unit.Percentage(5);
                            tdCPinCode.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["C_Pin Code"].ToString()) ? "-" : ds.Tables[0].Rows[i]["C_Pin Code"].ToString();
                            tdCPinCode.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdCPinCode);

                            // Permanent Address
                            TableCell tdPermanentAddress = new TableCell();
                            tdPermanentAddress.Width = Unit.Percentage(8);
                            tdPermanentAddress.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Permanent Address"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Permanent Address"].ToString();
                            tdPermanentAddress.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdPermanentAddress);

                            // Permanent District
                            TableCell tdPDistrict = new TableCell();
                            tdPDistrict.Width = Unit.Percentage(5);
                            tdPDistrict.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["P_District"].ToString()) ? "-" : ds.Tables[0].Rows[i]["P_District"].ToString();
                            tdPDistrict.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdPDistrict);

                            // Permanent State
                            TableCell tdPState = new TableCell();
                            tdPState.Width = Unit.Percentage(5);
                            tdPState.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["P_State"].ToString()) ? "-" : ds.Tables[0].Rows[i]["P_State"].ToString();
                            tdPState.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdPState);

                            // Permanent Pin Code
                            TableCell tdPPinCode = new TableCell();
                            tdPPinCode.Width = Unit.Percentage(5);
                            tdPPinCode.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["P_Pin Code"].ToString()) ? "-" : ds.Tables[0].Rows[i]["P_Pin Code"].ToString();
                            tdPPinCode.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdPPinCode);

                            // Course Completion Status
                            TableCell tdCourseComplete = new TableCell();
                            tdCourseComplete.Width = Unit.Percentage(5);
                            tdCourseComplete.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Course Complete"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Course Complete"].ToString();
                            tdCourseComplete.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdCourseComplete);

                            // Certificate Issued
                            TableCell tdCertificateIssued = new TableCell();
                            tdCertificateIssued.Width = Unit.Percentage(5);
                            tdCertificateIssued.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Certficate Issued"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Certficate Issued"].ToString();
                            tdCertificateIssued.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdCertificateIssued);

                            // Dropout Status
                            TableCell tdDropOut = new TableCell();
                            tdDropOut.Width = Unit.Percentage(5);
                            tdDropOut.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Drop Out"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Drop Out"].ToString();
                            tdDropOut.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdDropOut);

                            
                            tbl.Rows.Add(tr);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Error : " + ex.Message);
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

            tc1.ColumnSpan = 22;
            tc1.Text = "Project Complete Data";

            tc1.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tc1);

            tbl.Rows.Add(th1);

            TableHeaderRow th2 = new TableHeaderRow();
            th2.CssClass = "head1";

            TableHeaderCell tc2 = new TableHeaderCell();
            tc2.Width = Unit.Percentage(100);

            tc2.ColumnSpan = 22;
            tc2.Text = "Generated on: " + System.DateTime.Now.ToLongDateString();
            tc2.HorizontalAlign = HorizontalAlign.Center;
            th2.Cells.Add(tc2);

            tbl.Rows.Add(th2);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            // Sr. No.
            TableHeaderCell thSrNo = new TableHeaderCell();
            thSrNo.Width = Unit.Percentage(2);
            thSrNo.Text = "Sr. No.";
            thSrNo.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thSrNo);


            // NIELIT Centre
            TableHeaderCell thNIELITCentre = new TableHeaderCell();
            thNIELITCentre.Width = Unit.Percentage(5);
            thNIELITCentre.Text = "NIELIT Centre";
            thNIELITCentre.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thNIELITCentre);

            // Online Reference Number
            TableHeaderCell thOnlineReferenceNumber = new TableHeaderCell();
            thOnlineReferenceNumber.Width = Unit.Percentage(5);
            thOnlineReferenceNumber.Text = "Online Reference Number";
            thOnlineReferenceNumber.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thOnlineReferenceNumber);

            // Registration Number
            TableHeaderCell thRegistrationNumber = new TableHeaderCell();
            thRegistrationNumber.Width = Unit.Percentage(5);
            thRegistrationNumber.Text = "Registration Number";
            thRegistrationNumber.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thRegistrationNumber);

            // Batch Name
            TableHeaderCell thBatchName = new TableHeaderCell();
            thBatchName.Width = Unit.Percentage(5);
            thBatchName.Text = "Batch Name";
            thBatchName.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thBatchName);

            // Candidate Name
            TableHeaderCell thCandidateName = new TableHeaderCell();
            thCandidateName.Width = Unit.Percentage(8);
            thCandidateName.Text = "Candidate Name";
            thCandidateName.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thCandidateName);

            // Father Name
            TableHeaderCell thFatherName = new TableHeaderCell();
            thFatherName.Width = Unit.Percentage(8);
            thFatherName.Text = "Father Name";
            thFatherName.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thFatherName);

            // Mother Name
            TableHeaderCell thMotherName = new TableHeaderCell();
            thMotherName.Width = Unit.Percentage(8);
            thMotherName.Text = "Mother Name";
            thMotherName.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thMotherName);



            // Caste Category
            TableHeaderCell thCasteCategory = new TableHeaderCell();
            thCasteCategory.Width = Unit.Percentage(5);
            thCasteCategory.Text = "Caste Category";
            thCasteCategory.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thCasteCategory);

            // EWS
            TableHeaderCell thEWS = new TableHeaderCell();
            thEWS.Width = Unit.Percentage(4);
            thEWS.Text = "EWS";
            thEWS.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thEWS);

            // Handicapped
            TableHeaderCell thHandicapped = new TableHeaderCell();
            thHandicapped.Width = Unit.Percentage(4);
            thHandicapped.Text = "Handicapped";
            thHandicapped.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thHandicapped);

            // Ex-Servicemen
            TableHeaderCell thExServiceMan = new TableHeaderCell();
            thExServiceMan.Width = Unit.Percentage(4);
            thExServiceMan.Text = "Ex-Servicemen";
            thExServiceMan.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thExServiceMan);

            // Gender
            TableHeaderCell thGender = new TableHeaderCell();
            thGender.Width = Unit.Percentage(4);
            thGender.Text = "Gender";
            thGender.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thGender);


            // Date of Birth
            TableHeaderCell thDateOfBirth = new TableHeaderCell();
            thDateOfBirth.Width = Unit.Percentage(5);
            thDateOfBirth.Text = "Date of Birth";
            thDateOfBirth.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thDateOfBirth);

            // Course Enrolled
            TableHeaderCell thCourse = new TableHeaderCell();
            thCourse.Width = Unit.Percentage(6);
            thCourse.Text = "Course Enrolled";
            thCourse.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thCourse);





            // Batch Start Date
            TableHeaderCell thBatchStartDate = new TableHeaderCell();
            thBatchStartDate.Width = Unit.Percentage(5);
            thBatchStartDate.Text = "Batch Start Date";
            thBatchStartDate.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thBatchStartDate);

            // Batch End Date
            TableHeaderCell thBatchEndDate = new TableHeaderCell();
            thBatchEndDate.Width = Unit.Percentage(5);
            thBatchEndDate.Text = "Batch End Date";
            thBatchEndDate.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thBatchEndDate);

            // District
            TableHeaderCell thDistrict = new TableHeaderCell();
            thDistrict.Width = Unit.Percentage(5);
            thDistrict.Text = "District";
            thDistrict.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thDistrict);

            // Training Partner
            TableHeaderCell thTrainingPartner = new TableHeaderCell();
            thTrainingPartner.Width = Unit.Percentage(6);
            thTrainingPartner.Text = "Training Partner";
            thTrainingPartner.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thTrainingPartner);

            // Candidate Mobile
            TableHeaderCell thCandidateMobile = new TableHeaderCell();
            thCandidateMobile.Width = Unit.Percentage(5);
            thCandidateMobile.Text = "Candidate Mobile";
            thCandidateMobile.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thCandidateMobile);

            // Candidate Email
            TableHeaderCell thCandidateEmail = new TableHeaderCell();
            thCandidateEmail.Width = Unit.Percentage(6);
            thCandidateEmail.Text = "Candidate Email";
            thCandidateEmail.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thCandidateEmail);

            // Correspondence Address
            TableHeaderCell thCorrespondenceAddress = new TableHeaderCell();
            thCorrespondenceAddress.Width = Unit.Percentage(8);
            thCorrespondenceAddress.Text = "Correspondence Address";
            thCorrespondenceAddress.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thCorrespondenceAddress);

            // Correspondence District
            TableHeaderCell thCDistrict = new TableHeaderCell();
            thCDistrict.Width = Unit.Percentage(5);
            thCDistrict.Text = "Correspondence District";
            thCDistrict.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thCDistrict);

            // Correspondence State
            TableHeaderCell thCState = new TableHeaderCell();
            thCState.Width = Unit.Percentage(5);
            thCState.Text = "Correspondence State";
            thCState.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thCState);

            // Correspondence Pin Code
            TableHeaderCell thCPinCode = new TableHeaderCell();
            thCPinCode.Width = Unit.Percentage(5);
            thCPinCode.Text = "Correspondence Pin Code";
            thCPinCode.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thCPinCode);

            // Permanent Address
            TableHeaderCell thPermanentAddress = new TableHeaderCell();
            thPermanentAddress.Width = Unit.Percentage(8);
            thPermanentAddress.Text = "Permanent Address";
            thPermanentAddress.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thPermanentAddress);

            // Permanent District
            TableHeaderCell thPDistrict = new TableHeaderCell();
            thPDistrict.Width = Unit.Percentage(5);
            thPDistrict.Text = "Permanent District";
            thPDistrict.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thPDistrict);

            // Permanent State
            TableHeaderCell thPState = new TableHeaderCell();
            thPState.Width = Unit.Percentage(5);
            thPState.Text = "Permanent State";
            thPState.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thPState);

            // Permanent Pin Code
            TableHeaderCell thPPinCode = new TableHeaderCell();
            thPPinCode.Width = Unit.Percentage(5);
            thPPinCode.Text = "Permanent Pin Code";
            thPPinCode.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thPPinCode);

            // Course Completion Status
            TableHeaderCell thCourseComplete = new TableHeaderCell();
            thCourseComplete.Width = Unit.Percentage(5);
            thCourseComplete.Text = "Course Completion Status";
            thCourseComplete.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thCourseComplete);

            // Certificate Issued
            TableHeaderCell thCertificateIssued = new TableHeaderCell();
            thCertificateIssued.Width = Unit.Percentage(5);
            thCertificateIssued.Text = "Certificate Issued";
            thCertificateIssued.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thCertificateIssued);

            // Dropout Status
            TableHeaderCell thDropOut = new TableHeaderCell();
            thDropOut.Width = Unit.Percentage(5);
            thDropOut.Text = "Dropout Status";
            thDropOut.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thDropOut);


            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        //gvMain.DataSource = null;
        //gvMain.DataBind();
        gvMain.Visible = false;
        DataTable dt = new DataTable();
        lblMessage.Text = string.Empty;
        lblerror.Visible = false;
        ddlproject.SelectedIndex = 0;
        
        uPnlGrid.Update();
        gvMain.DataSource = null;
        gvMain.Visible = false;
        PagingBar1.Visible = false;



        btnDownload.Visible = false;
        btnDownloadpdf.Visible = false;

        btnShowData.Visible = true;
        lblMessage.Visible = false;
    }

}