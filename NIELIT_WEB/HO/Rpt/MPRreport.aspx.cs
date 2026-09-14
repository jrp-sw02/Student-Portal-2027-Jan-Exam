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


public partial class HO_Rpt_MPRreport : BasePage
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

            if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/Rpt/MPRreport.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            if (!IsPostBack)
            {
                entityID = Convert.ToInt64(Session["EntityID"]);

                FillProjects();
                BindGender();
                BindCastCategory();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    //protected bool checkUser()
    //{
    //    bool isHO = false;
    //    using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
    //    {
    //        connection.Open();
    //        using (var command = new SqlCommand("checkuserlogin", connection))
    //        {
    //            command.CommandType = CommandType.StoredProcedure;
    //            command.Parameters.Clear();
    //            command.Parameters.AddWithValue("@entity_id", entityID);

    //            object result = command.ExecuteScalar();
    //            if (result != null && result != DBNull.Value)
    //            {
    //                isHO = Convert.ToBoolean(result);
    //            }

    //        }
    //        connection.Close();
    //    }
    //    return isHO;
    //}
    protected bool isValidForm()
    {
        try
        {
            if (ddlproject.SelectedValue == "0") throw new Exception("Select project");

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
    protected void FillProjects()
    {
        try
        {

            string connstr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connstr))
            using (SqlCommand cmd = new SqlCommand("getProjectforMPR", con))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                ddlproject.DataSource = cmd.ExecuteReader();
                ddlproject.DataTextField = "projectName";
                ddlproject.DataValueField = "ID";
                ddlproject.DataBind();
            }
            ddlproject.Items.Insert(0, new ListItem("-- Select One --", "0"));
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    //protected void LoadYears()
    //{
    //    //ddlyear.Items.Insert(0, new ListItem("Select Year", "0"));
    //    int currentYear = DateTime.Now.Year;
    //    for (int year = currentYear; year >= 2000; year--)
    //    {
    //        ddlyear.Items.Add(year.ToString());
    //    }
    //}

    //protected void LoadMonths()
    //{
    //   // ddlmonth.Items.Insert(0, new ListItem("Select Month", "0"));
    //    for (int month = 1; month <= 12; month++)
    //    {
    //        string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(month);

    //        ddlmonth.Items.Add(new ListItem(monthName, month.ToString()));
    //    }
    //}
    protected void BindGridView()
    {
        try
        {
            // put validation
            long projectID = Convert.ToInt64(ddlproject.SelectedValue);
            //int month = Convert.ToInt32(ddlmonth.SelectedValue);
            //int year = Convert.ToInt32(ddlyear.SelectedValue);

            DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
            DateTime endDate = Convert.ToDateTime(txtEndDate.Text);

            //DateTime passedDate = new DateTime(year, month, 1);

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("getdataforMPR", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@projectid", projectID);
                    //command.Parameters.AddWithValue("@passedDate", passedDate);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);
                    command.Parameters.AddWithValue("@Gender",
                        ddlgender.SelectedValue == "0"
                            ? (object)DBNull.Value
                            : ddlgender.SelectedValue);

                    command.Parameters.AddWithValue("@Category",
                        ddlCategory.SelectedValue == "0"
                            ? (object)DBNull.Value
                            : ddlCategory.SelectedValue);

                    //command.Parameters.AddWithValue("@centre_ID", Convert.ToInt64(ddlinstitute.SelectedValue));


                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                    // lblMessage.Visible = true; //change
                    // lblMessage.Text = "Rows: " + dt.Rows.Count;   

                    uPnlGrid.Update();
                    uPnlNavigation.Update();

                    if (dt.Rows.Count > 0)
                    {
                        gvMain.Visible = true;
                        PagingBar1.Visible = true;
                        // gvMain.DataSource = dt;  //change
                        // gvMain.DataBind();        //change

                        PagingBar1.Bind(dt, ref gvMain);

                        // Apply alternating row styles
                        for (int i = 0; i < gvMain.Rows.Count; i++)
                        {
                            gvMain.Rows[i].CssClass = (i % 2 == 0) ? "gdalternate1" : "gdrow1";
                        }


                        btnDownload.Visible = true;
                        //btnDownloadpdf.Visible = true;
                        btnShowData.Visible = true;
                        lblerror.Visible = false;
                        lblMessage.Visible = false;
                    }
                    else
                    {
                        btnDownload.Visible = false;
                        // btnDownloadpdf.Visible = false;
                        lblMessage.Visible = true;
                        gvMain.DataSource = null;
                        gvMain.Visible = false;
                        uPnlGrid.Update();
                        PagingBar1.Visible = false;

                        lblMessage.Text = dt.Rows.Count == 0 ? "No record found." : "";
                    }
                }
                connection.Close();
            }
        }
        catch (Exception)
        {
            lblMessage.Visible = true;
            lblerror.Text = "Error Occurred in Fetching Data";
        }
    }

    public void BindCastCategory()
    {
        try
        {

            String connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("[sp_GetCategory]", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                ddlCategory.DataSource = cmd.ExecuteReader();
                ddlCategory.DataTextField = "name";
                ddlCategory.DataValueField = "id";
                ddlCategory.DataBind();
            }
            ddlCategory.Items.Insert(0, new ListItem("-- ALL --", "0"));

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindGender()
    {
        try
        {

            string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connStr))
            using (SqlCommand cmd = new SqlCommand("[sp_GetGender]", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                ddlgender.DataSource = cmd.ExecuteReader();
                ddlgender.DataTextField = "name";
                ddlgender.DataValueField = "id";
                ddlgender.DataBind();
                ddlgender.Items.Insert(0, new ListItem("-- ALL --", "0"));
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
        catch (Exception ex)
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

        Response.AddHeader("content-disposition", "attachment;filename=Monthly_progress_report.xls");
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
            long projectID = Convert.ToInt64(ddlproject.SelectedValue);
            DateTime startDate = Convert.ToDateTime(txtStartDate.Text);
            DateTime endDate = Convert.ToDateTime(txtEndDate.Text);

            //int month = Convert.ToInt32(ddlmonth.SelectedValue);
            //int year = Convert.ToInt32(ddlyear.SelectedValue);
            //DateTime passedDate = new DateTime(year, month, 1);

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("getdataforMPR", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@projectId", projectID);
                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);

                    command.Parameters.AddWithValue("@Gender",
                        ddlgender.SelectedValue == "0"
                            ? (object)DBNull.Value
                            : ddlgender.SelectedValue);

                    command.Parameters.AddWithValue("@Category",
                        ddlCategory.SelectedValue == "0"
                            ? (object)DBNull.Value
                            : ddlCategory.SelectedValue);
                    //command.Parameters.AddWithValue("@year", Convert.ToDateTime(ddlyear.Text));
                    //command.Parameters.AddWithValue("@month", Convert.ToDateTime(ddlmonth.Text));

                    //command.Parameters.AddWithValue("@centre_ID", ddlinstitute.SelectedValue);

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

                            // centreName
                            TableCell tdCentreName = new TableCell();
                            tdCentreName.Width = Unit.Percentage(5);
                            tdCentreName.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CentreName"].ToString()) ? "-" : ds.Tables[0].Rows[i]["CentreName"].ToString();
                            tdCentreName.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdCentreName);

                            // courseID
                            TableCell tdCourseID = new TableCell();
                            tdCourseID.Width = Unit.Percentage(10);
                            tdCourseID.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["CourseID"].ToString()) ? "-" : ds.Tables[0].Rows[i]["CourseID"].ToString();
                            tdCourseID.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdCourseID);


                            // courseName
                            TableCell tdcourseName = new TableCell();
                            tdcourseName.Width = Unit.Percentage(5);
                            tdcourseName.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["courseName"].ToString()) ? "-" : ds.Tables[0].Rows[i]["courseName"].ToString();
                            tdcourseName.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdcourseName);

                            // courseDurationInDays
                            TableCell tdcourseDurationInDays = new TableCell();
                            tdcourseDurationInDays.Width = Unit.Percentage(4);
                            tdcourseDurationInDays.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["courseDurationInDays"].ToString()) ? "-" : ds.Tables[0].Rows[i]["courseDurationInDays"].ToString();
                            tdcourseDurationInDays.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdcourseDurationInDays);

                            // courseDurationInHrs
                            TableCell tdcourseDurationInHrs = new TableCell();
                            tdcourseDurationInHrs.Width = Unit.Percentage(4);
                            tdcourseDurationInHrs.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["courseDurationInHrs"].ToString()) ? "-" : ds.Tables[0].Rows[i]["courseDurationInHrs"].ToString();
                            tdcourseDurationInHrs.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdcourseDurationInHrs);

                            // totalRegistered  
                            TableCell tdtotalRegistered = new TableCell();
                            tdtotalRegistered.Width = Unit.Percentage(5);
                            tdtotalRegistered.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["totalRegistered"].ToString()) ? "-" : ds.Tables[0].Rows[i]["totalRegistered"].ToString();
                            tdtotalRegistered.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdtotalRegistered);

                            // totalUndergoing 
                            TableCell tdtotalUndergoing = new TableCell();
                            tdtotalUndergoing.Width = Unit.Percentage(5);
                            tdtotalUndergoing.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["totalUndergoing"].ToString()) ? "-" : ds.Tables[0].Rows[i]["totalUndergoing"].ToString();
                            tdtotalUndergoing.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdtotalUndergoing);

                            // totalTrained
                            TableCell tdtotalTrained = new TableCell();
                            tdtotalTrained.Width = Unit.Percentage(8);
                            tdtotalTrained.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["totalTrained"].ToString()) ? "-" : ds.Tables[0].Rows[i]["totalTrained"].ToString();
                            tdtotalTrained.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdtotalTrained);

                            // totalCertified
                            TableCell tdtotalCertified = new TableCell();
                            tdtotalCertified.Width = Unit.Percentage(10);
                            tdtotalCertified.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["totalCertified"].ToString()) ? "-" : ds.Tables[0].Rows[i]["totalCertified"].ToString();
                            tdtotalCertified.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdtotalCertified);

                            // totalDropOut 
                            TableCell tdtotalDropOut = new TableCell();
                            tdtotalDropOut.Width = Unit.Percentage(10);
                            tdtotalDropOut.Text = string.IsNullOrEmpty(ds.Tables[0].Rows[i]["totalDropOut"].ToString()) ? "-" : ds.Tables[0].Rows[i]["totalDropOut"].ToString();
                            tdtotalDropOut.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdtotalDropOut);

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
        ddlproject.SelectedIndex = 0;

        //ddlyear.SelectedIndex = 0;
        //ddlmonth.SelectedIndex = 0;
        txtStartDate.Text = string.Empty;
        txtEndDate.Text = string.Empty;

        gvMain.DataSource = null;
        gvMain.Visible = false;
        uPnlGrid.Update();
        PagingBar1.Visible = false;



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