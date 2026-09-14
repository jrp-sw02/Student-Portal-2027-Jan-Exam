using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using EConnect.URM;
using EConnect.DAL;
using System.Text.RegularExpressions;
using EConnect.NIELIT;
using System.Web;
using System.Data.Objects;
using EConnect;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.IO;
// for email
using System.Net.Mail;
using System.Net;

public partial class Admin_StudentDetailsCentreCourseBatch : BasePage
    {
    String strMessage = string.Empty;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    Int32 UserTypeId = 0;
    Int32 entityID = 0;
    Attachment attachmentData;
    Int64 centreId = 0;
    protected void Page_Load(object sender, EventArgs e)
        {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
            {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            entityID = Convert.ToInt32(Session["EntityID"]);
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
                {
                Response.Write("Sorry! You don't have rights to view this page");
                Response.End();
                }

            if (!Page.IsPostBack)
                {
                User objUser;
                using (EConnectContext context = new EConnectContext())
                    {
                    centreId = Convert.ToInt64(Session["EntityID"]);
                    objUser = new EConnect.URM.User();
                    User loginUser = context.Users.Where(s => s.UserID == loginUserNo).FirstOrDefault();
                    RegionalCenter RegName = context.RegionalCenters.Find(loginUser.UserRefNumber);
                    if (!String.IsNullOrEmpty(Request.QueryString["Key"]))
                        {
                        FillCourseCategory(centreId);
                        }
                    else
                        {
                        using (NIELITMISContext context1 = new NIELITMISContext())
                            {
                            ViewState["SortField"] = "";
                            ViewState["SortOrder"] = "";
                            FillCourseCategory(centreId);
                            if (!String.IsNullOrEmpty(Request.QueryString["Id"]))
                                {
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Student Details For LMS", "Admin/StudentDetailsCentreCourseBatch.aspx?Id=" + Request.QueryString["Id"].ToString(), ""));
                                }
                            else
                                {
                                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Student Details For LMS", "Admin/StudentDetailsCentreCourseBatch.aspx", ""));
                                }
                            }
                        }
                    if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                        ShowAlert(Request.QueryString["msg"].ToString());
                    }
                }
            BreadCrumb1.Render();
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message, true);
            }
        }

    protected void btnCancel_Click(object sender, EventArgs e)
        {
        try
            {
            ddlBatchName.Items.Clear();
            ddlBatchName.Items.Insert(0, new ListItem("--Select One--", "0"));
            ddlcourseName.Items.Clear();
            ddlcourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
            ddlcoursecategory.Items.Clear();
            ddlcoursecategory.Items.Insert(0, new ListItem("--Select One--", "0"));
            centreId = Convert.ToInt64(Session["EntityID"]);
            FillCourseCategory(centreId);
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
        {
        try
            {
            ddlcourseName.Items.Clear();
            ddlcourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
            ddlBatchName.Items.Clear();
            ddlBatchName.Items.Insert(0, new ListItem("--Select One--", "0"));
            Int32 courseCategoryId = 0;
            centreId = Convert.ToInt64(Session["EntityID"]);
            courseCategoryId = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            FillCourse(centreId, courseCategoryId);
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message, true);
            }
        }
    protected void ddlcourseName_SelectedIndexChanged1(object sender, EventArgs e)
        {
        try
            {
            ddlBatchName.Items.Clear();
            ddlBatchName.Items.Insert(0, new ListItem("--Select One--", "0"));

            Int32 courseCategoryId = 0;
            Int64 courseId = 0;
            centreId = Convert.ToInt64(Session["EntityID"]);
            courseCategoryId = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            courseId = Convert.ToInt64(ddlcourseName.SelectedValue);



            FillBatch(centreId, courseCategoryId, courseId);
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message, true);
            }
        }

    #region Email
    protected void btnEmail_Click(object sender, EventArgs e)
        {
        String folderPath = String.Empty;
        try
            {
            Int64 pCentreId = 0, pCourseId = 0, pBatchId = 0;
            Int32 pCourseCategoryId = 0;
            string courseCatName = string.Empty, courseName = string.Empty, batchName = string.Empty, sheetname = string.Empty;

            courseCatName = ddlcoursecategory.SelectedItem.Text;
            courseName = (ddlcourseName.SelectedItem.Text).ToString().Substring(0, (ddlcourseName.SelectedItem.Text).ToString().IndexOf('('));
            batchName = ddlBatchName.SelectedItem.Text;
            pCentreId = Convert.ToInt64(Session["EntityID"]);
            pCourseCategoryId = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            pCourseId = Convert.ToInt64(ddlcourseName.SelectedValue);
            pBatchId = Convert.ToInt64(ddlBatchName.SelectedValue);

            if (pCentreId != 0 && pCourseCategoryId != 0 && pCourseId != 0 && pBatchId != 0)
                {
                using (DataTable dt = FillStudentsRecordsForEmailToLms(pCentreId, pCourseCategoryId, pCourseId, pBatchId))
                    {
                    if (dt.Rows.Count > 0)
                        {
                        sheetname = "VA_" + courseName + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss");
                        folderPath = Server.MapPath("~/Temp_Docs/" + sheetname + ".xls");
                        GridView GridView1 = new GridView();
                        GridView1.AllowPaging = false;
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                        System.IO.StringWriter sw = new System.IO.StringWriter();
                        HtmlTextWriter hw = new HtmlTextWriter(sw);
                        for (int i = 0; i < GridView1.Rows.Count; i++)
                            {
                            GridView1.Rows[i].Attributes.Add("class", "textmode");
                            }
                        GridView1.RenderControl(hw);
                        System.Text.StringBuilder sb = new System.Text.StringBuilder(sw.ToString());
                        System.IO.TextWriter tw = new System.IO.StreamWriter(HttpContext.Current.Server.MapPath("~/Temp_Docs/") + sheetname + ".xls");
                        tw.Write(sb.ToString());
                        tw.Flush();
                        tw.Close();

                        // sending Email
                        String subject = String.Empty, body = String.Empty, emailAddressTo = String.Empty;

                        subject = "Student and Faculty Details For Virtual Academy Course- " + courseName + "and Batch- " + batchName + " From: NIELIT";
                        body = "Dear Sir/Madam,<br/><br/>Kindly, Find the attached details of student and faculty for Virtual Academy <br/> Course- " + courseName + "and Batch- " + batchName + ".<br/><br/> From: NIELIT";
                         emailAddressTo = "vksaini.it@gmail.com";
                        //emailAddressTo = "vishal.it@live.in";   // for testing
                        if (!String.IsNullOrEmpty(emailAddressTo.Trim()))
                            {
                            if (IsValidEmailAddress(emailAddressTo))
                                {
                                attachmentData = new Attachment(folderPath);
                                Emailwithattachment(subject, body, emailAddressTo, attachmentData);
                                ShowAlert("Email Successfully Sent.");
                                }
                            }
                        }
                    else
                        {
                        throw new Exception("No record found.");
                        }
                    }
                }
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        finally
            {
            attachmentData.Dispose();
            if (File.Exists(folderPath))
            { File.Delete(folderPath); } // To delete Excel file after email from server
            }
        }

    private void Emailwithattachment(String subject, String body, String emailAddressTo, Attachment attachmentData)
        {
        String _crUserName = String.Empty, _crPassword = String.Empty, _host = String.Empty, _senderEmailId = String.Empty;
        String _password = String.Empty, _senderEmailIdName = String.Empty, _emailUrl = String.Empty;
        try
            {
            try
                {
                _crUserName = System.Web.Configuration.WebConfigurationManager.AppSettings["userName"].ToString();
                _crPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["password"].ToString();
                _host = System.Web.Configuration.WebConfigurationManager.AppSettings["host"].ToString();
                _senderEmailId = System.Web.Configuration.WebConfigurationManager.AppSettings["senderEmailId"].ToString();
                _password = System.Web.Configuration.WebConfigurationManager.AppSettings["senderEmailPassword"].ToString();
                _senderEmailIdName = "DoNotReply";
                _emailUrl = System.Web.Configuration.WebConfigurationManager.AppSettings["emailUrl"].ToString();
                }
            catch (Exception) { }

            if (string.IsNullOrEmpty(_senderEmailId))
                {
                _senderEmailId = "donotreply@nielit.gov.in";
                _password = "P@55w0rd";
                _emailUrl = "https://mail.nielit.in/web-request.jsp";
                _host = "202.41.97.145";
                _crUserName = "";
                _crPassword = "";
                }

            SmtpClient client = new SmtpClient();
            client.Host = _host;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(_crUserName, _crPassword);
           // client.Port = 587;  //for gmail

            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.From = new MailAddress(_senderEmailId, _senderEmailIdName);
            mail.IsBodyHtml = true;
            mail.To.Add(emailAddressTo);
            mail.Body = body;
            mail.Attachments.Add(attachmentData);
            //mail.Priority = MailPriority.High;  // for high priority of email
            client.Send(mail);
            mail.Dispose();
            }
        catch (Exception)
            {
            attachmentData.Dispose();
            throw new Exception("Email Not Sent.");
            }
        }

    //For Gmail 
    //private void Emailwithattachment(String subject, String body, String emailAddressTo, Attachment attachmentData)
    //    {
    //    try
    //        {
    //        String _crUserName = "vksaini.it@gmail.com";   //Sender Gmail ID
    //        String _senderEmailId = "vksaini.it@gmail.com";   //Sender Gmail ID
    //        String _crPassword = "deepsaini1122";    //password of Sender Gmail ID

    //        String _host = "smtp.gmail.com";
    //        String _senderEmailIdName = "DoNotReply";

    //        SmtpClient client = new SmtpClient();
    //        client.Host = _host;
    //        client.EnableSsl = true;
    //        client.UseDefaultCredentials = false;
    //        client.Credentials = new System.Net.NetworkCredential(_crUserName, _crPassword);
    //        client.Port = 587;

    //        MailMessage mail = new MailMessage();
    //        mail.Subject = subject;
    //        mail.From = new MailAddress(_senderEmailId, _senderEmailIdName);
    //        mail.IsBodyHtml = true;
    //        mail.To.Add(emailAddressTo);
    //        mail.Body = body;
    //        mail.Attachments.Add(attachmentData);
    //        //mail.Priority = MailPriority.High;  // for high priority of email
    //        client.Send(mail);
    //        mail.Dispose();
    //        }
    //    catch (Exception)
    //        {
    //        attachmentData.Dispose();
    //        throw new Exception("Email Not Sent.");
    //        }
    //    } 
    #endregion

    #region Download
    protected void btnSave_Click(object sender, EventArgs e)
        {
        try
            {
            Int64 pCentreId = 0, pCourseId = 0, pBatchId = 0;
            Int32 pCourseCategoryId = 0;
            string courseCatName = string.Empty, courseName = string.Empty, batchName = string.Empty, sheetname = string.Empty;

            courseCatName = ddlcoursecategory.SelectedItem.Text;
            courseName = (ddlcourseName.SelectedItem.Text).ToString().Substring(0, (ddlcourseName.SelectedItem.Text).ToString().IndexOf('('));
            batchName = ddlBatchName.SelectedItem.Text;
            pCentreId = Convert.ToInt64(Session["EntityID"]);
            pCourseCategoryId = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            pCourseId = Convert.ToInt64(ddlcourseName.SelectedValue);
            pBatchId = Convert.ToInt64(ddlBatchName.SelectedValue);

            if (pCentreId != 0 && pCourseCategoryId != 0 && pCourseId != 0 && pBatchId != 0)
                {
                using (DataTable dt = FillStudentsRecordsForEmailToLms(pCentreId, pCourseCategoryId, pCourseId, pBatchId))
                    {
                    if (dt.Rows.Count > 0)
                        {
                        sheetname = "VA_" + courseName + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss");

                        GridView GridView1 = new GridView();
                        GridView1.AllowPaging = false;
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                        Response.Clear();
                        Response.Buffer = true;
                        Response.AddHeader("content-disposition", "attachment;filename=" + sheetname + ".xls");
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.ms-excel";
                        StringWriter sw = new StringWriter();
                        HtmlTextWriter hw = new HtmlTextWriter(sw);
                        for (int i = 0; i < GridView1.Rows.Count; i++)
                            {
                            GridView1.Rows[i].Attributes.Add("class", "textmode");
                            }
                        GridView1.RenderControl(hw);
                        string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                        Response.Write(style);
                        Response.Output.Write(sw.ToString());
                        Response.Flush();
                        Response.End();
                        }
                    else
                        {
                        throw new Exception("No record found.");
                        }

                    }
                }
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }
    #endregion

    #region Data Filling Methods
    protected void FillCourseCategory(Int64 pCentre)
        {
        try
            {
            using (DataTable dt = FillCourseCategoryRecords(pCentre))
                {
                if (dt.Rows.Count > 0)
                    {
                    ddlcoursecategory.DataSource = dt;
                    ddlcoursecategory.DataTextField = "Name";
                    ddlcoursecategory.DataValueField = "ID";
                    ddlcoursecategory.DataBind();
                    ddlcoursecategory.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                }
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }
    public DataTable FillCourseCategoryRecords(Int64 pCentre)
        {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
            {
            try
                {
                using (SqlCommand cmd = new SqlCommand("GetvirtualAcademyCourseCategoryForEmailToLMS", con))
                    {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.Add("@pCentreId", SqlDbType.BigInt);
                    cmd.Parameters["@pCentreId"].Value = pCentre;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                        sda.Fill(myDt);
                        }
                    }
                }
            catch (Exception ex)
                {
                throw ex;
                }
            finally
                {
                con.Close();
                }
            }
        return myDt;
        }    

    protected void FillCourse(Int64 pCentre, int pCourseCategory)
        {
        try
            {
            using (DataTable dt = FillCourseRecords(pCentre, pCourseCategory))
                {
                if (dt.Rows.Count > 0)
                    {
                    ddlcourseName.DataSource = dt;
                    ddlcourseName.DataTextField = "Name";
                    ddlcourseName.DataValueField = "ID";
                    ddlcourseName.DataBind();
                    ddlcourseName.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                BreadCrumb1.Render();
                }
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }
    public DataTable FillCourseRecords(Int64 pCentre, int pCourseCategory)
        {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
            {
            try
                {
                using (SqlCommand cmd = new SqlCommand("GetvirtualAcademyCourseForEmailToLMS", con))
                    {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.Add("@pCentreId", SqlDbType.BigInt);
                    cmd.Parameters["@pCentreId"].Value = pCentre;
                    cmd.Parameters.Add("@pCourseCat", SqlDbType.BigInt);
                    cmd.Parameters["@pCourseCat"].Value = pCourseCategory;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                        sda.Fill(myDt);
                        }
                    }
                }
            catch (Exception ex)
                {
                throw ex;
                }
            finally
                {
                con.Close();
                }
            }
        return myDt;
        }    

    protected void FillBatch(Int64 pCentre, int pCourseCategory, Int64 pCourse)
        {
        try
            {
            using (DataTable dt = FillBatchRecords(pCentre, pCourseCategory, pCourse))
                {
                if (dt.Rows.Count > 0)
                    {
                    ddlBatchName.DataSource = dt;
                    ddlBatchName.DataTextField = "Name";
                    ddlBatchName.DataValueField = "ID";
                    ddlBatchName.DataBind();
                    ddlBatchName.Items.Insert(0, new ListItem("--Select One--", "0"));
                    }
                BreadCrumb1.Render();
                }
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }
    public DataTable FillBatchRecords(Int64 pCentre, int pCourseCategory, Int64 pCourse)
        {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
            {
            try
                {
                using (SqlCommand cmd = new SqlCommand("GetVirtualAcademyBatchRecordsForEmailToLms", con))   //   GetBatchRecordsForEmailToLms
                    {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.Add("@pCentreId", SqlDbType.BigInt);
                    cmd.Parameters["@pCentreId"].Value = pCentre;
                    cmd.Parameters.Add("@pCourseCat", SqlDbType.BigInt);
                    cmd.Parameters["@pCourseCat"].Value = pCourseCategory;
                    cmd.Parameters.Add("@pCourseId", SqlDbType.BigInt);
                    cmd.Parameters["@pCourseId"].Value = pCourse;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                        sda.Fill(myDt);
                        }
                    }
                }
            catch (Exception ex)
                {
                throw ex;
                }
            finally
                {
                con.Close();
                }
            }
        return myDt;
        }    

    public DataTable FillStudentsRecordsForEmailToLms(Int64 pCentre, int pCourseCategory, Int64 pCourse, Int64 pBatch)
        {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        DataTable myDt = new DataTable();
        using (SqlConnection con = new SqlConnection(constr))
            {
            try
                {
                using (SqlCommand cmd = new SqlCommand("GetVirtualAcademyStudentsRecordsForEmailToLms", con))
                    {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.Add("@pCentreId", SqlDbType.BigInt);
                    cmd.Parameters["@pCentreId"].Value = pCentre;
                    cmd.Parameters.Add("@pCourseCat", SqlDbType.BigInt);
                    cmd.Parameters["@pCourseCat"].Value = pCourseCategory;
                    cmd.Parameters.Add("@pCourseId", SqlDbType.BigInt);
                    cmd.Parameters["@pCourseId"].Value = pCourse;
                    cmd.Parameters.Add("@pBatchId", SqlDbType.BigInt);
                    cmd.Parameters["@pBatchId"].Value = pBatch;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                        sda.Fill(myDt);
                        }
                    }
                }
            catch (Exception ex)
                {
                throw ex;
                }
            finally
                {
                con.Close();
                }
            }
        return myDt;
        }
    #endregion    

    }