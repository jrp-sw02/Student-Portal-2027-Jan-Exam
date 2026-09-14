using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using EConnect.DAL;

public partial class DispatchCertificateExamData : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        lblErrMsg.Text = "";
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        string sql = "";
        string sqle = "";
        Int32 counter = 0;
        Int32 tablename = 0;
        string[] arr = new string[10];
        Int32 notupdatedrecords = 0;
        arr[3] = "Invalid Data Found.";
        EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
        try
        {
            if (ddltablename.SelectedValue == "0")
            {
                ShowAlert("Please select table name");
                return;
            }
            else if (txtRecords.Text == "")
            {
                throw new Exception("Please Enter no of records to export ");
            }
            else if (String.IsNullOrEmpty(txtRecords.Text) == false)
            {
                if (!isNumber(txtRecords))
                {
                    throw new Exception("Enter valid number");
                }
            }

            //EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            con.Open();
            if (ddltablename.SelectedValue == "1")
            {
                tablename = Convert.ToInt32(ddltablename.SelectedValue);
                sql = " select  top " + txtRecords.Text.Trim() + " case Already_Applied  when   'Y' then 1  when 'N' then 0  else 0  end as Already_Applied," +
                        "case Already_Applied  when   'Y' then Previous_Roll_Number  when 'N' then null  else null  end as prev_roll_no," +
                        "case Already_Applied  when   'Y' then Previous_Exam_ID  when 'N' then null  else null  end as prev_exam_name," +
                     " 2 as Course_Category_ID,5 as Course_ID," +
                     " case Institute_ID  when 'DIRECT' then 1 else 2 end as Applicant_Type_ID,0 as Exam_ID," +
                     " case Institute_ID  when 'DIRECT' then null else ISNULL((SELECT TOP 1 Institute_ID FROM  Intitute_Accreditation_Detail " +
                     " WHERE  Accreditation_Number  =temp.Institute_ID), 1094)  end as Institute_ID," +
                     " ISNULL((SELECT TOP 1 exm.ID FROM Exam_Center exm  WHERE  exm.Code = temp.Exam_Center1_ID), 300) as Exam_Center1_ID," +
                     " ISNULL((SELECT TOP 1 exm.ID FROM Exam_Center exm  WHERE  exm.Code = temp.Exam_Center2_ID), 300)as Exam_Center2_ID," +
                     " case Gender  when 'M' then 'Mr.'  when 'F' then 'Ms.' end as Salutaion," +
                     " substring(name,0,60)as Name ,substring(FATHER_NAME,0,60) as Father_Name,substring(Mother_Name,0,60) as Mother_Name," +
                     " case Gender  when 'M' then 'Male' when 'F' then 'Female' end as Gender,Dob ," +
                     " case Cast_Category_ID when 'GEN' then 1  when 'SC' then 2 when 'ST'then 3 when 'OBC' then 4 else 1 end as Cast_Category_ID," +
                     " case Occupation_ID when 'GEM' then 1  when 'SEM' then 2 when 'GOU' then 3 when 'OTH' then 4 else 4 end as Occupation_ID," +
                     " cast('1' as varbinary(max)) as Photo ,cast('1' as varbinary(max)) as Signature,cast('1' as varbinary(max)) as Left_Thumb," +
                     " case Educational_Qualification_ID when 'GRAD' then 20 when 'PD' then 21 when 'UX' then 22 when 'X' then 23 " +
                     " when 'XII' then 24 when 'XITI' then 25 else 18 end as Educational_Qualification_ID,0 as Mobile," +
                     " 'NA' as Email,ISNULL(substring(Cor_Address1,0,100),'NA')as Cor_Address1," +
                     " ISNULL(substring(Cor_Address2,0,100),'NA') as Cor_Address2,substring(Cor_Address3,0,100) as Cor_Address3, " +
                     " 1 as Cor_Country_ID,dbo.getStateID(ltrim(Cor_State_ID)) as Cor_State_ID,substring(Cor_City_Name,0,50) as Cor_City_Name," +
                     " isnull(Cor_Pin_Code,0) as Cor_Pin_Code,11 as Application_Status_ID," +
                     " Roll_Number,Exam_Centre_Name,Exam_Centre_Address,Date_of_Exam," +
                     " Exam_Batch_Number,Reporting_Time,dbo.getResultGradeID(Result_Grade_ID) as Result_Grade_ID ,dbo.getRegionalCentreID(Regional_Center_ID) as Regional_Center_ID,dbo.GetMonth(Exam_ID)as Exam_Month," +
                     " dbo.GetYear(REPLACE(Exam_ID,'EXAM CYCLE',''))as Exam_Year,5 as Examn_Cycle_ID from ConData_BCC_till_Mar2013  temp  where Is_Exported = 'false'";
            }
            else if (ddltablename.SelectedValue == "2")
            {
                tablename = Convert.ToInt32(ddltablename.SelectedValue);
                sql = " select  top " + txtRecords.Text.Trim() + " case Already_Applied  when   'Y' then 1  when 'N' then 0  else 0  end as Already_Applied," +
                        "case Already_Applied  when   'Y' then Previous_Roll_Number  when 'N' then null  else null  end as prev_roll_no," +
                        "case Already_Applied  when   'Y' then Previous_Exam_ID  when 'N' then null else null  end as prev_exam_name," +
                      " 2 as Course_Category_ID,7 as Course_ID," +
                      " case Institute_ID  when 'DIRECT' then 1 else 2 end as Applicant_Type_ID,0 as Exam_ID," +
                      " case Institute_ID  when 'DIRECT' then null else ISNULL((SELECT TOP 1 Institute_ID FROM  Intitute_Accreditation_Detail " +
                      " WHERE  Accreditation_Number  =temp.Institute_ID), 1094)  end as Institute_ID," +
                      " ISNULL((SELECT TOP 1 exm.ID FROM Exam_Center exm  WHERE  exm.Code = temp.Exam_Center1_ID), 300) as Exam_Center1_ID," +
                      " ISNULL((SELECT TOP 1 exm.ID FROM Exam_Center exm  WHERE  exm.Code = temp.Exam_Center2_ID), 300)as Exam_Center2_ID," +
                      " case Gender  when 'M' then 'Mr.'  when 'F' then 'Ms.' end as Salutaion," +
                      " substring(name,0,60)as Name ,substring(FATHER_NAME,0,60) as Father_Name,substring(Mother_Name,0,60) as Mother_Name," +
                      " case Gender  when 'M' then 'Male' when 'F' then 'Female' end as Gender,Dob ," +
                      " case Cast_Category_ID when 'GEN' then 1  when 'SC' then 2 when 'ST'then 3 when 'OBC' then 4 else 1 end as Cast_Category_ID," +
                      " case Occupation_ID when 'GEM' then 1  when 'SEM' then 2 when 'GOU' then 3 when 'OTH' then 4 else 4 end as Occupation_ID," +
                      " cast('1' as varbinary(max)) as Photo ,cast('1' as varbinary(max)) as Signature,cast('1' as varbinary(max)) as Left_Thumb," +
                      " case Educational_Qualification_ID when 'GRAD' then 20 when 'PD' then 21 when 'UX' then 22 when 'X' then 23 " +
                      " when 'XII' then 24 when 'XITI' then 25 else 18 end as Educational_Qualification_ID,0 as Mobile," +
                      " 'NA' as Email,ISNULL(substring(Cor_Address1,0,100),'NA')as Cor_Address1," +
                      " ISNULL(substring(Cor_Address2,0,100),'NA') as Cor_Address2,substring(Cor_Address3,0,100) as Cor_Address3, " +
                      " 1 as Cor_Country_ID,dbo.getStateID(ltrim(Cor_State_ID)) as Cor_State_ID,substring(Cor_City_Name,0,50) as Cor_City_Name," +
                      " isnull(Cor_Pin_Code,0) as Cor_Pin_Code,11 as Application_Status_ID," +
                      " Roll_Number,Exam_Centre_Name,Exam_Centre_Address,Date_of_Exam," +
                      " Exam_Batch_Number,Reporting_Time,dbo.getResultGradeID(Result_Grade_ID) as Result_Grade_ID, dbo.getRegionalCentreID(Regional_Center_ID) as Regional_Center_ID,dbo.GetMonth(Exam_ID)as Exam_Month," +
                      " dbo.GetYear(REPLACE(Exam_ID,'EXAM CYCLE',''))as Exam_Year,6 as Examn_Cycle_ID from ConData_CCC_EMEC_SPL  temp  where Is_Exported = 'false'";
            }
            else if (ddltablename.SelectedValue == "3")
            {
                tablename = Convert.ToInt32(ddltablename.SelectedValue);
                sql = " select  top " + txtRecords.Text.Trim() + " case Already_Applied  when   'Y' then 1  when 'N' then 0  else 0  end as Already_Applied," +
                    "case Already_Applied  when   'Y' then Previous_Roll_Number  when 'N' then null  else null end as prev_roll_no," +
                    "case Already_Applied  when   'Y' then Previous_Exam_ID  when 'N' then null else null  end as prev_exam_name," +
                     " 2 as Course_Category_ID,7 as Course_ID," +
                     " case Institute_ID  when 'DIRECT' then 1 else 2 end as Applicant_Type_ID,0 as Exam_ID," +
                     " case Institute_ID  when 'DIRECT' then null else ISNULL((SELECT TOP 1 Institute_ID FROM  Intitute_Accreditation_Detail " +
                     " WHERE  Accreditation_Number  =temp.Institute_ID), 1094)  end as Institute_ID," +
                     " ISNULL((SELECT TOP 1 exm.ID FROM Exam_Center exm  WHERE  exm.Code = temp.Exam_Center1_ID), 300) as Exam_Center1_ID," +
                     " ISNULL((SELECT TOP 1 exm.ID FROM Exam_Center exm  WHERE  exm.Code = temp.Exam_Center2_ID), 300)as Exam_Center2_ID," +
                     " case Gender  when 'M' then 'Mr.'  when 'F' then 'Ms.' end as Salutaion," +
                     " substring(name,0,60)as Name ,substring(FATHER_NAME,0,60) as Father_Name,substring(Mother_Name,0,60) as Mother_Name," +
                     " case Gender  when 'M' then 'Male' when 'F' then 'Female' end as Gender,Dob ," +
                     " case Cast_Category_ID when 'GEN' then 1  when 'SC' then 2 when 'ST'then 3 when 'OBC' then 4 else 1 end as Cast_Category_ID," +
                     " case Occupation_ID when 'GEM' then 1  when 'SEM' then 2 when 'GOU' then 3 when 'OTH' then 4 else 4 end as Occupation_ID," +
                     " cast('1' as varbinary(max)) as Photo ,cast('1' as varbinary(max)) as Signature,cast('1' as varbinary(max)) as Left_Thumb," +
                     " case Educational_Qualification_ID when 'GRAD' then 20 when 'PD' then 21 when 'UX' then 22 when 'X' then 23 " +
                     " when 'XII' then 24 when 'XITI' then 25 else 18 end as Educational_Qualification_ID,0 as Mobile," +
                     " 'NA' as Email,ISNULL(substring(Cor_Address1,0,100),'NA')as Cor_Address1," +
                     " ISNULL(substring(Cor_Address2,0,100),'NA') as Cor_Address2,substring(Cor_Address3,0,100) as Cor_Address3, " +
                     " 1 as Cor_Country_ID,dbo.getStateID(ltrim(Cor_State_ID)) as Cor_State_ID,substring(Cor_City_Name,0,50) as Cor_City_Name," +
                     " isnull(Cor_Pin_Code,0) as Cor_Pin_Code,11 as Application_Status_ID," +
                     " Roll_Number,Exam_Centre_Name,Exam_Centre_Address,Date_of_Exam," +
                     " Exam_Batch_Number,Reporting_Time,dbo.getResultGradeID(Result_Grade_ID) as Result_Grade_ID , dbo.getRegionalCentreID(Regional_Center_ID) as Regional_Center_ID,dbo.GetMonth(Exam_ID)as Exam_Month," +
                     " dbo.GetYear(REPLACE(Exam_ID,'EXAM CYCLE',''))as Exam_Year,6 as Examn_Cycle_ID from ConData_CCC_till_Feb2013_EMEC temp  where Is_Exported = 'false'";
            }
            else if (ddltablename.SelectedValue == "4")
            {
                tablename = Convert.ToInt32(ddltablename.SelectedValue);
                sql = " select  top " + txtRecords.Text.Trim() + " case Already_Applied  when   'Y' then 1  when 'N' then 0  else 0  end as Already_Applied," +
                     "case Already_Applied  when   'Y' then Previous_Roll_Number  when 'N' then null  else null  end as prev_roll_no," +
                    "case Already_Applied  when   'Y' then Previous_Exam_ID  when 'N' then null  else null  end as prev_exam_name," +
                 " 2 as Course_Category_ID,7 as Course_ID," +
                 " case Institute_ID  when 'DIRECT' then 1 else 2 end as Applicant_Type_ID,0 as Exam_ID," +
                 " case Institute_ID  when 'DIRECT' then null else ISNULL((SELECT TOP 1 Institute_ID FROM  Intitute_Accreditation_Detail " +
                 " WHERE  Accreditation_Number  =temp.Institute_ID), 1094)  end as Institute_ID," +
                 " ISNULL((SELECT TOP 1 exm.ID FROM Exam_Center exm  WHERE  exm.Code = temp.Exam_Center1_ID), 300) as Exam_Center1_ID," +
                 " ISNULL((SELECT TOP 1 exm.ID FROM Exam_Center exm  WHERE  exm.Code = temp.Exam_Center2_ID), 300)as Exam_Center2_ID," +
                 " case Gender  when 'M' then 'Mr.'  when 'F' then 'Ms.' end as Salutaion," +
                 " substring(name,0,60)as Name ,substring(FATHER_NAME,0,60) as Father_Name,substring(Mother_Name,0,60) as Mother_Name," +
                 " case Gender  when 'M' then 'Male' when 'F' then 'Female' end as Gender,Dob ," +
                 " case Cast_Category_ID when 'GEN' then 1  when 'SC' then 2 when 'ST'then 3 when 'OBC' then 4 else 1 end as Cast_Category_ID," +
                 " case Occupation_ID when 'GEM' then 1  when 'SEM' then 2 when 'GOU' then 3 when 'OTH' then 4 else 4 end as Occupation_ID," +
                 " cast('1' as varbinary(max)) as Photo ,cast('1' as varbinary(max)) as Signature,cast('1' as varbinary(max)) as Left_Thumb," +
                 " case Educational_Qualification_ID when 'GRAD' then 20 when 'PD' then 21 when 'UX' then 22 when 'X' then 23 " +
                 " when 'XII' then 24 when 'XITI' then 25 else 18 end as Educational_Qualification_ID,0 as Mobile," +
                 " 'NA' as Email,ISNULL(substring(Cor_Address1,0,100),'NA')as Cor_Address1," +
                 " ISNULL(substring(Cor_Address2,0,100),'NA') as Cor_Address2,substring(Cor_Address3,0,100) as Cor_Address3, " +
                 " 1 as Cor_Country_ID,dbo.getStateID(ltrim(Cor_State_ID)) as Cor_State_ID,substring(Cor_City_Name,0,50) as Cor_City_Name," +
                 " isnull(Cor_Pin_Code,0) as Cor_Pin_Code,11 as Application_Status_ID," +
                 " Roll_Number,Exam_Centre_Name,Exam_Centre_Address,Date_of_Exam," +
                 " Exam_Batch_Number,Reporting_Time,dbo.getResultGradeID(Result_Grade_ID) as Result_Grade_ID, dbo.getRegionalCentreID(Regional_Center_ID) as Regional_Center_ID,dbo.GetMonth(Exam_ID)as Exam_Month," +
                 " dbo.GetYear(REPLACE(Exam_ID,'EXAM CYCLE',''))as Exam_Year,7 as Examn_Cycle_ID from condata_ccc_Till_Feb2013_regular  temp  where Is_Exported = 'false' ";
            }
            SqlDataReader dr = EConnect.Utils.Data.DbUtility.ExecuteReader(sql, con, null, CommandType.Text, true);
            while (dr.Read())
            {
                try
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        //sqle = "insert into Certificate_Exam_Application(Already_Applied,Previous_Roll_Number,Previous_Exam_Name,Course_Category_ID,Course_ID,Applicant_Type_ID,Exam_ID," +
                        // " Institute_ID,Exam_Center1_ID,Exam_Center2_ID,Salutaion,Name,Father_Name,Mother_Name,Gender,Dob,Cast_Category_ID ," +
                        // " Occupation_ID,Photo,Signature,Left_Thumb,Educational_Qualification_ID,Mobile,Email,Cor_Address1,Cor_Address2,Cor_Address3," +
                        // " Cor_Country_ID,Cor_State_ID,Cor_City_Name,Cor_Pin_Code,Application_Status_ID,Roll_Number,Exam_Centre_Name,Exam_Centre_Address," +
                        // " Date_of_Exam,Exam_Batch_Number,Reporting_Time,Result_Grade_ID,Result_Updated_On,Result_Updated_By,Regional_Center_ID,Exam_Month,Exam_Year,Examn_Cycle_ID) values " +
                        // " ( '" + dr["Already_Applied"].ToString() + "','" + dr["prev_roll_no"].ToString() + "','" + dr["prev_exam_name"].ToString() + "','" + dr["Course_Category_ID"].ToString() + "','" + dr["Course_ID"].ToString() +
                        // "','" + dr["Applicant_Type_ID"].ToString() + "','" + dr["Exam_ID"].ToString() + "'," + (String.IsNullOrEmpty(dr["Institute_ID"].ToString()) ? "NULL" : dr["Institute_ID"]) + "," +
                        // " '" + dr["Exam_Center1_ID"].ToString() + "','" + dr["Exam_Center2_ID"].ToString() + "','" + dr["Salutaion"].ToString() + "'," +
                        // " '" + dr["Name"].ToString().Replace("'", "''") + "','" + dr["Father_Name"].ToString().Replace("'", "''") + "'," +
                        // " '" + dr["Mother_Name"].ToString().Replace("'", "''") + "','" + dr["Gender"].ToString() + "','" + dr["Dob"].ToString() + "'," +
                        // " '" + dr["Cast_Category_ID"].ToString() + "','" + dr["Occupation_ID"].ToString() + "',0x123,0x123,0x123," +
                        // " '" + dr["Educational_Qualification_ID"].ToString() + "'," +
                        // " '" + dr["Mobile"].ToString() + "','" + dr["Email"].ToString() + "','" + dr["Cor_Address1"].ToString().Replace("'", "''") + "'," +
                        // " '" + dr["Cor_Address2"].ToString().Replace("'", "''") + "','" + dr["Cor_Address3"].ToString().Replace("'", "''") + "','" + dr["Cor_Country_ID"].ToString() + "'," +
                        // " '" + dr["Cor_State_ID"].ToString() + "','" + dr["Cor_City_Name"].ToString().Replace("'", "''") + "','" + dr["Cor_Pin_Code"].ToString() + "'," +
                        // " '" + dr["Application_Status_ID"].ToString() + "','" + dr["Roll_Number"].ToString() + "','" + dr["Exam_Centre_Name"].ToString() + "'," +
                        // " '" + dr["Exam_Centre_Address"].ToString().Replace("'", "''") + "','" + dr["Date_of_Exam"].ToString() + "','" + dr["Exam_Batch_Number"].ToString() + "'," +
                        // " '" + dr["Reporting_Time"].ToString() + "','" + dr["Result_Grade_ID"].ToString() + "', '" + DateTime.Now + "','1','" + dr["Regional_Center_ID"].ToString() + "','" + dr["Exam_Month"].ToString() + "'," +
                        // " '" + dr["Exam_Year"].ToString() + "','" + dr["Examn_Cycle_ID"].ToString() + "')";

                        //December_2024
                        SqlParameter[] param1 = {  new SqlParameter("@alreadyApplied", dr["Already_Applied"].ToString()),
                                                   new SqlParameter("@prevRollNo", dr["prev_roll_no"].ToString()),
                                                   new SqlParameter("@prevExamName", dr["prev_exam_name"].ToString()),
                                                   new SqlParameter("@courseCategoryID", dr["Course_Category_ID"].ToString()),
                                                   new SqlParameter("@courseID", dr["Course_ID"].ToString()),
                                                   new SqlParameter("@applicantTypeID", dr["Applicant_Type_ID"].ToString()),
                                                   new SqlParameter("@examID", dr["Exam_ID"].ToString()),
                                                   new SqlParameter("@instituteID", (String.IsNullOrEmpty(dr["Institute_ID"].ToString()) ? "NULL" : dr["Institute_ID"]) ),
                                                   new SqlParameter("@examCenter1ID", dr["Exam_Center1_ID"].ToString()),
                                                   new SqlParameter("@examCenter2ID", dr["Exam_Center2_ID"].ToString()),
                                                   new SqlParameter("@salutaion", dr["Salutaion"].ToString()),
                                                   new SqlParameter("@name", dr["Name"].ToString().Replace("'", "''")),
                                                   new SqlParameter("@fatherName", dr["Father_Name"].ToString().Replace("'", "''")),
                                                   new SqlParameter("@motherName",dr["Mother_Name"].ToString().Replace("'", "''")),
                                                   new SqlParameter("@gender", dr["Gender"].ToString()),
                                                   new SqlParameter("@dob", dr["Dob"].ToString()),
                                                   new SqlParameter("@castCategoryID", dr["Cast_Category_ID"].ToString()),
                                                   new SqlParameter("@occupationID", dr["Occupation_ID"].ToString()),
                                                   new SqlParameter("@educationalQualificationID", dr["Educational_Qualification_ID"].ToString()),
                                                   new SqlParameter("@mobile", dr["Mobile"].ToString()),
                                                   new SqlParameter("@email", dr["Email"].ToString()),
                                                   new SqlParameter("@corAddress1", dr["Cor_Address1"].ToString().Replace("'", "''")),
                                                   new SqlParameter("@corAddress2", dr["Cor_Address2"].ToString().Replace("'", "''")),
                                                   new SqlParameter("@corAddress3", dr["Cor_Address3"].ToString().Replace("'", "''")),
                                                   new SqlParameter("@corCountryID",dr["Cor_Country_ID"].ToString()),
                                                   new SqlParameter("@corStateID", dr["Cor_State_ID"].ToString()),
                                                   new SqlParameter("@corCityName", dr["Cor_City_Name"].ToString().Replace("'", "''")),
                                                   new SqlParameter("@corPinCode", dr["Cor_Pin_Code"].ToString()),
                                                   new SqlParameter("@applicationStatusID", dr["Application_Status_ID"].ToString()),
                                                   new SqlParameter("@rollNumber", dr["Roll_Number"].ToString()),
                                                   new SqlParameter("@examCentreName",dr["Exam_Centre_Name"].ToString()),
                                                   new SqlParameter("@examCentreAddress", dr["Exam_Centre_Address"].ToString().Replace("'", "''")),
                                                   new SqlParameter("@dateOfExam", dr["Date_of_Exam"].ToString() ),
                                                   new SqlParameter("@examBatchNumber", dr["Exam_Batch_Number"].ToString()),
                                                   new SqlParameter("@reportingTime", dr["Reporting_Time"].ToString()),
                                                   new SqlParameter("@resultGradeID", dr["Result_Grade_ID"].ToString()),
                                                   new SqlParameter("@currentDate", DateTime.Now),
                                                   new SqlParameter("@regionalCenterID", dr["Regional_Center_ID"].ToString()),
                                                   new SqlParameter("@examMonth", dr["Exam_Month"].ToString()),
                                                   new SqlParameter("@examYear", dr["Exam_Year"].ToString()),
                                                   new SqlParameter("@examCycleID", dr["Examn_Cycle_ID"].ToString()),
                                                                    };
                        sqle = "insert into Certificate_Exam_Application(Already_Applied,Previous_Roll_Number,Previous_Exam_Name,Course_Category_ID,Course_ID,Applicant_Type_ID,Exam_ID," +
                         " Institute_ID,Exam_Center1_ID,Exam_Center2_ID,Salutaion,Name,Father_Name,Mother_Name,Gender,Dob,Cast_Category_ID ," +
                         " Occupation_ID,Photo,Signature,Left_Thumb,Educational_Qualification_ID,Mobile,Email,Cor_Address1,Cor_Address2,Cor_Address3," +
                         " Cor_Country_ID,Cor_State_ID,Cor_City_Name,Cor_Pin_Code,Application_Status_ID,Roll_Number,Exam_Centre_Name,Exam_Centre_Address," +
                         " Date_of_Exam,Exam_Batch_Number,Reporting_Time,Result_Grade_ID,Result_Updated_On,Result_Updated_By,Regional_Center_ID,Exam_Month,Exam_Year,Examn_Cycle_ID) values " +
                         " ( @alreadyApplied,@prevRollNo,@prevExamName,@courseCategoryID,@courseID "+
                         " ,@applicantTypeID,@examID,@instituteID," +
                         " @examCenter1ID,@examCenter2ID,@salutaion," +
                         " @name,@fatherName," +
                         " @motherName,@gender,@dob," +
                         " @castCategoryID,@occupationID,0x123,0x123,0x123," +
                         " @educationalQualificationID," +
                         " @mobile,@email,@corAddress1," +
                         " @corAddress2,@corAddress3,@corCountryID," +
                         " @corStateID,@corCityName,@corPinCode," +
                         " @applicationStatusID,@rollNumber,@examCentreName," +
                         " @examCentreAddress,@dateOfExam,@examBatchNumber," +
                         " @reportingTime,@resultGradeID, @currentDate,'1',@regionalCenterID,@examMonth," +
                         " @examYear,@examCycleID)";


                        context.Database.ExecuteSqlCommand(sqle,param1);


                        SqlParameter[] param2 = {  new SqlParameter("@currentDate", DateTime.Now),
                                                   new SqlParameter("@rollNumber", dr["Roll_Number"].ToString() )
                                                        };

                        if (ddltablename.SelectedValue == "1")
                        {
                            // update in NIELIT_CCC.dbo.ConData_BCC_till_Mar2013 
                            //context.Database.ExecuteSqlCommand("Update ConData_BCC_till_Mar2013  set Is_Exported = 'true', Exported_On = '" + DateTime.Now + "' where ROLL_NUMBER = '" + dr["Roll_Number"].ToString() + "'");

                            //December_2024
                            context.Database.ExecuteSqlCommand("Update ConData_BCC_till_Mar2013  set Is_Exported = 'true', Exported_On = @currentDate where ROLL_NUMBER = @rollNumber ", param2);
                        }
                        else if (ddltablename.SelectedValue == "2")
                        {
                            // update in NIELIT_CCC.dbo.ConData_CCC_EMEC_SPL
                            //context.Database.ExecuteSqlCommand("Update ConData_CCC_EMEC_SPL set Is_Exported = 'true', Exported_On = '" + DateTime.Now + "' where ROLL_NUMBER = '" + dr["Roll_Number"].ToString() + "'");

                            //December_2024
                            context.Database.ExecuteSqlCommand("Update ConData_CCC_EMEC_SPL set Is_Exported = 'true', Exported_On = @currentDate where ROLL_NUMBER = @rollNumber ", param2);
                        }
                        else if (ddltablename.SelectedValue == "3")
                        {
                            // update in NIELIT_CCC.dbo.ConData_CCC_till_Feb2013_EMEC 
                            //context.Database.ExecuteSqlCommand("Update ConData_CCC_till_Feb2013_EMEC  set Is_Exported = 'true', Exported_On = '" + DateTime.Now + "' where ROLL_NUMBER = '" + dr["Roll_Number"].ToString() + "'");

                            //December_2024
                            context.Database.ExecuteSqlCommand("Update ConData_CCC_till_Feb2013_EMEC  set Is_Exported = 'true', Exported_On = @currentDate where ROLL_NUMBER = @rollNumber ", param2);
                        }
                        else if (ddltablename.SelectedValue == "4")
                        {
                            // update in NIELIT_CCC.dbo.condata_ccc_Till_Feb2013_regular
                            //context.Database.ExecuteSqlCommand("Update condata_ccc_Till_Feb2013_regular set Is_Exported = 'true', Exported_On = '" + DateTime.Now + "' where ROLL_NUMBER = '" + dr["Roll_Number"].ToString() + "'");

                            //December_2024
                            context.Database.ExecuteSqlCommand("Update condata_ccc_Till_Feb2013_regular set Is_Exported = 'true', Exported_On = @currentDate where ROLL_NUMBER = @rollNumber ", param2);
                        }
                        context.SaveChanges();
                        counter = counter + 1;
                    };
                }
                catch (Exception)
                {
                    notupdatedrecords = notupdatedrecords + 1;
                    continue;
                }
               
            }
            con.Close();
            ShowAlert("Data Exported Successfully for " + counter + " applications. ");
            txtRecords.Text = "";
            ddltablename.SelectedValue = tablename.ToString();
            ddltablename_SelectedIndexChanged(tablename, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
            
            con.Close();
        }
        
    }
    protected void btnResetSelect_Click(object sender, EventArgs e)
    {
        Response.Redirect("DispatchCertificateExamData.aspx");
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (txtPassword.Text.Trim() == System.Web.Configuration.WebConfigurationManager.AppSettings["AdminPwd"].ToString())
        {
            trLogin.Visible = false;
            trQuery.Visible = true;
        }
        else
        {
            lblErrMsg.Text = "Incorrect password.";
        }
    }
    protected bool isNumber(TextBox txtBox)
    {
        try
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            if (txtBox.Text.Trim() != "")
            {
                if (!regex.IsMatch(txtBox.Text.Trim()))
                {
                    txtBox.Text = "";
                    txtBox.Focus();
                    return false;
                }
                else
                    return true;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddltablename_SelectedIndexChanged(object sender, EventArgs e)
    {
        EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
        try
        {

            //EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
            con.Open();
            if (ddltablename.SelectedValue == "1")
            {
                Lbexported.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller(" select count(*) from ConData_BCC_till_Mar2013  where Is_Exported = 'true'", con, null, CommandType.Text, true).ToString();
                Lbleftexported.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from ConData_BCC_till_Mar2013  where Is_Exported = 'false'", con, null, CommandType.Text, true).ToString();
            }
            else if (ddltablename.SelectedValue == "2")
            {
                Lbexported.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller(" select count(*) from ConData_CCC_EMEC_SPL   where Is_Exported = 'true'", con, null, CommandType.Text, true).ToString();
                Lbleftexported.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from ConData_CCC_EMEC_SPL    where Is_Exported = 'false'", con, null, CommandType.Text, true).ToString();
            }
            else if (ddltablename.SelectedValue == "3")
            {
                Lbexported.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller(" select count(*) from ConData_CCC_till_Feb2013_EMEC  where Is_Exported = 'true'", con, null, CommandType.Text, true).ToString();
                Lbleftexported.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from ConData_CCC_till_Feb2013_EMEC where Is_Exported = 'false'", con, null, CommandType.Text, true).ToString();
            }
            else if (ddltablename.SelectedValue == "4")
            {
                Lbexported.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller(" select count(*) from condata_ccc_Till_Feb2013_regular  where Is_Exported = 'true'", con, null, CommandType.Text, true).ToString();
                Lbleftexported.Text = EConnect.Utils.Data.DbUtility.ExecuteScaller("select count(*) from condata_ccc_Till_Feb2013_regular where Is_Exported = 'false'", con, null, CommandType.Text, true).ToString();
            }
            else
            {
                Lbexported.Text = "";
                Lbleftexported.Text = "";
            }
            Lbexported.Visible = Lbleftexported.Visible = true;
        }
        catch (Exception ex)
        {
            Lbexported.Visible = Lbleftexported.Visible = false;
            ShowAlert(ex.Message);

        }
        finally
        {
            con.Close();
        }
    }
}