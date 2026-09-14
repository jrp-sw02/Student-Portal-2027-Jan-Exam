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

public partial class Admin_AdmitCardUpload_DLC : BasePage
{
    SqlConnection con = new SqlConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
       if(!IsPostBack)
        FillCategories();
    }

    protected void FillCategories()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationExam);
                ListItem lst = new ListItem("--Select Course Category --", "0");
                //var Category = from p in context.CourseCategories
                // where p.ID == CourseType
                //orderby (p.Name)
                //select new { ValueField = p.ID, TextField = p.Name };

                //Int32 CourseType2 = Convert.ToInt32(enmCourseType.IRDACat);
                //var Category = from p in context.CourseCategories
                //               where p.ID == CourseType || p.ID == CourseType2
                //               orderby (p.Name) descending
                //               select new { ValueField = p.ID, TextField = p.Name };
                var Category = (from s in context.CourseCategories
                                join c in context.Courses on s.ID equals c.CourseCategoryID
                                where c.CourseTypeID == CourseType
                                orderby (s.Name)
                                select new { ValueField = s.ID, TextField = s.Name }).Distinct();

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlcourse.Items.Clear();
        ddlExamCycle.Items.Clear();
        //ddlExamYear.Items.Clear();
        //ddlExamName.Items.Clear();
        //ddlRc.Items.Clear();
        //ddlExamCycle.Items.Insert(0, "--Select Course Name One--");
        //ddlExamYear.Items.Insert(0, "--Select One--");
        //ddlExamName.Items.Insert(0, "--Select One--");
        //ddlRc.Items.Insert(0, "--Select One--");
        FillCourses();
    }
    protected void FillCourses()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationExam);
                ListItem lst = new ListItem("--Select Course Name --", "0");
                int id = Convert.ToInt32(ddlcoursecategory.SelectedValue);
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == id && p.CourseTypeID == CourseType
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourse, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExamCycle.Items.Clear();
        //ddlExamYear.Items.Clear();
        //ddlExamName.Items.Clear();
        //ddlRc.Items.Clear();
        //ddlExamYear.Items.Insert(0, "--Select One--");
        //ddlExamName.Items.Insert(0, "--Select One--");
       // ddlRc.Items.Insert(0, "--Select One--");
        FillExamCycle();
    }
    protected void FillExamCycle()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select Exam Cycle --", "0");
                int CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                var ExamCycleList = from p in context.ExaminationCycles
                                    where p.CourseID == CourseID
                                    select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamCycle, ExamCycleList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExamName.Items.Clear();
        //ddlRc.Items.Clear();
        //ddlRc.Items.Insert(0, "--Select One--");
        FillExamName();
    }
    protected void FillExamName()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select Exam Name --", "0");
                Int32 ExamYear = Convert.ToInt32(ddlExamYear.SelectedValue);
                Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
                Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
                var ExamName = from p in context.Exams
                               where p.ExamYear == ExamYear && p.CourseID == CourseID && p.ExaminationCycleID == ExamCycleID
                               && p.DateOfPublishingOfTimeTable <= System.Data.Entity.DbFunctions.TruncateTime(DateTime.Now)
                               select new { ValueField = p.ID, TextField = p.Name };
                ExamName = ExamName.OrderByDescending(s => s.ValueField);

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamName, ExamName.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlExamYear.Items.Clear();
        ddlExamName.Items.Clear();
        //ddlExamName.Items.Insert(0, "--Select Exam Year One--");
        //ddlRc.Items.Clear();
        //ddlRc.Items.Insert(0, "--Select One--");
        FillExamYear();
    }
    protected void FillExamYear()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select Exam Year --", "0");
                Int32 ExamCycleID = Convert.ToInt32(ddlExamCycle.SelectedValue);
                var ExamYear = (from p in context.Exams
                                where p.ExaminationCycleID == ExamCycleID
                                select new { ValueField = p.ExamYear, TextField = p.ExamYear }).Distinct();
                ExamYear = ExamYear.OrderByDescending(s => s.ValueField).Take(3);

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlExamYear, ExamYear, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {

        EConnectContext context = new EConnectContext();
        // Int32 CourseCategoryID = Convert.ToInt32(ddlcoursecategory.SelectedValue);
        // Int32 CourseID = Convert.ToInt32(ddlcourse.SelectedValue);
        // Int32 ExamID = Convert.ToInt32(ddlExamName.SelectedValue);
        // Int32 regCentreID = Convert.ToInt32(ddlRc.SelectedValue);
        //Int64 applicationID = 0;
        Int32 TotalRecords = 0, ValidateRecords = 0, NotValidateRecords = 0;
        List<StudentList> candidateList = new List<StudentList>();
        List<Int64> ValidatedAppRecords = new List<Int64>();
        StringBuilder sb = new StringBuilder();
       // String appIdList = "", appIdList1 = "", appIdList2 = "", appIdList3 = "", applistUpdated = "";
        String[] arr = new String[10];
        arr[0] = "Uploaded Admit Card Data is not correct.Please Correct the data and Upload again";
        arr[1] = "Admit Card Data File is not related to selected Regional Centre";
        arr[2] = "Data Already Uploaded"; arr[3] = "does not exist";
        EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
        BreadCrumb1.Render();
        //if (!fuInstitituteList.HasFile)
        //{
        //    ShowAlert("Please select file to upload.");
        //    return;
        //}
        //string connectionString = "";
        //string filepath = Server.MapPath("../CertExamAdmitCard");
        //fuInstitituteList.SaveAs(filepath + "/" + fuInstitituteList.FileName);
        //string accesspath = Server.MapPath("../CertExamAdmitCard/") + fuInstitituteList.FileName;
        //string ext = System.IO.Path.GetExtension(this.fuInstitituteList.PostedFile.FileName);

        //if (ext.ToUpper() == ".XLS")
        //{
        //    connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + accesspath + ";Persist Security Info=False";
        //}
        //else
        //{
        //    ShowAlert("Please Choose .ACCDB Extension Database", true);
        //    return;
        //}
  
        // string strSQL = "SELECT * FROM Exam_Database"; 
        //    // Create a connection  
        // using (OleDbConnection connection = new OleDbConnection(connectionString))
        // {
        //     // Create a command and set its connection  
        //     OleDbDataAdapter da = new OleDbDataAdapter(strSQL, connection);

        //     OleDbCommand command = new OleDbCommand(strSQL, connection);
        //     // Open the connection and execute the select command.

        //     try
        //     {
        //         // Open connecton  
        //         connection.Open();

        //         DataSet ds = new DataSet();
        //         da.Fill(ds);
        //         //storing datset in viewstate
        //         DataTable BulkTable = ds.Tables["Table"];
        //         ViewState["BulkData"] = BulkTable;
        //         ds.Dispose();
        //         BulkTable.Dispose();
        //         connection.Close();
        //         BulkInsertToDataBase();
        //         //BulkAdmitCardUploadDLC(BulkTable.Rows[0]["exam_month"].ToString(), BulkTable.Rows[0]["exam_year"].ToString());
                 
        //         ShowAlert("Admit Card Data Uploaded Successfully.");
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine(ex.Message);
        //     }
        //     finally
        //     {
        //         //dr.Close();
        //         //dr.Dispose();
        //         command.Dispose();
        //         connection.Close();
        //         connection.Dispose();
        //         //System.IO.File.Delete(path);
        //     }
        // }

        String filepath = Server.MapPath("../UploadedFiles");
        flUpload.SaveAs(filepath + "/" + flUpload.FileName);
        String path = (filepath + "/" + flUpload.FileName);
        String ext = System.IO.Path.GetExtension(this.flUpload.PostedFile.FileName);
        String excelConnectionString = "";
        if (ext.ToUpper() == ".XLS")
        {
            excelConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";", path);
            //excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;", path);
        }
        else if (ext.ToUpper() == ".XLSX")
        { //accessConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Persist Security Info=False;", path);
            excelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=Yes;IMEX=1\";", path);
        }
        else { ShowAlert("Please Choose .XLS/.XLSX Extension Database", true); return; }

        //  ExcelConn(_path);
        //string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", _path);
        OleDbConnection Econ = new OleDbConnection(excelConnectionString);
         string Query = string.Format("Select [app_no], [roll_no], [venue_code], [venue_address], [batch_no], [exam_date], [rep_time] FROM [{0}]", "cand$");
        //string Query = string.Format("Select [reg no], [roll no] FROM [{0}]", "cand$");
        // string Query = string.Format("Select * FROM [{0}]", "cand$");

        OleDbCommand Ecom = new OleDbCommand(Query, Econ);
        Econ.Open();

        DataSet ds = new DataSet();
        OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
        oda.Fill(ds);
        Econ.Close();
        DataTable Exceldt = ds.Tables[0];

        //for (int i = Exceldt.Rows.Count - 1; i >= 0; i--)
        //{
        //    if (Exceldt.Rows[i]["Employee Name"] == DBNull.Value || Exceldt.Rows[i]["Email"] == DBNull.Value)
        //    {
        //        Exceldt.Rows[i].Delete();
        //    }
        //}

        Exceldt.AcceptChanges();

        //creating object of SqlBulkCopy
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

        SqlBulkCopy objbulk = new SqlBulkCopy(constr);
        //assigning Destination table name
        objbulk.DestinationTableName = "Certificate_Exam_Schedule_Temp";
        //Mapping Table column

        objbulk.ColumnMappings.Add("[app_no]".Trim(), "Application_Number");
        objbulk.ColumnMappings.Add("[roll_no]".Trim(), "Roll_Number");
        objbulk.ColumnMappings.Add("[venue_code]".Trim(), "Venue_Code");
        objbulk.ColumnMappings.Add("[venue_address]".Trim(), "Venue_Address");
        objbulk.ColumnMappings.Add("[batch_no]".Trim(), "Batch_No");
        objbulk.ColumnMappings.Add("[exam_date]".Trim(), "Exam_Date");
        objbulk.ColumnMappings.Add("[rep_time]".Trim(), "Rept_Time");

        int userid = Convert.ToInt32(Session["UserID"]);
        //inserting Datatable Records to DataBase
        SqlConnection sqlConnection = new SqlConnection(constr);
        string sqlQuery = "Delete from Certificate_Exam_Schedule_Temp";
        SqlCommand cmd = new SqlCommand(sqlQuery, sqlConnection);
      
        string sqlQuery1 = "Certificate_Exam_Admit_Card_Upload";
        SqlCommand cmd1 = new SqlCommand(sqlQuery1, sqlConnection);
        cmd1.CommandType = CommandType.StoredProcedure;
        cmd1.CommandTimeout = 8000;
        cmd1.Parameters.Add("@PUser_Id", Convert.ToInt64(Session["UserID"]));
        cmd1.Parameters.Add("@PIs_Revised", Convert.ToInt16("0"));
        cmd1.Parameters.Add("@Invalid_record_count", SqlDbType.Int, 20);
        cmd1.Parameters["@Invalid_record_count"].Direction = ParameterDirection.Output;
        cmd1.Parameters.Add("@Invalid_app_id", SqlDbType.VarChar, 3000);
        cmd1.Parameters["@Invalid_app_id"].Direction = ParameterDirection.Output;
        sqlConnection.Open();
        cmd.ExecuteNonQuery();
        objbulk.WriteToServer(Exceldt);
        cmd1.ExecuteNonQuery();
       // lblNotValidate.Text = cmd1.Parameters["@Invalid_record_count"].Value.ToString();
       // lblFailed.Text = cmd1.Parameters["@Invalid_app_id"].Value.ToString();
        sqlConnection.Close();
        TotalRecords = ds.Tables[0].Rows.Count;
        ValidateRecords = (ds.Tables[0].Rows.Count - Convert.ToInt32(cmd1.Parameters["@Invalid_record_count"].Value));
        //MessageBox.Show("Data has been Imported successfully.", "Imported", MessageBoxButtons.OK, MessageBoxIcon.Information);
        SMSLbl.Text = "SMS/Email to be sent : " + ValidateRecords;
        ShowAlert("Admit Card Data  for " + ValidateRecords.ToString() + " candidates are updated successfully");

        //lblTotalRecords.Text = TotalRecords.ToString();
       // lblValidateRecords.Text = ValidateRecords.ToString();
        if (ValidateRecords != 0)
            lblCount.Text = "Admit Card Data  for " + ValidateRecords.ToString() + " candidates are updated successfully";

        // sending sms and email
       // candidateList = context.CertificateExamApplications.Where(s => ValidatedAppRecords.Contains(s.ID))
                      //  .Select(s => new StudentList { AppName = s.Name, Email = s.EmailAddress, Salutation = s.Salutation, MobileNo = s.MobileNumber, Appno = s.Number, ExamName = s.Exam.Name, CourseName = s.Course.Name }).ToList();
      //  SentBulkEmail(candidateList, Convert.ToInt32(Session["UserID"]));
    }
    public class StudentList
    {
        public Int64 MobileNo
        {
            get;
            set;
        }
        public String Email
        {
            get;
            set;
        }
        public String AppName
        {
            get;
            set;
        }
        public String Salutation
        {
            get;
            set;
        }
        public String ExamName
        {
            get;
            set;
        }
        public String CourseName
        {
            get;
            set;
        }
        public String Appno
        {
            get;
            set;
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

    

    protected void SendSMSBtn_Click(object sender, EventArgs e)
    {
        
        using (EConnectContext context = new EConnectContext())
        {
            //List<StudentList> candidateList = context.CertificateExamApplications.
            //                                  Where(s => s.ExamID == ExamID && s.RegionalCenterID == regcentreID && s.SMSSent == false && s.RollNumber != null).
            //                                  Select(x => new StudentList
            //                                  {
            //                                      AppName = x.Name,
            //                                      Email = x.EmailAddress,
            //                                      Salutation = x.Salutation,
            //                                      MobileNo = x.MobileNumber,
            //                                      Appno = x.Number,
            //                                      ExamName = x.Exam.Name,
            //                                      CourseName = x.Course.Name
            //                                  }).Take(5000).ToList();
            List<StudentList> candidateList = context.CertificateExamApplications.
              Where(s => s.SMSSent == false && s.CourseCategoryID == Convert.ToInt32(ddlcoursecategory.SelectedValue) && s.CourseID == Convert.ToInt32(ddlcourse.SelectedValue) && s.ExamID == Convert.ToInt32(ddlExamName.SelectedValue)).
                                            Select(x => new StudentList
                                            {
                                                AppName = x.Name,
                                                Email = x.EmailAddress,
                                                Salutation = x.Salutation,
                                                MobileNo = x.MobileNumber,
                                                Appno = x.Number,
                                                ExamName = x.Exam.Name,
                                                CourseName = x.Course.Name
                                            }).ToList();

            for (int i = 0; i < candidateList.Count(); i++)
            {
                try
                {
                    string Appno = candidateList[i].Appno;
                    StringBuilder mobilemsg = new StringBuilder();
                    mobilemsg.Append("Dear " + GetInitCap(candidateList[i].Salutation + " " + candidateList[i].AppName) + "," + " Admit Card for the " + candidateList[i].CourseName + " :- " + candidateList[i].ExamName + " Exam has been uploaded by NIELIT. Please check your E-mail regarding this.");

                    EConnect.NIELIT.SMS message = new SMS(mobilemsg.ToString(), candidateList[i].MobileNo.ToString(), "1307161052928807635", SmsServiceType.SignleSMS, false);
                    int sentMessageCount;
                    message.sendSingleSMS(out sentMessageCount);
                    mobilemsg.Clear();
                    if (sentMessageCount == 1)
                    {
                        try
                        {
                            var app = context.CertificateExamApplications.Where(p => p.Number == Appno).FirstOrDefault();
                            app.SMSSent = true;
                            context.Entry(app).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                        catch (Exception ex) { throw ex; }
                    }
                 }
                catch (Exception ex) { throw ex; }
            }
            //UnsendSMS(ExamID, regcentreID);
            UpdatePanelSMS.Update();
        }
    }

    //public async void SMS(List<StudentList> candidateList, Int32 UserID)
    //{
    //    using (EConnectContext context = new EConnectContext())
    //    {
    //        await Task.Factory.StartNew(() =>
    //        {
    //            for (int i = 0; i < candidateList.Count(); i++)
    //            {
    //                try
    //                {
    //                    string Appno = candidateList[i].AppNum;
    //                    StringBuilder mobilemsg = new StringBuilder();
    //                    //mobilemsg.Append("Dear " + GetInitCap(candidateList[i].Salutation + " " + candidateList[i].AppName) + "," + " Admit Card for the " + candidateList[i].CourseName + " :- " + candidateList[i].ExamName + " Exam has been uploaded by NIELIT. Please check your E-mail regarding this.");
    //                    mobilemsg.Append(candidateList[i].Message);
    //                    EConnect.NIELIT.SMS message = new SMS(mobilemsg.ToString(), candidateList[i].MobileNo.ToString(), "1307161052928807635", SmsServiceType.BulkSMS, false);
    //                    int sentMessageCount;
    //                    message.sendSingleSMS(out sentMessageCount);
    //                    mobilemsg.Clear();
    //                    if (sentMessageCount == 1)
    //                    {
    //                        try
    //                        {
    //                            var app = context.tblSMSs.Where(p => p.AppNum == Appno).FirstOrDefault();
    //                            app.SMS_Response = "SMS Successfully Sent";
    //                            app.SmsSent = true;
    //                            app.SMSSentDate = DateTime.Now;

    //                            context.Entry(app).State = System.Data.Entity.EntityState.Modified;
    //                            context.SaveChanges();
    //                        }
    //                        catch (Exception ex) { throw ex; }
    //                    }
    //                }
    //                catch (Exception ex) { throw ex; }
    //            }
    //        }, TaskCreationOptions.LongRunning);
            
    //        //UnsendSMS(ExamID, regcentreID);
    //        UpdatePanelSMS.Update();
    //    }
    //}

    protected void SentBulkEmail(List<StudentList> candidateList, Int32 UserID)
    {
        StringBuilder emailmsg = new StringBuilder();

        for (int i = 0; i < candidateList.Count(); i++)
        {
            emailmsg.Append("Dear " + GetInitCap(candidateList[i].Salutation + " " + candidateList[i].AppName) + ",<br/><br/>" + " Admit Card for the <b>" + candidateList[i].CourseName + " :- " + candidateList[i].ExamName + " </b> Exam has been uploaded by NIELIT. You can download your admit card from <b> NIELIT Website (https://student.nielit.gov.in) </b> with your Application-Number (<b>" + candidateList[i].Appno + "</b>)");
            try
            {
                //sending Email 
                if (candidateList[i].Email.Trim().Length > 0)
                {
                   //-- EConnect.NIELIT.Email mail1 = new Email("Admit Card Notification:NIELIT", emailmsg.ToString(), candidateList[i].Email);
                   //-- mail1.Send();

                    try
                    {
                        using (EConnectContext context = new EConnectContext())
                        {
                            var app = context.CertificateExamApplications.Where(p => p.Number == candidateList[i].Appno).FirstOrDefault();
                            app.EmailSent = true;
                            context.Entry(app).State = System.Data.Entity.EntityState.Modified;
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex) { throw ex; }
                }
            }
            catch (Exception ex)
            {

            }
            emailmsg.Clear();
        }
    }



    protected void SendEmailBtn_Click(object sender, EventArgs e)
    {
        List<StudentList> candidateList;
        using (EConnectContext context = new EConnectContext())
        {
            candidateList = context.CertificateExamApplications.
               Where(s => s.EmailSent == false && s.CourseCategoryID == Convert.ToInt32(ddlcoursecategory.SelectedValue) && s.CourseID == Convert.ToInt32(ddlcourse.SelectedValue) && s.ExamID == Convert.ToInt32(ddlExamName.SelectedValue)).
                                             Select(x => new StudentList
                                             {
                                                 AppName = x.Name,
                                                 Email = x.EmailAddress,
                                                 Salutation = x.Salutation,
                                                 MobileNo = x.MobileNumber,
                                                 Appno = x.Number,
                                                 ExamName = x.Exam.Name,
                                                 CourseName = x.Course.Name
                                              }).ToList();
        }
        SentBulkEmail(candidateList, Convert.ToInt32(Session["UserID"]));
        //List<StudentList> candidateList = context.CertificateExamApplications.Where(s => ValidatedAppRecords.Contains(s.ID))
        //                    .Select(s => new StudentList { AppName = s.Name, Email = s.EmailAddress, Salutation = s.Salutation, MobileNo = s.MobileNumber, Appno = s.Number, ExamName = s.Exam.Name, CourseName = s.Course.Name }).ToList();
        //SentBulkEmail(candidateList, Convert.ToInt32(Session["UserID"]));
    }
}