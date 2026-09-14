using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.Objects;
using System.Data.OleDb;
using System.IO;
using EConnect;
using EConnect.DAL;
using EConnect.Utils.Data;
using EConnect.NIELIT;
using EConnect.URM;
using System.Configuration;

public partial class HO_Frm_Download_NSQF_Application : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    UserType loginUserType;    
    EConnect.Connections.SqlCon con = new EConnect.Connections.SqlCon();
    //EConnect.Connections.SqlCon_nielittest con = new EConnect.Connections.SqlCon_nielittest();
    SqlDataAdapter da = new SqlDataAdapter();    
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
    Int32 currentRoleId = 0;  

    protected void Page_Load(object sender, EventArgs e)
    {      
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
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
                    ViewState["SortField"] = "";
                    ViewState["SortOrder"] = "";                    
                    bind_exam_year();                
                    BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download NSQF Application", "#", ""));
                }
            }
            else
            {
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download NSQF Application", "#", ""));
                //btnView.Visible = false;
                //btnReset.Visible = false;
                Lblerror.Text = "You can not download Candidate Applications.";
                Lblerror.Visible = true;
            }
            BreadCrumb1.Render();

        }
        catch (Exception ex)
        {
            //ShowAlert(ex.Message, true);
            Lblerror.Text = ex.Message;
            Lblerror.Visible = true;
        }
    }       

    protected void bind_exam_year()
    {
      
        var currentYear = DateTime.Today.Year-2;
        for (int i = 1; i <= 3; i++)
        {            
            ddl_year.Items.Add((currentYear+i).ToString());
        }
        //ddl_year.Items.Insert(0, new ListItem("--Select One--", "--Select One--"));
    }

    //protected void bind_exam_month()
    //{
    //    DataSet ds = new DataSet();
    //    using (SqlCommand scCommand = new SqlCommand("SL_display_month", new SqlConnection(con.ConnectionString)))
    //    {
    //        try
    //        {
    //            scCommand.CommandType = CommandType.StoredProcedure;               
    //            scCommand.CommandTimeout = 50000;
    //            if (scCommand.Connection.State == ConnectionState.Closed)
    //            {
    //                scCommand.Connection.Open();
    //            }
    //            da = new SqlDataAdapter(scCommand);
    //            da.Fill(ds);
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                ddl_exam_month.DataSource = ds;
    //                ddl_exam_month.DataTextField = "month_name";
    //                ddl_exam_month.DataValueField = "month_id";
    //                ddl_exam_month.DataBind();
    //                ddl_exam_month.Items.Insert(0, new ListItem("--Select One--", "--Select One--"));
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            ShowAlert(ex.Message, true);                
    //        }
    //    }
    //}

    protected void bind_course_Name()
    {
        DataSet ds = new DataSet();
        int exam_year = Convert.ToInt32(ddl_year.SelectedValue.Trim());
        int exam_month = Convert.ToInt32(ddl_exam_month.SelectedValue.Trim());
        int course_type = Convert.ToInt32(ddl_course_type.SelectedValue.Trim());
        using (SqlCommand scCommand = new SqlCommand("NSQF_Course_Name", new SqlConnection(con.ConnectionString)))
        {
            try
            {
                scCommand.CommandType = CommandType.StoredProcedure;
                scCommand.Parameters.Add("@Pexam_month", SqlDbType.Int).Value = exam_month;
                scCommand.Parameters.Add("@Pexam_year", SqlDbType.Int).Value = exam_year;
                scCommand.Parameters.Add("@Option", SqlDbType.Int).Value = course_type;
                scCommand.Parameters.Add("@course_id", SqlDbType.Int).Value = -1;
                scCommand.CommandTimeout = 50000;
                if (scCommand.Connection.State == ConnectionState.Closed)
                {
                    scCommand.Connection.Open();
                }
                da = new SqlDataAdapter(scCommand);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_course_name.DataSource = ds;
                    ddl_course_name.DataTextField = "Name";
                    ddl_course_name.DataValueField = "ID";
                    ddl_course_name.DataBind();
                    ddl_course_name.Items.Insert(0, new ListItem("--Select One--", "--Select One--"));
                }
            }
            catch (Exception ex)
            {
                ShowAlert(ex.Message, true);
            }
            finally
            {
                scCommand.Connection.Close();
            }
        }
    }

    protected void btn_download_Click(object sender, EventArgs e)
    {
        if (ddl_course_name.SelectedItem.Text == "--Select One--")
        {
            ShowAlert("Select Course Name Please...", true);
        }
        else
        {
            OleDbConnection connection = new OleDbConnection();
            OleDbCommand command = new OleDbCommand();
            //SqlDataAdapter da = new SqlDataAdapter();
            DataTable dtTbl = bindcandidaterecords();
            string mdbFilePath = "";
            String filename = "";
            try
            {
                context = new EConnectContext();
                int exam_year = Convert.ToInt32(ddl_year.SelectedValue.Trim());
                int exam_month = Convert.ToInt32(ddl_exam_month.SelectedValue.Trim());
                string course_name = ddl_course_name.SelectedItem.Text.Trim().Replace("[", "(").Replace("]", ")").Replace(",", "").Replace("/", "").Replace("{", "(").Replace("}", ")").Replace(".", "").Replace(";", "").Replace("-", "");
                string course_type = ddl_course_type.SelectedItem.Text.Trim();
                filename = course_name + "_" + course_type + "_" + exam_year + "_" + exam_month + "_" + DateTime.Now.ToString("ddMMyyyyHHmm") + ".mdb";
                mdbFilePath = Server.MapPath("~/Download/" + filename);
                if (System.IO.File.Exists(mdbFilePath))
                    System.IO.File.Delete(mdbFilePath);
                System.IO.File.Copy(Server.MapPath("~/Download/Database_format_NSQF.mdb"), mdbFilePath);
                string connect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + mdbFilePath + ";Persist Security Info=False;";
                connection.ConnectionString = connect;
                connection.Open();
                //command = new OleDbCommand("delete from  [MS Access;Database=" + mdbFilePath + "].[Exam_Database]", connection);
                
                //December_2024
                command = new OleDbCommand("delete from  [MS Access;Database=@mdbFilePath].[Exam_Database]", connection);
                command.Parameters.AddWithValue("@mdbFilePath", mdbFilePath);
                command.ExecuteNonQuery();
                String sqlStr = "";
                //String sql = "";
                //SqlCommand scCommand = new SqlCommand("NSQF_Exam_Data_Download", new SqlConnection(con.ConnectionString));           
                //    scCommand.CommandType = CommandType.StoredProcedure;
                //    scCommand.Parameters.Add("@Pexam_month", SqlDbType.Int).Value = exam_month;
                //    scCommand.Parameters.Add("@Pexam_year", SqlDbType.Int).Value = exam_year;
                //    scCommand.Parameters.Add("@PCourse_name", SqlDbType.VarChar).Value = course_name;
                //    scCommand.Parameters.Add("@PFLAG_download_Completed", SqlDbType.Int).Value = 0;
                //    scCommand.Parameters.Add("@PFLAG_Roll_no_Gen_Completed", SqlDbType.Int).Value = 0;
                //    //scCommand.Parameters.Add("@in_option", SqlDbType.Int).Value = 1;
                //    scCommand.CommandTimeout = 50000;
                //    if (scCommand.Connection.State == ConnectionState.Closed)
                //    {
                //        scCommand.Connection.Open();
                //    }
                //    da = new SqlDataAdapter(scCommand);
                //    da.Fill(dtTbl);            

                if (dtTbl.Rows.Count > 0)
                {
                    for (int i = 0; i < dtTbl.Rows.Count; i++)
                    {
                       sqlStr = " insert into [MS Access;Database=" + mdbFilePath + "].[Exam_Database] (Course_name, Exam_Paper_Name,Module_Code,Module_Short_Name,   " +
                                 " Previous_applied, Candidate_registration_no, registration_no_Valid_Upto_Date, Center_First_Choice, Center_Second_Choice, Candidate_Name, Candidate_Gender, Father_Name, Mother_Name, Guardian_Name," +
                                 " Candidate_Birth_date,  Candidate_Email, Candidate_Mobile,Course_Id, Exam_Id, Module_Id, Candidate_Corr_Address_City_State, Candidate_Perm_Address_City_State,  Institute_Name, Roll_Number , Is_Handicaped )" +
                                 " values('" + dtTbl.Rows[i]["Course_name"] + "','" + dtTbl.Rows[i]["Exam_Paper_Name"] + "','" + dtTbl.Rows[i]["Module_Code"] + "','" + dtTbl.Rows[i]["Module_Short_Name"] + "','" + dtTbl.Rows[i]["Previous_applied"].ToString() + "','" + dtTbl.Rows[i]["Candidate_registration_no"] + "','" + dtTbl.Rows[i]["registration_no_Valid_Upto_Date"].ToString().Replace("'", "") + "','"
                                 + dtTbl.Rows[i]["Center_First_Choice"].ToString() + "','" + dtTbl.Rows[i]["Center_Second_Choice"].ToString() + "','" + dtTbl.Rows[i]["Candidate_Name"] + "','" + dtTbl.Rows[i]["Candidate_Gender"] + "','" + dtTbl.Rows[i]["Father_Name"].ToString().Replace("'", "''") + "','"
                                 + dtTbl.Rows[i]["Mother_Name"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["Guardian_Name"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["Candidate_Birth_date"].ToString().Replace("'", "") + "','" + dtTbl.Rows[i]["Candidate_Email"] + "','" + dtTbl.Rows[i]["Candidate_Mobile"] + "','"
                                  + dtTbl.Rows[i]["Course_Id"] + "','"  + dtTbl.Rows[i]["Exam_Id"] + "','" + dtTbl.Rows[i]["Module_Id"] + "','" 
                                 + dtTbl.Rows[i]["Candidate_Corr_Address_City_State"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["Candidate_Perm_Address_City_State"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["Institute_Name"].ToString().Replace("'", "''") + "','" + dtTbl.Rows[i]["Roll_Number"] + "' , '" + dtTbl.Rows[i]["Is_Handicaped"] + "' )";


                        command = new OleDbCommand(sqlStr, connection);
                        command.ExecuteNonQuery();
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
            catch (Exception ex)
            {
                ShowAlert(ex.Message, true);
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            finally { context.Dispose(); }
        }
    }    

    protected void ddl_exam_month_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (Convert.ToInt32(ddl_year.SelectedValue.Trim()) != 0 && Convert.ToInt32(ddl_exam_month.SelectedValue.Trim()) != 0)
        //{
        //    bind_course_Name();
        //}           
       
        ddl_exam_month.Enabled = false;
        ddl_course_type.Enabled = true;
    }

    protected void ddl_course_name_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_course_name.SelectedItem.Text == "--Select One--" && lblApplicationsDetails.Text.Length > 0 && lbl_download_date.Text.Length >0)
        {
            ShowAlert("Select Course name...",true);
            lblApplicationsDetails.Visible = false;
            lbl_download_date.Visible = false;
            lblApplicationsDetails.Text = "";
            lbl_download_date.Text = "";
            Tr4.Visible = false;
            btn_download.Enabled = false;
        }
        else
        {
            lblApplicationsDetails.Text = "Total Module Candidate is: "+ candidatecount(Convert.ToInt32(ddl_course_type.SelectedValue.Trim())).Rows[0]["TOTAL_MODULE_CANDIDATE"] + " and Total Candidate is: " + candidatecount(Convert.ToInt32(ddl_course_type.SelectedValue.Trim())).Rows[0]["TOTAL_CANDIDATE"] + " and Already Passed Module Candidate is :" + candidatecount(Convert.ToInt32(ddl_course_type.SelectedValue.Trim())).Rows[0]["Already_Passed_Module_Candidate_Total"];
            lblApplicationsDetails.Visible = true;
            lbl_download_date.Text = "Download Date is:  " + candidatecount(Convert.ToInt32(ddl_course_type.SelectedValue.Trim())).Rows[0]["Download_start_date"];
            lbl_download_date.Visible = true;
            Tr4.Visible = true;
            btn_download.Enabled = true;
            ddl_course_type.Enabled = false;            
        }
    }

    DataTable bindcandidaterecords()
    {
        DataSet ds = new DataSet();
        int exam_year = Convert.ToInt32(ddl_year.SelectedValue.Trim());
        int exam_month = Convert.ToInt32(ddl_exam_month.SelectedValue.Trim());
        string course_name = ddl_course_name.SelectedItem.Text.Trim(); 
		Int32 courseId = Convert.ToInt32(ddl_course_name.SelectedValue);
        int course_type = Convert.ToInt32(ddl_course_type.SelectedValue.Trim());
        SqlCommand scCommand = new SqlCommand("NSQF_Exam_Data_Download", new SqlConnection(con.ConnectionString));
        scCommand.CommandType = CommandType.StoredProcedure;
        scCommand.Parameters.Add("@Pexam_month", SqlDbType.Int).Value = exam_month;
        scCommand.Parameters.Add("@Pexam_year", SqlDbType.Int).Value = exam_year;
       // scCommand.Parameters.Add("@PCourse_name", SqlDbType.VarChar).Value = course_name;
	   scCommand.Parameters.Add("@PCourse_id", SqlDbType.Int).Value = courseId;
        scCommand.Parameters.Add("@Option", SqlDbType.VarChar).Value = course_type;
        scCommand.Parameters.Add("@PFLAG_download_Completed", SqlDbType.Int).Value = 0;
        scCommand.Parameters.Add("@PFLAG_Roll_no_Gen_Completed", SqlDbType.Int).Value = 0;
        //if (ddl_course_type.SelectedValue.Trim() == "1")
        //{
        //    scCommand.Parameters.Add("@option", SqlDbType.Int).Value = 1;
        //}
        //else if (ddl_course_type.SelectedValue.Trim() == "2")
        //{
        //    scCommand.Parameters.Add("@option", SqlDbType.Int).Value = 2;
        //}
        //else if (ddl_course_type.SelectedValue.Trim() == "3")
        //{
        //    scCommand.Parameters.Add("@option", SqlDbType.Int).Value = 3;
        //}
        scCommand.CommandTimeout = 50000;
        if (scCommand.Connection.State == ConnectionState.Closed)
        {
            scCommand.Connection.Open();
        }
        da = new SqlDataAdapter(scCommand);
        da.Fill(ds);
        scCommand.Connection.Close();
        return ds.Tables[0];  
    }

    DataTable candidatecount(int course_type)
    {
        DataSet ds = new DataSet();
        int exam_year = Convert.ToInt32(ddl_year.SelectedValue.Trim());
        int exam_month = Convert.ToInt32(ddl_exam_month.SelectedValue.Trim());
        //int course_type = Convert.ToInt32(ddl_course_type.SelectedValue.Trim());
        string course_name = ddl_course_name.SelectedValue.Trim();
        SqlCommand scCommand = new SqlCommand("NSQF_Course_Name", new SqlConnection(con.ConnectionString));
        scCommand.CommandType = CommandType.StoredProcedure;
        scCommand.Parameters.Add("@Pexam_month", SqlDbType.Int).Value = exam_month;
        scCommand.Parameters.Add("@Pexam_year", SqlDbType.Int).Value = exam_year;
        scCommand.Parameters.Add("@Option", SqlDbType.Int).Value = course_type;
        scCommand.Parameters.Add("@course_id", SqlDbType.Int).Value = course_name;
        scCommand.CommandTimeout = 50000;
        if (scCommand.Connection.State == ConnectionState.Closed)
        {
            scCommand.Connection.Open();
        }
        da = new SqlDataAdapter(scCommand);
        da.Fill(ds);
        scCommand.Connection.Close();
        return ds.Tables[0];
    }

    protected void btn_tentative_date_Click(object sender, EventArgs e)
    {
        lbl_add.Visible = false;
        tbl_tent.Visible = true;
        //bind_course_Name(ddl_course,"ID");
        controlling(false);
    }

    void controlling(bool toggle)
    {
        ddl_course_name.Enabled = toggle;
        ddl_exam_month.Enabled = toggle;
        ddl_year.Enabled = toggle;
        btn_download.Enabled = toggle;
        btn_tentative_date.Enabled = toggle;
    }

    protected void btn_add_Click(object sender, EventArgs e) 
    {
        DataTable dt = new DataTable();
        int exam_year = Convert.ToInt32(ddl_year.SelectedValue.Trim());
        int exam_month = Convert.ToInt32(ddl_exam_month.SelectedValue.Trim());
        int course_name = Convert.ToInt32(ddl_course_name.SelectedValue.Trim());
        DateTime tent_date = Convert.ToDateTime(txt_add_date.Text);
        SqlCommand scCommand = new SqlCommand("CRUD_NSQF_Data_Processing", new SqlConnection(con.ConnectionString));
        scCommand.CommandType = CommandType.StoredProcedure;
        scCommand.Parameters.Add("@Pexam_month", SqlDbType.Int).Value = exam_month;
        scCommand.Parameters.Add("@Pexam_year", SqlDbType.Int).Value = exam_year;
        scCommand.Parameters.Add("@dn_tentative_date", SqlDbType.DateTime).Value = tent_date;
        scCommand.Parameters.Add("@PCourse_id", SqlDbType.Int).Value = course_name;
        scCommand.Parameters.Add("@PFLAG_download_Completed", SqlDbType.Int).Value = 0;
        scCommand.Parameters.Add("@PFLAG_Roll_no_Gen_Completed", SqlDbType.Int).Value = 0;
        scCommand.Parameters.Add("@in_option", SqlDbType.Int).Value = 1;
        scCommand.CommandTimeout = 50000;
        if (scCommand.Connection.State == ConnectionState.Closed)
        {
            scCommand.Connection.Open();
        }
        da = new SqlDataAdapter(scCommand);
        da.Fill(dt);
        
            lbl_add.Text="Date has been added to this course";
            lbl_add.Visible = true;
            scCommand.Connection.Close();
    }

    protected void btn_reset_Click(object sender, EventArgs e)
    {
        controlling(true);
        tbl_tent.Visible = false;
        ddl_year.SelectedIndex = 0;
        ddl_exam_month.SelectedIndex = 0;
		ddl_course_type.SelectedIndex = 0;
        ddl_course_name.Items.Clear();
        ddl_course_name.Items.Insert(0, new ListItem("--Select One--", "--Select One--"));       
        lblApplicationsDetails.Visible = false;
        lblApplicationsDetails.Text = "";
        lbl_add.Text = "";
        lbl_add.Visible = false;
        ddl_year.Enabled = true;
        ddl_exam_month.Enabled = false;
        ddl_course_name.Enabled = false;
        btn_download.Enabled = false;
        ddl_course_type.Enabled = false;
        Tr4.Visible = false;
        lbl_download_date.Text = "";
        lbl_download_date.Visible = false;
    }

    protected void ddl_year_SelectedIndexChanged1(object sender, EventArgs e)
    {
        //if (Convert.ToInt32(ddl_year.SelectedValue.Trim()) != 0 && Convert.ToInt32(ddl_exam_month.SelectedValue.Trim()) != 0)
        //{
        //    bind_course_Name();
        //} 
        ddl_exam_month.Enabled = true;
        ddl_year.Enabled = false;
    }

    protected void ddl_course_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        bind_course_Name();
        ddl_course_type.Enabled = false;
        ddl_course_name.Enabled = true;
    }
}