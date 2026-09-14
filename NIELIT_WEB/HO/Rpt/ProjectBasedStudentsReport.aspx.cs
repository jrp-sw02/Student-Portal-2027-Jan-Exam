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

public partial class HO_Rpt_ProjectBasedStudentsReport : BasePage
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
            entityID = 45000003;
            //entityID = 2492;



            //entityID = 2492;
            if (!IsPostBack)
            {
                entityID = Convert.ToInt64(Session["EntityID"]);
                entityID = 45000003;

                tbl.Width = Unit.Percentage(100);

           

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

        if (ddlinstitute.SelectedValue == "0")
        {
            lblerror.Visible = true;
            lblerror.Text = "Kindly choose a Institute";
            return false;
        }

        if (string.IsNullOrWhiteSpace(txtdate1.Text) || string.IsNullOrWhiteSpace(txtdate2.Text))
        {
            lblerror.Visible = true;
            lblerror.Text = "Please enter both dates.";
            return false;
        }

        try
        {
            DateTime date1 = DateTime.Parse(txtdate1.Text);
            DateTime date2 = DateTime.Parse(txtdate2.Text);

            if (date1 > date2)
            {
                lblerror.Visible = true;
                lblerror.Text = "The first date cannot be later than the second date.";
                return false;
            }
        }
        catch
        {
            lblerror.Visible = true;
            lblerror.Text = "Please enter valid dates.";
            return false;
        }


        return true;
    }
    protected void Ddlproject_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlproject.SelectedIndex == 0)
        {
            ddlinstitute.Items.Clear();
            ddlinstitute.Items.Insert(0, new ListItem("--Select Institute--", "0"));
        }
        else
        {
            FillInstitutes();
        }
    }
    protected void FillProjects()
    {
        ListItem lst1 = new ListItem(" --Select Project-- ", "0");
        try
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("fillprojects", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@entityid", entityID);


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
    protected void FillInstitutes()
    {
        //Int32 CourseID = Convert.ToInt32(Request.QueryString["id"]);
        Int64 projectid = Convert.ToInt64(ddlproject.SelectedValue);
        // if institute didnt logged in 
        ListItem lst1 = new ListItem("--Select Institute--", "0");

        // filling the institute using procedure start

        try
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("fillreportInstitute", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@project_id", projectid);
                    command.Parameters.AddWithValue("@entityid", entityID);

                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlinstitute, ds.Tables[0], lst1);


                        ddlinstitute.DataValueField = "centreID";
                        ddlinstitute.DataTextField = "Name";
                        ddlinstitute.DataBind();

                        ddlinstitute.Items.Insert(0, lst1);


                        //NOT HEADOFFICE
                        if (!checkUser())
                        {
                            ddlinstitute.SelectedIndex = 1;
                            ddlinstitute.Enabled = false;
                        }
                        else
                        {
                            ddlinstitute.Enabled = true;
                        }

                
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

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("getdataforProjects", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@batch_start", Convert.ToDateTime(txtdate1.Text));
                    command.Parameters.AddWithValue("@batch_end", Convert.ToDateTime(txtdate2.Text));
                    command.Parameters.AddWithValue("@project_id", Convert.ToInt64(ddlproject.SelectedValue));
                    command.Parameters.AddWithValue("@centre_ID", Convert.ToInt64(ddlinstitute.SelectedValue));


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
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("getdataforProjects", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@batch_start", Convert.ToDateTime(txtdate1.Text));
                    command.Parameters.AddWithValue("@batch_end", Convert.ToDateTime(txtdate2.Text));
                    command.Parameters.AddWithValue("@project_id", ddlproject.SelectedValue);
                    command.Parameters.AddWithValue("@centre_ID", ddlinstitute.SelectedValue);

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

            
            // Change number of columns and widths here to match the boundfields
            PdfPTable pdfTable = new PdfPTable(22);
            pdfTable.WidthPercentage = 100;
            float[] widths = { 2, 5, 10, 5, 3, 4, 5, 5, 5, 10, 10, 10, 8, 8, 4, 5, 4, 4, 15, 15, 5, 10 };
            pdfTable.SetWidths(widths);

       
            Font titleFont = new Font(Font.FontFamily.HELVETICA, 8f, Font.BOLD, BaseColor.WHITE);
            Font headerFont = new Font(Font.FontFamily.HELVETICA, 5f, Font.BOLD, BaseColor.WHITE);
            Font cellFont = new Font(Font.FontFamily.HELVETICA, 5f, Font.NORMAL, BaseColor.BLACK);

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
            PdfPCell cell = new PdfPCell(new Phrase("Project Based Schools Student Enrolment Report", titleFont));
            cell.Colspan = 22;  
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = headerBg;
            pdfTable.AddCell(cell);

            // Add generated date header
            cell = new PdfPCell(new Phrase("As on: " + System.DateTime.Now.ToLongDateString(), titleFont));
            cell.Colspan = 22;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = headerBg;
            pdfTable.AddCell(cell);

            // Add column headers
            string[] headers = { "#", "Udise Code", "Institute Name", "Institute Accr No.", "Class", "Roll No", "Application No.", "Regn No.", "Course Enrolled", "Name", "Father name", "Guardian name", "Mother name", "Date of Birth", "Gender", "Cast Category", "EWS", "Handicapped", "Corr. Address", "Permanent Address", "Payment Status", "Application Status" };
            foreach (string header in headers)
            {
                cell = new PdfPCell(new Phrase(header, headerFont));
                cell.BackgroundColor = headerBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);
            }

            // Add data rows with alternating blue shades
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                BaseColor currentRowBg = (i % 2 == 0) ? alternateRowBg : rowBg;

                // Sr. No.
                cell = new PdfPCell(new Phrase((i + 1).ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(cell);

                // UDISECode
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["UDISECode"].ToString()) ? "-" : ds.Tables[0].Rows[i]["UDISECode"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // InstituteName
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["InstituteName"].ToString()) ? "-" : ds.Tables[0].Rows[i]["InstituteName"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // InstituteAccrNo
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["InstituteAccrNo"].ToString()) ? "-" : ds.Tables[0].Rows[i]["InstituteAccrNo"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // Class
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Class"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Class"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // RollNo
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["RollNo"].ToString()) ? "-" : ds.Tables[0].Rows[i]["RollNo"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // ReferenceNo
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ReferenceNo"].ToString()) ? "-" : ds.Tables[0].Rows[i]["ReferenceNo"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // RegistrationNo
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["RegistrationNo"].ToString()) ? "-" : ds.Tables[0].Rows[i]["RegistrationNo"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // CourseEnrolled
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CourseEnrolled"].ToString()) ? "-" : ds.Tables[0].Rows[i]["CourseEnrolled"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // Name
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Name"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Name"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // FatherName
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["FatherName"].ToString()) ? "-" : ds.Tables[0].Rows[i]["FatherName"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // GuardianName
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["GuardianName"].ToString()) ? "-" : ds.Tables[0].Rows[i]["GuardianName"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // MotherName
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["MotherName"].ToString()) ? "-" : ds.Tables[0].Rows[i]["MotherName"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // DOB
                cell = new PdfPCell(new Phrase(ds.Tables[0].Rows[i]["DOB"] == DBNull.Value ? "-" : Convert.ToDateTime(ds.Tables[0].Rows[i]["DOB"]).ToString("dd-MMM-yyyy"), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // Gender
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Gender"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Gender"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // Category
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Category"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Category"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // EWS
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["EWS"].ToString()) ? "-" : ds.Tables[0].Rows[i]["EWS"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // Handicapped
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Handicapped"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Handicapped"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // Address
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Address"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Address"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // PermanentAddress
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PermanentAddress"].ToString()) ? "-" : ds.Tables[0].Rows[i]["PermanentAddress"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // PaymentStatus
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PaymentStatus"].ToString()) ? "-" : ds.Tables[0].Rows[i]["PaymentStatus"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                // ApplicationStatus
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ApplicationStatus"].ToString()) ? "-" : ds.Tables[0].Rows[i]["ApplicationStatus"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
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
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("getdataforProjects", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@batch_start", Convert.ToDateTime(txtdate1.Text));
                    command.Parameters.AddWithValue("@batch_end", Convert.ToDateTime(txtdate2.Text));
                    command.Parameters.AddWithValue("@project_id", ddlproject.SelectedValue);
                    command.Parameters.AddWithValue("@centre_ID", ddlinstitute.SelectedValue);

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

                            // UDISECode
                            TableCell tdUDISECode = new TableCell();
                            tdUDISECode.Width = Unit.Percentage(5);
                            tdUDISECode.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["UDISECode"].ToString()) ? "-" : ds.Tables[0].Rows[i]["UDISECode"].ToString();
                            tdUDISECode.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdUDISECode);

                            // InstituteName
                            TableCell tdInstituteName = new TableCell();
                            tdInstituteName.Width = Unit.Percentage(10);
                            tdInstituteName.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["InstituteName"].ToString()) ? "-" : ds.Tables[0].Rows[i]["InstituteName"].ToString();
                            tdInstituteName.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdInstituteName);

                            // InstituteAccrNo
                            TableCell tdInstituteAccrNo = new TableCell();
                            tdInstituteAccrNo.Width = Unit.Percentage(5);
                            tdInstituteAccrNo.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["InstituteAccrNo"].ToString()) ? "-" : ds.Tables[0].Rows[i]["InstituteAccrNo"].ToString();
                            tdInstituteAccrNo.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdInstituteAccrNo);

                            // Class
                            TableCell tdClass = new TableCell();
                            tdClass.Width = Unit.Percentage(4);
                            tdClass.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Class"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Class"].ToString();
                            tdClass.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdClass);

                            // RollNo
                            TableCell tdRollNo = new TableCell();
                            tdRollNo.Width = Unit.Percentage(4);
                            tdRollNo.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["RollNo"].ToString()) ? "-" : ds.Tables[0].Rows[i]["RollNo"].ToString();
                            tdRollNo.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRollNo);

                            // ReferenceNo (Application No.)
                            TableCell tdReferenceNo = new TableCell();
                            tdReferenceNo.Width = Unit.Percentage(5);
                            tdReferenceNo.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ReferenceNo"].ToString()) ? "-" : ds.Tables[0].Rows[i]["ReferenceNo"].ToString();
                            tdReferenceNo.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdReferenceNo);

                            // RegistrationNo (Regn No.)
                            TableCell tdRegistrationNo = new TableCell();
                            tdRegistrationNo.Width = Unit.Percentage(5);
                            tdRegistrationNo.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["RegistrationNo"].ToString()) ? "-" : ds.Tables[0].Rows[i]["RegistrationNo"].ToString();
                            tdRegistrationNo.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRegistrationNo);

                            // CourseEnrolled
                            TableCell tdCourseEnrolled = new TableCell();
                            tdCourseEnrolled.Width = Unit.Percentage(8);
                            tdCourseEnrolled.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CourseEnrolled"].ToString()) ? "-" : ds.Tables[0].Rows[i]["CourseEnrolled"].ToString();
                            tdCourseEnrolled.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdCourseEnrolled);

                            // Name
                            TableCell tdName = new TableCell();
                            tdName.Width = Unit.Percentage(10);
                            tdName.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Name"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Name"].ToString();
                            tdName.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdName);

                            // FatherName (Father/Guardian name)
                            TableCell tdFatherName = new TableCell();
                            tdFatherName.Width = Unit.Percentage(10);
                            tdFatherName.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["FatherName"].ToString()) ? "-" : ds.Tables[0].Rows[i]["FatherName"].ToString();
                            tdFatherName.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdFatherName);

                            TableCell tdGuardianName = new TableCell();
                            tdGuardianName.Width = Unit.Percentage(10);
                            tdGuardianName.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["GuardianName"].ToString()) ? "-" : ds.Tables[0].Rows[i]["GuardianName"].ToString();
                            tdGuardianName.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdGuardianName);

                            // MotherName
                            TableCell tdMotherName = new TableCell();
                            tdMotherName.Width = Unit.Percentage(8);
                            tdMotherName.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["MotherName"].ToString()) ? "-" : ds.Tables[0].Rows[i]["MotherName"].ToString();
                            tdMotherName.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdMotherName);

                            // DOB
                            TableCell tdDOB = new TableCell();
                            tdDOB.Width = Unit.Percentage(5);
                            tdDOB.Text = ds.Tables[0].Rows[i]["DOB"] == DBNull.Value ? "-" : Convert.ToDateTime(ds.Tables[0].Rows[i]["DOB"]).ToString("dd-MMM-yyyy");
                            tdDOB.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdDOB);

                            // Gender
                            TableCell tdGender = new TableCell();
                            tdGender.Width = Unit.Percentage(4);
                            tdGender.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Gender"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Gender"].ToString();
                            tdGender.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdGender);

                            // Category (Cast Category)
                            TableCell tdCategory = new TableCell();
                            tdCategory.Width = Unit.Percentage(5);
                            tdCategory.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Category"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Category"].ToString();
                            tdCategory.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdCategory);

                            // EWS
                            TableCell tdEWS = new TableCell();
                            tdEWS.Width = Unit.Percentage(4);
                            tdEWS.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["EWS"].ToString()) ? "-" : ds.Tables[0].Rows[i]["EWS"].ToString();
                            tdEWS.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdEWS);

                            // Handicapped
                            TableCell tdHandicapped = new TableCell();
                            tdHandicapped.Width = Unit.Percentage(4);
                            tdHandicapped.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Handicapped"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Handicapped"].ToString();
                            tdHandicapped.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdHandicapped);

                            // Address (Corr. Address)
                            TableCell tdAddress = new TableCell();
                            tdAddress.Width = Unit.Percentage(15);
                            tdAddress.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["Address"].ToString()) ? "-" : ds.Tables[0].Rows[i]["Address"].ToString();
                            tdAddress.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdAddress);

                            // PermanentAddress
                            TableCell tdPermanentAddress = new TableCell();
                            tdPermanentAddress.Width = Unit.Percentage(15);
                            tdPermanentAddress.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PermanentAddress"].ToString()) ? "-" : ds.Tables[0].Rows[i]["PermanentAddress"].ToString();
                            tdPermanentAddress.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdPermanentAddress);

                            // PaymentStatus
                            TableCell tdPaymentStatus = new TableCell();
                            tdPaymentStatus.Width = Unit.Percentage(5);
                            tdPaymentStatus.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["PaymentStatus"].ToString()) ? "-" : ds.Tables[0].Rows[i]["PaymentStatus"].ToString();
                            tdPaymentStatus.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdPaymentStatus);

                            // ApplicationStatus
                            TableCell tdApplicationStatus = new TableCell();
                            tdApplicationStatus.Width = Unit.Percentage(5);
                            tdApplicationStatus.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["ApplicationStatus"].ToString()) ? "-" : ds.Tables[0].Rows[i]["ApplicationStatus"].ToString();
                            tdApplicationStatus.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdApplicationStatus);

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
            tc1.Text = "Project Based Students Enrolment Details";

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

            // Udise Code
            TableHeaderCell thUDISECode = new TableHeaderCell();
            thUDISECode.Width = Unit.Percentage(5);
            thUDISECode.Text = "Udise Code";
            thUDISECode.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thUDISECode);

            // Institute Name
            TableHeaderCell thInstituteName = new TableHeaderCell();
            thInstituteName.Width = Unit.Percentage(5);
            thInstituteName.Text = "Institute Name";
            thInstituteName.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thInstituteName);

            // Institute Accr No.
            TableHeaderCell thInstituteAccrNo = new TableHeaderCell();
            thInstituteAccrNo.Width = Unit.Percentage(5);
            thInstituteAccrNo.Text = "Institute Accr No.";
            thInstituteAccrNo.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thInstituteAccrNo);

            // Class
            TableHeaderCell thClass = new TableHeaderCell();
            thClass.Width = Unit.Percentage(4);
            thClass.Text = "Class";
            thClass.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thClass);

            // Roll No
            TableHeaderCell thRollNo = new TableHeaderCell();
            thRollNo.Width = Unit.Percentage(4);
            thRollNo.Text = "Roll No";
            thRollNo.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thRollNo);

            // Application No.
            TableHeaderCell thApplicationNo = new TableHeaderCell();
            thApplicationNo.Width = Unit.Percentage(5);
            thApplicationNo.Text = "Application No.";
            thApplicationNo.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thApplicationNo);

            // Regn No.
            TableHeaderCell thRegnNo = new TableHeaderCell();
            thRegnNo.Width = Unit.Percentage(5);
            thRegnNo.Text = "Regn No.";
            thRegnNo.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thRegnNo);

            // Course Enrolled
            TableHeaderCell thCourseEnrolled = new TableHeaderCell();
            thCourseEnrolled.Width = Unit.Percentage(8);
            thCourseEnrolled.Text = "Course Enrolled";
            thCourseEnrolled.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thCourseEnrolled);

            // Name
            TableHeaderCell thName = new TableHeaderCell();
            thName.Width = Unit.Percentage(10);
            thName.Text = "Name";
            thName.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thName);

            // Father/Guardian name
            TableHeaderCell thFatherGuardian = new TableHeaderCell();
            thFatherGuardian.Width = Unit.Percentage(10);
            thFatherGuardian.Text = "Father name";
            thFatherGuardian.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thFatherGuardian);

            // Guardian name
            TableHeaderCell thGuardian = new TableHeaderCell();
            thGuardian.Width = Unit.Percentage(10);
            thGuardian.Text = "Guardian name";
            thGuardian.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thGuardian);

            // Mother name
            TableHeaderCell thMotherName = new TableHeaderCell();
            thMotherName.Width = Unit.Percentage(2);
            thMotherName.Text = "Mother name";
            thMotherName.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thMotherName);

            // Date of Birth
            TableHeaderCell thDOB = new TableHeaderCell();
            thDOB.Width = Unit.Percentage(2);
            thDOB.Text = "Date of Birth";
            thDOB.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thDOB);

            // Gender
            TableHeaderCell thGender = new TableHeaderCell();
            thGender.Width = Unit.Percentage(2);
            thGender.Text = "Gender";
            thGender.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thGender);

            // Cast Category
            TableHeaderCell thCategory = new TableHeaderCell();
            thCategory.Width = Unit.Percentage(2);
            thCategory.Text = "Cast Category";
            thCategory.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thCategory);

            // EWS
            TableHeaderCell thEWS = new TableHeaderCell();
            thEWS.Width = Unit.Percentage(2);
            thEWS.Text = "EWS";
            thEWS.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thEWS);

            // Handicapped
            TableHeaderCell thHandicapped = new TableHeaderCell();
            thHandicapped.Width = Unit.Percentage(2);
            thHandicapped.Text = "Handicapped";
            thHandicapped.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thHandicapped);

            // Corr. Address
            TableHeaderCell thCorrAddress = new TableHeaderCell();
            thCorrAddress.Width = Unit.Percentage(10);
            thCorrAddress.Text = "Corr. Address";
            thCorrAddress.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thCorrAddress);

            // Permanent Address
            TableHeaderCell thPermanentAddress = new TableHeaderCell();
            thPermanentAddress.Width = Unit.Percentage(10);
            thPermanentAddress.Text = "Permanent Address";
            thPermanentAddress.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thPermanentAddress);

            // Payment Status
            TableHeaderCell thPaymentStatus = new TableHeaderCell();
            thPaymentStatus.Width = Unit.Percentage(5);
            thPaymentStatus.Text = "Payment Status";
            thPaymentStatus.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thPaymentStatus);

            // Application Status
            TableHeaderCell thApplicationStatus = new TableHeaderCell();
            thApplicationStatus.Width = Unit.Percentage(5);
            thApplicationStatus.Text = "Application Status";
            thApplicationStatus.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thApplicationStatus);

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
     
        txtdate1.Text = string.Empty;
        txtdate2.Text = string.Empty;

        gvMain.DataSource = null;
        gvMain.Visible = false;
        uPnlGrid.Update();
        PagingBar1.Visible = false;



        btnDownload.Visible = false;
        btnDownloadpdf.Visible = false;

        btnShowData.Visible = true;
        lblMessage.Visible = false;
        //ddlinstitute.Items.Clear();
        // clear in the case of  HO
        if (checkUser())
        {
            ddlinstitute.Items.Clear();
            ddlinstitute.Items.Insert(0, "--Select Institute--");
        }
    }

}