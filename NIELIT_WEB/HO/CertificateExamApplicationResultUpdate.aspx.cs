using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Web.UI;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data.SqlClient;

public partial class HO_CertificateExamApplicationResultUpdate : BasePage
{
    Int32 currentRoleId = 0;
    Int64 currentUserId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            currentUserId = Convert.ToInt64(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Result Update from ABS to @", "", ""));
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }
    protected void btnValidate_Click(object sender, EventArgs e)
    {

        BreadCrumb1.Render();
        string filepath = Server.MapPath("../UploadedFiles");
        Random Random = new Random();
        int rand = Random.Next(10000, 99999);
        flUpload.SaveAs(filepath + "/" + rand + flUpload.FileName);
        string path = (filepath + "/" + rand + flUpload.FileName);
        string ext = System.IO.Path.GetExtension(this.flUpload.PostedFile.FileName);
        string sExcelConnectionString = "";
        if (ext.ToUpper() == ".XLS")
            sExcelConnectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";", path);
        else if (ext.ToUpper() == ".XLSX")
            sExcelConnectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=No;IMEX=1\";", path);
        else
        {
            ShowAlert("Please Choose ..XLS/.XLSX Excel File.", true);
            return;
        }

        int ValidateRecords = 0;
        StringBuilder sa = new StringBuilder();
        List<string> NotValidatedRecords = new List<string>();
        OleDbConnection connection = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();
        OleDbDataAdapter oda = new OleDbDataAdapter();
        DataTable dt = new DataTable();
        connection.ConnectionString = sExcelConnectionString;
        connection.Open();
        command = new OleDbCommand("select * from [Result_Update$]", connection);
        oda.SelectCommand = command;
        oda.Fill(dt);
        connection.Close();
        try
        {
            using (EConnectContext vContext = new EConnectContext())
            {
                String applNumber = string.Empty;
                String RollNumber = string.Empty;
                int AbsGradeId = 0;
                int CancelGradeId = 0;
                for (int i = 1; i < dt.Rows.Count; i++)
                {
                    applNumber = dt.Rows[i]["F1"].ToString();
                    RollNumber = dt.Rows[i]["F2"].ToString();
                    var Application = vContext.CertificateExamApplications.Where(s => s.Number == applNumber).FirstOrDefault();
                    IQueryable<ResultGrade> resultGrading = vContext.ResultGrades;
                    if (Application != null)
                    {
                        if (RollNumber == Application.RollNumber)
                        {
                            resultGrading = resultGrading.Where(s => s.CourseCategoryID == Application.CourseCategoryID && s.VersionID == Application.Exam.ResultGradeVersionID);
                            AbsGradeId = resultGrading.Where(s => s.Code == "ABS").FirstOrDefault().ID;
                            CancelGradeId = resultGrading.Where(s => s.Code == "@").FirstOrDefault().ID;

                            if (Application.ResultGradeID == AbsGradeId)
                            {
                                //String backbuttonsql = "INSERT INTO Certificate_Exam_Application_Result_Update_History(CertificateExamApplication_Id,Number,Roll_Number,Exam_Id,Course_Id,Result_Grade_Id,Created_By,Created_Date)" +
                                                       //"(select ID,Number,Roll_Number,Exam_ID,Course_ID,Result_Grade_ID," + currentUserId + ",'" + DateTime.Now + "' from Certificate_Exam_Application Where ID=" + Application.ID + ")";
                                String backbuttonsql = "INSERT INTO Certificate_Exam_Application_Result_Update_History(CertificateExamApplication_Id,Number,Roll_Number,Exam_Id,Course_Id,Result_Grade_Id,Created_By,Created_Date)" +
                                                       "(select ID,Number,Roll_Number,Exam_ID,Course_ID,Result_Grade_ID,@currentUserId,@CreatedDate from Certificate_Exam_Application Where ID=@ID)";
                                                    SqlParameter[] para1 ={
				                                                            new SqlParameter("@currentUserId", currentUserId),
                                                                            new SqlParameter("@CreatedDate",DateTime.Now),
                                                                            new SqlParameter("@ID",Application.ID)
                                                                            };
                                vContext.Database.ExecuteSqlCommand(backbuttonsql,para1);
                                vContext.SaveChanges();

                                Application.ResultGradeID = CancelGradeId;
                                vContext.Entry(Application).State = System.Data.Entity.EntityState.Modified;
                                vContext.SaveChanges();
                                ValidateRecords++;
                            }
                            else { NotValidatedRecords.Add(applNumber); }
                        }
                        else { NotValidatedRecords.Add(applNumber); }
                    }
                    applNumber = string.Empty;
                    RollNumber = string.Empty;
                }
            };

            foreach (string Number in NotValidatedRecords)
            { sa.Append(Number + ", "); }
            InValidLabel.Text = sa.ToString();
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
        finally
        {
            command.Dispose();
            connection.Dispose();
            connection.Close();
            System.IO.File.Delete(path);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BreadCrumb1.Render();
        InValidLabel.Text = "";
    }
}