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
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;

public partial class Admin_BSBSchoolDataUpload : BasePage
{
    Int64 entityID = 0;
    UserType loginUserType;
    Int32 currentRoleId = 0;
    Table tbl = new Table();
    //int lastUploadedId = 0;
    SqlConnection con = new SqlConnection();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            //entityID = Convert.ToInt64(Session["EntityID"]);
            entityID = 5009;
            //FillProjects();

        }

        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;

        try
        {
            //if (IsSessionAlive() == true)
            //if (IsSessionAlive() == false)
            //    Response.Redirect("~/Index.aspx");
            //loginUserType = (UserType)Session["UserType"];
            //entityID = Convert.ToInt64(Session["EntityID"]);
            //currentRoleId = Convert.ToInt32(Session["RoleID"]);
            //if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/UPProjectStudentDataUpload.aspx"))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}

            if (!Page.IsPostBack)
            {
                //entityID = Convert.ToInt64(Session["EntityID"]);
                entityID = 5009;
                //FillProjects();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    //protected void FillProjects()
    //{
    //    try
    //    {
    //        using (NIELITMISContext context = new NIELITMISContext())
    //        {

    //            ListItem lst1 = new ListItem(" -- Select Project -- ", "0");
    //            var projects = (from t in context.NielitProjectss
    //                            join x in context.projectMainCentres on t.ID equals x.projectID
    //                            where t.projectTodate >= DateTime.Today
    //                            && x.centreID == entityID
    //                            orderby (t.ProjectName)
    //                            select new { ValueField = t.ID, TextField = t.ProjectName });

    //            //.Union(from t in context.NielitProjectss
    //            //       join x in context.projectSubCentres on t.ID equals x.projectID
    //            //       join i in context.AffInstitutes on x.centreID equals i.ID
    //            //       where t.projectTodate >= DateTime.Today
    //            //           && i.instituteID == entityID
    //            //       orderby (t.ProjectName)
    //            //       select new { ValueField = t.ID, TextField = t.ProjectName });

    //            //EConnect.Utils.Common.ControlUtility.BindListObject(ddlProject, projects, lst1);

    //        };
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    ////}

    protected void btnUpload_Click(object sender, EventArgs e)
    {

        //EConnectContext context = new EConnectContext();

        //Int32 TotalRecords = 0, ValidateRecords = 0, NotValidateRecords = 0;
        //List<Int64> ValidatedAppRecords = new List<Int64>();
        bool projEqual = true;
        StringBuilder sb = new StringBuilder();
        //String[] arr = new String[10];
        //arr[0] = "Uploaded Excel Data is not correct.Please Correct the data and Upload again";
        //arr[1] = "Excel Data File is not related to selected Project";
        //arr[2] = "Data Already Uploaded"; arr[3] = "does not exist";
        //EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();

        String filepath = Server.MapPath("../UploadedFiles");
        if (!Directory.Exists(filepath))
        {
            Directory.CreateDirectory(filepath);
        }
        flUpload.SaveAs(filepath + "/" + flUpload.FileName);
        String path = (filepath + "/" + flUpload.FileName);
        //string path = Server.MapPath("../UploadedFiles/" + flUpload.FileName);
        //flUpload.SaveAs(path);
        String ext = System.IO.Path.GetExtension(this.flUpload.PostedFile.FileName);
        String excelConnectionString = "";
        if (ext.ToUpper() == ".XLS")
        {
            excelConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";", path);
            //excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;", path);
        }
        else if (ext.ToUpper() == ".XLSX")
        { //accessConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=False;", path);
            //excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.16.0; Data Source={0};Extended Properties=\"Excel 16.0 Xml;HDR=Yes;IMEX=1\";", path);
            excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\";", path);

        }
        else { ShowAlert("Please Choose .XLS/.XLSX Extension Database", true); return; }

        OleDbConnection Econ = new OleDbConnection(excelConnectionString);

        //New_Added_My_Code_20_12_2024_Start
        Econ.Open();
        System.Data.DataTable dtExcelSchema;
        dtExcelSchema = Econ.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();

        //New_Added_My_Code_20_12_2024_End

        //string Query = "SELECT * FROM [@sheetName] WHERE projID  IS NOT NULL AND projID <> ''";

        string Query = "SELECT * From [" + sheetName + "]";
        //string Query = "SELECT * From [@sheetName]";
        OleDbCommand Ecom = new OleDbCommand(Query, Econ);
        //Ecom.Parameters.AddWithValue("@sheetName", sheetName);
        //Econ.Open();

        //DataSet ds = new DataSet();
        System.Data.DataTable dt = new System.Data.DataTable();
        OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
        //oda.Fill(ds);
        oda.Fill(dt);
        Econ.Close();
        int totalExcelRows = dt.Rows.Count;

        //foreach (DataRow row in dt.Rows)
        //{
        //    string id = row["projID"].ToString();
        //}

        //   var validRows = dt.AsEnumerable().ToList();
        //                .Where(row => row["projID"] != DBNull.Value && !string.IsNullOrWhiteSpace(row["projID"].ToString()))
        //                .ToList();

        // Made to check whether uploaded data is matching with the required Project, but since we dont have such criteria in this page it is useless
        //foreach (DataRow row in dt.Rows)
        //foreach (DataRow row in validRows)
        //{

        //    //if (!string.IsNullOrWhiteSpace(row["projID"].ToString()))
        //    //{
        //        if (ddlProject.SelectedValue == row["projID"].ToString())
        //            continue;
        //        else
        //        {
        //            projEqual = false;
        //            break;
        //        }
        //    //}
        //}

        //if (projEqual == true)
        //    if (projEqual == true)
        //{

        //Commented_Procedure_23_12_2024_For_Testing_Start
        using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["EConnectContext"].ToString()))
        {
            connection.Open();

           

            using (var command = new SqlCommand("uploadtempBSBSchools", connection))
            {

                foreach (DataRow row in dt.Rows)
                {
                    //if (!string.IsNullOrWhiteSpace(row["projID"].ToString()))3
                    //{

                    string UDISECode = row["UDISECode"].ToString();
                    string SchoolName = row["SchoolName"].ToString();
                    string District = row["District"].ToString();



                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@UDISECode", string.IsNullOrWhiteSpace(UDISECode) ? (object)DBNull.Value : UDISECode);
                    command.Parameters.AddWithValue("@SchoolName", string.IsNullOrWhiteSpace(SchoolName) ? (object)DBNull.Value : SchoolName);
                    command.Parameters.AddWithValue("@District", string.IsNullOrWhiteSpace(District) ? (object)DBNull.Value : District);
                    command.Parameters.AddWithValue("@uploadedFileName", flUpload.FileName);
                    command.Parameters.AddWithValue("@enterBy", Convert.ToInt64(Session["UserID"]));
                    command.ExecuteNonQuery();
                    //}
                }
            }

            connection.Close();
            connection.Open();
            using (var command = new SqlCommand("uploadBSBSchools", connection))
            {

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                command.Parameters.Add("@SuccessCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@FailureCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                command.Parameters.Add("@lastUploadedID", SqlDbType.Int).Direction = ParameterDirection.Output;
                //command.Parameters.AddWithValue("@projID", string.IsNullOrWhiteSpace(ddlProject.SelectedValue) ? (object)DBNull.Value : ddlProject.SelectedValue);

                command.ExecuteNonQuery();

                // Fetch the output parameters
                Int32 successCount = Convert.ToInt32(command.Parameters["@SuccessCount"].Value);
                Int32 failureCount = Convert.ToInt32(command.Parameters["@FailureCount"].Value);
                Int32 lastUploadedId = Convert.ToInt32(command.Parameters["@lastUploadedID"].Value);
                lblLastId.Text = lastUploadedId.ToString();

                lblCount.Text = " Total Records: " +totalExcelRows+" Records Uploaded: " + successCount + " Records Failed: " + failureCount;
                lblCount.ForeColor = System.Drawing.Color.Black;

                if (failureCount > 0)
                    DownloadFailureData.Visible = true;

                //DownloadFailueDataInExcel(lastUploadedId);

            }
            connection.Close();

            //Commented_Procedure_23_12_2024_For_Testing_End

        }
        // closed bracket of if }
        //else
        //{
        //    ShowAlert("Selected Project Does not Match With The Project Values Present in the Excel File Uploaded");
        //    lblCount.Text = " File Not Uploaded Successfully";
        //    lblCount.ForeColor = System.Drawing.Color.Red;
        //    DownloadFailureData.Visible = false;
        //}
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

    //protected void DownloadFailueDataInExcel(int lastId)
    //{

    //}

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

            // SINCE WE HAVE TO SHOW ONLY FAILURE DATA, AND TEMP TABLE IS FAILURE DATA , THE STATUS WILL BE 0

            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ToString();
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand scCommand = new SqlCommand(
                    "SELECT [ID],[UDISECode],[SchoolName],[District],[enterDate], [remarks], [uploadedFileName] FROM [NIELIT].[dbo].[tempBSBSchools] where id> @lastId and status=0", new SqlConnection(con.ConnectionString));

                // last id because, when we upload whole excel in temp talbe. 
                scCommand.Parameters.AddWithValue("@lastId", Convert.ToInt32(lblLastId.Text.ToString()));

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
                        tdRow3.Text = ds.Tables[0].Rows[i]["UDISECode"].ToString();
                        tdRow3.HorizontalAlign = HorizontalAlign.Center;

                        tr.Cells.Add(tdRow3);

                        TableCell tdRow7a = new TableCell();
                        tdRow7a.Width = Unit.Percentage(5);
                        tdRow7a.Text = ds.Tables[0].Rows[i]["SchoolName"].ToString();
                        tdRow7a.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow7a);

                        TableCell tdRow16 = new TableCell();
                        tdRow16.Width = Unit.Percentage(5);
                        tdRow16.Text = ds.Tables[0].Rows[i]["District"].ToString();
                        tdRow16.HorizontalAlign = HorizontalAlign.Center;

                        tr.Cells.Add(tdRow16);


                        TableCell tdRow12a = new TableCell();
                        tdRow12a.Width = Unit.Percentage(4);
                        tdRow12a.Text = ds.Tables[0].Rows[i]["EnterDate"].ToString();
                        tdRow12a.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow12a);

                        TableCell tdRow4 = new TableCell();
                        tdRow4.Width = Unit.Percentage(20);
                        tdRow4.Text = ds.Tables[0].Rows[i]["Remarks"].ToString();
                        tdRow4.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow4);

                        TableCell tdRow5 = new TableCell();
                        tdRow5.Width = Unit.Percentage(15);
                        tdRow5.Text = ds.Tables[0].Rows[i]["UploadedFileName"].ToString();
                        tdRow5.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow5);


                        tbl.Rows.Add(tr);

                    } //lblheading.Visible = true; 
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
            tc1.Text = " UnImported Data";

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
            tdRow113.Text = "UDISE Code";
            tdRow113.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tdRow113);

            TableHeaderCell tcCol7a = new TableHeaderCell();
            tcCol7a.Width = Unit.Percentage(5);
            tcCol7a.Text = "School Name";
            tcCol7a.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7a);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(15);
            tcCol6.Text = "District";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);


            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(15);
            tcCol3.Text = "Entered Date";
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(15);
            tcCol4.Text = "Remarks";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(20);
            tcCol5.Text = "UploadedFileName";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);

            //TableHeaderCell tcCol15 = new TableHeaderCell();
            //tcCol15.Width = Unit.Percentage(20);
            //tcCol15.Text = "Date of Birth";
            //tcCol15.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol15);

            //TableHeaderCell tcCol7 = new TableHeaderCell();
            //tcCol7.Width = Unit.Percentage(5);
            //tcCol7.Text = "Failure Reason";
            //tcCol7.HorizontalAlign = HorizontalAlign.Center;
            //th.Cells.Add(tcCol7);

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

}