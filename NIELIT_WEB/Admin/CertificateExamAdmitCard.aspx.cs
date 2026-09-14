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

public partial class Admin_CertificateExamAdmitCard : BasePage
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
        string connectionString = "";
        string filepath = Server.MapPath("../CertExamAdmitCard");
        fuInstitituteList.SaveAs(filepath + "/" + fuInstitituteList.FileName);
        string accesspath = Server.MapPath("../CertExamAdmitCard/") + fuInstitituteList.FileName;
        string ext = System.IO.Path.GetExtension(this.fuInstitituteList.PostedFile.FileName);

        if (ext.ToUpper() == ".ACCDB")
        {
            connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + accesspath + ";Persist Security Info=False";
        }
        else
        {
            ShowAlert("Please Choose .ACCDB Extension Database", true);
            return;
        }
  
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
                 ds.Dispose();
                 BulkTable.Dispose();
                 connection.Close();
                 BulkInsertToDataBase();
                 //BulkAdmitCardUploadDLC(BulkTable.Rows[0]["exam_month"].ToString(), BulkTable.Rows[0]["exam_year"].ToString());
                 
                 ShowAlert("Admit Card Data Uploaded Successfully.");
             }
             catch (Exception ex)
             {
                 Console.WriteLine(ex.Message);
             }
             finally
             {
                 //dr.Close();
                 //dr.Dispose();
                 command.Dispose();
                 connection.Close();
                 connection.Dispose();
                 //System.IO.File.Delete(path);
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

    public void BulkAdmitCardUploadDLC(string _sExamMonth, string _sYear)
    {
        try
        {
            DataTable DT = new DataTable();
            //SqlConnection Conn = new SqlConnection(ConfigurationManager.AppSettings["Con"].ToString());
            connection();
            //Conn.Open();
            using (SqlCommand Cmm = new SqlCommand("BulkAdmitCardUploadDLC", con))
            {
                Cmm.CommandType = CommandType.StoredProcedure;
                Cmm.Parameters.AddWithValue("@pMonth", _sExamMonth);
                Cmm.Parameters.AddWithValue("@pYear", _sYear);
                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                Sda.Fill(DT);
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async void BulkInsertToDataBase()
    {
        await Task.Factory.StartNew(() =>
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
        });
    }

    protected void SendSMSBtn_Click(object sender, EventArgs e)
    {
        List<StudentList> candidateList;
        using (EConnectContext context = new EConnectContext())
        {
            candidateList = context.tblSMSs.
                    Where(s => s.SmsSent == false).
                                                  Select(x => new StudentList
                                                  {
                                                      AppNum = x.AppNum,
                                                      Message = x.Message,
                                                      MobileNo = x.MobileNumber,
                                                  }).ToList();

        }
        SMS(candidateList, Convert.ToInt32(Session["UserID"]));
    }

    public async void SMS(List<StudentList> candidateList, Int32 UserID)
    {
        using (EConnectContext context = new EConnectContext())
        {
            await Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < candidateList.Count(); i++)
                {
                    try
                    {
                        string Appno = candidateList[i].AppNum;
                        StringBuilder mobilemsg = new StringBuilder();
                        //mobilemsg.Append("Dear " + GetInitCap(candidateList[i].Salutation + " " + candidateList[i].AppName) + "," + " Admit Card for the " + candidateList[i].CourseName + " :- " + candidateList[i].ExamName + " Exam has been uploaded by NIELIT. Please check your E-mail regarding this.");
                        mobilemsg.Append(candidateList[i].Message);
                        EConnect.NIELIT.SMS message = new SMS(mobilemsg.ToString(), candidateList[i].MobileNo.ToString(), "1307161052928807635", SmsServiceType.BulkSMS, false);
                        int sentMessageCount;
                        message.sendSingleSMS(out sentMessageCount);
                        mobilemsg.Clear();
                        if (sentMessageCount == 1)
                        {
                            try
                            {
                                var app = context.tblSMSs.Where(p => p.AppNum == Appno).FirstOrDefault();
                                app.SMS_Response = "SMS Successfully Sent";
                                app.SmsSent = true;
                                app.SMSSentDate = DateTime.Now;

                                context.Entry(app).State = System.Data.Entity.EntityState.Modified;
                                context.SaveChanges();
                            }
                            catch (Exception ex) { throw ex; }
                        }
                    }
                    catch (Exception ex) { throw ex; }
                }
            }, TaskCreationOptions.LongRunning);
            
            //UnsendSMS(ExamID, regcentreID);
            UpdatePanelSMS.Update();
        }
    }

    public async void SentBulkEmail(List<StudentList> candidateList, Int32 UserID)
    {
        StringBuilder emailmsg = new StringBuilder();
        await Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < candidateList.Count(); i++)
                {
                    //emailmsg.Append("Dear " + GetInitCap(candidateList[i].Salutation + " " + candidateList[i].AppName) + ",<br/><br/>" + " Admit Card for the <b>" + candidateList[i].CourseName + " :- " + candidateList[i].ExamName + " </b> Exam has been uploaded by NIELIT. You can download your admit card from <b> NIELIT Website (https://student.nielit.gov.in) </b> with your Application-Number (<b>" + candidateList[i].Appno + "</b>)");
                    emailmsg.Append(candidateList[i].EmailMsg);

                    try
                    {
                        //sending Email 
                        if (candidateList[i].Email.Trim().Length > 0)
                        {
                            EConnect.NIELIT.Email mail1 = new Email("Admit Card Notification:NIELIT", emailmsg.ToString(), candidateList[i].Email);
                            mail1.Send();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    emailmsg.Clear();
                    //}
                }
            });
    }

    public class StudentList
    {
        public String AppNum
        {
            get;
            set;
        }

        public double MobileNo
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public String Message
        {
            get;
            set;
        }

        public String Email
        {
            get;
            set;
        }
        public String EmailMsg
        {
            get;
            set;
        }
        //public String SMS_Response
        //{
        //    get;
        //    set;
        //}
        //public String ExamName
        //{
        //    get;
        //    set;
        //}
        //public String CourseName
        //{
        //    get;
        //    set;
        //}
        //public String Appno
        //{
        //    get;
        //    set;
        //}
    }

    protected void SendEmailBtn_Click(object sender, EventArgs e)
    {
        List<StudentList> candidateList;
        using (EConnectContext context = new EConnectContext())
        {
             candidateList = context.tblSMSs.
                Where(s => s.SmsSent == false).
                                              Select(x => new StudentList
                                              {
                                                  AppNum = x.AppNum,
                                                  Email = x.Email,
                                                  EmailMsg = x.EmailMsg
                                              }).ToList();
        }
        SentBulkEmail(candidateList, Convert.ToInt32(Session["UserID"]));
    }
}