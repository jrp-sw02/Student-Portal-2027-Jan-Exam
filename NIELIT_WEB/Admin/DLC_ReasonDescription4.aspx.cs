using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;

public partial class Admin_DLC_ReasonDescription4 : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    protected void Page_Load(object sender, EventArgs e)
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
        loginUserType = (UserType)Session["UserType"];
        entityID = Convert.ToInt64(Session["EntityID"]);
        if (loginUserType == UserType.RegionalCenter || loginUserType == UserType.HeadOffice || loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
        {
                if (!IsPostBack)
                {
                    BindCourse();
                    BindExamYear();
                    BindReason();
                }
        }
    }

    protected void BindCourse()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("-- Select One --", "0");

                var courses = (from c in context.Courses
                               where c.CourseCategoryID == 2
                               select new { ValueField = c.ID, TextField = c.Name });
                courses = courses.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindExamYear()
    {
        var currentYear = DateTime.Today.Year - 3;
        for (int i = 1; i <= 4; i++)
        {
            ddlYear.Items.Add((currentYear + i).ToString());           
        }
        //ddlYear.Items.Insert(1, "2012");        
    }
    protected void BindReason()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("-- Select One --", "0");

                var reasons = (from a in context.ApplicationStatuses
                               where (a.ID > 21 || a.ID == 13)
                               select new { ValueField = a.ID, TextField = a.Description });
                reasons = reasons.Distinct();

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlReason, reasons, lst);
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlReasonB, reasons, lst);           
                //ddlReasonB.Items.Insert(1, "Others");
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void Rdoownertype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Rdoownertype.SelectedValue == "I")
        {
            tblInd.Visible = true;
            tblBulk.Visible = false;
        }
        if (Rdoownertype.SelectedValue == "B")
        {
            tblInd.Visible = false;
            tblBulk.Visible = true;
        }   
    }
    protected void ddlReason_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 statusId = Convert.ToInt32(ddlReason.SelectedValue);
    }

    public Int64 CompareExamId(string applNo, Int32 examMonth, Int64 examYear, Int64 courseName)
    {
        try
        {
            courseName = Convert.ToInt64(ddlCourseName.SelectedValue);
            examMonth = Convert.ToInt32(ddlMonth.SelectedValue);
            examYear = Convert.ToInt64(ddlYear.SelectedValue);
            applNo = txtAppNo.Text;
            Int32 validRecord;

            using (var context = new EConnectContext())
            {
                var examIdSelection =  (from e in context.Exams where e.ExamMonth == examMonth 
                                       && e.ExamYear == examYear && e.CourseID == courseName
                                       select new { examId = e.ID }).FirstOrDefault();

                var examIdTable =  (from c in context.CertificateExamApplications
                                   where c.Number == applNo && c.ResultGradeID == null
                                   select new { examId = c.ExamID }).FirstOrDefault();

                if (examIdSelection.examId == examIdTable.examId)
                {  
                    validRecord = 1; 
                }
                else
                { 
                    validRecord = 0;
                }

                return validRecord;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public Int64 CompareExamIdB(string applicationNumber, Int32 examMonth, Int64 examYear, Int64 courseName)
    {
        try
        {
            courseName = Convert.ToInt64(ddlCourseName.SelectedValue);
            examMonth = Convert.ToInt32(ddlMonth.SelectedValue);
            examYear = Convert.ToInt64(ddlYear.SelectedValue);
            Int32 validRecord;

            using (var context = new EConnectContext())
            {
                var examIdSelection = (from e in context.Exams
                                       where e.ExamMonth == examMonth
                                       && e.ExamYear == examYear && e.CourseID == courseName
                                       select new { examId = e.ID }).FirstOrDefault();

                var examIdTable = (from c in context.CertificateExamApplications
                                   where c.Number == applicationNumber && c.ResultGradeID == null
                                   select new { examId = c.ExamID }).FirstOrDefault();

                if (examIdSelection.examId == examIdTable.examId)
                {
                    validRecord = 1;
                }
                else
                {
                    validRecord = 0;
                }

                return validRecord;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            Int32 statusId = Convert.ToInt32(ddlReason.SelectedValue);
            string applicationNumber = txtAppNo.Text;
            Int64 courseName = Convert.ToInt64(ddlCourseName.SelectedValue);
            Int32 examMonth = Convert.ToInt32(ddlMonth.SelectedValue);
            Int64 examYear = Convert.ToInt64(ddlYear.SelectedValue);

            Int32 userNo = loginUserNo;

            using (var context = new EConnectContext())
            {
                if (CompareExamId(applicationNumber, examMonth, examYear, courseName) == 1)
                {
                    //create history 
                    try
                    {
                        //var HistoryCheck = (from h in context.CertificateExamApplicationRejectedByExamWing
                        //                    where h.Number == applicationNumber
                        //                    select new { applNum = h.Number }).FirstOrDefault();
                        //if (HistoryCheck == null)
                        //{
                            context.Database.ExecuteSqlCommand("INSERT INTO [dbo].[Certificate_Exam_Application_RejectedByExamWing] ([Certificate_Exam_Application_ID],[Date],[Number],[Candidate_ID],[Already_Applied],[Previous_Exam_ID]," +
                            " [Previous_Roll_Number],[Previous_Application_ID],[Course_Category_ID],[Course_ID],[Applicant_Type_ID],[Exam_ID],[Institute_ID],[Exam_Center1_ID],[Exam_Center2_ID],[Regional_Center_ID],[Salutaion],[Name],[Father_Name] " +
                            " ,[Mother_Name],[Gender],[Dob],[Cast_Category_ID],[Occupation_ID],[Photo_File_Name],[Photo],[Signature_File_Name],[Signature],[Left_Thumb_File_Name],[Left_Thumb],[Educational_Qualification_ID] " +
                            " ,[Passing_Year],[Mobile],[Std],[Phone],[Email],[Cor_Address1],[Cor_Address2],[Cor_Address3],[Cor_Country_ID],[Cor_State_ID],[Cor_District_ID],[Cor_City_Name],[Cor_Pin_Code],[Final_Submitted] " +
                            " ,[Final_Submission_Date],[Demand_Note_ID],[Course_Duration_From],[Course_Duration_To],[Fee_Type_ID],[Fee_Amt],[Late_Fee_Amt],[Total_Fee_Amt],[Is_Verified_By_Institute],[Verified_On_By_Institute] " +
                            " ,[Payment_Status_ID],[Application_Status_ID],[Batch_Item_ID],[Roll_Number],[Exam_Centre_Name],[Exam_Centre_Address],[Date_of_Exam],[Exam_Batch_Number],[Reporting_Time],[Updated_On],[Updated_By] " +
                            " ,[Guardian_Name],[Result_Grade_ID],[Result_Updated_On],[Result_Updated_By],[Exempted_Application_ID],[Is_Exempted],[App_Source],[Is_Synced],[Synced_On],[Exam_Month],[Exam_Year],[Examn_Cycle_ID] " +
                            " ,[Previous_Exam_Name],[Aadhar_Number],[Aadhar_Verfied],[Department],[Employee_Code],[Designation],[Posting_City],[Date_of_Joining],[Date_of_Retirement],[Is_Downloaded],[Downloaded_On],[UID_Type] " +
                            " ,[UID_Number],[Email_sent_flag],[SMS_sent_flag],[Is_Disability],[Disability_Type_ID],[Disability_Percentage],[Is_EWS],[onlineRefID],[isUmang],[New_Application_Status_Id],[Created_On],[Created_By]) " +
                            " (SELECT s.[ID],s.[Date],s.[Number],s.[Candidate_ID],s.[Already_Applied],s.[Previous_Exam_ID],s.[Previous_Roll_Number],s.[Previous_Application_ID],s.[Course_Category_ID],s.[Course_ID],s.[Applicant_Type_ID] " +
                            " ,s.[Exam_ID],s.[Institute_ID],s.[Exam_Center1_ID],s.[Exam_Center2_ID],s.[Regional_Center_ID],s.[Salutaion],s.[Name],s.[Father_Name],s.[Mother_Name],s.[Gender],s.[Dob],s.[Cast_Category_ID],s.[Occupation_ID] " +
                            " ,s.[Photo_File_Name],s.[Photo],s.[Signature_File_Name],s.[Signature],s.[Left_Thumb_File_Name],s.[Left_Thumb],s.[Educational_Qualification_ID],s.[Passing_Year],s.[Mobile],s.[Std],s.[Phone],s.[Email] " +
                            " ,s.[Cor_Address1],s.[Cor_Address2],s.[Cor_Address3],s.[Cor_Country_ID],s.[Cor_State_ID],s.[Cor_District_ID],s.[Cor_City_Name],s.[Cor_Pin_Code],s.[Final_Submitted],s.[Final_Submission_Date],s.[Demand_Note_ID] " +
                            " ,s.[Course_Duration_From],s.[Course_Duration_To],s.[Fee_Type_ID],s.[Fee_Amt],s.[Late_Fee_Amt],s.[Total_Fee_Amt],s.[Is_Verified_By_Institute],s.[Verified_On_By_Institute],s.[Payment_Status_ID],s.[Application_Status_ID] " +
                            " ,s.[Batch_Item_ID],s.[Roll_Number],s.[Exam_Centre_Name],s.[Exam_Centre_Address],s.[Date_of_Exam],s.[Exam_Batch_Number],s.[Reporting_Time],s.[Updated_On],s.[Updated_By],s.[Guardian_Name],s.[Result_Grade_ID] " +
                            " ,s.[Result_Updated_On],s.[Result_Updated_By],s.[Exempted_Application_ID],s.[Is_Exempted],s.[App_Source],s.[Is_Synced],s.[Synced_On],s.[Exam_Month],s.[Exam_Year],s.[Examn_Cycle_ID],s.[Previous_Exam_Name] " +
                            " ,s.[Aadhar_Number],s.[Aadhar_Verfied],s.[Department],s.[Employee_Code],s.[Designation],s.[Posting_City],s.[Date_of_Joining],s.[Date_of_Retirement],s.[Is_Downloaded],s.[Downloaded_On],s.[UID_Type],s.[UID_Number] " +
                            " ,s.[Email_sent_flag],s.[SMS_sent_flag],s.[Is_Disability],s.[Disability_Type_ID],s.[Disability_Percentage],s.[Is_EWS],s.[onlineRefID],s.[isUmang],'" + statusId + "','" + DateTime.Now + "',' " + userNo + "' " +
                            "  FROM [dbo].[Certificate_Exam_Application] s where s.Number = '" + applicationNumber + "')");
                            context.SaveChanges();
                        //}
                        //else
                        //{
                        //    context.Database.ExecuteSqlCommand(" update Certificate_Exam_Application_RejectedByExamWing  set New_Application_Status_Id = '" + statusId + "', Created_On = '" + DateTime.Now + "', Created_By = '" + userNo + "' where  Number = '" + applicationNumber + "'");
                        //}
                    }
                    catch (Exception ex)
                    {
                        ShowAlert(ex.Message + " History is not created.");
                    }

                    var updateCEA = (from c in context.CertificateExamApplications
                                     where c.Number == applicationNumber && c.ResultGradeID == null
                                     select c).FirstOrDefault();

                    updateCEA.ApplicationStatusID = statusId;
                    context.Entry(updateCEA).State = System.Data.Entity.EntityState.Modified;
                    context.SaveChanges();
                    Lblerror.Visible = true;
                    Lblerror.Text = "You have successfully updated the reason.";
                }
                else
                {
                    Lblerror.Visible = true;
                    Lblerror.Text = " Not a valid record .Please contact Administrator.";
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {

            Int32 userNo = loginUserNo;

            String filepath = Server.MapPath("~/UploadedFiles");
            flUpload.SaveAs(filepath + "/" + flUpload.FileName);
            string path = (filepath + "/" + flUpload.FileName);
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
            divValidateData.Visible = true;

            OleDbConnection connection = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            connection.ConnectionString = sExcelConnectionString;
           // DataTable dtexcel = new DataTable();
            connection.Open();

            command = new OleDbCommand("select * from [Sheet1$]", connection);
            OleDbDataAdapter db = new OleDbDataAdapter(command);
           // db.Fill(dtexcel);

            OleDbDataReader dr = command.ExecuteReader();
            StringBuilder appIdList2 = new StringBuilder();
            StringBuilder appIdList1 = new StringBuilder();
            string applicationNumber = null;
            string[] arr = new string[10];
            arr[1] = "Already updated records detail.";
            arr[2] = "Failed records detail.";
            Int32 applicationStatus = 0;
            Int32 totalRecordsCount = 0;
            Int32 updatedRecordCount = 0;
            Int32 failedRecordCount = 0;
            Int32 alreadyUpdatedCount = 0;
            string applNo = txtAppNo.Text;
            Int32 examMonth = Convert.ToInt32(ddlMonth.SelectedValue);
            Int64 examYear = Convert.ToInt64(ddlYear.SelectedValue);
            Int64 courseName = Convert.ToInt64(ddlCourseName.SelectedValue);

            try
            {
                using (EConnectContext context = new EConnectContext())
                {
                    Int32 readCount = 0;

                    while (dr.Read())
                    {
                        readCount = readCount + 1;
                        if (readCount > 1)
                        {
                            totalRecordsCount = totalRecordsCount + 1;
                            applicationNumber = dr[1].ToString();
                            //dt.Rows[0]["linkedToCentre"].ToString()
                            //  applicationNumber = dtexcel.Rows[0]["Application Number"].ToString();
                            applicationStatus = Convert.ToInt32(ddlReasonB.SelectedValue);

                            if (CompareExamIdB(applicationNumber, examMonth, examYear, courseName) == 1)
                            {
                                var alreadyUpdate  = (from c in context.CertificateExamApplications
                                                       where c.Number == applicationNumber && c.ResultGradeID == null
                                                       select new { applicationStatus = c.ApplicationStatusID }).FirstOrDefault();

                                if (alreadyUpdate.applicationStatus == applicationStatus)
                                {
                                    alreadyUpdatedCount = alreadyUpdatedCount + 1;
                                    appIdList1.Append(applicationNumber.ToString() + ",");
                                    if (alreadyUpdatedCount % 10 == 0 && alreadyUpdatedCount > 0)
                                        appIdList1.Append(WebUtility.HtmlDecode("<br/>"));
                                }
                                else
                                {
                                    updatedRecordCount = updatedRecordCount + 1;

                                    //var HistoryCheck = (from h in context.CertificateExamApplicationRejectedByExamWing
                                    //                    where h.Number == applicationNumber
                                    //                    select new { applNum = h.Number }).FirstOrDefault();

                                    //if (HistoryCheck == null)
                                    //{
                                    context.Database.ExecuteSqlCommand("INSERT INTO [dbo].[Certificate_Exam_Application_RejectedByExamWing] ([Certificate_Exam_Application_ID],[Date],[Number],[Candidate_ID],[Already_Applied],[Previous_Exam_ID]," +
                                   " [Previous_Roll_Number],[Previous_Application_ID],[Course_Category_ID],[Course_ID],[Applicant_Type_ID],[Exam_ID],[Institute_ID],[Exam_Center1_ID],[Exam_Center2_ID],[Regional_Center_ID],[Salutaion],[Name],[Father_Name] " +
                                   " ,[Mother_Name],[Gender],[Dob],[Cast_Category_ID],[Occupation_ID],[Photo_File_Name],[Photo],[Signature_File_Name],[Signature],[Left_Thumb_File_Name],[Left_Thumb],[Educational_Qualification_ID] " +
                                   " ,[Passing_Year],[Mobile],[Std],[Phone],[Email],[Cor_Address1],[Cor_Address2],[Cor_Address3],[Cor_Country_ID],[Cor_State_ID],[Cor_District_ID],[Cor_City_Name],[Cor_Pin_Code],[Final_Submitted] " +
                                   " ,[Final_Submission_Date],[Demand_Note_ID],[Course_Duration_From],[Course_Duration_To],[Fee_Type_ID],[Fee_Amt],[Late_Fee_Amt],[Total_Fee_Amt],[Is_Verified_By_Institute],[Verified_On_By_Institute] " +
                                   " ,[Payment_Status_ID],[Application_Status_ID],[Batch_Item_ID],[Roll_Number],[Exam_Centre_Name],[Exam_Centre_Address],[Date_of_Exam],[Exam_Batch_Number],[Reporting_Time],[Updated_On],[Updated_By] " +
                                   " ,[Guardian_Name],[Result_Grade_ID],[Result_Updated_On],[Result_Updated_By],[Exempted_Application_ID],[Is_Exempted],[App_Source],[Is_Synced],[Synced_On],[Exam_Month],[Exam_Year],[Examn_Cycle_ID] " +
                                   " ,[Previous_Exam_Name],[Aadhar_Number],[Aadhar_Verfied],[Department],[Employee_Code],[Designation],[Posting_City],[Date_of_Joining],[Date_of_Retirement],[Is_Downloaded],[Downloaded_On],[UID_Type] " +
                                   " ,[UID_Number],[Email_sent_flag],[SMS_sent_flag],[Is_Disability],[Disability_Type_ID],[Disability_Percentage],[Is_EWS],[onlineRefID],[isUmang],[New_Application_Status_Id],[Created_On],[Created_By]) " +
                                   " (SELECT s.[ID],s.[Date],s.[Number],s.[Candidate_ID],s.[Already_Applied],s.[Previous_Exam_ID],s.[Previous_Roll_Number],s.[Previous_Application_ID],s.[Course_Category_ID],s.[Course_ID],s.[Applicant_Type_ID] " +
                                   " ,s.[Exam_ID],s.[Institute_ID],s.[Exam_Center1_ID],s.[Exam_Center2_ID],s.[Regional_Center_ID],s.[Salutaion],s.[Name],s.[Father_Name],s.[Mother_Name],s.[Gender],s.[Dob],s.[Cast_Category_ID],s.[Occupation_ID] " +
                                   " ,s.[Photo_File_Name],s.[Photo],s.[Signature_File_Name],s.[Signature],s.[Left_Thumb_File_Name],s.[Left_Thumb],s.[Educational_Qualification_ID],s.[Passing_Year],s.[Mobile],s.[Std],s.[Phone],s.[Email] " +
                                   " ,s.[Cor_Address1],s.[Cor_Address2],s.[Cor_Address3],s.[Cor_Country_ID],s.[Cor_State_ID],s.[Cor_District_ID],s.[Cor_City_Name],s.[Cor_Pin_Code],s.[Final_Submitted],s.[Final_Submission_Date],s.[Demand_Note_ID] " +
                                   " ,s.[Course_Duration_From],s.[Course_Duration_To],s.[Fee_Type_ID],s.[Fee_Amt],s.[Late_Fee_Amt],s.[Total_Fee_Amt],s.[Is_Verified_By_Institute],s.[Verified_On_By_Institute],s.[Payment_Status_ID],s.[Application_Status_ID] " +
                                   " ,s.[Batch_Item_ID],s.[Roll_Number],s.[Exam_Centre_Name],s.[Exam_Centre_Address],s.[Date_of_Exam],s.[Exam_Batch_Number],s.[Reporting_Time],s.[Updated_On],s.[Updated_By],s.[Guardian_Name],s.[Result_Grade_ID] " +
                                   " ,s.[Result_Updated_On],s.[Result_Updated_By],s.[Exempted_Application_ID],s.[Is_Exempted],s.[App_Source],s.[Is_Synced],s.[Synced_On],s.[Exam_Month],s.[Exam_Year],s.[Examn_Cycle_ID],s.[Previous_Exam_Name] " +
                                   " ,s.[Aadhar_Number],s.[Aadhar_Verfied],s.[Department],s.[Employee_Code],s.[Designation],s.[Posting_City],s.[Date_of_Joining],s.[Date_of_Retirement],s.[Is_Downloaded],s.[Downloaded_On],s.[UID_Type],s.[UID_Number] " +
                                   " ,s.[Email_sent_flag],s.[SMS_sent_flag],s.[Is_Disability],s.[Disability_Type_ID],s.[Disability_Percentage],s.[Is_EWS],s.[onlineRefID],s.[isUmang],'" + applicationStatus + "','" + DateTime.Now + "',' " + userNo + "' " +
                                   "  FROM [dbo].[Certificate_Exam_Application] s where s.Number = '" + applicationNumber + "')");
                                    context.SaveChanges();
                                    //}
                                    //else
                                    //{
                                    //    context.Database.ExecuteSqlCommand(" update Certificate_Exam_Application_RejectedByExamWing  set New_Application_Status_Id = '" + applicationStatus + "', Created_On = '" + DateTime.Now + "', Created_By = '" + userNo + "' where  Number = '" + applicationNumber + "'");
                                    //}
                                    //if ( CompareExamIdB(applicationNumber, examMonth, examYear, courseName) 
                                    //CompareExamIdB(applicationNumber, examMonth, examYear, courseName) 

                                    var updateCEAExcel = (from c in context.CertificateExamApplications
                                                          where c.Number == applicationNumber && c.ResultGradeID == null
                                                          select c).FirstOrDefault();

                                    updateCEAExcel.ApplicationStatusID = Convert.ToInt32(applicationStatus);
                                    context.Entry(updateCEAExcel).State = System.Data.Entity.EntityState.Modified;
                                    context.SaveChanges();
                                    //Lblerror.Visible = true;
                                    //Lblerror.Text = "You have successfully updated the reason.";
                                }
                            }

                            else
                            {
                                failedRecordCount = failedRecordCount + 1;
                                appIdList2.Append(applicationNumber.ToString() + ",");
                                if (failedRecordCount % 10 == 0 && failedRecordCount > 0)
                                    appIdList2.Append(WebUtility.HtmlDecode("<br/>"));
                            }

                            lblTotalRecords.Text = totalRecordsCount.ToString();
                            lblUpdatedRecords.Text = updatedRecordCount.ToString();
                            lblFailedRecords.Text = failedRecordCount.ToString();
                            lblAlreadyUpdatedCount.Text = alreadyUpdatedCount.ToString();
                            if (appIdList2.Length > 0)
                                lblFailedRecords.Text  += "<BR>" + failedRecordCount.ToString() + " (Application Number:- " + appIdList2.ToString().TrimEnd(',').ToString() + ":-" + arr[2].ToString() + ")";
                            if (appIdList1.Length > 0)
                                lblAlreadyUpdatedCount.Text += "<BR>" + alreadyUpdatedCount.ToString() + " (Application Number:- " + appIdList1.ToString().TrimEnd(',').ToString() + ":-" + arr[1].ToString() + ")";
                        }
                    }

                    //System.IO.File.Delete(path);
                    dr.Close();
                    dr.Dispose();
                    db.Dispose();
                    command.Dispose();
                    connection.Close();
                    connection.Dispose();                                    
                }
            }             
            catch (Exception ex)
            {   
                ShowAlert(ex.Message);
            }
            finally
            {
                dr.Close();
                dr.Dispose();
                command.Dispose();
                connection.Close();
                connection.Dispose();              
                //System.IO.File.Delete(path);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }       
    }
}