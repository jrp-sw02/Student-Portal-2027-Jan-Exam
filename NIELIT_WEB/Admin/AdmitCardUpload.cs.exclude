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

public partial class AdmitCardUpload : BasePage
{
    SqlConnection con = new SqlConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
       
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        
        if (!fuInstitituteList.HasFile)
        {
            ShowAlert("Please select file to upload.");
            return;
        }

        string filepath = Server.MapPath("../CertExamAdmitCard");
        fuInstitituteList.SaveAs(filepath + "/" + fuInstitituteList.FileName);
        string accesspath = Server.MapPath("../CertExamAdmitCard/") + fuInstitituteList.FileName;

        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + accesspath + ";Persist Security Info=False";
            
         string strSQL = "SELECT * FROM Exam_Database"; 
            // Create a connection  
         using (OleDbConnection connection = new OleDbConnection(connectionString))
         {
             // Create a command and set its connection  
             OleDbDataAdapter da = new OleDbDataAdapter(strSQL, connection);

             OleDbCommand command = new OleDbCommand(strSQL, connection);
             // Open the connection and execute the select command.

             try
             {
                 // Open connecton  
                 connection.Open();

                 DataSet ds = new DataSet();
                 da.Fill(ds);
                 //storing datset in viewstate
                 DataTable BulkTable = ds.Tables["Table"];
                 ViewState["BulkData"] = BulkTable;

                 BulkInsertToDataBase();

                 connection.Close();
                 ShowAlert("Data successfully inserted in Database.");
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);
             }  
         }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        fuInstitituteList.Dispose();
    }

    public void connection()
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        con = new SqlConnection(constr);
        con.Open();
    }

    private void BulkInsertToDataBase()
    {
        try
        {
            DataTable dtBulkData = (DataTable)ViewState["BulkData"];
            connection();
            //creating object of SqlBulkCopy  
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            //assigning Destination table name  
            objbulk.DestinationTableName = "tempCertificteExamAdmitCard";
            //Mapping Table column  

            objbulk.ColumnMappings.Add("APPLICATION_ID", "APPLICATION_ID");
            //objbulk.ColumnMappings.Add("Application_Date", "Application_Date");
            objbulk.ColumnMappings.Add("application_number", "APPLICATION_NUMBER");

            objbulk.ColumnMappings.Add("NAME", "NAME");
            objbulk.ColumnMappings.Add("F_NAME", "F_NAME");
            objbulk.ColumnMappings.Add("M_NAME", "M_NAME");
            objbulk.ColumnMappings.Add("D_O_B", "D_O_B");
            objbulk.ColumnMappings.Add("Rollno", "Rollno");
            objbulk.ColumnMappings.Add("cent_allot", "cent_allot");
            objbulk.ColumnMappings.Add("cent_add", "cent_add");
            objbulk.ColumnMappings.Add("examDate", "examDate");
            objbulk.ColumnMappings.Add("batch", "batch");
            objbulk.ColumnMappings.Add("rep_time", "rep_time");
            objbulk.ColumnMappings.Add("exam_month", "exam_month");
            objbulk.ColumnMappings.Add("exam_year", "exam_year");
            objbulk.ColumnMappings.Add("regexamname", "regexamname");



            //inserting bulk Records into DataBase   
            objbulk.WriteToServer(dtBulkData);
            con.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        } 
    }  

}