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
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using ListItem = System.Web.UI.WebControls.ListItem;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;


public partial class HO_Rpt_UPdistrictwiseschoolwisecandidate : BasePage
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
            entityID = Convert.ToInt64(Session["EntityID"]);

            if (!IsPostBack)
            {
                //entityID = Convert.ToInt64(Session["EntityID"]);

                tbl.Width = Unit.Percentage(100);
                txttodaydate.Text = DateTime.Now.ToString("dd-MMMM-yyyy");
                FillExamCycle();
                FillSchools();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected bool isValidForm()
    {


        if (ddlexamcycle.SelectedValue == "0")
        {
            lblerror.Visible = true;
            lblerror.Text = "Kindly choose a Exam Cycle";
            return false;
        }

        return true;
    }
    protected void FillExamCycle()
    {
        ListItem lst1 = new ListItem("--Select Exam Cycle--", "0");
        try
        {
            using(var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))
            {
                conn.Open();
                using(var cmd = new SqlCommand("fillprojectexamcycle", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }

                    ddlexamcycle.DataSource = dt;

                    ddlexamcycle.DataValueField = "ID";
                    ddlexamcycle.DataTextField = "Name";
                    ddlexamcycle.DataBind();
                    ddlexamcycle.Items.Insert(0, lst1);

                }
            }
        }
        catch(Exception)
        {
            ShowAlert("Some Error Occured in Filling Exam Cycle.");
        }
    }
    protected void FillSchools()
    {
        ListItem lst1 = new ListItem("--All Schools--", "0");
        try
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                connection.Open();
                string sql = @"
                select af.name as Name, af.instituteID as ID from nielitmis.dbo.AffInstitute af
                inner join nielitmis.dbo.projectSubCentre sc on af.id = sc.centreID
                inner join nielit.dbo.Institute i on i.id = af.instituteID
                inner join nielit.dbo.location loc on loc.ID = i.District_ID
                where projectID in (36,10023,10025)
            ";

                using (var command = new SqlCommand(sql, connection))
                {

                    //command.Parameters.AddWithValue("@project_id", projectid);
                   // command.Parameters.AddWithValue("@district_id", ddldistrict.SelectedValue);

                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        DataSet ds = new DataSet();
                        da.Fill(ds);

                        ddlinstitute.DataSource = ds;
                        ddlinstitute.DataValueField = "ID";
                        ddlinstitute.DataTextField = "Name";
                        ddlinstitute.DataBind();

                        ddlinstitute.Items.Insert(0, lst1);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Institute cannot be binded");
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
                using (var command = new SqlCommand("Get_Districtwise_data_for_Project_Schools", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@institute_id", Convert.ToInt64(ddlinstitute.SelectedValue));
                    command.Parameters.AddWithValue("@Exam_Id",Convert.ToInt64(ddlexamcycle.SelectedValue));

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
                        ddlexamcycle.Enabled = false;
                        ddlinstitute.Enabled = false;

                    }
                    else
                    {
                        lblMessage.Visible = true;
                        btnDownload.Visible = false;
                        btnDownloadpdf.Visible = false ;
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
    protected void btnDownloadpdf_Click(object sender, EventArgs e)
    {
        try
        {
            // Fetch the data (reuse your existing command logic)
            DataSet ds = new DataSet();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("Get_Districtwise_data_for_Project_Schools", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Exam_Id", Convert.ToInt64(ddlexamcycle.SelectedValue));
                    command.Parameters.AddWithValue("@institute_id", Convert.ToInt64(ddlinstitute.SelectedValue));
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

            // Set up PDF document
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
            pdfDoc.Open();


            PdfPTable pdfTable = new PdfPTable(4);
            pdfTable.WidthPercentage = 100;

            // Set relative column widths based on your Unit.Percentage values
            float[] widths = { 3, 5, 10, 5 };
            pdfTable.SetWidths(widths);

            // Fonts and colors
            Font titleFont = new Font(Font.FontFamily.HELVETICA, 10f, Font.BOLD, BaseColor.WHITE);
            Font headerFont = new Font(Font.FontFamily.HELVETICA, 10f, Font.BOLD, BaseColor.WHITE);
            Font cellFont = new Font(Font.FontFamily.HELVETICA, 10f, Font.NORMAL, BaseColor.BLACK);

            BaseColor headerBg = BaseColor.DARK_GRAY;
            BaseColor alternateRowBg = new BaseColor(230, 240, 240); // #E6F0F0 light blue-ish
            BaseColor rowBg = new BaseColor(201, 215, 226); // #c9d7e2 light blue

            // Add main title header
            PdfPCell cell = new PdfPCell(new Phrase("NIELIT Uttar Pradesh Madhyamik Shiksha Board Districtwise Schoolwise Registered Candidate Report", titleFont));
            cell.Colspan = 4;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(0, 0, 128);
            pdfTable.AddCell(cell);

            // Add generated date header
            cell = new PdfPCell(new Phrase("Date of Report: " + System.DateTime.Now.ToLongDateString(), titleFont));
            cell.Colspan = 4;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.BackgroundColor = new BaseColor(0, 0, 128);
            pdfTable.AddCell(cell);

            // Add column headers
            string[] headers = { "Sr. No.", "District", "School Name", "Total Registrations" };
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
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["District"].ToString()) ? "-" : ds.Tables[0].Rows[i]["District"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["SchoolName"].ToString()) ? "-" : ds.Tables[0].Rows[i]["SchoolName"].ToString(), cellFont));
                cell.BackgroundColor = currentRowBg;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                pdfTable.AddCell(cell);

 
                cell = new PdfPCell(new Phrase(string.IsNullOrEmpty(ds.Tables[0].Rows[i]["TotalRegistrations"].ToString()) ? "-" : ds.Tables[0].Rows[i]["TotalRegistrations"].ToString(), cellFont));
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
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
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
    protected void ShowData()
    {
        try
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EconnectContext"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("Get_Districtwise_data_for_Project_Schools", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Exam_Id", Convert.ToInt64(ddlexamcycle.SelectedValue));
                    command.Parameters.AddWithValue("@institute_id", Convert.ToInt64(ddlinstitute.SelectedValue));
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
                            tr.CssClass = (i % 2 == 0) ? "gdalternate1" : "gdrow1";

                            // Sr. No.
                            TableCell tdSrNo = new TableCell();
                            tdSrNo.Width = Unit.Percentage(5);
                            tdSrNo.Text = (i + 1).ToString();
                            tdSrNo.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdSrNo);

                            // District
                            TableCell tdDistrict = new TableCell();
                            tdDistrict.Width = Unit.Percentage(20);
                            tdDistrict.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["District"].ToString()) ? "-" : ds.Tables[0].Rows[i]["District"].ToString();
                            tdDistrict.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdDistrict);

                            // School Name
                            TableCell tdSchoolName = new TableCell();
                            tdSchoolName.Width = Unit.Percentage(55);
                            tdSchoolName.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["SchoolName"].ToString()) ? "-" : ds.Tables[0].Rows[i]["SchoolName"].ToString();
                            tdSchoolName.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdSchoolName);

                            // Count
                            TableCell tdCount = new TableCell();
                            tdCount.Width = Unit.Percentage(20);
                            tdCount.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["TotalRegistrations"].ToString()) ? "-" : ds.Tables[0].Rows[i]["TotalRegistrations"].ToString();
                            tdCount.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdCount);

                            tbl.Rows.Add(tr);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Error while generating Excel data ");
        }
    }
    protected void ShowTableHeader()
    {
        try
        {
            // Main heading
            TableHeaderRow th1 = new TableHeaderRow();
            th1.CssClass = "head1";

            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);
            tc1.ColumnSpan = 4;
            tc1.Text = "NIELIT Uttar Pradesh Madhyamik Shiksha Board Districtwise Schoolwise Registered Candidate Report";
            tc1.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tc1);

            tbl.Rows.Add(th1);

            // Generated date row
            TableHeaderRow th2 = new TableHeaderRow();
            th2.CssClass = "head1";

            TableHeaderCell tc2 = new TableHeaderCell();
            tc2.Width = Unit.Percentage(100);
            tc2.ColumnSpan = 4;
            tc2.Text = "Generated on: " + System.DateTime.Now.ToLongDateString();
            tc2.HorizontalAlign = HorizontalAlign.Center;
            th2.Cells.Add(tc2);

            tbl.Rows.Add(th2);

            // Column headers
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            // Sr. No.
            TableHeaderCell thSrNo = new TableHeaderCell();
            thSrNo.Text = "Sr. No.";
            thSrNo.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thSrNo);

            // District
            TableHeaderCell thDistrict = new TableHeaderCell();
            thDistrict.Text = "District";
            thDistrict.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thDistrict);

            // Institute Name
            TableHeaderCell thInstituteName = new TableHeaderCell();
            thInstituteName.Text = "Institute Name";
            thInstituteName.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thInstituteName);

            // No. of Registrations
            TableHeaderCell thRegistrations = new TableHeaderCell();
            thRegistrations.Text = "No. of Registrations";
            thRegistrations.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thRegistrations);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert("Error while generating table header.");
        }
    }
    
    
    protected void btnClear_Click(object sender, EventArgs e)
    {
        gvMain.Visible = false;
        DataTable dt = new DataTable();
        lblMessage.Text = string.Empty;
        lblerror.Visible = false;
        ddlexamcycle.SelectedIndex = 0; 
        //txtdate1.Text = string.Empty;
        //txtdate2.Text = string.Empty;
        lblMessage.Text = string.Empty;
        uPnlGrid.Update();
        PagingBar1.Visible = false;
        btnDownload.Visible = false;
        btnDownloadpdf.Visible = false;
        ddlexamcycle.Enabled = true;
        ddlinstitute.Enabled = true;
        ddlinstitute.SelectedIndex = 0;

        btnShowData.Visible = true;
    }

}

