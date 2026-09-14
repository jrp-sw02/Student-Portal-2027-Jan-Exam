using AjaxControlToolkit;
using DocumentFormat.OpenXml.Office.Word;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using ListItem = System.Web.UI.WebControls.ListItem;


public partial class HO_Rpt_DataforNCVET : BasePage
{
    Int64 entityID = 0;
    UserType loginUserType;
    Int32 currentRoleId = 0;
    Table tbl = new Table();
    String currentRoleName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;


        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("~/Index.aspx");

            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            currentRoleName = (string)Session["RoleName"];
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);

            //if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/Rpt/DataforNCVET.aspx"))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}

            if (!IsPostBack)
            {
                entityID = Convert.ToInt64(Session["EntityID"]);

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }


    protected bool isValidForm()
    {
        try
        {
     

            if (String.IsNullOrWhiteSpace(txtStartDate.Text) ||
                String.IsNullOrWhiteSpace(txtEndDate.Text))
            {
                throw new Exception("Please select start date and end date.");
            }

            DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
            DateTime endDate = Convert.ToDateTime(txtEndDate.Text);

            if (endDate < startDate)
            {
                throw new Exception("End date cannot be before start date.");
            }

            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void BindGridView()
    {
        try
        {

            DateTime startDate = Convert.ToDateTime(txtStartDate.Text); 
            DateTime endDate = Convert.ToDateTime(txtEndDate.Text);
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
            {
                connection.Open();

                string required_SP = String.Empty;

                switch (ddlTypeofData.SelectedValue)
                {
                    case "0":
                        required_SP = "[usp_GetNSQFModuleCandidateDetails]";
                        break;
                    case "1":
                        required_SP = "[usp_getCCC_CandidateWiseDetails]";
                        break;
                    case "2":
                        required_SP = "[usp_getNSQF_CandidateWiseDetails]";
                        break;
                    case "3":
                        required_SP = "[usp_OABC_CandidateWiseDetail]";
                        break;
                    case "4":
                        required_SP = "[usp_OABC_MarksData]";
                        break;
                }
                

                using (var command = new SqlCommand(required_SP, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@from_date", startDate);
                    command.Parameters.AddWithValue("@to_date", endDate);

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(dt);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        gvMain.Visible = true;
                        PagingBar1.Visible = true;
                        gvMain.AutoGenerateColumns = true;
                        //gvMain.DataSource = dt;
                        //gvMain.PageSize = 10;
                        //gvMain.DataBind();
                        PagingBar1.Bind(dt, ref gvMain);
                        // Optional: Customize column headers or formatting after binding
                        //CustomizeGridColumns();

                        // Your existing alternating row styles
                        for (int i = 0; i < gvMain.Rows.Count; i++)
                        {
                            gvMain.Rows[i].CssClass = (i % 2 == 0) ? "gdalternate1" : "gdrow1";
                        }

                        uPnlGrid.Update();
                        uPnlNavigation.Update();

                        ddlTypeofData.Enabled = false;
                        btnDownload.Visible = true;
                        btnShowData.Visible = true;
                        lblerror.Visible = false;
                    }
                    else
                    {
                        lblerror.Text = "No Data Found";
                        lblerror.Visible = true;
                        gvMain.DataSource = null;
                        gvMain.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = "Error Occurred in Fetching Data";
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
        try
        {

            if (!isValidForm())
                return;

            BindGridView();
        }
        catch(Exception ex)
        {
            lblerror.Visible = true;
            lblerror.Text = ex.Message;
            ShowAlert(ex.Message);
        }

    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        //Query to fetch data
        System.IO.StringWriter StringWrite = new System.IO.StringWriter();
        Html32TextWriter htmlWrite;

        ShowData();

        String today_date = DateTime.Today.ToString("dd-MM-yyyy hh:mm");
        String filename = string.Empty;

        switch (ddlTypeofData.SelectedValue)
        {
            case "0":
                filename = "NSQFModuleCandidateDetails";
                break;
            case "1":
                filename = "getCCC_CandidateWiseDetails";
                break;
            case "2":
                filename = "getNSQF_CandidateWiseDetails";
                break;
            case "3":
                filename = "OABC_CandidateWiseDetail";
                break;
            case "4":
                filename = "OABC_MarksData";
                break;

        }

        Response.AddHeader("content-disposition", "attachment;filename=" + filename  + " " + today_date +".xls");
        Response.Charset = "";
        //Response.ContentType = "application/vnd.xls";
        Response.ContentType = "application/vnd.ms-excel";
        htmlWrite = new Html32TextWriter(StringWrite);
        tbl.RenderControl(htmlWrite);
        Response.Write(StringWrite.ToString());

        Response.End();
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
            DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
            DateTime endDate = Convert.ToDateTime(txtEndDate.Text);

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString))
            {
                connection.Open();


                string required_SP = String.Empty;

                switch (ddlTypeofData.SelectedValue)
                {
                    case "0":
                        required_SP = "[usp_GetNSQFModuleCandidateDetails]";
                        break;
                    case "1":
                        required_SP = "[usp_getCCC_CandidateWiseDetails]";
                        break;
                    case "2":
                        required_SP = "[usp_getNSQF_CandidateWiseDetails]";
                        break;
                    case "3":
                        required_SP = "[usp_OABC_CandidateWiseDetail]";
                        break;
                    case "4":
                        required_SP = "[usp_OABC_MarksData]";
                        break;
                }


                using (var command = new SqlCommand(required_SP, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@from_date", startDate);
                    command.Parameters.AddWithValue("@to_date", endDate);


                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(dt);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        tbl.Rows.Clear();// Clear previous content

                        // Add Report Title & Date
                        AddReportHeader();

                        // Add Column Header Row
                        AddDynamicTableHeader(dt);

                        // Add Data Rows
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            TableRow tr = new TableRow();

                            // Serial Number (Optional - you said not necessary, but kept it simple)
                            TableCell tdSr = new TableCell();
                            tdSr.Text = (i + 1).ToString();
                            tdSr.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdSr);

                            // Dynamic Data Cells
                            foreach (DataColumn col in dt.Columns)
                            {
                                TableCell td = new TableCell();
                                string value = dt.Rows[i][col].ToString().Trim();

                                td.Text = string.IsNullOrEmpty(value) ? "-" : value;
                                td.HorizontalAlign = HorizontalAlign.Center;


                                tr.Cells.Add(td);
                            }

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

    private void AddReportHeader()
    {
        // Title Row
        TableHeaderRow titleRow = new TableHeaderRow();
        TableHeaderCell titleCell = new TableHeaderCell();
        titleCell.ColumnSpan = 20;                    // Adjust if needed
        titleCell.Text = "NSQF Result Data";
        titleCell.HorizontalAlign = HorizontalAlign.Center;
        titleRow.Cells.Add(titleCell);
        tbl.Rows.Add(titleRow);

        // Generated Date Row
        TableHeaderRow dateRow = new TableHeaderRow();
        TableHeaderCell dateCell = new TableHeaderCell();
        dateCell.ColumnSpan = 20;
        dateCell.Text = "Generated on: " + DateTime.Now.ToLongDateString();
        dateCell.HorizontalAlign = HorizontalAlign.Center;
        dateRow.Cells.Add(dateCell);
        tbl.Rows.Add(dateRow);
    }

    private void AddDynamicTableHeader(DataTable dt)
    {
        TableHeaderRow headerRow = new TableHeaderRow();

        // Sr. No. Header
        TableHeaderCell srHeader = new TableHeaderCell();
        srHeader.Text = "Sr. No.";
        srHeader.HorizontalAlign = HorizontalAlign.Center;
        headerRow.Cells.Add(srHeader);

        // Dynamic Column Headers
        foreach (DataColumn col in dt.Columns)
        {
            TableHeaderCell hc = new TableHeaderCell();
            hc.Text = col.ColumnName;
            hc.HorizontalAlign = HorizontalAlign.Center;
            headerRow.Cells.Add(hc);
        }

        tbl.Rows.Add(headerRow);
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
            tc1.Text = "Monthly Progress Report of MIS";

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

            // centre Name
            TableHeaderCell thcentreName = new TableHeaderCell();
            thcentreName.Width = Unit.Percentage(5);
            thcentreName.Text = "Centre Name";
            thcentreName.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thcentreName);

            // course ID
            TableHeaderCell thcourseID = new TableHeaderCell();
            thcourseID.Width = Unit.Percentage(5);
            thcourseID.Text = "Course ID";
            thcourseID.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thcourseID);

            // course Name
            TableHeaderCell thcourseName = new TableHeaderCell();
            thcourseName.Width = Unit.Percentage(5);
            thcourseName.Text = "Course Name";
            thcourseName.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thcourseName);

            //course Duration In Days
            TableHeaderCell thcourseDurationInDays = new TableHeaderCell();
            thcourseDurationInDays.Width = Unit.Percentage(4);
            thcourseDurationInDays.Text = "Course Duration In Days";
            thcourseDurationInDays.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thcourseDurationInDays);

            // courseDurationInHrs
            TableHeaderCell thcourseDurationInHrs = new TableHeaderCell();
            thcourseDurationInHrs.Width = Unit.Percentage(4);
            thcourseDurationInHrs.Text = "Course Duration In Hrs";
            thcourseDurationInHrs.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thcourseDurationInHrs);

            // total Registered
            TableHeaderCell thtotalRegistered = new TableHeaderCell();
            thtotalRegistered.Width = Unit.Percentage(5);
            thtotalRegistered.Text = "Total Registered";
            thtotalRegistered.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thtotalRegistered);

            // total Undergoing
            TableHeaderCell thtotalUndergoing = new TableHeaderCell();
            thtotalUndergoing.Width = Unit.Percentage(5);
            thtotalUndergoing.Text = "Total Undergoing";
            thtotalUndergoing.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thtotalUndergoing);

            // total Trained
            TableHeaderCell thtotalTrained = new TableHeaderCell();
            thtotalTrained.Width = Unit.Percentage(8);
            thtotalTrained.Text = "Total Trained";
            thtotalTrained.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thtotalTrained);

            // total Certified
            TableHeaderCell thtotalCertified = new TableHeaderCell();
            thtotalCertified.Width = Unit.Percentage(10);
            thtotalCertified.Text = "Total Certified";
            thtotalCertified.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thtotalCertified);

            // total DropOut
            TableHeaderCell thtotalDropOut = new TableHeaderCell();
            thtotalDropOut.Width = Unit.Percentage(10);
            thtotalDropOut.Text = "Total DropOut";
            thtotalDropOut.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(thtotalDropOut);

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

        //ddlyear.SelectedIndex = 0;
        //ddlmonth.SelectedIndex = 0;
        txtStartDate .Text = string.Empty;
        txtEndDate .Text = string.Empty;

        gvMain.DataSource = null;
        gvMain.Visible = false;
        uPnlGrid.Update();
        PagingBar1.Visible = false;


        ddlTypeofData.Enabled = true;
        btnDownload.Visible = false;
        //btnDownloadpdf.Visible = false;

        btnShowData.Visible = true;
        lblMessage.Visible = false;
        //ddlinstitute.Items.Clear();
        // clear in the case of  HO
        //if (checkUser())
        //{
        //    ddlinstitute.Items.Clear();
        //    ddlinstitute.Items.Insert(0, "--Select Institute--");
        //}
    }
}