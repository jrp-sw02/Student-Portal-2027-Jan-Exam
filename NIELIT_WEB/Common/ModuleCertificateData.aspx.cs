using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Data;
using Ionic.Zip;
using System.Configuration;
using System.Data.SqlClient;


public partial class ModuleCertificateData : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
            if (!IsPostBack)
            {
                ModuleCertificateRequestEntryAuto();
                BindCourseLevel();
                BindNewRequest();
            }
        }
        catch (Exception) { }
    }

    void ModuleCertificateRequestEntryAuto()
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection Conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("ModuleCertificateRequestEntryAuto", Conn))
                {
                    Conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
        
    }

    protected void BindCourseLevel()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                ListItem lst = new ListItem("--Select One--", "0");
                var level = from p in context.Courses
                            where p.CourseCategoryID == 1
                            orderby (p.ID)
                            select new { ValueField = p.ID, TextField = p.Code };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, level.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void BindNewRequest()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                btnProcess.Visible = false; ;
                Int32 PayStatus = Convert.ToInt32(enmPaymentStatus.Paid);
                var application = (from p in context.ModuleCertificateRequests
                                   where (p.PaymentStatusID == PayStatus || p.PaymentStatusID==5)
                                   && p.BatchID == null && p.IsBlocked != true
                                   group p by new { p.CourseID, p.Course.Name } into c
                                   select new { CourseId = c.Key.CourseID, CourseName = c.Key.Name, RequestCount = c.Count(p => p.BatchID == null) });
                application = application.OrderBy(p => p.CourseId);
                PagingBar1.Bind(application, ref gbapplicant);
                uPnlGrid1.Update();
                uPnlNavigation.Update();

                if (application.Count() > 0)
                { btnProcess.Visible = true; }
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }

    protected void ModuleBatchAssignment(Int32 CourseId)
    {
        //int BatchId = 0;
        //Int32 PayStatus = Convert.ToInt32(enmPaymentStatus.Paid);
        //using (TransactionScope scope = new TransactionScope())
        //{
        //    using (var context = new EConnectContext())
        //    {
        //        //var unaggignedrequest = (from p in context.ModuleCertificateRequests
        //        //                         where p.BatchID == null
        //        //                         && p.CourseID == CourseId
        //        //                         && p.PaymentStatusID == PayStatus
        //        //                         select p).ToList();

        //        var unaggignedrequest = (from p in context.ModuleCertificateRequests
        //                                 where p.BatchID == null
        //                                 && p.CourseID == CourseId
        //                                 && (p.PaymentStatusID == PayStatus || p.PaymentStatusID==5 )
        //                                 && p.IsBlocked ==  false
        //                                 select p).ToList();

        //        if (unaggignedrequest.Count() > 0)
        //        {
        //            ModuleCertificateBatch newbatch = new ModuleCertificateBatch();
        //            newbatch.CourseID = CourseId;
        //            newbatch.Size = unaggignedrequest.Count();
        //            newbatch.CreatedDate = DateTime.Now;
        //            context.ModuleCertificateBatchs.Add(newbatch);
        //            context.SaveChanges();

        //            BatchId = newbatch.ID;

        //            for (int i = 0; i < unaggignedrequest.Count(); i++)
        //            {
        //                ModuleCertificateRequest AssignBatch = context.ModuleCertificateRequests.Find(unaggignedrequest[i].ID);
        //                AssignBatch.BatchID = BatchId;
        //                context.Entry(AssignBatch).State = System.Data.Entity.EntityState.Modified;
        //            }
        //            context.SaveChanges();
        //        } scope.Complete();
        //    }
        //}

        try
        {
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            using (SqlConnection Conn = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Module_certificate_batch_assign", Conn))
                {
                    Conn.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@course_id", CourseId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlCourseName_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int32 level = Convert.ToInt32(ddlCourseName.SelectedValue);
                ListItem lst = new ListItem("--Select One--", "0");
                var batch = (context.ModuleCertificateBatchs
                            .Where(p => p.CourseID == level)
                            .OrderByDescending(p => p.ID)).ToList();

                var cbatch = batch.Select(s => new { ValueField = s.ID, TextField = string.Format("{0,-15} (count:{1,7})", s.ID, s.Size) });

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlBatchNumber, cbatch, lst);
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {
        OleDbConnection connection = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();
        string mdbFilePath = "";
        String filename = "";

        EConnectContext context = new EConnectContext();
        try
        {

            DataTable dtTbl = new DataTable();

            Int32 BatchID = Convert.ToInt32(ddlBatchNumber.SelectedValue);
            Int32 CourseId = Convert.ToInt32(ddlCourseName.SelectedValue);
            string FileTypeShortName = "";
            if (ddlDataFile.SelectedValue == "1")
                FileTypeShortName = "C_M_D";
            else if (ddlDataFile.SelectedValue == "2")
                FileTypeShortName = "C_P_D";
            else if (ddlDataFile.SelectedValue == "3")
                FileTypeShortName = "C_Photo";
            string courseCode = ddlCourseName.SelectedItem.Text;

            if (ddlDataFile.SelectedValue == "1" || ddlDataFile.SelectedValue == "2")
            {
                filename = FileTypeShortName + "_" + BatchID + "_" + courseCode + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".accdb";

                mdbFilePath = Server.MapPath("~/Download/" + filename);
                if (System.IO.File.Exists(mdbFilePath))
                    System.IO.File.Delete(mdbFilePath);

                if (ddlDataFile.SelectedValue == "1")
                    System.IO.File.Copy(Server.MapPath("~/StandardFileFormat/Standard_OABC_module_detail.accdb"), mdbFilePath);
                else if (ddlDataFile.SelectedValue == "2")
                    System.IO.File.Copy(Server.MapPath("~/StandardFileFormat/Standard_OABC_personal_detail.accdb"), mdbFilePath);

                string connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mdbFilePath + ";Persist Security Info=False;";
               // string connect = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbFilePath + ";Persist Security Info=False;";
                //System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;" 
                connection.ConnectionString = connect;
                connection.Open();
                //command = new OleDbCommand("delete from  [MS Access;Database=" + mdbFilePath + "].[CERT]", connection);

                //November_2024
                command = new OleDbCommand("delete from  [MS Access;Database=@mdbFilePath].[CERT]", connection);
                command.Parameters.AddWithValue("@mdbFilePath", mdbFilePath);
                command.ExecuteNonQuery();
                String sqlStr = "";
                String sql = "";
                SqlParameter[] para1 = new SqlParameter[2];

                if (ddlDataFile.SelectedValue == "1")
                {
                    Int32 Batch = Convert.ToInt32(ddlBatchNumber.SelectedValue);

                    //sql = "select a.Registration_Number as 'registration_no', " +
                    //        "case a.Course_ID  when 1 then 'O' when 2 then 'A' when 3 then 'B' when 4 then 'C' end  as 'level_code', " +
                    //        "b.Short_Name  module_code ,b.Name  module_desc,isnull(b.NumberOfHours, '') hours,isnull(b.NumberOfCredits, '') credits, " +
                    //        "DateName( month , DateAdd( month , a.exam_month , -1 ) ) as 'passing_month', " +
                    //        "a.Exam_Year as 'passing_year', r.Code as 'grade', '' as 'module_remarks', 'Y' as 'whether_module_certificate' " +
                    //        "from Course_Exam_Application_Detail a , Module b, ModuleCertificateRequest m , Result_Grading r " +
                    //        "where a.Registration_Number =m.Registration_No and a.Course_ID = m.Course_ID " +
                    //        "and a.Module_ID = b.ID and b.ID = m.Module_ID and ((a.Result_Grade_ID = r.ID and r.Description = 'Pass') OR (a.Is_Canceled = 1 and a.Old_Result_Grade_ID = r.ID and r.Description = 'Pass'))  and a.Course_ID =" + CourseId + " and m.Batch_ID = " + Batch + "  ";

                    //sql = "select a.Registration_Number as 'registration_no'," +
                    //            "case a.Course_ID  when 1 then 'O' when 2 then 'A' when 3 then 'B' when 4 then 'C' end  as 'level_code'," +
                    //            "b.Short_Name  module_code ,b.Name  module_desc,isnull(b.NumberOfHours, '') hours,isnull(b.NumberOfCredits, '') credits," +
                    //            "DateName( month , DateAdd( month , a.exam_month , -1 ) ) as 'passing_month'," +
                    //            "a.Exam_Year as 'passing_year', r.Code as 'grade', '' as 'module_remarks', 'Y' as 'whether_module_certificate' from Course_Exam_Application_Detail a , Module b, ModuleCertificateRequest m , Result_Grading r " +
                    //            "where m.Is_Blocked = 0 and a.Registration_Number =m.Registration_No and a.Course_ID = m.Course_ID " +
                    //            "and a.Module_ID = b.ID and b.ID = m.Module_ID and ((a.Result_Grade_ID = r.ID and r.Description = 'Pass') OR (a.Is_Canceled = 1 and a.Old_Result_Grade_ID = r.ID and r.Description = 'Pass'))  and a.Course_ID = '" + CourseId + "' and m.Batch_ID ='" + Batch + "'" +
                    //           "and not exists (select 1 from Candidate c, Course_Exam_Application_Detail rd, course_registration_application cra where rd.Candidate_ID =c.id and rd.Registration_Number =a.Registration_Number and cra.Candidate_ID =c.id and ((c.Guardian_Name is not null and cra.affidavitNo is null )  ))";

                    //November_2024
                    para1[0] =new SqlParameter("@CourseId", CourseId);
                    para1[1] = new SqlParameter("@Batch", Batch);   
                    sql = "select a.Registration_Number as 'registration_no'," +
                               "case a.Course_ID  when 1 then 'O' when 2 then 'A' when 3 then 'B' when 4 then 'C' end  as 'level_code'," +
                               "b.Short_Name  module_code ,b.Name  module_desc,isnull(b.NumberOfHours, '') hours,isnull(b.NumberOfCredits, '') credits," +
                               "DateName( month , DateAdd( month , a.exam_month , -1 ) ) as 'passing_month'," +
                               "a.Exam_Year as 'passing_year', r.Code as 'grade', '' as 'module_remarks', 'Y' as 'whether_module_certificate' from Course_Exam_Application_Detail a , Module b, ModuleCertificateRequest m , Result_Grading r " +
                               "where m.Is_Blocked = 0 and a.Registration_Number =m.Registration_No and a.Course_ID = m.Course_ID " +
                               "and a.Module_ID = b.ID and b.ID = m.Module_ID and ((a.Result_Grade_ID = r.ID and r.Description = 'Pass') OR (a.Is_Canceled = 1 and a.Old_Result_Grade_ID = r.ID and r.Description = 'Pass'))  and a.Course_ID = @CourseId and m.Batch_ID =@Batch" +
                              "and not exists (select 1 from Candidate c, Course_Exam_Application_Detail rd, course_registration_application cra where rd.Candidate_ID =c.id and rd.Registration_Number =a.Registration_Number and cra.Candidate_ID =c.id and ((c.Guardian_Name is not null and cra.affidavitNo is null )  ))";
                }
                else if (ddlDataFile.SelectedValue == "2")
                {
                    Int32 Batch = Convert.ToInt32(ddlBatchNumber.SelectedValue);
                    //sql = "select '' certificate_no, a.Registration_No as 'registration_no', b.Name as 'Name', " +
                    //        "b.Father_Name as 'f_name', b.Mother_Name as 'm_name', b.Guardian_Name as 'g_Name', " +
                    //        "'' as 'affidavit_srno', cast(GETDATE() as date) as 'affidavit_date', b.Dob as 'd_o_b', " +
                    //        "'0' as 'phase_no', cast(GETDATE() as date) as 'phase_generation_date','January' as 'phase_month','1900' as 'phase_year', " +
                    //        "case a.Course_ID  when 1 then 'O' when 2 then 'A' when 3 then 'B' when 4 then 'C' end  as 'level_code', " +
                    //        "'' as 'final_grade', '0' as 'overall_percentage', '' as 'address1' , '' as 'address2', '' as 'address3','' as 'city','' as 'State','' as 'pin', '' as 'remarks' " +
                    //        "from Registration_Detail a , Candidate b, ModuleCertificateRequest m " +
                    //        "where a.Candidate_ID =b.ID and a.Registration_No = m.Registration_No and a.Course_ID ='" + CourseId + "' and m.Batch_ID = '" + Batch + "' ";


                    //sql =   "select '' certificate_no, a.Registration_No as 'registration_no', b.Name as 'Name'," +
                    //        "b.Father_Name as 'f_name', b.Mother_Name as 'm_name', b.Guardian_Name as 'g_Name'," +
                    //        "cra.affidavitNo as 'affidavit_srno', cra.affidavitDate as 'affidavit_date', b.Dob as 'd_o_b'," +
                    //        "'0' as 'phase_no', cast(GETDATE() as date) as 'phase_generation_date','January' as 'phase_month','1900' as 'phase_year'," +
                    //        "case a.Course_ID  when 1 then 'O' when 2 then 'A' when 3 then 'B' when 4 then 'C' end  as 'level_code', " +
                    //        "'' as 'final_grade', '0' as 'overall_percentage', '' as 'address1' , '' as 'address2', '' as 'address3','' as 'city','' as 'State','' as 'pin', '' as 'remarks'" +
                    //        "from  ModuleCertificateRequest m ,Registration_Detail a join Candidate b on a.Candidate_ID =b.ID left join " +
                    //        "course_registration_application cra  on  cra.Candidate_ID =b.id and cra.id =(select max(cra1.id)  from course_registration_application cra1 where cra1.Candidate_ID  = b.id and cra1.Course_ID =a.Course_ID ) and ((b.Guardian_Name is not null and cra.affidavitNo is not null ) ) where " +
                    //       "  a.Registration_No = m.Registration_No and a.Course_ID ='" + CourseId + "' and m.Batch_ID = '" + Batch + "' and m.Is_Blocked = 0 ";

                    //November_2024
                    para1[0] = new SqlParameter("@CourseId", CourseId);
                    para1[1] = new SqlParameter("@Batch", Batch);
                    sql = "select '' certificate_no, a.Registration_No as 'registration_no', b.Name as 'Name'," +
                           "b.Father_Name as 'f_name', b.Mother_Name as 'm_name', b.Guardian_Name as 'g_Name'," +
                           "cra.affidavitNo as 'affidavit_srno', cra.affidavitDate as 'affidavit_date', b.Dob as 'd_o_b'," +
                           "'0' as 'phase_no', cast(GETDATE() as date) as 'phase_generation_date','January' as 'phase_month','1900' as 'phase_year'," +
                           "case a.Course_ID  when 1 then 'O' when 2 then 'A' when 3 then 'B' when 4 then 'C' end  as 'level_code', " +
                           "'' as 'final_grade', '0' as 'overall_percentage', '' as 'address1' , '' as 'address2', '' as 'address3','' as 'city','' as 'State','' as 'pin', '' as 'remarks'" +
                           "from  ModuleCertificateRequest m ,Registration_Detail a join Candidate b on a.Candidate_ID =b.ID left join " +
                           "course_registration_application cra  on  cra.Candidate_ID =b.id and cra.id =(select max(cra1.id)  from course_registration_application cra1 where cra1.Candidate_ID  = b.id and cra1.Course_ID =a.Course_ID ) and ((b.Guardian_Name is not null and cra.affidavitNo is not null ) ) where " +
                          "  a.Registration_No = m.Registration_No and a.Course_ID =@CourseId and m.Batch_ID = @Batch and m.Is_Blocked = 0 ";

                }

                //dtTbl = DbUtility.GetDataTable(sql, new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
                //November_2024
                dtTbl = DbUtility.GetDataTable(sql, new EConnect.Connections.SqlCon(), para1, CommandType.Text, false);
                if (dtTbl.Rows.Count > 0)
                {
                    if (ddlDataFile.SelectedValue == "1")
                    {
                        for (int i = 0; i < dtTbl.Rows.Count; i++)
                        {
                            sqlStr = " insert into [MS Access;Database=" + mdbFilePath + "].[CERT] (registration_no, level_code, module_code, module_desc, hours, credits, passing_month, " +
                                     " passing_year, grade, module_remarks, whether_module_certificate";
                            sqlStr += "      ) " +
                                            " values('" + dtTbl.Rows[i]["registration_no"] + "', '" + dtTbl.Rows[i]["level_code"].ToString() + "', '" + dtTbl.Rows[i]["module_code"].ToString().Replace("'", "''") + "', '"
                                            + dtTbl.Rows[i]["module_desc"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["hours"].ToString() + "', '" + dtTbl.Rows[i]["credits"].ToString() + "', '" + dtTbl.Rows[i]["passing_month"].ToString().Replace("'", "''") + "', '"
                                            + dtTbl.Rows[i]["passing_year"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["grade"] + "','"
                                            + dtTbl.Rows[i]["module_remarks"].ToString() + "','" + dtTbl.Rows[i]["whether_module_certificate"].ToString() + "'";
                            sqlStr += ")";

                            command = new OleDbCommand(sqlStr, connection);
                            command.ExecuteNonQuery();
                        }
                    }
                    else if (ddlDataFile.SelectedValue == "2")
                    {
                        for (int i = 0; i < dtTbl.Rows.Count; i++)
                        {
                            sqlStr = " insert into [MS Access;Database=" + mdbFilePath + "].[CERT] (certificate_no, registration_no, Name, f_name, m_name, g_name, " +
                                     " affidavit_srno, affidavit_date, d_o_b, phase_no, phase_generation_date, phase_month, phase_year, level_code, final_grade, " +
                                     " overall_percentage, address1, address2, address3, city, State, pin, remarks";


                            int check_affidavit_date_null = dtTbl.Rows[i]["affidavit_date"].ToString().Length ;

                            if (check_affidavit_date_null >= 1)
                            {

                                sqlStr += "      ) " +
                                                " values('" + dtTbl.Rows[i]["certificate_no"] + "', '" + dtTbl.Rows[i]["registration_no"] + "', '" + dtTbl.Rows[i]["Name"].ToString().Replace("'", "''") + "', '"
                                                + dtTbl.Rows[i]["f_name"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["m_name"].ToString().Replace("'", "''") + "', '"
                                                + dtTbl.Rows[i]["g_name"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["affidavit_srno"].ToString() + "', '"
                                                + dtTbl.Rows[i]["affidavit_date"].ToString() + "', #" + dtTbl.Rows[i]["d_o_b"].ToString() + "#,'" + dtTbl.Rows[i]["phase_no"] + "','" + dtTbl.Rows[i]["phase_generation_date"].ToString() + "','"
                                                + dtTbl.Rows[i]["phase_month"].ToString() + "','" + dtTbl.Rows[i]["phase_year"].ToString() + "','" + dtTbl.Rows[i]["level_code"].ToString() + "','" + dtTbl.Rows[i]["final_grade"].ToString() + "','"
                                                + dtTbl.Rows[i]["overall_percentage"].ToString() + "','" + dtTbl.Rows[i]["address1"].ToString() + "','" + dtTbl.Rows[i]["address2"].ToString() + "','"
                                                + dtTbl.Rows[i]["address3"].ToString() + "','" + dtTbl.Rows[i]["city"].ToString() + "','" + dtTbl.Rows[i]["State"].ToString() + "','" + dtTbl.Rows[i]["pin"].ToString() + "','" + dtTbl.Rows[i]["remarks"].ToString() + "'";
                                sqlStr += ")";

                            }
                            else
                            {

                                sqlStr += "      ) " +
                                " values('" + dtTbl.Rows[i]["certificate_no"] + "', '" + dtTbl.Rows[i]["registration_no"] + "', '" + dtTbl.Rows[i]["Name"].ToString().Replace("'", "''") + "', '"
                                + dtTbl.Rows[i]["f_name"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["m_name"].ToString().Replace("'", "''") + "', '"
                                + dtTbl.Rows[i]["g_name"].ToString().Replace("'", "''") + "', '" + dtTbl.Rows[i]["affidavit_srno"].ToString() + "',null "

                                + ", #" + dtTbl.Rows[i]["d_o_b"].ToString() + "#,'" + dtTbl.Rows[i]["phase_no"] + "','" + dtTbl.Rows[i]["phase_generation_date"].ToString() + "','"

                                + dtTbl.Rows[i]["phase_month"].ToString() + "','" + dtTbl.Rows[i]["phase_year"].ToString() + "','" + dtTbl.Rows[i]["level_code"].ToString() + "','" + dtTbl.Rows[i]["final_grade"].ToString() + "','"
                                + dtTbl.Rows[i]["overall_percentage"].ToString() + "','" + dtTbl.Rows[i]["address1"].ToString() + "','" + dtTbl.Rows[i]["address2"].ToString() + "','"
                                + dtTbl.Rows[i]["address3"].ToString() + "','" + dtTbl.Rows[i]["city"].ToString() + "','" + dtTbl.Rows[i]["State"].ToString() + "','" + dtTbl.Rows[i]["pin"].ToString() + "','" + dtTbl.Rows[i]["remarks"].ToString() + "'";
                                sqlStr += ")";
                            }


                            command = new OleDbCommand(sqlStr, connection);
                            command.ExecuteNonQuery();
                        }
                    }
                    command.Dispose();
                    connection.Close();
                    connection.Dispose();
                    Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                    Response.ContentType = "application/octet-stream";
                    Response.Charset = "UTF-8";
                    Response.WriteFile(mdbFilePath);
                }
            }
            else if (ddlDataFile.SelectedValue == "3")
            {              
                Int32 Batch = Convert.ToInt32(ddlBatchNumber.SelectedValue);

                //string sqlphotoId = "select distinct uf.Uploaded_File as 'Photo' ,mcr.Registration_No as 'RegistrationNo' from Uploaded_File uf, Regn_I_Card_Candidate_Photos ricp, ModuleCertificateRequest mcr where mcr.Registration_No =ricp.Registration_No and ricp.Photo_uploaded_file_Id =uf.ID and mcr.Batch_ID = '" + Batch + "' ";

                //November_2024
                SqlParameter[] para1 = { new SqlParameter("@Batch", Batch) };
                string sqlphotoId = "select distinct uf.Uploaded_File as 'Photo' ,mcr.Registration_No as 'RegistrationNo' from Uploaded_File uf, Regn_I_Card_Candidate_Photos ricp, ModuleCertificateRequest mcr where mcr.Registration_No =ricp.Registration_No and ricp.Photo_uploaded_file_Id =uf.ID and mcr.Batch_ID = @Batch ";

                //dtTbl = DbUtility.GetDataTable(sqlphotoId, new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
                //November_2024
                dtTbl = DbUtility.GetDataTable(sqlphotoId, new EConnect.Connections.SqlCon(), para1, CommandType.Text, false);

                String directoryName = FileTypeShortName + "_" + BatchID + "_" + courseCode + "_" + DateTime.Now.ToString("ddMMyyyyHHmm");
                System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName));
                string photoName = "";
                foreach (var photo in dtTbl.AsEnumerable())
                {
                    photoName = photo.Field<Int64>("RegistrationNo").ToString();
                    FileStream fs = File.Create(Server.MapPath("~/Download/" + directoryName + "/" + photoName + ".jpg"));
                    BinaryWriter bw = new BinaryWriter(fs);
                    bw.Write(photo.Field<byte[]>("Photo"));
                    bw.Close();
                    fs.Close();
                }
                using (ZipFile zipFile = new ZipFile())
                {
                    zipFile.AddDirectory(Server.MapPath("~/Download/" + directoryName));
                    Response.Clear();
                    Response.ContentType = "application/zip";
                    Response.AddHeader("content-disposition", "filename=" + directoryName + ".zip");
                    zipFile.Save(Response.OutputStream);
                }
                System.IO.Directory.Delete(Server.MapPath("~/Download/" + directoryName), true);
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message, true);
            if(connection .State ==ConnectionState .Open )
                    connection.Close();
        }
        finally { context.Dispose(); }
    }
    protected void btnProcess_Click(object sender, EventArgs e)
    {
        using (var context = new EConnectContext())
        {
            Int32 CourseId = 0;

            for (int i = 0; i < gbapplicant.Rows.Count; i++)
            {
                CheckBox cbx = (CheckBox)gbapplicant.Rows[i].FindControl("chkchild");
                if (cbx != null)
                {
                    if (cbx.Checked)
                    {
                        CourseId = Convert.ToInt32(gbapplicant.DataKeys[i].Value);
                        if (CourseId != 0)
                            ModuleBatchAssignment(CourseId);
                    }
                }
            }
            BindNewRequest();
        }
    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gbapplicant.PageIndex = PagingBar1.CurrentPageIndex;
            BindNewRequest();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void gbapplicant_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();
            }
        }
        catch (Exception ex) { ShowAlert(ex.Message); }
    }
}