using System;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Text;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Net;
using System.Web;
using System.Data.Objects;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Data;
using EConnect.Utils.Common;
using System.Configuration;

public partial class HO_ProtsahanPuraskarReconcillation : BasePage
{
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //if (IsSessionAlive() == false)
            //    Response.Redirect("../Index.aspx");
            //currentRoleId = Convert.ToInt32(Session["RoleID"]);
            //if (!UserManager.HasRight(currentRoleId, enmRight.View))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //}
            if (!Page.IsPostBack)
            {
                ExamName();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Protsahan Puraskar Online Reconcillation", "HO/ProtsahanPuraskarReconcillation.aspx", ""));
                
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
   
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
           
            divValidateData.Visible = false;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void ExamName()
    {
       
        ddlflExam.Items.Clear();
        System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
        using (System.Data.DataTable dt = GetDataForPurskarApplicationCommitteeReport(1))
        {
            if (dt.Rows.Count > 0)
            {
                int k;
                for (k = 0; k < 1; k++)
                {
                    ddlflExam.DataSource = dt;
                    ddlflExam.DataTextField = "ExamName";
                    ddlflExam.DataValueField = "ExamMonthYear";
                    ddlflExam.DataBind();
                    ddlflExam.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select One--", "0"));
                }
            }
            else
            {
                ddlflExam.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select One--", "0"));
            }
        }

    }
    public System.Data.DataTable GetDataForPurskarApplicationCommitteeReport(int ViewCode)
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        System.Data.DataTable myDt = new System.Data.DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetExamNamePurskarApplication", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }
    protected void ddlflExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string ExamMonthYear = "18471";
            //BindGridView(ExamMonthYear);
            //divGrid.Visible = true;
            //lblError.Text = "";
            //// divRefundFileDownload.Visible = false;
            //btnDownloadWithLockCell.Enabled = false;
            //btnPdfDownload.Enabled = false;
            //btnDownload3.Enabled = false;

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    //protected void btnViewRecords_Click(object sender, EventArgs e)
    //{
    //    //btnDownloadWithLockCell.Enabled = true;
    //    //btnPdfDownload.Enabled = true;
    //    //btnDownload3.Enabled = true;
    //    //string ExamMonthYear = ddlflExam.SelectedValue;
    //    //BindGridView(ExamMonthYear);
    //    //divGrid.Visible = true;
    //}
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            divValidateData.Visible = true;
            BreadCrumb1.Render();
            StringBuilder FaildRecords = new StringBuilder();
            FaildRecords.Append("Failed TransactionIDs:-");
            int failedRecordCount = 0;
            int ValidateRecords = 0;
            int TotalRecords = 0;
            string filepath = Server.MapPath("../PuraskarUploadedFiles");
           // string filenameForDB = flUpload.FileName;
            string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + flUpload.FileName;
            flUpload.SaveAs(filepath + "/" + filename);
            string path = (filepath + "/" + filename);
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
            OleDbConnection connection = new OleDbConnection();
            connection.ConnectionString = sExcelConnectionString;
            connection.Open();

            //Add on 13 September 2021 
            string SheetName = "";
            DataTable Sheets = connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);  
            foreach (DataRow dr1 in Sheets.Rows)
            {
                if (SheetName == "")
                {
                    SheetName = dr1[2].ToString().Replace("'", "");
                }
            }
            if (SheetName != "AmountReleasedToBank$")
            {
                divValidateData.Visible = false;
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Please upload only Downloaded excel file!')", true);
                return; 
            }
                
            OleDbCommand command = new OleDbCommand("select * from [@SheetName]", connection);              
            command.Parameters.AddWithValue("@SheetName", SheetName);                                       //November_2024

            //13 September 2021
            //OleDbCommand command = new OleDbCommand("select * from [AmountReleasedToBank$]", connection);
            OleDbDataReader dr = command.ExecuteReader();
            Int64 transid = 0, RegnNo=0;
            try
            {

                using (EConnectContext context = new EConnectContext())
                {                    
                    while (dr.Read())
                    {                       
                      string   transid2 = dr[0].ToString();

                      if (dr[0].ToString() != "")
                      {
                          if (CommonFunctions.IsNumeric(dr[0].ToString()))
                          {
                              transid = Convert.ToInt64(dr[9]);// TRANSACTION ID
                              RegnNo = Convert.ToInt64(dr[1]); //RegnNo
                              TotalRecords = TotalRecords + 1; // total record
                          }
                          else
                          {
                              continue;
                          }                          
                          using (TransactionScope scope = new TransactionScope())
                          {
                              Int32 statusid = Convert.ToInt32(enmPuraskarApplicationStatus.VerifiedByFinanceWingButPaymentToBeProcessed);
                             
                              //DEEP Add on 14 Sep 2022 
                              string ExamMonthYear = ddlflExam.SelectedValue;
                              string ExamMonths = ExamMonthYear.Substring(4, 1);
                              string ExamYears = ExamMonthYear.Substring(0, 4);
                              Int32 ExamMonth = Convert.ToInt32(ExamMonthYear.Substring(4, 1));
                              Int32 ExamYear = Convert.ToInt32(ExamMonthYear.Substring(0, 4));                             
                              var Levels = (from r in context.PuraskarApplicationForms
                                                   where r.RegnNo == RegnNo && r.applicationStatusID == statusid
                                                   select new
                                                   {
                                                       OnLineRefNN = r.OnlineRefNo
                                                   }).FirstOrDefault();
                              string s = Levels.OnLineRefNN.ToString();                            
                              int courseID = 0; Int32 Examid = 0; s = s.Substring(0, 1);                            
                              if (s == "O"){courseID = 1;}
                              if (s == "A"){courseID = 2;}
                              if (s == "B"){courseID = 3;}
                              if (s == "C"){courseID = 4;}
                              var ExamIds = (from r in context.Exams
                                             where r.ExamMonth == ExamMonth && r.ExamYear == ExamYear && r.CourseID == courseID
                                             select r).FirstOrDefault();
                              if (ExamIds != null)
                              {
                                   Examid = Convert.ToInt32(ExamIds.ID);
                              }
                              
                              //DEEP END on 14 Sep 2022

                              var PuraskarAppData = (from r in context.PuraskarApplicationForms
                                                     where r.RegnNo == RegnNo && r.applicationStatusID == statusid && r.ExamID == Examid
                                                     select r).FirstOrDefault();


                              if (PuraskarAppData != null)
                              {
                                  try
                                  {                                     
                                      PuraskarAppData.AmountReleaseDate =Convert.ToDateTime( dr[11].ToString()); // amount transfer date
                                      PuraskarAppData.TransactionId = transid.ToString(); // transaction id
                                      //PuraskarAppData.TransactionStatus = "Success";
                                      PuraskarAppData.TransactionStatus = dr[10].ToString(); // transaction status
                                      PuraskarAppData.isSettled = true;
                                      PuraskarAppData.settledBy = Convert.ToInt32(Session["UserID"]);
                                      PuraskarAppData.settledOn = DateTime.Now;
                                      PuraskarAppData.settledFileName = filename; // uploaded file name
                                      PuraskarAppData.Remarks = Convert.ToString(dr[12]).Trim(); // remarks
                                      PuraskarAppData.applicationStatusID = Convert.ToInt32(enmPuraskarApplicationStatus.PaymentTransferredToBankAccount);  // 9 for payment to bank A/c                                    
                                     
                                      context.SaveChanges();
                                      ValidateRecords = ValidateRecords + 1;
                                  }
                                  catch (Exception ex)
                                  {
                                      FaildRecords.Append(transid.ToString() + ",");
                                      failedRecordCount++;
                                      if (failedRecordCount % 10 == 0 && failedRecordCount > 0)
                                          FaildRecords.Append(WebUtility.HtmlDecode("<br/>"));
                                  }
                              }
                              else
                              {
                                  FaildRecords.Append(transid.ToString() + ",");
                                  failedRecordCount++;
                                  if (failedRecordCount % 10 == 0 && failedRecordCount > 0)
                                      FaildRecords.Append(WebUtility.HtmlDecode("<br/>"));
                              }
                              string remarks = Convert.ToString(dr[12]).Trim();
                              Int64 Amount = Convert.ToInt64(dr[3]);
                              SendEmail(RegnNo, remarks, Amount);
                              scope.Complete();
                          };
                      }
                    }
                };
                failedRecordCount = TotalRecords - ValidateRecords;
                lblTotalRecords.Text = TotalRecords.ToString();
                lblValidateRecords.Text = ValidateRecords.ToString();
                lblFailedRecords.Text = failedRecordCount.ToString() + WebUtility.HtmlDecode("<br/>") + FaildRecords.ToString().TrimEnd(',');
            }
           catch (Exception ex)
            {
                ShowAlert(ex.Message, true);
            }
            finally
            {
                dr.Close();
                dr.Dispose();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void SendEmail(Int64 RegnNo,string vRemarks,Int64 Amount)
    {
        string subject = "";
        String EmailMsg = "";
            try
            {
                using (EConnectContext context = new EConnectContext())
                {
                    var contactreg = (from s in context.RegistrationDetails 
                                      join p in context.CourseRegistrationApplications  on s.CandidateID equals p.CandidateID 
					 join q in context.Exams on p.ApplicableExamID equals q.ID // modified on 26 July 2023
                                      //join q in context.Exams on p.ID equals q.ExaminationCycleID
                                      join t in context.Courses on q.CourseID equals t.ID
                                     // join p in context.ExternalEntities on s.UserRefNumber equals p.ID
                                      where s.RegistrationNo  == RegnNo && t.ID==s.CourseID 
                                      && t.ID==p.CourseID 
                                      select new { EmailID=p.EmailAddress,Name=p.Name, EName=q.Name,level=t.Name}).FirstOrDefault();

                    if (contactreg.EmailID != null)
                    {
                        if (vRemarks.Trim().ToLower() == "aadhaar inactive" || vRemarks.Trim().ToLower().Contains("mapping") ||vRemarks.Trim().ToLower()!="success")
                        {
                   
                         EmailMsg = "Dear " + contactreg.Name + ",<br/> " + "NIELIT has released the scholarship payment to the eligible candidates for "
                            + contactreg.EName+ " on " + System.DateTime .Today .ToString ("dd/MMM/yyyy")+ ". <br/>" +
                        " The payment was processed through DBT through bank. The payment got declined and report of bank is attached herewith. <br/>"
                        +" 2. This is to inform you , that the payment got failed due to Aadhaar Inactive with your Bank a/c. Please get your Aadhaar "
                        +"no. mapped with National Payment Corporation of India (NPCI). Mapping of Aadhaar No. is mandatory for payments made through Aadhaar payment Bridge"
                        + "(APB).<br/> 3. It is, therefore,requested to kindly provide your Aadhaar No. along with the confirmation that your Aadhaar No. is "
                        +"mapped with the database of National Payment Corporation of India (NPCI) through revert mail.<br/>4. Kindly visit https://resident.uidai.gov.in/bank-mapper to"
                        +" confirm whether your Aadhaar No. is "
                        + "mapped with NPCI) or not .In case your Aadhaar No. is not mapped with the database of NPCI, then you should visit your bank branch"
                        +" for mapping the same.Further,you are also requested to send the confirmation of the same.<br/>"
                        +"5. You are required to send the details in the format given below: <br/>"
                        +"Name of the Candidate: <br/>Registration No.: <br/>Aadhaar No.: <br/>Bank Name: <br/>Bank Branch: <br/>"
                        +"A/c No.: <br/>IFSC Code: <br/> NIELIT";
                        EmailMsg = EmailMsg.Replace("\r\n", "<br/>");
                        subject = "Protsahan Puraskar " + contactreg.EName;
                        }
                        if (vRemarks.Trim().ToLower() == "success")
                        {

                             EmailMsg = "Dear " + contactreg.Name + ",<br/> " + "This is with reference to your application for award of Scholarship "
                                + " under NIELIT Protsahan Puraskar Scheme for SC/ST/Physically Handicapped/Female students based on your result for "
                            + contactreg.EName + " Examination of " + contactreg.level + " pursuing O/A/B/C level coursesthrough institutes authorized to conduct NIELIT courses. <br/>"
                            + " 2. You have been found to be eligible for Scholarship as per Rules. The amount of Rs." + Amount.ToString().Trim() + " has been processed through DBT process through Bank. <br/> NIELIT";
                            
                            EmailMsg = EmailMsg.Replace("\r\n", "<br/>");
                            subject = "Protsahan Puraskar for SC/ST/Physically Handicapped/Female students pursuing O/A/B/C level courses";
                        }
                   
                    }
                    else
                    {
                        ShowAlert("Email not available");
                        return;
                    }
                        //sending Email
                        if (contactreg.EmailID.Length > 0)
                        {
                            try
                            {
                                string MailTo = contactreg.EmailID + "; himanish@nielit.gov.in; chaman@nielit.gov.in";
                                EConnect.NIELIT.Email mail = new Email(subject, EmailMsg, MailTo);
                                mail.Send();
                            }
                            catch { ShowAlert(" E-mail not sent"); }
                        }
                    }
                   
                }
           
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
