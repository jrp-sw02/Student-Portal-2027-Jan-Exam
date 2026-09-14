using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Net;
using System.Text;
using System.IdentityModel.Metadata;
using System.IO;
using System.Data.Entity.Core.Objects;
using System.Globalization;


public partial class Admin_ProjectStudentDataUpload : BasePage
{
    Int64 entityID = 0;
    UserType loginUserType;
    Int32 currentRoleId = 0;
    Table tbl = new Table();

    SqlConnection con = new SqlConnection();
    protected void Page_Load(object sender, EventArgs e)
    {

        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;

        try
        {

            if (!IsSessionAlive())
            {
                Response.Redirect("~/Index.aspx");
                return;
            }


            if (!IsPostBack)
            {
               entityID = Convert.ToInt64(Session["EntityID"]);
                //entityID = 45000003;
                FillProjects();
            }

            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            currentRoleId = Convert.ToInt32(Session["RoleID"]);

            entityID = 45000003;

            //if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/ProjectStudentDataUpload.aspx"))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //    return;
            //}

            if (!IsUserAllowedToUpload(entityID))
            {
                Response.Write("You are not allowed to view this page.");
                Response.End();
                return;
            }


        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    private bool IsUserAllowedToUpload(long userId)
    {
        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ToString()))
        {
            using (SqlCommand cmd = new SqlCommand("IsUserAllowedToUpload", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@user_id", userId);

                connection.Open();
                object result = cmd.ExecuteScalar();
                connection.Close();

                return Convert.ToInt32(result) == 1;
            }
        }
    }


    //protected void FillProjects()
    //{
    //    try
    //    {
    //        using (NIELITMISContext context = new NIELITMISContext())
    //        {

    //            ListItem lst1 = new ListItem(" -- Select Project -- ", "0");
    //            //var projects = (from t in context.NielitProjectss
    //            //                where t.ID == 36
    //            //                orderby (t.ProjectName)
    //            //                select new { ValueField = t.ID, TextField = t.ProjectName });

    //            var projects = (from t in context.NielitProjectss
    //                            join x in context.projectMainCentres on t.ID equals x.projectID
    //                            where t.projectTodate >= DateTime.Today
    //                            && x.centreID == entityID
    //                            orderby (t.ProjectName)
    //                            select new { ValueField = t.ID, TextField = t.ProjectName })

    //            .Union(from t in context.NielitProjectss
    //                   join x in context.projectSubCentres on t.ID equals x.projectID
    //                   join i in context.AffInstitutes on x.centreID equals i.ID
    //                   where t.projectTodate >= DateTime.Today
    //                       && i.instituteID == entityID
    //                   orderby (t.ProjectName)
    //                   select new { ValueField = t.ID, TextField = t.ProjectName });

    //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlProject, projects, lst1);

    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}

    protected void FillProjects()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                ListItem lst1 = new ListItem(" -- Select Project -- ", "0");
                var today = DateTime.Today;

                var projects = (from t in context.NielitProjectss
                                join x in context.projectMainCentres on t.ID equals x.projectID
                                join pda in context.projDataUploadAllowed on t.ID equals pda.projID
                                where x.centreID == entityID
                                   && pda.effectiveFrom <= today
                                   && pda.effectiveTo >= today
                                orderby t.ProjectName
                                select new { ValueField = t.ID, TextField = t.ProjectName })

                .Union(from t in context.NielitProjectss
                       join x in context.projectSubCentres on t.ID equals x.projectID
                       join i in context.AffInstitutes on x.centreID equals i.ID
                       join pda in context.projDataUploadAllowed on t.ID equals pda.projID
                       where i.instituteID == entityID
                           && pda.effectiveFrom <= today
                           && pda.effectiveTo >= today
                       orderby t.ProjectName
                       select new { ValueField = t.ID, TextField = t.ProjectName });

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlProject, projects, lst1);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected bool WhetherAccExist(string acc)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                //int[] excludedStatus = {5,6,7,8 };
                //return context.AccreditationDetails
                //              .Any(x => x.AccreditationNumber.Trim() == acc.Trim() &&  !excludedStatus.Contains(x.AccreditationStatusID));
                return true;
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Accreditation Error Occurred.");

            return false;
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {

        //System.Threading.Thread.Sleep(3000);

        
        try
        {
            bool projEqual = true;
            StringBuilder sb = new StringBuilder();

            String filepath = Server.MapPath("../UploadedFiles");

            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }


            flUpload.SaveAs(filepath + "/" + flUpload.FileName);
            String path = (filepath + "/" + flUpload.FileName);

            String ext = System.IO.Path.GetExtension(this.flUpload.PostedFile.FileName);
            String excelConnectionString = "";
            if (ext.ToUpper() == ".XLS")
            {
                excelConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";", path);
            }
            else if (ext.ToUpper() == ".XLSX")
            {
                excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\";", path);
            }
            else
            {
                ShowAlert("Please Choose .XLS/.XLSX Extension Database", true);

                return;
            }

            OleDbConnection Econ = new OleDbConnection(excelConnectionString);

            Econ.Open();
            System.Data.DataTable dtExcelSchema = Econ.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

            string Query = "SELECT * From [" + sheetName + "]";
            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
            System.Data.DataTable dt = new System.Data.DataTable();
            oda.Fill(dt);
            Econ.Close();

            var validRows = dt.AsEnumerable()
                            .Where(row => row["projID"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["projID"].ToString()))
                            .ToList();


            // checking if project criteria exist for selected project or not.


            try
            {
                string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ToString();
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand sc = new SqlCommand("SELECT 1 FROM PROJCRITERIAMASTER WHERE projid = @projid", con))
                    {
                        sc.Parameters.AddWithValue("@projid", ddlProject.SelectedValue);
                        con.Open();

                        object result = sc.ExecuteScalar();

                        if (result == null)
                        {
                            ShowAlert("Conditions Not Defined. Please contact NIELIT Head Office");

                            return;
                        }
                    }
                    con.Close();
                }
            }
            catch (Exception)
            {
                ShowAlert("Error Occured");
                return;
            }


            try
            {
                string selectedProj = ddlProject.SelectedValue;

                //checking if the column data is consistent or not

                string field1first = null;
                string field5first = null;

                if (validRows.FirstOrDefault()["field1"] != DBNull.Value)
                {
                    field1first = validRows.FirstOrDefault()["field1"].ToString().Trim();
                }

                if (validRows.FirstOrDefault()["field5"] != DBNull.Value)
                {
                    field5first = validRows.FirstOrDefault()["field5"].ToString().Trim();
                }


                foreach (DataRow row in validRows)
                {
                    // 1. Project ID must match
                    if (selectedProj != row["projID"].ToString())
                    {
                        projEqual = false;
                        ShowAlert("Project ID mismatch in uploaded data or not consistent in uploaded data");

                        lblCount.Text = "File Not Uploaded Successfully";
                        lblCount.ForeColor = System.Drawing.Color.Red;
                        return;
                    }


                    if (!WhetherAccExist(row["field5"].ToString()))
                    {
                        projEqual = false;
                        ShowAlert("One or more Accreditation Number entered does not exist in Database");

                        lblCount.Text = "File Not Uploaded Successfully";
                        lblCount.ForeColor = System.Drawing.Color.Red;
                        return;
                    }


             

                    if (field1first != row["field1"].ToString())
                    {
                        projEqual = false;
                        ShowAlert("Inconsistent Values for Field1");

                        lblCount.Text = "File Not Uploaded Successfully";
                        lblCount.ForeColor = System.Drawing.Color.Red;
                        return;
                    }

                    if (field5first != row["field5"].ToString())
                    {
                        projEqual = false;
                        ShowAlert("Inconsistent Values for Field5");

                        lblCount.Text = "File Not Uploaded Successfully";
                        lblCount.ForeColor = System.Drawing.Color.Red;
                        return;
                    }


                    // case to match Accreditation number recienve in field5 to the DB 


                    string UDISEVal = "", accVal = "", accNumericPart = "";
                    string projRow = row["projID"].ToString();


                    // 2. Extract UDISE and ACCR fields (null-safe)
                    if (row["field1"] != DBNull.Value)
                        UDISEVal = row["field1"].ToString().Trim();

                    if (row["field5"] != DBNull.Value)
                        accVal = row["field5"].ToString().Trim();

                    if (!string.IsNullOrEmpty(accVal) && accVal.Contains("-"))
                    {
                        string[] split = accVal.Split('-');
                        if (split.Length > 1)
                            accNumericPart = split[1].Trim();
                    }

                    // 3. For project 36, UDISE must match numeric part of ACCR
                    if (selectedProj == "10023")
                    {
                        if (UDISEVal != accNumericPart)
                        {
                            projEqual = false;
                            ShowAlert("UDISE Code and Accreditation Code do not match for UP Project.");

                            lblCount.Text = "File Not Uploaded Successfully";
                            lblCount.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }

                    try
                    {
                        object dobValue = row["dob(YYYY-MM-DD)"];
                        string dobString = null;

                        if (dobValue == null || dobValue == DBNull.Value)
                        {
                            ShowAlert("DOB is missing or null. Use format: YYYY-MM-DD.");
                            lblCount.Text = "File Not Uploaded Successfully";
                            lblCount.ForeColor = System.Drawing.Color.Red;
                            return;
                        }

                        if (dobValue is DateTime)
                        {
                            dobString = ((DateTime)dobValue).ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            dobString = dobValue.ToString().Trim();
                        }

                        DateTime dob;
                        if (!DateTime.TryParseExact(dobString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob))
                        {
                            ShowAlert("Invalid DOB format. Use format: YYYY-MM-DD.");
                            lblCount.Text = "File Not Uploaded Successfully";
                            lblCount.ForeColor = System.Drawing.Color.Red;
                            return;
                        }
                    }
                    catch
                    {
                        ShowAlert("Error in processing DOB. Use format: YYYY-MM-DD.");
                        lblCount.Text = "File Not Uploaded Successfully";
                        lblCount.ForeColor = System.Drawing.Color.Red;
                        return;
                    }


                    // checking date format 
                    //try
                    //{
                    //    object dobValue = row["dob(YYYY-MM-DD)"];
                    //    string dobString = null;

                    //    if (dobValue == DBNull.Value || dobValue == null)
                    //    {
                    //        ShowAlert("DOB is missing or null.");
                    //        lblCount.Text = "File Not Uploaded Successfully";
                    //        lblCount.ForeColor = System.Drawing.Color.Red;
                    //        return;
                    //    }
                    //    else if (dobValue is DateTime)
                    //    {
                    //        dobString = ((DateTime)dobValue).ToString("yyyy-MM-dd");
                    //    }

                    //    else if (dobValue is string)
                    //    {
                    //        dobString = dobValue.ToString().Trim();
                    //    }
                    //    else
                    //    {
                    //        ShowAlert("Unsupported DOB format in Excel.");
                    //        lblCount.Text = "File Not Uploaded Successfully";
                    //        lblCount.ForeColor = System.Drawing.Color.Red;
                    //        return;
                    //    }

                    //    string format = "yyyy-MM-dd";
                    //    DateTime dob;
                    //    if (!string.IsNullOrWhiteSpace(dobString) &&
                    //        !DateTime.TryParseExact(dobString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dob))
                    //    {
                    //        ShowAlert("Invalid date formatExpected format: yyyy-MM-dd");
                    //        lblCount.Text = "File Not Uploaded Successfully";
                    //        lblCount.ForeColor = System.Drawing.Color.Red;
                    //        return;
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    ShowAlert("Errors in Date column");
                    //    lblCount.Text = "File Not Uploaded Successfully";
                    //    lblCount.ForeColor = System.Drawing.Color.Red;
                    //    return;
                    //}


                }





                if (projEqual)
                {
                    // generating a random batch id for one group of upload

                    hf_uploadedfilename.Value = flUpload.FileName + DateTime.Now.ToString();
                    string uploadedFilename = flUpload.FileName + DateTime.Now.ToString();
                    Int64 enterby = Convert.ToInt64(Session["UserID"]);

                    using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ToString()))
                    {


                        connection.Open();
                        using (var command = new SqlCommand("tempProjStudentDataImport", connection))
                        {
                            foreach (DataRow row in validRows)
                            {
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.Clear();

                                command.Parameters.AddWithValue("@projID", string.IsNullOrWhiteSpace(row["projID"].ToString()) ? (object)DBNull.Value : row["projID"].ToString());
                                command.Parameters.AddWithValue("@field1", string.IsNullOrWhiteSpace(row["field" +
                                    "1"].ToString()) ? (object)DBNull.Value : row["field1"].ToString());
                                command.Parameters.AddWithValue("@field2", string.IsNullOrWhiteSpace(row["field2"].ToString()) ? (object)DBNull.Value : row["field2"].ToString());
                                command.Parameters.AddWithValue("@field3", string.IsNullOrWhiteSpace(row["field3"].ToString()) ? (object)DBNull.Value : row["field3"].ToString());
                                command.Parameters.AddWithValue("@field4", string.IsNullOrWhiteSpace(row["field4"].ToString()) ? (object)DBNull.Value : row["field4"].ToString());
                                command.Parameters.AddWithValue("@field5", string.IsNullOrWhiteSpace(row["field5"].ToString()) ? (object)DBNull.Value : row["field5"].ToString());

                                command.Parameters.AddWithValue("@name", string.IsNullOrWhiteSpace(row["name"].ToString()) ? (object)DBNull.Value : row["name"].ToString());
                                command.Parameters.AddWithValue("@uploadedFileName", uploadedFilename);
                                command.Parameters.AddWithValue("@dob", string.IsNullOrWhiteSpace(row["dob(YYYY-MM-DD)"].ToString()) ? (object)DBNull.Value : Convert.ToDateTime(row["dob(YYYY-MM-DD)"].ToString()));
                                command.Parameters.AddWithValue("@enterBy", enterby);

                                // new addded

                                command.ExecuteNonQuery();

                            }
                            //loadingtxt.ForeColor = System.Drawing.Color.Red;

                        }

                        connection.Close();


                        connection.Open();
                        using (var command = new SqlCommand("ProjStudentMasterDataImport", connection))
                        {

                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.Clear();
                            command.Parameters.Add("@SuccessCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                            command.Parameters.Add("@FailureCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                            command.Parameters.Add("@lastUploadedID", SqlDbType.Int).Direction = ParameterDirection.Output;
                            command.Parameters.AddWithValue("@projID", string.IsNullOrWhiteSpace(ddlProject.SelectedValue) ? (object)DBNull.Value : ddlProject.SelectedValue);
                            // new addded
                            command.Parameters.AddWithValue("@uploadedFileName", uploadedFilename);
                            command.Parameters.AddWithValue("@enterBy_main", enterby);


                            command.ExecuteNonQuery();

                            Int32 successCount = Convert.ToInt32(command.Parameters["@SuccessCount"].Value);
                            Int32 failureCount = Convert.ToInt32(command.Parameters["@FailureCount"].Value);
                            Int32 lastUploadedId = Convert.ToInt32(command.Parameters["@lastUploadedID"].Value);
                            lblLastId.Text = lastUploadedId.ToString();

                            lblCount.Text = " Successful Count : " + successCount + " failureCount : " + failureCount;
                            lblTotal.Text = " Total Count : " + validRows.Count;

                            lblCount.ForeColor = System.Drawing.Color.Black;

                            if (failureCount > 0)
                                DownloadFailureData.Visible = true;

                            ddlProject.Enabled = false;
                            flUpload.Enabled = false;

                        }
                        connection.Close();
                        //litLoadingMessage.Text = string.Empty;
                    }
                }
                else
                {
                    ShowAlert("Selected Project Does not Match With The Project Values Present in the Excel File Uploaded");

                    lblCount.Text = " File Not Uploaded Successfully ";
                    lblCount.ForeColor = System.Drawing.Color.Red;
                    DownloadFailureData.Visible = false;
                }

            }
            catch (Exception ex)
            {
                ShowAlert("Missing Values Found Error or Uploaded sheet is improper Please Check and Reupload. Kindly refer Upload Instruction.");
                return;
            }
        }
        catch (SqlException sqlEx)
        {
            string cleanMsg = sqlEx.Message.ToUpper().Replace("\r", "").Replace("\n", "").Trim();

            if (cleanMsg.Contains("CRITERIA_MASTER_NOT_FOUND"))
            {
                ShowAlert("Conditions Not Defined. Please contact NIELIT Head Office", true);
                return;
            }
            else
            {
                // Log ex if needed
                ShowAlert("Errors in uploaded file, Kindly check Date Format and Other fields ", true);
                return;
            }
        }
        catch (Exception ex)
        {
            // Log ex if needed
            ShowAlert("Errors in uploaded file, Kindly check Date Format and Other fields ", true);
            return;
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        flUpload.Dispose();
    }
    public void connection()
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        con = new SqlConnection(constr);
        con.Open();
    }
    protected void DownloadFailureData_Click(object sender, EventArgs e)
    {
        // Query to fetch data
        System.IO.StringWriter StringWrite = new System.IO.StringWriter();
        Html32TextWriter htmlWrite;
        ShowData();

        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=IncorrectData.xls");
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
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                //SqlCommand scCommand = new SqlCommand("SELECT [ID],[field1],[field2],[field3],[field4],[field5],[name],[dob],[remarks],[uploadedFileName]  FROM [NIELIT].[dbo].[temp_projStudentsMaster] where id> @lastId and status=0", new SqlConnection(con.ConnectionString));
                SqlCommand scCommand = new SqlCommand("SELECT [ID],[field1],[field2],[field3],[field4],[field5],[name],[dob],[remarks],[uploadedFileName]  FROM [NIELIT_PREPROD].[dbo].[temp_projStudentsMaster] where id> @lastId and status=0 and projid= @projid and enterBy = @enterby and uploadedFileName= @uploadedFileName", new SqlConnection(con.ConnectionString));

                scCommand.Parameters.AddWithValue("@lastId", Convert.ToInt32(lblLastId.Text.ToString()));
                scCommand.Parameters.AddWithValue("@projID", ddlProject.SelectedValue);
                scCommand.Parameters.AddWithValue("@enterBy", Convert.ToInt64(Session["UserID"]));
                scCommand.Parameters.AddWithValue("@uploadedFileName", hf_uploadedfilename.Value.ToString());

                //scCommand.CommandTimeout = 50000;
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
                        tdRow2.Text = ds.Tables[0].Rows[i]["ID"].ToString();
                        tdRow2.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(tdRow2);

                        TableCell tdRow3 = new TableCell();
                        tdRow3.Width = Unit.Percentage(5);
                        tdRow3.Text = ds.Tables[0].Rows[i]["field1"].ToString();
                        tdRow3.HorizontalAlign = HorizontalAlign.Center;

                        tr.Cells.Add(tdRow3);

                        TableCell tdRow7a = new TableCell();
                        tdRow7a.Width = Unit.Percentage(5);
                        tdRow7a.Text = ds.Tables[0].Rows[i]["field2"].ToString();
                        tdRow7a.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow7a);

                        TableCell tdRow16 = new TableCell();
                        tdRow16.Width = Unit.Percentage(5);
                        tdRow16.Text = ds.Tables[0].Rows[i]["field3"].ToString();
                        tdRow16.HorizontalAlign = HorizontalAlign.Center;

                        tr.Cells.Add(tdRow16);

                        TableCell tdRow12a = new TableCell();
                        tdRow12a.Width = Unit.Percentage(4);
                        tdRow12a.Text = ds.Tables[0].Rows[i]["field4"].ToString();
                        tdRow12a.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow12a);

                        TableCell tdRow12 = new TableCell();
                        tdRow12.Width = Unit.Percentage(4);
                        tdRow12.Text = ds.Tables[0].Rows[i]["field5"].ToString();
                        tdRow12.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow12);

                        TableCell tdRow13 = new TableCell();
                        tdRow13.Width = Unit.Percentage(5);
                        tdRow13.Text = ds.Tables[0].Rows[i]["name"].ToString();
                        tdRow13.HorizontalAlign = HorizontalAlign.Center;

                        tr.Cells.Add(tdRow13);

                        TableCell tdRow4 = new TableCell();
                        tdRow4.Width = Unit.Percentage(20);
                        tdRow4.Text = ds.Tables[0].Rows[i]["dob"].ToString();
                        tdRow4.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow4);

                        TableCell tdRow5 = new TableCell();
                        tdRow5.Width = Unit.Percentage(15);
                        tdRow5.Text = ds.Tables[0].Rows[i]["remarks"].ToString();
                        tdRow5.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow5);

                        tbl.Rows.Add(tr);

                    }
                    //lblheading.Visible = true; 
                }
                else
                {
                    lblLastId.Visible = true;
                    lblLastId.Text = "No Record Found !";
                }
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

            tc1.ColumnSpan = 14;
            tc1.Text = " UnUploaded Data";

            tc1.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tc1);

            tbl.Rows.Add(th1);

            TableHeaderRow th2 = new TableHeaderRow();
            th2.CssClass = "head1";

            TableHeaderCell tc2 = new TableHeaderCell();
            tc2.Width = Unit.Percentage(100);

            tc2.ColumnSpan = 14;
            tc2.Text = " Generated on: " + System.DateTime.Now.ToLongDateString();
            tc2.HorizontalAlign = HorizontalAlign.Center;
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
            tcCol2.Text = "Id";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tdRow113 = new TableHeaderCell();
            tdRow113.Width = Unit.Percentage(15);
            tdRow113.Text = "School Code";
            tdRow113.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tdRow113);

            TableHeaderCell tcCol7a = new TableHeaderCell();
            tcCol7a.Width = Unit.Percentage(5);
            tcCol7a.Text = "Class";
            tcCol7a.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7a);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(15);
            tcCol6.Text = "Roll Number";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);


            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(15);
            tcCol3.Text = "Field 4";
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(15);
            tcCol4.Text = "Field 5";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(20);
            tcCol5.Text = "Name";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol15 = new TableHeaderCell();
            tcCol15.Width = Unit.Percentage(20);
            tcCol15.Text = "Date of Birth";
            tcCol15.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol15);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(5);
            tcCol7.Text = "Reason";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);

            //TableHeaderCell tcCol8 = new TableHeaderCell();
            //tcCol8.Width = Unit.Percentage(5);
            //tcCol8.Text = "uploadedFileName";
            //tcCol8.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol8);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnreset_Click(object sender, EventArgs e)
    {
        ddlProject.SelectedIndex = 0;
        lblTotal.Text = "Total Count : 0";
        lblCount.Text = "";
        DownloadFailureData.Visible = false;

        ddlProject.Enabled = true;
        flUpload.Enabled = true;
    }
}