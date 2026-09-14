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
using EConnect.URM;

public partial class HO_CHMT_ES_ResultUpload : BasePage
{
    string conString = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 loginUserNo = 0;
    Int32 currentRoleId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {

        //if (IsSessionAlive() == false)
        //    Response.Redirect("../Index.aspx");
        //currentRoleId = Convert.ToInt32(Session["RoleID"]);
        //loginUserNo = Convert.ToInt32(Session["UserID"]);
        //// loginUserNo = 1111;
        //if (!UserManager.HasRight(currentRoleId, enmRight.View))
        //{
        //    Response.Write("Sorry! You don't have rights  to view this page");
        //    Response.End();
        //}
    }

    protected void rbtnESMarks_CheckedChanged(object sender, EventArgs e)
    {

    }

    private bool ContainsInvalidCharacters(string str, char[] invalidChars)
    {
        return str.IndexOfAny(invalidChars) != -1;
    }

   private bool ValidatDataES(DataTable table, out int errorRow, out string errorColumn, out List<string> errorMessages)
     {
         errorRow = -1;
         errorColumn = "";
         errorMessages = new List<string>();

         // Specify the columns that should be excluded from validation
         List<string> excludedColumns = new List<string> { "Module Name", "Absent Flag", "Exam Date" };

         // Perform your data type validation here
         // You may need to manually check each cell against the expected data type

         for (int rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
         {
             DataRow row = table.Rows[rowIndex];
             HashSet<string> uniqueEntries = new HashSet<string>();


             if (row["Absent Flag"] != DBNull.Value && string.Equals(row["Absent Flag"].ToString(), "A", StringComparison.OrdinalIgnoreCase))
             {
                 // Check if the marks are not 0 for an absent student

                 decimal totalMarks = row["Marks"] != DBNull.Value ? Convert.ToDecimal(row["Marks"]) : 0;

                 if (totalMarks != 0)
                 {
                     // Accumulate error message for this row
                     errorRow = rowIndex + 1;
                     errorMessages.Add("Error in Row " + errorRow + ": Student is marked absent, but marks are given.");
                     return false;
                 }
             }

             

             // Extract registration number, marks, and exam date for uniqueness check
             string registrationNo = row["Registration No"] != DBNull.Value ? row["Registration No"].ToString() : "";
             string marks = row["Marks"] != DBNull.Value ? row["Marks"].ToString() : "";
             string examDate = row["Exam Date"] != DBNull.Value ? Convert.ToDateTime(row["Exam Date"]).ToString("yyyy-MM-dd") : "";

             // Check if the current combination of registration number, marks, and exam date is already encountered
             if (!string.IsNullOrWhiteSpace(registrationNo) && !string.IsNullOrWhiteSpace(marks) && !string.IsNullOrWhiteSpace(examDate))
             {
                 string entryKey = registrationNo + "|" + marks + "|" + examDate;
                 if (!uniqueEntries.Add(entryKey))
                 {
                     // Accumulate error message for this row
                     errorRow = rowIndex + 1;
                     errorMessages.Add("Error in Row " + errorRow + ": Duplicate combination of Registration No, Marks, and Exam Date.");
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


                         Char attendanceFlag = 'a';
                         if (column.ColumnName.Equals("Absent Flag", StringComparison.OrdinalIgnoreCase))
                         {
                             if (attendanceFlag != 'A' && attendanceFlag != 'P' && attendanceFlag != 'a' && attendanceFlag != 'p')
                             {
                                 errorRow = rowIndex + 1;
                                 errorMessages.Add("Error  in  row" + errorRow + ": Attendance is  not  marked in  correct  format.");
                                 return false;
                             }
                         }

                         if (column.ColumnName.Equals("Marks", StringComparison.OrdinalIgnoreCase) )
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

   protected void grdError_RowDataBound(object sender, GridViewRowEventArgs e)
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
        
        SqlConnection sqlCon = new SqlConnection(conString);
        try
        {
            
            DataTable dt = new DataTable();

            // Assuming you have a SqlConnection named sqlCon
            using (SqlCommand cmd = new SqlCommand("CHMT_ES_Excel_Mismatch", sqlCon))
            {
                

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
  
    protected void btnUpload_Click(object sender, EventArgs e)
    {     

        if (rbtnESMarks.Checked)
        {
            
                     try
                     {
                         bool isValid = true;
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

                         }

                         OleDbConnection Econ = new OleDbConnection(excelConnectionString);

                         string Query = string.Format("Select [Registration No],[Module Name], [Absent Flag], [Marks], [Exam Date] FROM [{0}]", "Sheet1$");

                         OleDbCommand Ecom = new OleDbCommand(Query, Econ);
                         Econ.Open();

                         DataSet ds = new DataSet();
                         OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
                         oda.Fill(ds);
                         Econ.Close();
                         DataTable Exceldt = ds.Tables[0];


                         Exceldt.AcceptChanges();

                           

                         using (SqlConnection sqlConnection = new SqlConnection(conString))
                         {
                             sqlConnection.Open();

                        
                             string clearTableQuery = "DELETE FROM CHMT_OLevel_ES_from_Excel";
                             SqlCommand clearTableCommand = new SqlCommand(clearTableQuery, sqlConnection);
                             clearTableCommand.ExecuteNonQuery();

                             if (IsValid)
                             {
                                 using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(sqlConnection))
                                 {
                                     sqlBulkCopy.DestinationTableName = "CHMT_OLevel_ES_from_Excel";


                                     int errorRow;
                                     string errorColumn;
                                     List<string> errorMessages;
                                     isValid = ValidatDataES(Exceldt, out errorRow, out errorColumn, out errorMessages);

                                     if (!isValid)
                                     {
                                         string errorMessageString = string.Join("<br/>", errorMessages);
                                         ShowAlert(errorMessageString, true);
                                         return;
                                     }
                                     else
                                     {
                                         // Column mappings
                                         sqlBulkCopy.ColumnMappings.Add("Registration No", "registration_no");
                                         sqlBulkCopy.ColumnMappings.Add("Module Name", "module_name");
                                         sqlBulkCopy.ColumnMappings.Add("Absent Flag", "absent_flag");
                                         sqlBulkCopy.ColumnMappings.Add("Marks".Trim(), "marks");
                                         sqlBulkCopy.ColumnMappings.Add("Exam Date".Trim(), "exam_date");

                                         sqlBulkCopy.WriteToServer(Exceldt);
                                     }
                                 }
                             }
                             //Int64 examId = Convert.ToInt64(ddlExam.SelectedValue);
                             Int64 tempTableCount;
                             tempTableCount = Convert.ToInt64(EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from  CHMT_OLevel_ES_from_Excel", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                                                        

                                 DataTable dataTable = GetMismatchResults();

                                 if (dataTable.Rows.Count > 0)
                                 {

                                     //lblPortCount.Visible = true;
                                     //lblPortCount.Text = " Records for theory  and practical paper do not match. Result cannot  be compiled.Discrepancy exists in below records.";
                                     lblError.Visible = false;
                                     grdMismatch.Visible = true;
                                     grdMismatch.DataSource = dataTable;
                                     grdMismatch.DataBind();

                                 }
                             else
                             {
                                 lblError.Visible = true;
                                 lblError.ForeColor = System.Drawing.Color.Green;
                                 lblError.Text = "CHM-T O Level ES marks has been imported successfully. No. of  records moved to  temp table : " + Convert.ToString(tempTableCount);
                             }
                         }
                     }
                     catch (Exception ex)
                     {
                         ShowAlert("Data has not been Imported due to " + ex.Message + " Not Imported");
                         //return tempTableCount;
                     }
                }
            }

    protected void BindGridView()
    {
        try
        {
            SqlConnection sqlCon = new SqlConnection(conString);
            SqlCommand cmd = new SqlCommand("CHMT_ES_Checks_before_final_transfer", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@user_id", loginUserNo);
            cmd.Parameters.AddWithValue("@process_date", DateTime.Now);

            SqlParameter outP = new SqlParameter("@data_inserted", SqlDbType.Int);
            outP.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outP);

            sqlCon.Open();



            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);


            Int32 insertOut = Convert.IsDBNull(cmd.Parameters["@data_inserted"].Value) ? 0 : Convert.ToInt32(cmd.Parameters["@data_inserted"].Value);

            DataColumn srNoColumn = new DataColumn("SrNo", typeof(int));
            dataTable.Columns.Add(srNoColumn);
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dataTable.Rows[i]["SrNo"] = i + 1;
            }

            if (dataTable.Rows.Count > 0)
            {
                lblError.Visible = true;
                lblError.Text = "Total No of records inserted in  final table:" + insertOut;
                grdError.Visible = true;
                grdError.DataSource = dataTable;
                grdError.DataBind();
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "CHM-T  ES result is compiled and finalized. \nTotal No of records inserted in  final table:" + insertOut;
                grdError.Visible = false;

            }
            sqlCon.Close();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
  
    protected void btnFTransfer_Click(object sender, EventArgs e)
    {
        BindGridView();
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            grdError.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
   
}