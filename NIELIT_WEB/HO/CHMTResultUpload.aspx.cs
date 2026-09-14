using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using iTextSharp.text;
using iTextSharp.text.pdf;
using EConnect.DAL;

public partial class HO_CHMTResultUpload : BasePage
{

    string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 loginUserNo = 0;
    Int32 currentRoleId = 0;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            else
            {      
            //ddlExam_SelectedIndexChanged(ddlExam, EventArgs.Empty);
                if (!IsPostBack)
                {
                    BindExamDropdown();
                    btnCompile.Enabled = false;
                    //ddlExam_SelectedIndexChanged(ddlExam, EventArgs.Empty);
                    //ddlExam_SelectedIndexChanged(sender, e);
                    //btnTransfer.Enabled = true;
                    // btnTransfer.Visible = true;
                }
                else
                {
                    btnTransfer.Enabled = false;
                    lblcount.Visible = false;
                    lblPortCount.Visible = false;
                    lblCompileCount.Visible = false;
                    lblFinalizeCount.Visible = false;
                    lblError.Visible = false;
                   // btnFinalize.Enabled = false;
                }
           }
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        
    }
  
    private void BindExamDropdown()
    {

        try
        {           
            string query = " select distinct(e.name) as examName,e.id  as id  from Course_Exam_Application_Detail cead, exam e where e.Course_ID =1213 and cead.Course_ID = e.Course_ID and e.Result_Publish_Date is  null" +
                           " and not exists ( select  1  from CHMT_OLevel_ResultFreeze f where e.ID= f.exam_id and f.whether_finalized='Y')" +
                          "  and  exists ( select   1  from   Course_Exam_Application_Detail cead  where   cead.Exam_ID = e.ID)";

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
               
                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }                
                    ddlExam.DataSource = dt;
                    ddlExam.DataTextField = "examName"; // Displayed text
                    ddlExam.DataValueField = "id";  // Value associated with the item
                    ddlExam.DataBind();

                    ddlExam.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select One--", "0"));            

                }
            }
        }
        catch (Exception ex)
        {
              ShowAlert(ex.Message);
        }
    }

    protected void ddlExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //rbtnTheoryMarks_CheckedChanged( rbtnTheoryMarks,  e);
            int examId = Convert.ToInt32(ddlExam.SelectedValue);
           // if (examId != 0)
           // {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("CHMT_IsCompiled", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@examId", examId);

                        SqlParameter outputParam = new SqlParameter("@compiledTrue", SqlDbType.Int);
                        outputParam.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(outputParam);

                        con.Open();
                        cmd.ExecuteNonQuery();

                        Int32 compiledFlag = Convert.ToInt32(cmd.Parameters["@compiledTrue"].Value);
                        if (compiledFlag == 1)
                        {
                            btnCompile.Enabled = false;

                            using (SqlCommand cmd1 = new SqlCommand("CHMT_IsFinalized", con))
                            {
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.Parameters.AddWithValue("@examId", examId);

                                SqlParameter outParam = new SqlParameter("@finalizedTrue", SqlDbType.Int);
                                outParam.Direction = ParameterDirection.Output;
                                cmd1.Parameters.Add(outParam);

                                //con.Open();
                                cmd1.ExecuteNonQuery();
                                Int32 finalizedFlag = Convert.ToInt32(cmd1.Parameters["@finalizedTrue"].Value);
                                if (finalizedFlag == 0)
                                {
                                    btnFinalize.Enabled = true;
                                    //  btnFinalize.Text = "Hello";

                                   // ShowAlert("HEllo");
                                }
                                else
                                {
                                    lblError.Visible = true;
                                    lblError.Text = "Result has been finalized for this exam.";
                                    btnFinalize.Enabled = false;
                                }
                            }
                        }
                    }
                }
            //}
            //else
            //{
            //    btnFinalize.Enabled = false;
            //}
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void btnCompile_Click(object sender, EventArgs e)
    {

        int examId = Convert.ToInt32(ddlExam.SelectedValue);
        //string conString = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("CHMTFinalResultTransfer", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@examId", examId);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@User", loginUserNo);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@enteredDate", DateTime.Now);

                SqlParameter outputParam = new SqlParameter("@msg", SqlDbType.NVarChar, 255);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                con.Open();
                cmd.ExecuteNonQuery();

                String message = Convert.ToString(cmd.Parameters["@msg"].Value);

                if (!string.IsNullOrEmpty(message))
                {             
                    lblCompileCount.Visible = true;
                    lblCompileCount.Text = "Marks  already finalized cannot compile again.";
                   // btnDownloadPDF.Visible = true;
                    btnCompile.Enabled = false;
                }

                else
                {                   
                    lblCompileCount.Visible = true;
                    lblCompileCount.Text = "Data has been Compiled  successfully.";
                    lblCompileCount.ForeColor = System.Drawing.Color.Green;
                   //btnDownloadPDF.Visible = true;
                    btnCompile.Enabled = false;
                    btnFinalize.Enabled = true;
                }
                
            }
        }
    }

    private bool ContainsInvalidCharacters(string str, char[] invalidChars)
    {
        // Check if the string contains any of the specified invalid characters
        return str.IndexOfAny(invalidChars) != -1;
    }

    private bool ValidatDataPractical(DataTable table, out int errorRow, out string errorColumn, out List<string> errorMessages)
    {
        errorRow = -1;
        errorColumn = "";
        errorMessages = new List<string>();

        // Specify the columns that should be excluded from validation
        List<string> excludedColumns = new List<string> { "Level", "Module Name", "ATTENDANCE", "Exam Date" };

        // Perform your data type validation here
        // You may need to manually check each cell against the expected data type

        for (int rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
        {
            DataRow row = table.Rows[rowIndex];

             if (row["ATTENDANCE"] != DBNull.Value && string.Equals(row["ATTENDANCE"].ToString(), "A", StringComparison.OrdinalIgnoreCase))
                {
                    // Check if the marks are not 0 for an absent student
                    decimal examinerMarks = row["EXAMINER Marks(40)"] != DBNull.Value ? Convert.ToDecimal(row["EXAMINER Marks(40)"]) : 0;
                    decimal observerMarks40 = row["OBSERVER Marks(40)"] != DBNull.Value ? Convert.ToDecimal(row["OBSERVER Marks(40)"]) : 0;
                    decimal observerMarks20 = row["OBSERVER Marks (20)"] != DBNull.Value ? Convert.ToDecimal(row["OBSERVER Marks (20)"]) : 0;
                    decimal totalMarks = row["Total Marks"] != DBNull.Value ? Convert.ToDecimal(row["Total Marks"]) : 0;

                    if (examinerMarks != 0 || observerMarks40 != 0 || observerMarks20 != 0 || totalMarks != 0)
                    {
                        // Accumulate error message for this row
                        errorRow = rowIndex + 1;
                        errorMessages.Add("Error in Row " + errorRow + ": Student is marked absent, but marks are given.");
                        return false;
                    }
                }
             else
                {

                for (int columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
                {
                    DataColumn column = table.Columns[columnIndex];

                    // Skip excluded columns
                    if (excludedColumns.Contains(column.ColumnName))
                    {
                        continue;
                    }

                    // Check for null values
                    if (row[column] != DBNull.Value)
                    {
                        string strValue = row[column].ToString();

                        // Specify invalid characters
                        char[] invalidChars = { 'O', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

                        // Check for invalid characters or inability to parse to decimal
                        decimal tempValue;
                        if (ContainsInvalidCharacters(strValue, invalidChars) || !decimal.TryParse(strValue, out tempValue))
                        {
                            // Set the error details and return false
                            errorRow = rowIndex + 1; // Adding 1 because row index is zero-based
                            errorColumn = column.ColumnName;
                            return false;
                        }

                        if (column.ColumnName.Equals("Examiner Marks(40)", StringComparison.OrdinalIgnoreCase))
                        {
                            if (tempValue < 1 || tempValue > 40)
                            {
                                   // Set the error details and return false
                                errorRow = rowIndex + 1;
                                errorColumn = column.ColumnName;
                                errorMessages.Add("Error in Row "+ errorRow +", Column "+errorColumn+": Examiner Marks(40) should be between 1 and 40.");
                                return false;
                            }
                        }

                     if (column.ColumnName.Equals("Observer Marks(40)", StringComparison.OrdinalIgnoreCase))
                        {
                            if (tempValue < 1 || tempValue > 40)
                            {
                                // Set the error details and return false
                                errorRow = rowIndex + 1;
                                errorColumn = column.ColumnName;
                                errorMessages.Add("Error in Row "+ errorRow +", Column "+errorColumn+": Observer Marks(40) should be between 1 and 40.");
                                return false;
                            }
                        }

                        if (column.ColumnName.Equals("Observer Marks (20)", StringComparison.OrdinalIgnoreCase))
                        {
                            if (tempValue < 0 || tempValue > 20)
                            {
                                // Set the error details and return false
                                errorRow = rowIndex + 1;
                                errorColumn = column.ColumnName;
                                errorMessages.Add("Error in Row "+ errorRow +", Column "+errorColumn+": Observer Marks (20) should be between 0 and 20.");
                                return false;
                            }
                        }

                        decimal examinerMarks = row["Examiner Marks(40)"] != DBNull.Value ? Convert.ToDecimal(row["Examiner Marks(40)"]) : 0;
                        decimal observerMarks40 = row["Observer Marks(40)"] != DBNull.Value ? Convert.ToDecimal(row["Observer Marks(40)"]) : 0;
                        decimal observerMarks20 = row["Observer Marks (20)"] != DBNull.Value ? Convert.ToDecimal(row["Observer Marks (20)"]) : 0;
                        decimal totalMarks = examinerMarks + observerMarks40 + observerMarks20;

                        if (column.ColumnName.Equals("Total Marks", StringComparison.OrdinalIgnoreCase))
                        {
                            if (tempValue != totalMarks)
                            {
                                // Set the error details and return false
                                errorRow = rowIndex + 1;
                                errorColumn = column.ColumnName;
                                errorMessages.Add("Error in Row "+ errorRow +", Column "+errorColumn+": Total Marks should be the sum of Examiner Marks(40), Observer Marks(40), and Observer Marks (20).");
                                return false;
                            }
                        }

                        // Check if the total marks is in the range of 0 to 100
                        if (column.ColumnName.Equals("Total Marks", StringComparison.OrdinalIgnoreCase) || column.ColumnName.Equals("Total_marks_obtained", StringComparison.OrdinalIgnoreCase))
                        {
                            if (totalMarks < 0 || totalMarks > 100)
                            {
                                // Set the error details and return false
                                errorRow = rowIndex + 1;
                                errorColumn = column.ColumnName;
                                errorMessages.Add("Error in Row " + errorRow + ", Column " + errorColumn + ": Total Marks should be between 0 and 100.");
                                return false;
                            }
                        }                       
                    }
                }
            
             }
        }


        return true;
    }
    
    private bool ValidatDataTheory(DataTable table, out int errorRow, out string errorColumn, out List<string> errorMessages)
    {
         errorRow = -1;
    errorColumn = "";
    errorMessages = new List<string>();

    // Specify the columns that should be excluded from validation
    List<string> excludedColumns = new List<string> { "level_code", "module_type", "Absent_flag" };

    // Perform your data type validation here
    // You may need to manually check each cell against the expected data type

    for (int rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
    {
        DataRow row = table.Rows[rowIndex];


        if (row["Absent_flag"] != DBNull.Value && string.Equals(row["Absent_flag"].ToString(), "A", StringComparison.OrdinalIgnoreCase))
        {
            // Check if the marks are not 0 for an absent student

            decimal totalMarks = row["Total_marks_obtained"] != DBNull.Value ? Convert.ToDecimal(row["Total_marks_obtained"]) : 0;

            if ( totalMarks != 0)
            {
                // Accumulate error message for this row
                errorRow = rowIndex + 1;
                errorMessages.Add("Error in Row " + errorRow + ": Student is marked absent, but marks are given.");
                return false;
            }
        }

        else
        {

            for (int columnIndex = 0; columnIndex < table.Columns.Count; columnIndex++)
            {
                DataColumn column = table.Columns[columnIndex];

                // Skip excluded columns
                if (excludedColumns.Contains(column.ColumnName))
                {
                    continue;
                }

                // Check for null values
                if (row[column] != DBNull.Value)
                {
                    string strValue = row[column].ToString();

                    // Specify invalid characters
                    char[] invalidChars = { 'O', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

                    // Check for invalid characters or inability to parse to decimal
                    decimal tempValue;
                    if (ContainsInvalidCharacters(strValue, invalidChars) || !decimal.TryParse(strValue, out tempValue))
                    {
                        // Set the error details and return false
                        errorRow = rowIndex + 1; // Adding 1 because row index is zero-based
                        errorColumn = column.ColumnName;
                        errorMessages.Add("Error in Row " + errorRow + ", Column " + errorColumn + ": Invalid characters or invalid decimal format.");
                        return false;
                    }

                    if (column.ColumnName.Equals("Total_marks_obtained", StringComparison.OrdinalIgnoreCase) ||
                        column.ColumnName.Equals("Total_marks", StringComparison.OrdinalIgnoreCase))
                    {
                        if (tempValue < 0 || tempValue > 100)
                        {
                            // Set the error details and return false
                            errorRow = rowIndex + 1; // Adding 1 because row index is zero-based
                            errorColumn = column.ColumnName;
                            errorMessages.Add("Error in Row " + errorRow + ", Column " + errorColumn + ": Total marks should be between 0 and 100.");
                            return false;
                        }
                    }
                }
            }
        }
    }
    if (errorMessages.Count > 0)
    {   
        ShowAlert(string.Join("<br/>", errorMessages), true);
        return false;
    }
    return true;
    }
    
    protected void uploadPracticalMarks()
    {

        Int64 tempTableCount = 0;
        bool isValid = true;
        try
        {
            string filepath = Server.MapPath("../UploadedFiles");
            string fileName = "fl" + DateTime.Now.ToString("ddMMyyyyhhhhss") + "_" + flUpload.FileName;
            flUpload.SaveAs(filepath + "/" + fileName);
            string _path = (filepath + "/" + fileName);
            string ext = System.IO.Path.GetExtension(this.flUpload.PostedFile.FileName);
            string excelConnectionString = "";
            if (ext.ToUpper() == ".XLS")
                excelConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";", _path);
            else if (ext.ToUpper() == ".XLSX")
                excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\";", _path);
            else
            {
                ShowAlert("Please Choose .XLS/.XLSX Extension File", true);
                //return tempTableCount;
            }
            //  ExcelConn(_path);
            //string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", _path);
            OleDbConnection Econ = new OleDbConnection(excelConnectionString);
            //string Query = string.Format("Select [level_code], [Course_ID], [Exam_ID], [module_type], [Module_id], [module_code], [Absent_flag], [roll_no], [Registration_No], [Total_marks_obtained], [Exam Paper ID] FROM [{0}]", "Sheet1$");

            string Query = string.Format("Select [Level],[RegistrationNo], [Roll Number], [Module Code], [Module Name],[ATTENDANCE], [EXAMINER Marks(40)] , [OBSERVER Marks(40)], [OBSERVER Marks (20)], [Total Marks],[Exam Date],[Exam Paper ID] FROM [{0}]", "Sheet1$");

            OleDbCommand Ecom = new OleDbCommand(Query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
            oda.Fill(ds);
            Econ.Close();
            DataTable Exceldt = ds.Tables[0];
           
            Exceldt.AcceptChanges();

            //creating object of SqlBulkCopy
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

            using (SqlConnection sqlConnection = new SqlConnection(constr))
            {
                sqlConnection.Open();

                // Clear existing data in the table if needed
                string clearTableQuery = "DELETE FROM CHMT_OLevel_PracResult_from_Excel ";
                SqlCommand clearTableCommand = new SqlCommand(clearTableQuery, sqlConnection);
                clearTableCommand.ExecuteNonQuery();

                if (isValid)
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConnection))
                    {
                        sqlBulkCopy.DestinationTableName = "CHMT_OLevel_PracResult_from_Excel";

                        int errorRow;
                        string errorColumn;
                        List<string> errorMessages;
                         isValid = ValidatDataPractical(Exceldt, out errorRow, out errorColumn, out errorMessages);

                        if (!isValid)
                        {
                            string errorMessageString = string.Join("<br/>", errorMessages);
                            ShowAlert(errorMessageString, true);
                            return;
                        }

                        else
                        {
                            // Column mappings
                            sqlBulkCopy.ColumnMappings.Add("Level", "level_code");
                            sqlBulkCopy.ColumnMappings.Add("RegistrationNo", "registration_no");
                            sqlBulkCopy.ColumnMappings.Add("Roll Number", "roll_number");
                            sqlBulkCopy.ColumnMappings.Add("[Module Code]".Trim(), "module_code");

                            sqlBulkCopy.ColumnMappings.Add("[Module Name]".Trim(), "module_name");
                            sqlBulkCopy.ColumnMappings.Add("[ATTENDANCE]".Trim(), "attendance");
                            sqlBulkCopy.ColumnMappings.Add("[EXAMINER Marks(40)]".Trim(), "examiner_marks_40");
                            sqlBulkCopy.ColumnMappings.Add("[OBSERVER Marks(40)]".Trim(), "observer_marks_40");

                            sqlBulkCopy.ColumnMappings.Add("[OBSERVER Marks (20)]".Trim(), "observer_marks_20");
                            sqlBulkCopy.ColumnMappings.Add("[Total Marks]".Trim(), "total_marks");
                            sqlBulkCopy.ColumnMappings.Add("[Exam Date]".Trim(), "exam_date");
                            sqlBulkCopy.ColumnMappings.Add("[Exam Paper ID]".Trim(), "exam_paper_id");
                            //sqlBulkCopy.ColumnMappings.Add("[Exam_ID]".Trim(), "exam_id");

                            sqlBulkCopy.WriteToServer(Exceldt);
                        }
                    }
                }
                Int64 examId =    Convert.ToInt64(ddlExam.SelectedValue);

                 tempTableCount = Convert.ToInt64(EConnect.Utils.Data.DbUtility.ExecuteScaller("select  count(*) from  CHMT_OLevel_PracResult_from_Excel  ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

                if (tempTableCount != 0)
                {
                    //ShowAlert("Data has been imported successfully.");
                    lblcount.Visible = true;
                    lblcount.Text = "CHM-T O Level Practical marks has been imported successfully. No. of  records moved to  temp table : " + Convert.ToString(tempTableCount);
                    lblcount.ForeColor = System.Drawing.Color.Green;
                    IsPracticalMarksUploaded = true;
                    rbtnTheoryMarks.Enabled = false;
                }
                else
                {
                    lblcount.Visible = true;
                    lblcount.Text = "CHM-T O Level Practical marks has not been imported.Please contact the Administrator.";
                }              
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Data has not been Imported due to " + ex.Message + " Not Imported");          
        }
    }

    protected void uploadTheoryMarks()
    {
        Int64 tempTableCount = 0;
        bool isValid = true;
        try
        {
            string filepath = Server.MapPath("../UploadedFiles");
            string fileName = "fl" + DateTime.Now.ToString("ddMMyyyyhhhhss") + "_" + flUpload.FileName;
            flUpload.SaveAs(filepath + "/" + fileName);
            string _path = (filepath + "/" + fileName);
            string ext = System.IO.Path.GetExtension(this.flUpload.PostedFile.FileName);
            string excelConnectionString = "";

            if (ext.ToUpper() == ".XLS")
                excelConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";", _path);
            else if (ext.ToUpper() == ".XLSX")
                excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\";", _path);
            else
            {
                ShowAlert("Please Choose .XLS/.XLSX Extension File", true);
               // return tempTableCount;
            }

            OleDbConnection Econ = new OleDbConnection(excelConnectionString);

            string Query = string.Format("Select [level_code],[Course_ID], [Exam_ID], [module_type], [Module_id],[module_code], [Absent_flag] , [roll_no],[Registration_No], [Total_marks_obtained], [Exam Paper ID] FROM [{0}]", "Sheet1$");

            OleDbCommand Ecom = new OleDbCommand(Query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
            oda.Fill(ds);
            Econ.Close();
            DataTable Exceldt = ds.Tables[0];

            Exceldt.AcceptChanges();

            using (SqlConnection sqlConnection = new SqlConnection(constr))
            {
                sqlConnection.Open();

                string clearTableQuery = "DELETE FROM CHMT_OLevel_ThResult_from_Excel";
                SqlCommand clearTableCommand = new SqlCommand(clearTableQuery, sqlConnection);
                clearTableCommand.ExecuteNonQuery();


                if (isValid)
                {

                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConnection))
                    {
                        sqlBulkCopy.DestinationTableName = "CHMT_OLevel_ThResult_from_Excel";

                        int errorRow;
                        string errorColumn;
                        List<string> errorMessages;
                        isValid = ValidatDataTheory(Exceldt, out errorRow, out errorColumn, out errorMessages);

                        if (!isValid)
                        {
                            // Use the error messages list to display all errors
                            string errorMessageString = string.Join("<br/>", errorMessages);
                            ShowAlert(errorMessageString, true);
                            return;
                        }

                        else
                        {
                            // Explicit column mappings with data type specifications
                            sqlBulkCopy.ColumnMappings.Add("level_code", "level_code");
                            sqlBulkCopy.ColumnMappings.Add("Course_ID", "Course_ID");
                            sqlBulkCopy.ColumnMappings.Add("Exam_ID", "Exam_ID");
                            sqlBulkCopy.ColumnMappings.Add("module_type", "module_type");
                            sqlBulkCopy.ColumnMappings.Add("Module_id", "Module_id");
                            sqlBulkCopy.ColumnMappings.Add("module_code", "module_code");
                            sqlBulkCopy.ColumnMappings.Add("Absent_flag", "Absent_flag");
                            sqlBulkCopy.ColumnMappings.Add("roll_no", "roll_no");
                            sqlBulkCopy.ColumnMappings.Add("Registration_No", "Registration_No");
                            sqlBulkCopy.ColumnMappings.Add("Total_marks_obtained", "Total_marks_obtained");
                            sqlBulkCopy.ColumnMappings.Add("Exam Paper ID", "exam_paper_id");

                            sqlBulkCopy.WriteToServer(Exceldt);
                        }
                    }
                }

                Int64 examId = Convert.ToInt64(ddlExam.SelectedValue);
                tempTableCount = Convert.ToInt64(EConnect.Utils.Data.DbUtility.ExecuteScaller("select  count(*) from  CHMT_OLevel_ThResult_from_Excel  where exam_id= '" + examId + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

                if (tempTableCount != 0)
                {
                    lblcount.Visible = true;
                    lblcount.Text = "CHM-T O Level Theory marks has been imported successfully. No. of  records moved to  temp table : " + Convert.ToString(tempTableCount);
                    lblcount.ForeColor = System.Drawing.Color.Green;
                    IsTheoryMarksUploaded = true;
                    //rbtnPracticalMarks.Enabled = false;
                }
                else
                {
                    lblcount.Visible = true;
                    lblcount.Text = "CHM-T O Level Theory marks has not been imported. Please contact the Administrator.";
                }

               // return tempTableCount;
            }
        }
        catch (Exception ex)
        {
            ShowAlert("Data has not been Imported due to " + ex.Message + " Not Imported");
            ///return tempTableCount;
        }
    }

    protected bool IsTheoryMarksUploaded
    {
        get { return ViewState["IsTheoryMarksUploaded"] as bool? ?? false; }
        set { ViewState["IsTheoryMarksUploaded"] = value; }
    }

    protected bool IsPracticalMarksUploaded
    {
        get { return ViewState["IsPracticalMarksUploaded"] as bool? ?? false; }
        set { ViewState["IsPracticalMarksUploaded"] = value; }
    }

    private void UpdateTransferMarksButtonStatus()
    {
        // Enable the Transfer Marks button if both theory and practical marks are uploaded
        btnTransfer.Enabled = IsTheoryMarksUploaded && IsPracticalMarksUploaded;
    }

    protected void btnValidate_Click(object sender, EventArgs e)
    {       
         try
                {

                    if (!rbtnTheoryMarks.Checked && !rbtnPracticalMarks.Checked)
                    {
                        lblError.Text = "Please select either Theory or Practical Marks";
                        lblError.Visible = true;
                        return;
                    }
                                   
                    if (rbtnTheoryMarks.Checked)
                    {
                        uploadTheoryMarks();
                      //  IsTheoryMarksUploaded = tempTableThCount > 0;

                        //if (IsTheoryMarksUploaded)
                        //{
                        //    IsTheoryMarksUploaded = false;
                        //}
                    }

                    if (rbtnPracticalMarks.Checked)
                    {
                          uploadPracticalMarks();
                       // IsTheoryMarksUploaded = tempTableThCount > 0;


                        //if (IsPracticalMarksUploaded)
                        //{
                        //    IsPracticalMarksUploaded = false;
                        //}
                    }
                 
                    UpdateTransferMarksButtonStatus();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(string.Format("Data has not been Imported due to :{0}", ex.Message), "Not Imported", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ShowAlert("Data has not been Imported due to " + ex.Message + " Not Imported");                    
                }                
            }

    //protected void showMismatchedData()
    //{
    //    try
    //    {

    //        int examId = Convert.ToInt32(ddlExam.SelectedValue);
    //        string conString = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

    //        using (SqlConnection con = new SqlConnection(conString))
    //        {
    //            using (SqlCommand cmd = new SqlCommand("CHMTResultTransfer", con))
    //            {
    //                cmd.CommandType = CommandType.StoredProcedure;

    //                // Add parameters if your stored procedure requires them
    //                cmd.Parameters.AddWithValue("@exam_id", examId);

    //                // Open the connection
    //                con.Open();

    //                // Execute the stored procedure and load the results into a DataTable
    //                DataTable dt = new DataTable();
    //                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
    //                {
    //                    da.Fill(dt);
    //                }

    //                dt.Columns.Add("SrNo", typeof(int));
    //                int SrNo = 1;

    //                foreach (DataRow row in dt.Rows)
    //                {
    //                    row["SrNo"] = SrNo;
    //                    SrNo++;
    //                }

    //                grdMismatch.Visible = true;
    //                grdMismatch.DataSource = dt;
    //                grdMismatch.DataBind();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }    
    //}

    protected void grdMismatch_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        // Check if the current row is a data row
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Find the label control in the row
            Label lblSrNo = (Label)e.Row.FindControl("lblSrNo");

            // Check if the label control is found
            if (lblSrNo != null)
            {
                // Set the serial number in the label
                lblSrNo.Text = (e.Row.RowIndex + 1).ToString();
            }
        }
    }

    private DataTable GetMismatchResults()
    {

        string conString = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection sqlCon = new SqlConnection(conString);
        try
        {
            int examId = Convert.ToInt32(ddlExam.SelectedValue);
            DataTable dt = new DataTable();

            // Assuming you have a SqlConnection named sqlCon
            using (SqlCommand cmd = new SqlCommand("CHMT_Mismatch", sqlCon))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@examid", examId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
            }

            DataColumn srNoColumn = new DataColumn("SrNo", typeof(int));
            dt.Columns.Add(srNoColumn);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["SrNo"] = i + 1;
            }

            return dt;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
            return null;
        }
    }

    protected void btnTransfer_Click(object sender, EventArgs e)
    {
        try
        {

            int examId = Convert.ToInt32(ddlExam.SelectedValue);
            string conString = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            SqlConnection sqlCon = new SqlConnection(conString);


            SqlCommand cmd = new SqlCommand("CHMT_ThPrCountMatch", sqlCon);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@examId", examId);

            SqlParameter outP = new SqlParameter("@result", SqlDbType.NVarChar, 500);
            outP.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outP);
          
            sqlCon.Open();
            cmd.ExecuteNonQuery();
            sqlCon.Close();

            string resultCount = cmd.Parameters["@result"].Value as string;

            if (resultCount == "1")
            {

                    /*Commented 30  Apr 2024 DataTable dataTable  =  GetMismatchResults();

                     if (dataTable.Rows.Count > 0)
                     {

                         lblPortCount.Visible = true;
                         lblPortCount.Text = " Records for theory  and practical paper do not match. Result cannot  be compiled.Discrepancy exists in below records.";
                         grdMismatch.Visible = true;
                         grdMismatch.DataSource = dataTable;
                         grdMismatch.DataBind();
                     }    */
			 DataTable dataTable  =  GetMismatchResults();

                     if (dataTable.Rows.Count > 0)
                     {

                         lblPortCount.Visible = true;
                         lblPortCount.Text = " Records for theory  and practical paper do not match. Result cannot  be compiled.Discrepancy exists in below records.";
                         grdMismatch.Visible = true;
                         grdMismatch.DataSource = dataTable;
                         grdMismatch.DataBind();

                         using (EConnectContext context = new EConnectContext())
                         {

                             var mappedData = dataTable.AsEnumerable().Select((row, index) => new
                             {
                                 SrNo = index + 1, // Add sequential number
                                 Reg_no = row["Reg_no"], // Assuming 'ID' is a column in your DataTable
                                 Module_Code = row["Module_Code"], // Assuming 'Registration_Number' is a column in your DataTable
                                 Result = row["Result"] // Assuming 'Candidate_Name' is a column in your DataTable
                             });

                             PagingBar1.Bind(mappedData, ref grdMismatch);

                         }
                     }   
                

                     else
                     {
                         SqlCommand sqlCmd = new SqlCommand("CHMTResultTransfer", sqlCon);

                         sqlCmd.CommandType = CommandType.StoredProcedure;
                         sqlCmd.Parameters.AddWithValue("@exam_id", examId);

                         SqlParameter outParam1 = new SqlParameter("@inserted_Throw_count", SqlDbType.Int);
                         outParam1.Direction = ParameterDirection.Output;
                         sqlCmd.Parameters.Add(outParam1);

                         SqlParameter outParam2 = new SqlParameter("@inserted_Prrow_count", SqlDbType.Int);
                         outParam2.Direction = ParameterDirection.Output;
                         sqlCmd.Parameters.Add(outParam2);

                         //SqlParameter outParam3 = new SqlParameter("@mismatchOutput", SqlDbType.Int);
                         //outParam3.Direction = ParameterDirection.Output;
                         //sqlCmd.Parameters.Add(outParam3);

                         SqlParameter outParam4 = new SqlParameter("@message", SqlDbType.NVarChar, 500);
                         outParam4.Direction = ParameterDirection.Output;
                         sqlCmd.Parameters.Add(outParam4);

                         sqlCon.Open();
                         sqlCmd.ExecuteNonQuery();

                         //string msg = Convert.IsDBNull(sqlCmd.Parameters["@message"].Value) ? 0 : Convert.ToInt32(sqlCmd.Parameters["@message"].Value);

                         string msg = sqlCmd.Parameters["@message"].Value as string;

                         // Use the null-coalescing operator to handle null values
                         // msg = msg ?? "DefaultStringValue"; // Replace "DefaultStringValue" with your desired default value

                         // Now msg contains a non-null string value

                         //if (!string.IsNullOrEmpty(msg))
                         //{

                         //    lblPortCount.Visible = true;
                         //    lblPortCount.Text = msg;
                         //    btnCompile.Enabled = true;
                         //}

                         //else
                         //{

                         int insertedThRowCount = Convert.IsDBNull(sqlCmd.Parameters["@inserted_Throw_count"].Value) ? 0 : Convert.ToInt32(sqlCmd.Parameters["@inserted_Throw_count"].Value);

                         int insertedPrRowCount = Convert.IsDBNull(sqlCmd.Parameters["@inserted_Prrow_count"].Value) ? 0 : Convert.ToInt32(sqlCmd.Parameters["@inserted_Prrow_count"].Value);

                         // int mismatchCount = Convert.IsDBNull(sqlCmd.Parameters["@mismatchOutput"].Value) ? 0 : Convert.ToInt32(sqlCmd.Parameters["@mismatchOutput"].Value);
                         // if (insertedThRowCount != insertedPrRowCount)
                         // {               
                         //  lblPortCount.Visible = true;
                         // lblPortCount.Text = "Records for theory  and practical paper do not match. Result cannot  be compiled";
                         // if (mismatchCount > 0 || (insertedThRowCount == 0 && insertedPrRowCount == 0))
                         // if (insertedThRowCount == 0 && insertedPrRowCount == 0)
                         if (insertedThRowCount != insertedPrRowCount)
                         {
                            // lblPortCount.Visible = true;
                            // lblPortCount.Text = " Records for theory  and practical paper do not match. Result cannot  be compiled.Discrepancy exists in below records.";
                             //showMismatchedData();
                         }
                         else
                         {
                             lblPortCount.Visible = true;
                             lblPortCount.Text = " Data Validated. No  mismatch found .CHM-T O Level Theory records transferred successfully: " + insertedThRowCount + "  ,CHM-T O Level Practical records transferred successfully: " + insertedPrRowCount + "";
                             lblPortCount.ForeColor = System.Drawing.Color.Green;
                             btnValidate.Enabled = false;
                             btnCompile.Enabled = true;
                         }
                         // }

                         sqlCon.Close();
                         sqlCon.Dispose();
                     }
 
            }
            else if (resultCount == "-1")
            {
                ShowAlert(" These Theory and Practical records do not exist  in the database.");
                return;

            }
            else
            {
                ShowAlert("Theory  Count  and Practical  Count  do  not  Match.Result cannot  be processed further. ");
                return;
            }          

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
            // sqlCon.close();
        }

    }

    protected void rbtnTheoryMarks_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnTheoryMarks.Checked)
        {
            btnValidate.Text = "Upload Theory Marks";
           // IsPracticalMarksUploaded = false;

        }
      
        UpdateTransferMarksButtonStatus();
    }

    protected void rbtnPracticalMarks_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnPracticalMarks.Checked)
        {
            btnValidate.Text = "Upload Practical Marks";
           // IsTheoryMarksUploaded = false;
            lblcount.Visible = false;
        }
      
        UpdateTransferMarksButtonStatus();
    }

    protected void btnFinalize_Click(object sender, EventArgs e)
    {
        int examId = Convert.ToInt32(ddlExam.SelectedValue);


        if (examId != 0)
        {
		
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("CHMT_Grade_calculation", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@examId", examId);


                    SqlParameter outParam1 = new SqlParameter("@record_updated", SqlDbType.Int);
                    outParam1.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outParam1);

                    SqlParameter outParam2 = new SqlParameter("@record_Pass_Fail", SqlDbType.Int);
                    outParam2.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outParam2);

                    SqlParameter outParam3 = new SqlParameter("@record_pending", SqlDbType.Int);
                    outParam3.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outParam3);

                    SqlParameter outP4 = new SqlParameter("@record_absent", SqlDbType.Int);
                    outP4.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outP4);

                    SqlParameter outParam4 = new SqlParameter("@msgout", SqlDbType.NVarChar, 1000);
                    outParam4.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outParam4);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@finalizeUser", loginUserNo);

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@finalizeEntryDate", DateTime.Now);
		    cmd.CommandTimeout=300;

                    con.Open();
                    cmd.ExecuteNonQuery();

                    string message = Convert.ToString(cmd.Parameters["@msgout"].Value);

                    if (string.IsNullOrEmpty(message))
                    {
                        int recordsFinalized = Convert.IsDBNull(cmd.Parameters["@record_updated"].Value) ? 0 : Convert.ToInt32(cmd.Parameters["@record_updated"].Value);

                        int recordsPassFail = Convert.IsDBNull(cmd.Parameters["@record_Pass_Fail"].Value) ? 0 : Convert.ToInt32(cmd.Parameters["@record_Pass_Fail"].Value);

                        int recordsPending = Convert.IsDBNull(cmd.Parameters["@record_pending"].Value) ? 0 : Convert.ToInt32(cmd.Parameters["@record_pending"].Value);

                        int absentRecords = Convert.IsDBNull(cmd.Parameters["@record_absent"].Value) ? 0 : Convert.ToInt32(cmd.Parameters["@record_absent"].Value);


                        lblFinalizeCount.Visible = true;
                        lblFinalizeCount.Text = "Total candidates who are Finailzed:" + recordsFinalized + ",Total candidates for whom grades have been assigned:" + recordsPassFail + " ,Total candidates for whom grades have not been assigned: " + recordsPending + ",Total candidates who are absent in both paper: " + absentRecords;
                        lblFinalizeCount.ForeColor = System.Drawing.Color.Green;
                        btnFinalize.Enabled = false;
                        IsPracticalMarksUploaded = false;
                    }
                    else
                    {
                        lblFinalizeCount.Visible = true;
                        lblFinalizeCount.Text = "The data  appears  to be either  not  compiled or  already  finalized. ";

                    }
                }
            }
        }
       
        
        //else

    }

    
}