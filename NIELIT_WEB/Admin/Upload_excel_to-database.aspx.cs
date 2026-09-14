using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;

public partial class Upload_excel_to_database : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
 
    }
  
    protected void Button1_Click(object sender, EventArgs e)
    {

         if (FileUpload1.HasFile )
            {

                Label5.Visible = false;
                Label5.Text = "";

                Label4.Visible = false;
                Label4.Text = "";

                    string path = string.Concat(Server.MapPath("~/Uploaded Folder/" + FileUpload1.FileName));
                    FileUpload1.SaveAs(path);

                    string str = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                    SqlConnection conn2 = new SqlConnection();
                    conn2.ConnectionString = str;

                    //SqlConnection conn2 = new SqlConnection("Data Source =.; Initial catalog=testExcel;integrated security=true");

                    string myexceldataquery = "select [SrNo]  ,[CentreUniqueKey]  ,[Level]  ,[RegistrationNo]  ,[candidate Name]  ,[Module Name]  ,[ATTENDANCE]  ,[EXAMINER Marks(40)]  ,[OBSERVER Marks(40)]  ,[OBSERVER Marks (20)]  ,[Total Marks]  ,[Batch] ,[Examiner Name] ,[Exam Date] from [" + txtSheetname.Text + "$]";


                    string PathConn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended properties='Excel 8.0;HDR=False' ");
                    OleDbConnection conn = new OleDbConnection(PathConn);


                    OleDbCommand oledbcmd = new OleDbCommand(myexceldataquery, conn);

                    OleDbDataReader dr = null;

                   




                    try
                    {


                        conn.Open();
                        DataTable dtCategories = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        List<ListItem> lstCategories = new List<ListItem>();

                        string sheetName = string.Empty;
                        foreach (DataRow dr1 in dtCategories.Rows)
                        {
                            sheetName = dr1["TABLE_NAME"].ToString();
                            // if (!sheetName.Contains("Sheet"))
                            lstCategories.Add(new ListItem(sheetName.Replace("$", "")));
                        }
                        //DropDownList1.DataSource = lstCategories;
                        //DropDownList1.DataBind();
                        //oledbcmd.ExecuteReader();
                        conn.Close();

                        foreach (var Item in lstCategories)
                        {
                            if (txtSheetname.Text.Equals(Item.ToString()))
                            {





                                //conn.Open();
                                //dr = oledbcmd.ExecuteReader();
                                //conn.Close();


                                string tablename = "randomtable1";//write the table name here

                                SqlBulkCopy bulkcopy = new SqlBulkCopy(conn2);
                                bulkcopy.DestinationTableName = tablename;

                                conn2.Open();
                                conn.Open();

                                dr = oledbcmd.ExecuteReader();

                                bulkcopy.WriteToServer(dr);


                                conn2.Close();

                                conn.Close();

                                Label4.Visible = true;
                                Label4.Text = "Excel Sheet Successfully Uploaded.";
                                Label4.ForeColor = Color.Green;
                                txtSheetname.Text = "";
                                break;
                            }
                            else
                            {
                                txtSheetname.Text = "";
                                Label4.Visible = true;
                                Label4.ForeColor = Color.Red;
                                Label4.Text = "Enter a Valid Sheet";
                            }
                        }

                    }

                    catch (Exception ex)
                    {
                        txtSheetname.Text = "";
                        Label4.Visible = true;
                        Label4.Text = ex.Message;
                        Label4.ForeColor = Color.Red;
                    }
                    finally
                    {
                        //dr.Close();
                        //dr.Dispose();
                        conn2.Close();
                        conn.Close();

                    }
         }

 


            
            else
            {
                Label5.Visible = true;
                Label5.Text = "Select a valid file";
                Label5.ForeColor = Color.Red;
            }
        }


}
