using System;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using EConnect.DAL;

public partial class accessimport :BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        showdata();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {

        string Access = Server.MapPath("Download/Database_formatCCC_EMEC.mdb");
        string Excel = Server.MapPath("Download/Result.xls");
        string connect = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Excel + ";Extended Properties=Excel 8.0;";
        using (OleDbConnection conn = new OleDbConnection(connect))
        {
            using (OleDbCommand cmd = new OleDbCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "SELECT * INTO [MS Access;Database=@Access].[New Table] FROM [Sheet1$]";
		cmd.Parameters.Add("@Access", OleDbType.VarChar, 500).Value = Access;
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            if(conn.State ==ConnectionState .Open)
                    conn.Close();
        }
        
    }
    protected void showdata()
   {
        Int32 cid = 5;
        OleDbConnection connection = new OleDbConnection(); ;
        OleDbCommand command = new OleDbCommand();
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var certificate = (from c in context.CertificateExamApplications
                                   where c.CourseID == cid//need to change it
                                   select new
                                   {

                                       exam_year = "",
                                       C_Name = c.Name,
                                       F_NAME = c.FatherName,
                                       M_NAME = c.MotherName,
                                       D_O_B = c.DateOfBirth,
                                       SEX = c.Gender,
                                       H_Qual = c.EducationalQualification.Name,
                                       ADD1 = c.CorAddressLine1,
                                       ADD2 = c.CorAddressLine2,
                                       ADD3 = c.CorAddressLine3,
                                       CITY = c.CorDistrict.Name,
                                       STATE = c.CorState.Name,
                                       PINCODE = c.CorPinCode,
                                       PH_NO_C = c.PhoneNumber.Value,
                                       EMAIL_C = c.EmailAddress,
                                       CCC_NO = c.InstituteID.Value,
                                       INST_NAME = c.Institute.Name,
                                       INST_ADD = c.Institute.AddressLine1 + c.Institute.AddressLine2 + c.Institute.State.Name,
                                       INST_STAT = c.Institute.AccreditationDetails.FirstOrDefault().AccreditationStatus.Name,
                                       TH_CENT_CH = c.ExamCenter1.Name,
                                       SEC_TH_CEN = c.ExamCenter2.Name,
                                       OCCUPATION = c.Occupation.Name,
                                       CATEGORY = c.CastCategory.Name,
                                       PREV_APP = c.AlreadyApplied,
                                       PREV_M = c.PreviousExam.Name,
                                       PREV_YEAR = "",//need to work on this
                                       PREV_ROLL = (!string.IsNullOrEmpty(c.PreviousRollNumber)) ? c.PreviousRollNumber :"NA",
                                       Rollno = "",
                                       cent_allot = "",
                                       cent_add = "",
                                       examDate = "",
                                       batch = "",
                                       rep_time = ""
                                   }).FirstOrDefault();
                string Excel = Server.MapPath("Download/BCC.xlsx");
                string connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Excel + ";Extended Properties=Excel 12.0;";
                connection.ConnectionString = connect;
                connection.Open();
              //  command = new OleDbCommand("insert into [Sheet1$] values ('" + certificate + "')", connection);
		  command = new OleDbCommand("insert into [Sheet1$] values (@certificate)", connection);
                command.Parameters.AddWithValue("@certificate", certificate);
                command.ExecuteNonQuery();
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        finally
        {
            command.Dispose();
            connection.Close();
            connection.Dispose();

        }
}
}