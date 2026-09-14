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

public partial class CAND_testamit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        if (!FileUpload1.HasFile)
        {
            lblMessage.Text = "Please select an Excel file.";
            return;
        }

        string fileExt = Path.GetExtension(FileUpload1.FileName);
        if (fileExt != ".xls" && fileExt != ".xlsx")
        {
            lblMessage.Text = "Only .xls or .xlsx files are allowed.";
            return;
        }

        // Save file temporarily
        string filePath = Server.MapPath("~/Upload/" + FileUpload1.FileName);
        FileUpload1.SaveAs(filePath);

        // Read Excel
        string connStr = "";
        if (fileExt == ".xls")
        {
            connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath +
                      ";Extended Properties='Excel 16.0 Xml;HDR=YES;'";
        }
        else
        {
            //connStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath +
            //          ";Extended Properties='Excel 16.0 Xml;HDR=YES;'";

             connStr =
     "Provider=Microsoft.ACE.OLEDB.16.0;Data Source=" + filePath +
     ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";";

        }

        OleDbConnection excelConn = new OleDbConnection(connStr);
        excelConn.Open();

        DataTable dt = new DataTable();
        string sheetName = excelConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null)
                                    .Rows[0]["TABLE_NAME"].ToString();

        using (OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [" + sheetName + "]", excelConn))
        {
            da.Fill(dt);
        }

        excelConn.Close();

        string sqlConnStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ToString();

        using (SqlConnection sqlConn = new SqlConnection(sqlConnStr))
        {
            sqlConn.Open();
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqlConn))
            {
                bulkCopy.DestinationTableName = "[dbo].[temp_a]";
                bulkCopy.WriteToServer(dt);
            }
        }

        File.Delete(filePath);
        lblMessage.ForeColor = System.Drawing.Color.Green;
        lblMessage.Text = "File uploaded and data inserted successfully!";
    }
}