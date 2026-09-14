using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.IO;
using Ionic.Zip;
using System.Data.Objects;
using System.Text;
using System.Web.UI;
using System.Data;
using System.Data.OleDb;
using EConnect.Utils.Data;
using System.Data.SqlClient;
using System.Configuration;


public partial class DownloadDataForCertificate : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        Lblerror.Text = "";
        Lblerror.Visible = false;
        lblNoRecord.Text = "";
        lblNoRecord.Visible = false;

        try
        {
            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Index.aspx");
            //currentRoleId = Convert.ToInt32(Session["RoleID"]);
            //loginUserNo = Convert.ToInt32(Session["UserID"]);
            //if (!UserManager.HasRight(currentRoleId, enmRight.View))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}
            //loginUserType = (UserType)Session["UserType"];
            //entityID = Convert.ToInt64(Session["EntityID"]);
            //if (loginUserType == UserType.RegionalCenter || loginUserType == UserType.HeadOffice || loginUserType == UserType.Admin || loginUserType == UserType.ExternalAdmin)
            //{
                if (!IsPostBack)
                {
                    BindCourseCategory();
                    ddlCourseCategry.SelectedValue = "6";
                    BindDataDownloadedSequence();
                    //  BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Photographs For Certificate", "Admin/DownloadImagesForCertificate.aspx", ""));

                    lblNoRecord.Text = "";
                    lblNoRecord.Visible = false;
                }
            //}
            //else
            //{
            //    //  BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Photographs For Certificate", "#", ""));
            //    //  BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Photographs For Certificate", "Admin/DownloadImagesForCertificate.aspx", ""));
            //    btnView.Visible = false;
            //    btnReset.Visible = false;
            //    Lblerror.Text = "You can not download candidate photographs.";
            //    Lblerror.Visible = true;
            //}
            // BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            Lblerror.Text = ex.Message;
            Lblerror.Visible = true;
        }
    }
    protected void BindCourseCategory()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var CourseList = from p in context.CourseCategories
                                 where p.ID == 6
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategry, CourseList, lst);
                //ddlCourseCategry.SelectedValue = "6";
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    protected void ddlCourseCategry_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //BreadCrumb1.Render();
            //lblNoRecord.Text = "";
            //int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            //using (var context = new EConnectContext())
            //{
            //    ListItem lst = new ListItem("--Select One--", "0");
            //    if (courseCatId > 0)
            //    {

            //        var DataDownloadedSequence = from p in context.NSQFCertPhasePrintDetails
            //                                     join c in context.CourseMappingWithNSQFCoursecodes on p.course_code equals c.NSQFCourseCode
            //                                     join k in context.Courses on c.CourseID equals k.ID
            //                                     where k.CourseCategoryID == courseCatId && p.data_downloaded_sequence != null
            //                                     orderby (p.data_downloaded_sequence)
            //                                     select new { ValueField = p.data_downloaded_sequence, TextField = p.data_downloaded_sequence };

            //        DataDownloadedSequence= DataDownloadedSequence.Distinct().Take(20);
            //        DataDownloadedSequence = DataDownloadedSequence.Distinct().OrderByDescending(a=>a.TextField);
            //        EConnect.Utils.Common.ControlUtility.BindListObject(ddlDataDownloadedSequence, DataDownloadedSequence, lst);
            //    }
            //    else
            //    {
            //        ddlDataDownloadedSequence.Items.Clear();
            //        ddlDataDownloadedSequence.Items.Insert(0, lst);
            //    }
            //    ddlDataDownloadedSequence.SelectedValue = "0";
            //    ddlDataDownloadedSequence_SelectedIndexChanged(ddlDataDownloadedSequence, EventArgs.Empty);
            //};

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void ddlDataDownloadedSequence_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            lblNoRecord.Text = "";
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            int DataDownloadSequenceId = Convert.ToInt32(ddlDataDownloadedSequence.SelectedValue);
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("-- All --", "0");
                if (courseCatId > 0 && DataDownloadSequenceId > 0)
                {
                    var courses = from p in context.NSQFCertPhasePrintDetails
                                  join c in context.CourseMappingWithNSQFCoursecodes on p.course_code equals c.NSQFCourseCode
                                  join k in context.Courses on c.CourseID equals k.ID
                                  where k.CourseCategoryID == courseCatId && p.data_downloaded_sequence != null
                                  && p.data_downloaded_sequence == DataDownloadSequenceId
                                  select new { ValueField = c.CourseID, TextField = k.Name + " (" + c.NSQFCourseCode + ")" };
                    courses = courses.Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
                }
                else
                {
                    ddlCourseName.Items.Clear();
                    ddlCourseName.Items.Insert(0, lst);
                }
                ddlCourseName.SelectedValue = "0";
                ddlCourseName_SelectedIndexChanged(ddlCourseName, EventArgs.Empty);
            };
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
            BreadCrumb1.Render();
            ddlPhaseNumber.Items.Clear();
            lblNoRecord.Text = "";
            Int32 cid = Convert.ToInt32(ddlCourseName.SelectedValue);
            int DataDownloadSequenceId = Convert.ToInt32(ddlDataDownloadedSequence.SelectedValue);
            ListItem lst = new ListItem("-- All --", "0");
            using (EConnectContext context = new EConnectContext())
            {
                if (cid != null)
                {
                    var PhaseNumberList = from p in context.NSQFCertPhasePrintDetails
                                          join k in context.CourseMappingWithNSQFCoursecodes on p.course_code equals k.NSQFCourseCode
                                          where k.CourseID == cid && p.data_downloaded_sequence != null
                                          && p.data_downloaded_sequence == DataDownloadSequenceId
                                          select new { ValueField = p.phase, TextField = p.phase };
                    PhaseNumberList = PhaseNumberList.Distinct();
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlPhaseNumber, PhaseNumberList, lst);
                }
                else
                {
                    ddlPhaseNumber.Items.Clear();
                    ddlPhaseNumber.Items.Insert(0, lst);
                }
                ddlPhaseNumber.SelectedValue = "0";
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void BindDataDownloadedSequence()
    {
        try
        {
            int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
            BreadCrumb1.Render();
            lblNoRecord.Text = "";
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                if (courseCatId > 0)
                {

                    var DataDownloadedSequence = from p in context.NSQFCertPhasePrintDetails
                                                 where p.phase > 10 && p.data_downloaded_sequence != null
                                                 orderby p.phase, p.course_code, p.registration_no
                                                 select new { ValueField = p.data_downloaded_sequence, TextField = p.data_downloaded_sequence };

                    // DataDownloadedSequence = DataDownloadedSequence.Distinct().Take(20);
                    DataDownloadedSequence = DataDownloadedSequence.Distinct().OrderByDescending(a => a.TextField);
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlDataDownloadedSequence, DataDownloadedSequence, lst);
                }
                else
                {
                    ddlDataDownloadedSequence.Items.Clear();
                    ddlDataDownloadedSequence.Items.Insert(0, lst);
                }
                ddlDataDownloadedSequence.SelectedValue = "0";
                ddlDataDownloadedSequence_SelectedIndexChanged(ddlDataDownloadedSequence, EventArgs.Empty);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ddlCourseCategry.SelectedValue = "0";
            ddlDataDownloadedSequence.SelectedValue = "0";
            ddlCourseName.SelectedValue = "0";
            ddlPhaseNumber.SelectedValue = "0";

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {

        OleDbConnection connection = new OleDbConnection();
        OleDbCommand command = new OleDbCommand();
        string mdbFilePath = "";
        String filename = "";

        try
        {
            using (EConnectContext context = new EConnectContext())
            {

                int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);//6
                int DataDownloadSequenceId = Convert.ToInt32(ddlDataDownloadedSequence.SelectedValue);
                Int32 courseID = Convert.ToInt32(ddlCourseName.SelectedValue);
                int PhaseNumber = Convert.ToInt32(ddlPhaseNumber.SelectedValue);
                int CertificateType = Convert.ToInt32(ddlCertType.SelectedValue);
                //--Course currentCourse = context.Courses.Find(courseID);
                String SqlStrSub = "";

                if (CertificateType == 0 || CertificateType == 2)
                {

                    filename = "Phase_Detail" + "_" + DataDownloadSequenceId + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".accdb";

                    mdbFilePath = Server.MapPath("~/Download/" + filename);
                    if (System.IO.File.Exists(mdbFilePath))
                        System.IO.File.Delete(mdbFilePath);

                    //System.IO.File.Copy(Server.MapPath("~/Download/nsqf_Certificate_file_format.accdb"), mdbFilePath);
                    System.IO.File.Copy(Server.MapPath("~/Download/nsqf_Certificate_file_format.mdb"), mdbFilePath);

                    //string connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mdbFilePath + ";Persist Security Info=False;";
                    string connect = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbFilePath + ";Persist Security Info=False;";
                    connection.ConnectionString = connect;
                    connection.Open();
                    // command = new OleDbCommand("delete from  [MS Access;Database=" + mdbFilePath + "].[CERT]", connection);
                    command = new OleDbCommand("delete from  [CERT]", connection);
                    command.Parameters.AddWithValue("@mdbFilePath", mdbFilePath);
                }

                if (CertificateType == 1)
                {

                    //filename = "Certificate_Detail" + "_" + DataDownloadSequenceId + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".accdb";

                    filename = "Certificate_Detail" + "_" + DataDownloadSequenceId + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".mdb";

                    mdbFilePath = Server.MapPath("~/Download/" + filename);
                    if (System.IO.File.Exists(mdbFilePath))
                        System.IO.File.Delete(mdbFilePath);

                    //System.IO.File.Copy(Server.MapPath("~/Download/Certificate_Detail_NCVET.accdb"), mdbFilePath);
                    System.IO.File.Copy(Server.MapPath("~/Download/Certificate_Detail_NCVET.mdb"), mdbFilePath);

                   // string connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mdbFilePath + ";Persist Security Info=False;";
                    string connect = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mdbFilePath + ";Persist Security Info=False;";
                    connection.ConnectionString = connect;
                    connection.Open();
                    command = new OleDbCommand("delete from [CERT]", connection);
                    command.Parameters.AddWithValue("@mdbFilePath", mdbFilePath);
                }

                command.ExecuteNonQuery();
                command.Dispose();
                
                String sqlStr = "";
                String sql = "";



                if (DataDownloadSequenceId != 0 && courseID != 0 && PhaseNumber != 0 && (CertificateType == 2 || CertificateType == 0))
                {
                    DataTable dtTb2 = new DataTable();

                    /*     sql = " select application_no, registration_no, rollno,p.name,f_name,m_name,G_NAME g_name,dob d_o_b,inst_name, [grade_code] grade," +
                                 " case exam_month when 1 then 'JANUARY'when 2 then 'FEBRUARY'when 3 then 'MARCH'when 4 then 'APRIL'when 5 then 'MAY'when 6 then 'JUNE'when 7 then 'JULY'when 8 then 'AUGUST'when 9 then 'SEPTEMBER'when 10 then 'OCTOBER'when 11 then 'NOVEMBER'when 12 then 'DECEMBER' end as exam_month ," +
                                 "  exam_year,exam_type,[course_code] coursename,Aadhaar,Aadhaar_Enrl,grade_legends_desc,Remarks,regional_centre," +
                                 " affidavit_srno,affidavit_date,pan_no,tp_name,'' as sponsor,address1,address2,address3,address_city,address_state,address_pin," +
                                 " '' as course_start_on ,course_end_on  from NSQF_Cert_Phase_Print_Detail p,  CourseMappingWithNSQFCoursecode c, Course k where p.phase > 10 and " +
                                 " p.course_code = c.NSQFCourseCode and c.CourseID = k.ID and  p.data_downloaded_SEQUENCE = '" + DataDownloadSequenceId + "' and c.CourseID = '" + courseID + "'and p.phase ='" + PhaseNumber + "'" +
                                 " and (select top 1 d.credits from NIELIT.dbo.CourseLevelDuration d where Course_ID =c.CourseID   " +
                                 " and (( SELECT top 1  MAX(Commencement_From_Date) FROM Registration_detail  ) between Effective_From_Date and " +
                                 " isnull(Effective_To_Date,CURRENT_TIMESTAMP))) is null and Credits  is  null  ";*/

                    SqlParameter[] parameters = new SqlParameter[3];

                    // Assign the parameters, ensuring compatibility with older .NET versions
                    parameters[0] = new SqlParameter("@DataDownloadedSeq", SqlDbType.Int);
                    if (DataDownloadSequenceId != null)
                    {
                        parameters[0].Value = DataDownloadSequenceId;
                    }
                    else
                    {
                        parameters[0].Value = DBNull.Value;
                    }

                    parameters[1] = new SqlParameter("@courseId", SqlDbType.Int);
                    if (courseID != null)
                    {
                        parameters[1].Value = courseID;
                    }
                    else
                    {
                        parameters[1].Value = DBNull.Value;
                    }

                    parameters[2] = new SqlParameter("@phaseNo", SqlDbType.Int);
                    if (PhaseNumber != null)
                    {
                        parameters[2].Value = PhaseNumber;
                    }
                    else
                    {
                        parameters[2].Value = DBNull.Value;
                    }

                    dtTb2 = DbUtility.GetDataTable("NSQFCertificateDataNonNCVET", new EConnect.Connections.SqlCon(), parameters, CommandType.StoredProcedure, false);
                    if (dtTb2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtTb2.Rows.Count; i++)
                        {

                            if (dtTb2.Rows[i]["affidavit_date"].ToString().Trim().Length == 0)
                            {
                                SqlStrSub = "NULL,'";
                            }
                            else
                            {
                                SqlStrSub = "'" + dtTb2.Rows[i]["affidavit_date"] + "','";
                            }

                            string encryptedApaarID = dtTb2.Rows[i]["apaarID"].ToString();
                            string decryptedApaarID = EncryptDecrypt.DecryptString(encryptedApaarID);
                            /*  sqlStr = " insert into [MS Access;Database=" + mdbFilePath + "].[CERT] (application_no, registration_no,rollno,name,f_name,m_name,g_name,d_o_b,inst_name,grade,exam_month,exam_year,exam_type,coursename,Aadhaar,Aadhaar_Enrl,grade_legends_desc,Remarks,regional_centre,affidavit_srno,affidavit_date,pan_no,tp_name,sponsor,address1,address2,address3,address_city,address_state,address_pin,course_end_on";
                              sqlStr += "      ) " +
                                              " values('" + dtTb2.Rows[i]["application_no"] + "', '" + dtTb2.Rows[i]["registration_no"] + "', '" + dtTb2.Rows[i]["rollno"] + "','" + dtTb2.Rows[i]["name"].ToString().Replace("'", "''") + "','" + dtTb2.Rows[i]["f_name"].ToString().Replace("'", "''") + "','" + dtTb2.Rows[i]["m_name"].ToString().Replace("'", "''") + "','" + dtTb2.Rows[i]["g_name"].ToString().Replace("'", "''") + "',#" +
                                              dtTb2.Rows[i]["d_o_b"].ToString() + "#, '" + dtTb2.Rows[i]["inst_name"].ToString().Replace("'", "''") + "', '" + dtTb2.Rows[i]["grade"] + "','" + dtTb2.Rows[i]["exam_month"].ToString() + "', '" + dtTb2.Rows[i]["exam_year"].ToString() + "', '" + dtTb2.Rows[i]["exam_type"].ToString() + "', '" + dtTb2.Rows[i]["coursename"].ToString() + "','" + dtTb2.Rows[i]["Aadhaar"].ToString() + "','" +
                                              dtTb2.Rows[i]["Aadhaar_Enrl"].ToString() + "','" + dtTb2.Rows[i]["grade_legends_desc"].ToString() + "','" + dtTb2.Rows[i]["Remarks"].ToString() + "','" + dtTb2.Rows[i]["regional_centre"].ToString() + "','" + dtTb2.Rows[i]["affidavit_srno"].ToString() + "'," + SqlStrSub + dtTb2.Rows[i]["pan_no"].ToString() + "','" + dtTb2.Rows[i]["tp_name"].ToString() +
                                              "','" + dtTb2.Rows[i]["sponsor"].ToString() + "','" + dtTb2.Rows[i]["address1"].ToString().Replace("'", "''") + "','" + dtTb2.Rows[i]["address2"].ToString().Replace("'", "''") + "','" + dtTb2.Rows[i]["address3"].ToString().Replace("'", "''") + "','" + dtTb2.Rows[i]["address_city"].ToString().Replace("'", "''") + "','" + dtTb2.Rows[i]["address_state"] + "','" + dtTb2.Rows[i]["address_pin"] + "',#" +
                                              dtTb2.Rows[i]["course_end_on"].ToString() + "#    ";
                              sqlStr += ")";*/

                            sqlStr = " insert into CERT (application_no, registration_no,rollno,name,f_name,m_name,g_name,d_o_b,inst_name,grade,exam_month,exam_year,exam_type,coursename,Aadhaar,Aadhaar_Enrl,grade_legends_desc,Remarks,regional_centre,affidavit_srno,affidavit_date,pan_no,tp_name,sponsor,address1,address2,address3,address_city,address_state,address_pin,course_end_on,apaarId";
                            sqlStr += "      ) " +
                                            " values(@application_no, @registration_no, @rollno,@name,@f_name,@m_name,@g_name,@d_o_b, @inst_name,@grade,@exam_month,@exam_year,@exam_type,@coursename,@Aadhaar," +
                                            " @Aadhaar_Enrl,@grade_legends_desc,@Remarks,@regional_centre,@affidavit_srno,@affidavit_date,@pan_no,@tp_name,@sponsor,@address1,@address2,@address3" + ",@address_city,@address_state,@address_pin,@course_end_on, @decryptedApaarID  ";
                            sqlStr += ")";
                            command = new OleDbCommand(sqlStr, connection);

                            command.Parameters.Clear();
                            // command.Parameters.AddWithValue("@mdbFilePath", mdbFilePath1);
                            command.Parameters.AddWithValue("@application_no", dtTb2.Rows[i]["application_no"]);
                            command.Parameters.AddWithValue("@registration_no", dtTb2.Rows[i]["registration_no"]);
                            command.Parameters.AddWithValue("@rollno", dtTb2.Rows[i]["rollno"]);
                            command.Parameters.AddWithValue("@name", dtTb2.Rows[i]["name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@f_name", dtTb2.Rows[i]["f_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@m_name", dtTb2.Rows[i]["m_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@g_name", dtTb2.Rows[i]["g_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@d_o_b", Convert.ToDateTime(dtTb2.Rows[i]["d_o_b"].ToString()));
                            command.Parameters.AddWithValue("@inst_name", dtTb2.Rows[i]["inst_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@grade", dtTb2.Rows[i]["grade"]);
                            command.Parameters.AddWithValue("@exam_month", dtTb2.Rows[i]["exam_month"].ToString());
                            command.Parameters.AddWithValue("@exam_year", dtTb2.Rows[i]["exam_year"].ToString());
                            command.Parameters.AddWithValue("@exam_type", dtTb2.Rows[i]["exam_type"].ToString());
                            command.Parameters.AddWithValue("@coursename", dtTb2.Rows[i]["coursename"].ToString());
                            command.Parameters.AddWithValue("@Aadhaar", dtTb2.Rows[i]["Aadhaar"].ToString());
                            command.Parameters.AddWithValue("@Aadhaar_Enrl", dtTb2.Rows[i]["Aadhaar_Enrl"].ToString());
                            command.Parameters.AddWithValue("@grade_legends_desc", dtTb2.Rows[i]["grade_legends_desc"].ToString());
                            command.Parameters.AddWithValue("@Remarks", dtTb2.Rows[i]["Remarks"].ToString());
                            command.Parameters.AddWithValue("@regional_centre", dtTb2.Rows[i]["regional_centre"].ToString());
                            command.Parameters.AddWithValue("@affidavit_srno", dtTb2.Rows[i]["affidavit_srno"].ToString());
                            command.Parameters.AddWithValue("@affidavit_date", SqlStrSub == "NULL" ? (object)DBNull.Value : dtTb2.Rows[i]["affidavit_date"]); // Handle NULL condition);
                            command.Parameters.AddWithValue("@pan_no", dtTb2.Rows[i]["pan_no"].ToString());
                            command.Parameters.AddWithValue("@tp_name", dtTb2.Rows[i]["tp_name"].ToString());
                            command.Parameters.AddWithValue("@sponsor", dtTb2.Rows[i]["sponsor"].ToString());
                            command.Parameters.AddWithValue("@address1", dtTb2.Rows[i]["address1"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@address2", dtTb2.Rows[i]["address2"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@address3", dtTb2.Rows[i]["address3"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@address_city", dtTb2.Rows[i]["address_city"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@address_state", dtTb2.Rows[i]["address_state"]);
                            command.Parameters.AddWithValue("@address_pin", dtTb2.Rows[i]["address_pin"]);
                            command.Parameters.AddWithValue("@course_end_on", Convert.ToDateTime(dtTb2.Rows[i]["course_end_on"].ToString()));
                            command.Parameters.AddWithValue("@decryptedApaarID", decryptedApaarID);
                            command.ExecuteNonQuery();

                        }
                    }
                }

                if (DataDownloadSequenceId != 0 && courseID != 0 && PhaseNumber != 0 && CertificateType == 1)
                {
                    DataTable dtTb3 = new DataTable();
                  
                    SqlParameter[] parameters = new SqlParameter[3];
                    
                    parameters[0] = new SqlParameter("@DataDownloadedSeq", SqlDbType.Int);
                    if (DataDownloadSequenceId != null)
                    {
                        parameters[0].Value = DataDownloadSequenceId;
                    }
                    else
                    {
                        parameters[0].Value = DBNull.Value;
                    }

                    parameters[1] = new SqlParameter("@courseId", SqlDbType.Int);
                    if (courseID != null)
                    {
                        parameters[1].Value = courseID;
                    }
                    else
                    {
                        parameters[1].Value = DBNull.Value;
                    }

                    parameters[2] = new SqlParameter("@phaseNo", SqlDbType.Int);
                    if (PhaseNumber != null)
                    {
                        parameters[2].Value = PhaseNumber;
                    }
                    else
                    {
                        parameters[2].Value = DBNull.Value;
                    }


                    dtTb3 = DbUtility.GetDataTable("NSQFCertificateDataNCVET", new EConnect.Connections.SqlCon(), parameters, CommandType.StoredProcedure, false);
                    if (dtTb3.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtTb3.Rows.Count; i++)
                        {

                            //SqlStrSub = " cast ((CASE LEN(LTRIM(RTRIM('" + dtTb3.Rows[i]["affidavit_date"].ToString().Replace("'", "''") + "'))) WHEN 0 THEN NULL ELSE '" + dtTb3.Rows[i]["affidavit_date"].ToString().Replace("'", "''") + "' END) as date) ";

                            string affidavitDateValue = dtTb3.Rows[i]["affidavit_date"].ToString().Trim().Length == 0 ? "NULL" : "#" + Convert.ToDateTime(dtTb3.Rows[i]["affidavit_date"]).ToString("MM/dd/yyyy") + "#";

                            string encryptedApaarID = dtTb3.Rows[i]["apaarID"].ToString();
                            string decryptedApaarID = EncryptDecrypt.DecryptString(encryptedApaarID);

                            sqlStr = "INSERT INTO [CERT] " +
                                           "(NCVET_sr_no, registration_no, rollno, exam_type, course_type, coursename, exam_month, exam_year, name, d_o_b, f_name, m_name, g_name, affidavit_srno, affidavit_date, " +
                                           "phase, phase_generation_date, course_code, Final_grade_for_qualification, overall_percentage, application_no, course_duration_for_qualification_in_hours, NSQF_Framwork_Level_for_qualification, NCVETQualCode, " +
                                           "sectorShortCode, tp_name, inst_address, inst_name, Traning_center_name_district, Traning_center_name_state, sponsor, regional_centre, grade_legends_desc, Place_of_issue, Remarks, credits,apaarId) " +
                                           "VALUES (@NCVET_sr_no, @registration_no, @rollno, @exam_type, @course_type, @coursename,@exam_month',@exam_year,@name, @d_o_b, " +
                                           "@f_name, @m_name,@g_name,@affidavit_srno,@affidavit_date,@phase,@phase_generation_date,@course_code,@Final_grade_for_qualification" +
                                           ",@overall_percentage,@application_no, @course_duration_for_qualification_in_hours, @NSQF_Framwork_Level_for_qualification, @NCVETQualCode" + "@sectorShortCode,@tp_name,@inst_address,@inst_name,@Traning_center_name_district,@Traning_center_name_state,@sponsor,@regional_centre,@grade_legends_desc, 'New Delhi',"+ "@Remarks,@credits,@decryptedApaarID );";


                            command = new OleDbCommand(sqlStr, connection);
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@NCVET_sr_no", dtTb3.Rows[i]["NCVET_sr_no"]);
                            command.Parameters.AddWithValue("@registration_no", dtTb3.Rows[i]["registration_no"]);
                            command.Parameters.AddWithValue("@rollno", dtTb3.Rows[i]["rollno"]);
                            command.Parameters.AddWithValue("@exam_type", dtTb3.Rows[i]["exam_type"]);
                            command.Parameters.AddWithValue("@course_type", dtTb3.Rows[i]["course_type"]);
                            command.Parameters.AddWithValue("@coursename", dtTb3.Rows[i]["coursename"]);
                            command.Parameters.AddWithValue("@exam_month", dtTb3.Rows[i]["exam_month"]);
                            command.Parameters.AddWithValue("@exam_year", dtTb3.Rows[i]["exam_year"]);
                            command.Parameters.AddWithValue("@name", dtTb3.Rows[i]["name"]);
                            command.Parameters.AddWithValue("@d_o_b",Convert.ToDateTime(dtTb3.Rows[i]["d_o_b"]));
                            command.Parameters.AddWithValue("@f_name", dtTb3.Rows[i]["f_name"]);
                            command.Parameters.AddWithValue("@m_name", dtTb3.Rows[i]["m_name"]);
                            command.Parameters.AddWithValue("@g_name", dtTb3.Rows[i]["g_name"]);
                            command.Parameters.AddWithValue("@affidavit_srno", dtTb3.Rows[i]["affidavit_srno"]);
                            command.Parameters.AddWithValue("@affidavit_date", SqlStrSub == "NULL" ? (object)DBNull.Value : dtTb3.Rows[i]["affidavit_date"]); // Handle NULL condition);
                            command.Parameters.AddWithValue("@phase", dtTb3.Rows[i]["phase"]);
                            command.Parameters.AddWithValue("@phase_generation_date",Convert.ToDateTime(dtTb3.Rows[i]["phase_generation_date"]));
                            command.Parameters.AddWithValue("@course_code", dtTb3.Rows[i]["course_code"]);
                            command.Parameters.AddWithValue("@Final_grade_for_qualification", dtTb3.Rows[i]["Final_grade_for_qualification"]);
                            command.Parameters.AddWithValue("@overall_percentage", dtTb3.Rows[i]["overall_percentage"]);
                            command.Parameters.AddWithValue("@application_no", dtTb3.Rows[i]["application_no"]);
                            command.Parameters.AddWithValue("@course_duration_for_qualification_in_hours", dtTb3.Rows[i]["course_duration_for_qualification_in_hours"]);
                            command.Parameters.AddWithValue("@NSQF_Framwork_Level_for_qualification", dtTb3.Rows[i]["NSQF_Framwork_Level_for_qualification"]);
                            command.Parameters.AddWithValue("@NCVETQualCode", dtTb3.Rows[i]["NCVETQualCode"]);
                            command.Parameters.AddWithValue("@sectorShortCode", dtTb3.Rows[i]["sectorShortCode"]);
                            command.Parameters.AddWithValue("@tp_name", dtTb3.Rows[i]["tp_name"]);
                            command.Parameters.AddWithValue("@inst_address", dtTb3.Rows[i]["inst_address"]);
                            command.Parameters.AddWithValue("@inst_name", dtTb3.Rows[i]["inst_name"]);
                            command.Parameters.AddWithValue("@Traning_center_name_district", dtTb3.Rows[i]["Traning_center_name_district"]);
                            command.Parameters.AddWithValue("@Traning_center_name_state", dtTb3.Rows[i]["Traning_center_name_state"]);
                            command.Parameters.AddWithValue("@sponsor", dtTb3.Rows[i]["sponsor"]);
                            command.Parameters.AddWithValue("@regional_centre", dtTb3.Rows[i]["regional_centre"]);
                            command.Parameters.AddWithValue("@grade_legends_desc", dtTb3.Rows[i]["grade_legends_desc"]);
                            command.Parameters.AddWithValue("@Remarks", dtTb3.Rows[i]["Remarks"]);
                            command.Parameters.AddWithValue("@credits", dtTb3.Rows[i]["credits"]);
                            command.Parameters.AddWithValue("@Traning_center_name_state", dtTb3.Rows[i]["Traning_center_name_state"]);
                            command.Parameters.AddWithValue("@decryptedApaarID", decryptedApaarID);

                            command.ExecuteNonQuery();
                        }
                    }
                }


              

                if (DataDownloadSequenceId != 0 && courseID != 0 && PhaseNumber == 0 && (CertificateType == 2 || CertificateType == 0))
                {
                    DataTable dtTb2 = new DataTable();

                    /*   sql = " select application_no, registration_no, rollno,p.name,f_name,m_name,G_NAME g_name,dob d_o_b,inst_name, [grade_code] grade," +
                              " case exam_month when 1 then 'JANUARY'when 2 then 'FEBRUARY'when 3 then 'MARCH'when 4 then 'APRIL'when 5 then 'MAY'when 6 then 'JUNE'when 7 then 'JULY'when 8 then 'AUGUST'when 9 then 'SEPTEMBER'when 10 then 'OCTOBER'when 11 then 'NOVEMBER'when 12 then 'DECEMBER' end as exam_month ," +
                              "  exam_year,exam_type,[course_code] coursename,Aadhaar,Aadhaar_Enrl,grade_legends_desc,Remarks,regional_centre," +
                              " affidavit_srno,affidavit_date,pan_no,tp_name,'' as sponsor,address1,address2,address3,address_city,address_state,address_pin," +
                              " '' as course_start_on ,course_end_on  from NSQF_Cert_Phase_Print_Detail p,  CourseMappingWithNSQFCoursecode c, Course k where p.phase > 10 and " +
                              " p.course_code = c.NSQFCourseCode and c.CourseID = k.ID and  p.data_downloaded_SEQUENCE = '" + DataDownloadSequenceId + "' and c.CourseID = '" + courseID + "'" +
                              " and (select top 1 d.credits from NIELIT.dbo.CourseLevelDuration d where Course_ID =c.CourseID   " +
                              "  and (( SELECT top 1  MAX(Commencement_From_Date) FROM Registration_detail  ) between Effective_From_Date and " +
                              " isnull(Effective_To_Date,CURRENT_TIMESTAMP))) is  null and Credits  is  null  ";*/


                    SqlParameter[] parameters = new SqlParameter[3];

                    // Assign the parameters, ensuring compatibility with older .NET versions
                    parameters[0] = new SqlParameter("@DataDownloadedSeq", SqlDbType.Int);
                    if (DataDownloadSequenceId != null)
                    {
                        parameters[0].Value = DataDownloadSequenceId;
                    }
                    else
                    {
                        parameters[0].Value = DBNull.Value;
                    }

                    parameters[1] = new SqlParameter("@courseId", SqlDbType.Int);
                    if (courseID != null)
                    {
                        parameters[1].Value = courseID;
                    }
                    else
                    {
                        parameters[1].Value = DBNull.Value;
                    }

                    parameters[2] = new SqlParameter("@phaseNo", SqlDbType.Int);                    
                    parameters[2].Value = DBNull.Value;
                  

                    // Call the stored procedure and pass the parameters
                    dtTb2 = DbUtility.GetDataTable("NSQFCertificateDataNonNCVET", new EConnect.Connections.SqlCon(), parameters, CommandType.StoredProcedure, false);
                    if (dtTb2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtTb2.Rows.Count; i++)
                        {

                            if (dtTb2.Rows[i]["affidavit_date"].ToString().Trim().Length == 0)
                            {
                                SqlStrSub = "NULL,'";
                            }
                            else
                            {
                                SqlStrSub = "'" + dtTb2.Rows[i]["affidavit_date"] + "','";
                            }


                            string encryptedApaarID = dtTb2.Rows[i]["apaarID"].ToString();
                            string decryptedApaarID = EncryptDecrypt.DecryptString(encryptedApaarID);
                            sqlStr = " insert into [CERT] (application_no, registration_no,rollno,name,f_name,m_name,g_name,d_o_b,inst_name,grade,exam_month,exam_year,exam_type,coursename,Aadhaar,Aadhaar_Enrl,grade_legends_desc,Remarks,regional_centre,affidavit_srno,affidavit_date,pan_no,tp_name,sponsor,address1,address2,address3,address_city,address_state,address_pin,course_end_on,apaarId";
                            sqlStr += "      ) " +
                                            " VALUES (@application_no, @registration_no, @rollno, @name, @f_name, @m_name, @g_name, @d_o_b, @inst_name, @grade, @exam_month, @exam_year, @exam_type, @coursename, " +
         "@Aadhaar, @Aadhaar_Enrl, @grade_legends_desc, @Remarks, @regional_centre, @affidavit_srno, @affidavit_date, @pan_no, @tp_name, @sponsor, @address1, @address2, @address3, " +
         "@address_city, @address_state, @address_pin, @course_end_on, @apaarId);";
                           // sqlStr += ")";
                            command = new OleDbCommand(sqlStr, connection);
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@application_no", dtTb2.Rows[i]["application_no"]);
                            command.Parameters.AddWithValue("@registration_no", dtTb2.Rows[i]["registration_no"]);
                            command.Parameters.AddWithValue("@rollno", dtTb2.Rows[i]["rollno"]);
                            command.Parameters.AddWithValue("@name", dtTb2.Rows[i]["name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@f_name", dtTb2.Rows[i]["f_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@m_name", dtTb2.Rows[i]["m_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@g_name", dtTb2.Rows[i]["g_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@d_o_b", Convert.ToDateTime(dtTb2.Rows[i]["d_o_b"]));
                            command.Parameters.AddWithValue("@inst_name", dtTb2.Rows[i]["inst_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@grade", dtTb2.Rows[i]["grade"]);
                            command.Parameters.AddWithValue("@exam_month", dtTb2.Rows[i]["exam_month"]);
                            command.Parameters.AddWithValue("@exam_year", dtTb2.Rows[i]["exam_year"]);
                            command.Parameters.AddWithValue("@exam_type", dtTb2.Rows[i]["exam_type"]);
                            command.Parameters.AddWithValue("@coursename", dtTb2.Rows[i]["coursename"]);
                            command.Parameters.AddWithValue("@Aadhaar", dtTb2.Rows[i]["Aadhaar"]);
                            command.Parameters.AddWithValue("@Aadhaar_Enrl", dtTb2.Rows[i]["Aadhaar_Enrl"]);
                            command.Parameters.AddWithValue("@grade_legends_desc", dtTb2.Rows[i]["grade_legends_desc"]);
                            command.Parameters.AddWithValue("@Remarks", dtTb2.Rows[i]["Remarks"]);
                            command.Parameters.AddWithValue("@regional_centre", dtTb2.Rows[i]["regional_centre"]);
                            command.Parameters.AddWithValue("@affidavit_srno", dtTb2.Rows[i]["affidavit_srno"]);
                            command.Parameters.AddWithValue("@affidavit_date", SqlStrSub == "NULL" ? (object)DBNull.Value : dtTb2.Rows[i]["affidavit_date"]);
                            command.Parameters.AddWithValue("@pan_no", dtTb2.Rows[i]["pan_no"]);
                            command.Parameters.AddWithValue("@tp_name", dtTb2.Rows[i]["tp_name"]);
                            command.Parameters.AddWithValue("@sponsor", dtTb2.Rows[i]["sponsor"]);
                            command.Parameters.AddWithValue("@address1", dtTb2.Rows[i]["address1"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@address2", dtTb2.Rows[i]["address2"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@address3", dtTb2.Rows[i]["address3"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@address_city", dtTb2.Rows[i]["address_city"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@address_state", dtTb2.Rows[i]["address_state"]);
                            command.Parameters.AddWithValue("@address_pin", dtTb2.Rows[i]["address_pin"]);
                            command.Parameters.AddWithValue("@course_end_on", Convert.ToDateTime(dtTb2.Rows[i]["course_end_on"]));
                            command.Parameters.AddWithValue("@apaarId", decryptedApaarID);
                            command.ExecuteNonQuery();
                        }
                    }
                }

                if (DataDownloadSequenceId != 0 && courseID != 0 && PhaseNumber == 0 && CertificateType == 1)
                {
                    DataTable dtTb3 = new DataTable();

                  

                    /* sql = "select NCVET_sr_no,registration_no,rollno,exam_type, course_type,course_code coursename,exam_month,exam_year,p.name,dob d_o_b, " +
                             "f_name,m_name,G_NAME,affidavit_srno,affidavit_date,phase,phase_date phase_generation_date, course_code, " +
                             "grade_code [Final_grade_for_qualification],percentage overall_percentage,application_no,course_duration_for_qualification_in_hours, " +
                             "[NSQF_Framwork_Level_for_qualification],NCVETQualCode,sectorShortCode,tp_name,inst_address,inst_name,[Traning_center_name_district], " +
                             "[Traning_center_name_state],sponsor,regional_centre,grade_legends_desc, 'New Delhi' [Place_of_issue], Remarks,credits " +
                             "from  NSQF_Cert_Phase_Print_Detail p JOIN  CourseMappingWithNSQFCoursecode c ON p.course_code = c.NSQFCourseCode  JOIN  Course k  ON c.CourseID = k.ID   where " +
                             "phase > 10  AND data_downloaded_SEQUENCE= '" + DataDownloadSequenceId + "' and c.CourseID = '" + courseID + "'  and Credits  is  not  null ";*/

                    SqlParameter[] parameters = new SqlParameter[3];

                    // Assign the parameters, ensuring compatibility with older .NET versions
                    parameters[0] = new SqlParameter("@DataDownloadedSeq", SqlDbType.Int);
                    if (DataDownloadSequenceId != null)
                    {
                        parameters[0].Value = DataDownloadSequenceId;
                    }
                    else
                    {
                        parameters[0].Value = DBNull.Value;
                    }

                    parameters[1] = new SqlParameter("@courseId", SqlDbType.Int);
                    if (courseID != null)
                    {
                        parameters[1].Value = courseID;
                    }
                    else
                    {
                        parameters[1].Value = DBNull.Value;
                    }

                    parameters[2] = new SqlParameter("@phaseNo", SqlDbType.Int);

                    parameters[2].Value = DBNull.Value;


                    // Call the stored procedure and pass the parameters
                    dtTb3 = DbUtility.GetDataTable("NSQFCertificateDataNCVET", new EConnect.Connections.SqlCon(), parameters, CommandType.StoredProcedure, false);

                    if (dtTb3.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtTb3.Rows.Count; i++)
                        {

                            //SqlStrSub = " cast ((CASE LEN(LTRIM(RTRIM('" + dtTb3.Rows[i]["affidavit_date"].ToString().Replace("'", "''") + "'))) WHEN 0 THEN NULL ELSE '" + dtTb3.Rows[i]["affidavit_date"].ToString().Replace("'", "''") + "' END) as date) ";

                            string affidavitDateValue = dtTb3.Rows[i]["affidavit_date"].ToString().Trim().Length == 0 ? "NULL" : "#" + Convert.ToDateTime(dtTb3.Rows[i]["affidavit_date"]).ToString("MM/dd/yyyy") + "#";


                             string encryptedApaarID = dtTb3.Rows[i]["apaarID"].ToString();
                            string decryptedApaarID = EncryptDecrypt.DecryptString(encryptedApaarID);

                            sqlStr = "INSERT INTO [CERT] " +
                                           "(NCVET_sr_no, registration_no, rollno, exam_type, course_type, coursename, exam_month, exam_year, name, d_o_b, f_name, m_name, g_name, affidavit_srno, affidavit_date, " +
                                           "phase, phase_generation_date, course_code, Final_grade_for_qualification, overall_percentage, application_no, course_duration_for_qualification_in_hours, NSQF_Framwork_Level_for_qualification, NCVETQualCode, " +
                                           "sectorShortCode, tp_name, inst_address, inst_name, Traning_center_name_district, Traning_center_name_state, sponsor, regional_centre, grade_legends_desc, Place_of_issue, Remarks, credits,apaarId) " +
                                          "VALUES (@NCVET_sr_no, @registration_no, @rollno, @exam_type, @course_type, @coursename, @exam_month, @exam_year, @name, @d_o_b, @f_name, @m_name, @g_name, @affidavit_srno, @affidavit_date, " +
                "@phase, @phase_generation_date, @course_code, @Final_grade_for_qualification, @overall_percentage, @application_no, @course_duration_for_qualification_in_hours, @NSQF_Framwork_Level_for_qualification, @NCVETQualCode, " +
                "@sectorShortCode, @tp_name, @inst_address, @inst_name, @Traning_center_name_district, @Traning_center_name_state, @sponsor, @regional_centre, @grade_legends_desc, @Place_of_issue, @Remarks, @credits, @apaarId);";

                            command = new OleDbCommand(sqlStr, connection);

                            command.Parameters.AddWithValue("@NCVET_sr_no", dtTb3.Rows[i]["NCVET_sr_no"]);
                            command.Parameters.AddWithValue("@registration_no", dtTb3.Rows[i]["registration_no"]);
                            command.Parameters.AddWithValue("@rollno", dtTb3.Rows[i]["rollno"]);
                            command.Parameters.AddWithValue("@exam_type", dtTb3.Rows[i]["exam_type"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@course_type", dtTb3.Rows[i]["course_type"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@coursename", dtTb3.Rows[i]["coursename"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@exam_month", dtTb3.Rows[i]["exam_month"]);
                            command.Parameters.AddWithValue("@exam_year", dtTb3.Rows[i]["exam_year"]);
                            command.Parameters.AddWithValue("@name", dtTb3.Rows[i]["name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@d_o_b", Convert.ToDateTime(dtTb3.Rows[i]["d_o_b"]));
                            command.Parameters.AddWithValue("@f_name", dtTb3.Rows[i]["f_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@m_name", dtTb3.Rows[i]["m_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@g_name", dtTb3.Rows[i]["g_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@affidavit_srno", dtTb3.Rows[i]["affidavit_srno"]);

                            // Handle affidavit_date with conditional null
                            if (string.IsNullOrEmpty(dtTb3.Rows[i]["affidavit_date"].ToString()))
                                command.Parameters.AddWithValue("@affidavit_date", DBNull.Value);
                            else
                                command.Parameters.AddWithValue("@affidavit_date", Convert.ToDateTime(dtTb3.Rows[i]["affidavit_date"]));

                            command.Parameters.AddWithValue("@phase", dtTb3.Rows[i]["phase"]);
                            command.Parameters.AddWithValue("@phase_generation_date", Convert.ToDateTime(dtTb3.Rows[i]["phase_generation_date"]));
                            command.Parameters.AddWithValue("@course_code", dtTb3.Rows[i]["course_code"]);
                            command.Parameters.AddWithValue("@Final_grade_for_qualification", dtTb3.Rows[i]["Final_grade_for_qualification"]);
                            command.Parameters.AddWithValue("@overall_percentage", dtTb3.Rows[i]["overall_percentage"]);
                            command.Parameters.AddWithValue("@application_no", dtTb3.Rows[i]["application_no"]);
                            command.Parameters.AddWithValue("@course_duration_for_qualification_in_hours", dtTb3.Rows[i]["course_duration_for_qualification_in_hours"]);
                            command.Parameters.AddWithValue("@NSQF_Framwork_Level_for_qualification", dtTb3.Rows[i]["NSQF_Framwork_Level_for_qualification"]);
                            command.Parameters.AddWithValue("@NCVETQualCode", dtTb3.Rows[i]["NCVETQualCode"]);
                            command.Parameters.AddWithValue("@sectorShortCode", dtTb3.Rows[i]["sectorShortCode"]);
                            command.Parameters.AddWithValue("@tp_name", dtTb3.Rows[i]["tp_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@inst_address", dtTb3.Rows[i]["inst_address"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@inst_name", dtTb3.Rows[i]["inst_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@Traning_center_name_district", dtTb3.Rows[i]["Traning_center_name_district"]);
                            command.Parameters.AddWithValue("@Traning_center_name_state", dtTb3.Rows[i]["Traning_center_name_state"]);
                            command.Parameters.AddWithValue("@sponsor", dtTb3.Rows[i]["sponsor"]);
                            command.Parameters.AddWithValue("@regional_centre", dtTb3.Rows[i]["regional_centre"]);
                            command.Parameters.AddWithValue("@grade_legends_desc", dtTb3.Rows[i]["grade_legends_desc"]);
                            command.Parameters.AddWithValue("@Place_of_issue", "New Delhi");
                            command.Parameters.AddWithValue("@Remarks", dtTb3.Rows[i]["Remarks"]);
                            command.Parameters.AddWithValue("@credits", dtTb3.Rows[i]["credits"]);
                            command.Parameters.AddWithValue("@apaarId", decryptedApaarID);


                            command.ExecuteNonQuery();
                        }
                    }
                }

                if (DataDownloadSequenceId != 0 && courseID == 0 && PhaseNumber == 0 && (CertificateType == 2 || CertificateType == 0))
                {
                    DataTable dtTb2 = new DataTable();

                    /* sql = " select application_no, registration_no, rollno,p.name,f_name,m_name,G_NAME g_name,dob d_o_b,inst_name, [grade_code] grade," +
                            " case exam_month when 1 then 'JANUARY'when 2 then 'FEBRUARY'when 3 then 'MARCH'when 4 then 'APRIL'when 5 then 'MAY'when 6 then 'JUNE'when 7 then 'JULY'when 8 then 'AUGUST'when 9 then 'SEPTEMBER'when 10 then 'OCTOBER'when 11 then 'NOVEMBER'when 12 then 'DECEMBER' end as exam_month ," +
                            "  exam_year,exam_type,[course_code] coursename,Aadhaar,Aadhaar_Enrl,grade_legends_desc,Remarks,regional_centre," +
                            " affidavit_srno,affidavit_date,pan_no,tp_name,'' as sponsor,address1,address2,address3,address_city,address_state,address_pin," +
                            " '' as course_start_on ,course_end_on  from NSQF_Cert_Phase_Print_Detail p,  CourseMappingWithNSQFCoursecode c, Course k where p.phase > 10 and " +
                            " p.course_code = c.NSQFCourseCode and c.CourseID = k.ID and  p.data_downloaded_SEQUENCE = '" + DataDownloadSequenceId + "'" +
                            " and (select top 1 d.credits from NIELIT.dbo.CourseLevelDuration d where Course_ID =c.CourseID   " +
                            "  and (( SELECT top 1  MAX(Commencement_From_Date) FROM Registration_detail  ) between Effective_From_Date and " +
                            " isnull(Effective_To_Date,CURRENT_TIMESTAMP))) is  null and Credits  is  null  ";*/
                    SqlParameter[] parameters = new SqlParameter[3];

                    // Assign the parameters, ensuring compatibility with older .NET versions
                    parameters[0] = new SqlParameter("@DataDownloadedSeq", SqlDbType.Int);
                    if (DataDownloadSequenceId != null)
                    {
                        parameters[0].Value = DataDownloadSequenceId;
                    }
                    else
                    {
                        parameters[0].Value = DBNull.Value;
                    }

                    parameters[1] = new SqlParameter("@courseId", SqlDbType.Int);

                    parameters[1].Value = DBNull.Value;


                    parameters[2] = new SqlParameter("@phaseNo", SqlDbType.Int);

                    parameters[2].Value = DBNull.Value;


                    // Call the stored procedure and pass the parameters
                    dtTb2 = DbUtility.GetDataTable("NSQFCertificateDataNonNCVET", new EConnect.Connections.SqlCon(), parameters, CommandType.StoredProcedure, false);
                    if (dtTb2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtTb2.Rows.Count; i++)
                        {

                            if (dtTb2.Rows[i]["affidavit_date"].ToString().Trim().Length == 0)
                            {
                                SqlStrSub = "NULL,'";
                            }
                            else
                            {
                                SqlStrSub = "'" + dtTb2.Rows[i]["affidavit_date"] + "','";
                            }


                            string encryptedApaarID = dtTb2.Rows[i]["apaarID"].ToString().Trim();
                            string decryptedApaarID = EncryptDecrypt.DecryptString(encryptedApaarID);
                            sqlStr = " insert into  [CERT] (application_no, registration_no,rollno,name,f_name,m_name,g_name,d_o_b,inst_name,grade,exam_month,exam_year,exam_type,coursename,Aadhaar,Aadhaar_Enrl,grade_legends_desc,Remarks,regional_centre,affidavit_srno,affidavit_date,pan_no,tp_name,sponsor,address1,address2,address3,address_city,address_state,address_pin,course_end_on,apaarId";
                            sqlStr += "      ) " +
                                            "VALUES (@application_no, @registration_no, @rollno, @name, @f_name, @m_name, @g_name, @d_o_b, @inst_name, @grade, @exam_month, @exam_year, @exam_type, @coursename, @Aadhaar, @Aadhaar_Enrl, @grade_legends_desc, @Remarks, @regional_centre, @affidavit_srno, @affidavit_date, @pan_no, @tp_name, @sponsor, @address1, @address2, @address3, @address_city, @address_state, @address_pin, @course_end_on, @apaarId);";

                            command = new OleDbCommand(sqlStr, connection);
                            command.Parameters.AddWithValue("@application_no", dtTb2.Rows[i]["application_no"]);
                            command.Parameters.AddWithValue("@registration_no", dtTb2.Rows[i]["registration_no"]);
                            command.Parameters.AddWithValue("@rollno", dtTb2.Rows[i]["rollno"]);
                            command.Parameters.AddWithValue("@name", dtTb2.Rows[i]["name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@f_name", dtTb2.Rows[i]["f_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@m_name", dtTb2.Rows[i]["m_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@g_name", dtTb2.Rows[i]["g_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@d_o_b", Convert.ToDateTime(dtTb2.Rows[i]["d_o_b"]));
                            command.Parameters.AddWithValue("@inst_name", dtTb2.Rows[i]["inst_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@grade", dtTb2.Rows[i]["grade"]);
                            command.Parameters.AddWithValue("@exam_month", dtTb2.Rows[i]["exam_month"]);
                            command.Parameters.AddWithValue("@exam_year", dtTb2.Rows[i]["exam_year"]);
                            command.Parameters.AddWithValue("@exam_type", dtTb2.Rows[i]["exam_type"]);
                            command.Parameters.AddWithValue("@coursename", dtTb2.Rows[i]["coursename"]);
                            command.Parameters.AddWithValue("@Aadhaar", dtTb2.Rows[i]["Aadhaar"]);
                            command.Parameters.AddWithValue("@Aadhaar_Enrl", dtTb2.Rows[i]["Aadhaar_Enrl"]);
                            command.Parameters.AddWithValue("@grade_legends_desc", dtTb2.Rows[i]["grade_legends_desc"]);
                            command.Parameters.AddWithValue("@Remarks", dtTb2.Rows[i]["Remarks"]);
                            command.Parameters.AddWithValue("@regional_centre", dtTb2.Rows[i]["regional_centre"]);
                            command.Parameters.AddWithValue("@affidavit_srno", dtTb2.Rows[i]["affidavit_srno"]);

                            // Handle affidavit_date with conditional null
                            if (string.IsNullOrEmpty(dtTb2.Rows[i]["affidavit_date"].ToString()))
                                command.Parameters.AddWithValue("@affidavit_date", DBNull.Value);
                            else
                                command.Parameters.AddWithValue("@affidavit_date", Convert.ToDateTime(dtTb2.Rows[i]["affidavit_date"]));

                            command.Parameters.AddWithValue("@pan_no", dtTb2.Rows[i]["pan_no"]);
                            command.Parameters.AddWithValue("@tp_name", dtTb2.Rows[i]["tp_name"]);
                            command.Parameters.AddWithValue("@sponsor", dtTb2.Rows[i]["sponsor"]);
                            command.Parameters.AddWithValue("@address1", dtTb2.Rows[i]["address1"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@address2", dtTb2.Rows[i]["address2"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@address3", dtTb2.Rows[i]["address3"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@address_city", dtTb2.Rows[i]["address_city"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@address_state", dtTb2.Rows[i]["address_state"]);
                            command.Parameters.AddWithValue("@address_pin", dtTb2.Rows[i]["address_pin"]);
                            command.Parameters.AddWithValue("@course_end_on", Convert.ToDateTime(dtTb2.Rows[i]["course_end_on"]));
                            command.Parameters.AddWithValue("@apaarId", decryptedApaarID);
                            command.ExecuteNonQuery();
                        }
                    }
                }

                if (DataDownloadSequenceId != 0 && courseID == 0 && PhaseNumber == 0 && CertificateType == 1)
                {
                    DataTable dtTb3 = new DataTable();
             
                    /*sql = "select NCVET_sr_no,registration_no,rollno,exam_type, course_type,course_code coursename,exam_month,exam_year,p.name,dob d_o_b, " +
                            "f_name,m_name,G_NAME,affidavit_srno,affidavit_date,phase,phase_date phase_generation_date, course_code, " +
                            "grade_code [Final_grade_for_qualification],percentage overall_percentage,application_no,course_duration_for_qualification_in_hours, " +
                            "[NSQF_Framwork_Level_for_qualification],NCVETQualCode,sectorShortCode,tp_name,inst_address,inst_name,[Traning_center_name_district], " +
                            "[Traning_center_name_state],sponsor,regional_centre,grade_legends_desc, 'New Delhi' [Place_of_issue], Remarks,credits " +
                            "from  NSQF_Cert_Phase_Print_Detail p JOIN  CourseMappingWithNSQFCoursecode c ON p.course_code = c.NSQFCourseCode  JOIN  Course k  ON c.CourseID = k.ID   where " +
                            "phase > 10  AND data_downloaded_SEQUENCE ='" + DataDownloadSequenceId + "'  and Credits  is  not  null ";*/



                    SqlParameter[] parameters = new SqlParameter[3];

                    // Assign the parameters, ensuring compatibility with older .NET versions
                    parameters[0] = new SqlParameter("@DataDownloadedSeq", SqlDbType.Int);
                    if (DataDownloadSequenceId != null)
                    {
                        parameters[0].Value = DataDownloadSequenceId;
                    }
                    else
                    {
                        parameters[0].Value = DBNull.Value;
                    }

                    parameters[1] = new SqlParameter("@courseId", SqlDbType.Int);
                    if (courseID != null)

                        parameters[1].Value = DBNull.Value;


                    parameters[2] = new SqlParameter("@phaseNo", SqlDbType.Int);
                    if (PhaseNumber != null)

                        parameters[2].Value = DBNull.Value;


                    // Call the stored procedure and pass the parameters
                    dtTb3 = DbUtility.GetDataTable("NSQFCertificateDataNCVET", new EConnect.Connections.SqlCon(), parameters, CommandType.StoredProcedure, false);
                    if (dtTb3.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtTb3.Rows.Count; i++)
                        {

                            //SqlStrSub = " cast ((CASE LEN(LTRIM(RTRIM('" + dtTb3.Rows[i]["affidavit_date"].ToString().Replace("'", "''") + "'))) WHEN 0 THEN NULL ELSE '" + dtTb3.Rows[i]["affidavit_date"].ToString().Replace("'", "''") + "' END) as date) ";

                            //string affidavitDateValue = dtTb3.Rows[i]["affidavit_date"].ToString().Trim().Length == 0 ? "NULL" : "#" + Convert.ToDateTime(dtTb3.Rows[i]["affidavit_date"]).ToString("MM/dd/yyyy") + "#";

                            //if (dtTb3.Rows[i]["affidavit_date"].ToString().Trim().Length == 0)
                            //{
                            //   // SqlStrSub = "NULL,'";
                            //    SqlStrSub = "";
                            //}
                            //else
                            //{
                            //    //SqlStrSub = "'" + dtTb3.Rows[i]["affidavit_date"] + "','";
                            //    SqlStrSub = dtTb3.Rows[i]["affidavit_date"].ToString();
                            //}

                            DateTime parsedAffidavitDate;
                            object affidavitDateValue;

                            if (DateTime.TryParse(dtTb3.Rows[i]["affidavit_date"].ToString(), out parsedAffidavitDate))
                            {
                                affidavitDateValue = parsedAffidavitDate;
                            }
                            else
                            {
                                affidavitDateValue = DBNull.Value;
                            }

                            string encryptedApaarID = dtTb3.Rows[i]["apaarID"].ToString();
                            string decryptedApaarID = EncryptDecrypt.DecryptString(encryptedApaarID);
                            sqlStr = "INSERT INTO [CERT] " +
                                           "(NCVET_sr_no, registration_no, rollno, exam_type, course_type, coursename, exam_month, exam_year, name, d_o_b, f_name, m_name, g_name, affidavit_srno,affidavit_date, " +
                                           "phase, phase_generation_date, course_code, Final_grade_for_qualification, overall_percentage, application_no, course_duration_for_qualification_in_hours, NSQF_Framwork_Level_for_qualification, NCVETQualCode, " +
                                           "sectorShortCode, tp_name, inst_address, inst_name, Traning_center_name_district, Traning_center_name_state, sponsor, regional_centre, grade_legends_desc, Place_of_issue, Remarks, credits,apaarId) " +
                                           "VALUES (@NCVET_sr_no, @registration_no, @rollno, @exam_type, @course_type, @coursename, @exam_month, @exam_year, @name, @d_o_b, @f_name, @m_name, @g_name, @affidavit_srno,@affidavit_date, " +
                "@phase, @phase_generation_date, @course_code, @Final_grade_for_qualification, @overall_percentage, @application_no, @course_duration_for_qualification_in_hours, @NSQF_Framwork_Level_for_qualification, @NCVETQualCode, " +
                "@sectorShortCode, @tp_name, @inst_address, @inst_name, @Traning_center_name_district, @Traning_center_name_state, @sponsor, @regional_centre, @grade_legends_desc, @Place_of_issue, @Remarks, @credits, @apaarId);";



                            command = new OleDbCommand(sqlStr, connection);
                            command.Parameters.AddWithValue("@NCVET_sr_no", dtTb3.Rows[i]["NCVET_sr_no"]);
                            command.Parameters.AddWithValue("@registration_no", dtTb3.Rows[i]["registration_no"]);
                            command.Parameters.AddWithValue("@rollno", dtTb3.Rows[i]["rollno"]);
                            command.Parameters.AddWithValue("@exam_type", dtTb3.Rows[i]["exam_type"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@course_type", dtTb3.Rows[i]["course_type"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@coursename", dtTb3.Rows[i]["coursename"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@exam_month", dtTb3.Rows[i]["exam_month"]);
                            command.Parameters.AddWithValue("@exam_year", dtTb3.Rows[i]["exam_year"]);
                            command.Parameters.AddWithValue("@name", dtTb3.Rows[i]["name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@d_o_b", Convert.ToDateTime(dtTb3.Rows[i]["d_o_b"]));
                            command.Parameters.AddWithValue("@f_name", dtTb3.Rows[i]["f_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@m_name", dtTb3.Rows[i]["m_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@g_name", dtTb3.Rows[i]["g_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@affidavit_srno", dtTb3.Rows[i]["affidavit_srno"]);
                           // command.Parameters.AddWithValue("@affidavit_date", Convert.ToDateTime(SqlStrSub));
                             //Handle affidavit_date with conditional null
                            //if (string.IsNullOrEmpty(dtTb3.Rows[i]["affidavit_date"].ToString()))
                            //    command.Parameters.AddWithValue("@affidavit_date", DBNull.Value);
                            //else
                            //    command.Parameters.AddWithValue("@affidavit_date", Convert.ToDateTime(dtTb3.Rows[i]["affidavit_date"]));
                            command.Parameters.Add("@affidavit_date", OleDbType.Date).Value = affidavitDateValue;
                            command.Parameters.AddWithValue("@phase", dtTb3.Rows[i]["phase"]);
                            command.Parameters.AddWithValue("@phase_generation_date", Convert.ToDateTime(dtTb3.Rows[i]["phase_generation_date"]));
                            command.Parameters.AddWithValue("@course_code", dtTb3.Rows[i]["course_code"]);
                            command.Parameters.AddWithValue("@Final_grade_for_qualification", dtTb3.Rows[i]["Final_grade_for_qualification"]);
                            command.Parameters.AddWithValue("@overall_percentage", dtTb3.Rows[i]["overall_percentage"]);
                            command.Parameters.AddWithValue("@application_no", dtTb3.Rows[i]["application_no"]);
                            command.Parameters.AddWithValue("@course_duration_for_qualification_in_hours", dtTb3.Rows[i]["course_duration_for_qualification_in_hours"]);
                            command.Parameters.AddWithValue("@NSQF_Framwork_Level_for_qualification", dtTb3.Rows[i]["NSQF_Framwork_Level_for_qualification"]);
                            command.Parameters.AddWithValue("@NCVETQualCode", dtTb3.Rows[i]["NCVETQualCode"]);
                            command.Parameters.AddWithValue("@sectorShortCode", dtTb3.Rows[i]["sectorShortCode"]);
                            command.Parameters.AddWithValue("@tp_name", dtTb3.Rows[i]["tp_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@inst_address", dtTb3.Rows[i]["inst_address"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@inst_name", dtTb3.Rows[i]["inst_name"].ToString().Replace("'", "''"));
                            command.Parameters.AddWithValue("@Traning_center_name_district", dtTb3.Rows[i]["Traning_center_name_district"]);
                            command.Parameters.AddWithValue("@Traning_center_name_state", dtTb3.Rows[i]["Traning_center_name_state"]);
                            command.Parameters.AddWithValue("@sponsor", dtTb3.Rows[i]["sponsor"]);
                            command.Parameters.AddWithValue("@regional_centre", dtTb3.Rows[i]["regional_centre"]);
                            command.Parameters.AddWithValue("@grade_legends_desc", dtTb3.Rows[i]["grade_legends_desc"]);
                            command.Parameters.AddWithValue("@Place_of_issue", "New Delhi");
                            command.Parameters.AddWithValue("@Remarks", dtTb3.Rows[i]["Remarks"]);
                            command.Parameters.AddWithValue("@credits", dtTb3.Rows[i]["credits"]);
                            command.Parameters.AddWithValue("@apaarId", decryptedApaarID);


                            command.ExecuteNonQuery();
                            command.Dispose();
                        }
                    }
                }


               
               // command.Dispose();
                connection.Close();
                connection.Dispose();
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.ContentType = "application/octet-stream";
                Response.Charset = "UTF-8";
                Response.WriteFile(mdbFilePath);
            }
        }


        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }

    }
}