using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;

public partial class Admin_InterRCExamReschedule : BasePage
{

    UserType loginUserType;
    Int64 entityID = 0;
   // Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
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
    }
  
    protected void btnDetails_Click(object sender, EventArgs e)
    {
        try
        {           
            string  appNumber = txtAppNum.Text;

            using (var context = new EConnectContext())
            {
                divDetails.Visible = true;
                var info = (from a in context.CertificateExamApplications.AsNoTracking()
                            where a.Number == appNumber
                            select new
                            {
                                // name = a.Candidate.Salutation + a.Candidate.Name,
                                name = a.Salutation + a.Name,
                                applicationNumber = a.Number,
                                rollno = a.RollNumber,
                                examcentrename = a.ExamCentreName,
                                examcentreaddress = a.ExamCentreAddress,
                                examdate = a.DateOfExam,
                                batchnumber = a.ExamBatchNumber,
                                reportingtime = a.ReportingTime,
                                resultgradeid = a.ResultGradeID

                            }).FirstOrDefault();

                //  txtName.Text = string.IsNullOrEmpty(info.name.ToString()) == true ? Lblerror.Text = " Please check the details " : info.name.ToString();
                txtName.Text = info.name.ToString();
                //return;
                string rcDetail = appNumber.Substring(0, 2);
                var regionalCentre = (from r in context.RegionalCenters
                                      where r.Code == rcDetail
                                      select new { rC = r.Name }).FirstOrDefault();

                txtRC.Text = regionalCentre.rC.ToString();
                if (info.rollno == null)
                {
                    txtRollNo.Text = "NA";
                }
                else
                {
                    txtRollNo.Text = info.rollno.ToString();
                }
                if (info.examcentrename == null)
                {
                    txtExamCentreName.Text = "NA";
                }
                else
                {
                    txtExamCentreName.Text = info.examcentrename.ToString();
                }
                if (info.examcentreaddress == null)
                {
                    txtExamCentreAddress.Text = "NA";
                }
                else
                {
                    txtExamCentreAddress.Text = info.examcentreaddress.ToString();
                }
                if (info.examdate == null)
                {
                    txtExamDate.Text = "NA";
                }
                else
                {
                    txtExamDate.Text = info.examdate.ToString().Substring(0, 9);
                }
                if (info.batchnumber == null)
                {
                    txtbatchNumber.Text = "NA";
                }
                else
                {
                    txtbatchNumber.Text = info.batchnumber.ToString();
                }
                if (info.reportingtime == null)
                {
                    txtReportingTime.Text = "NA";
                }
                else
                {
                    txtReportingTime.Text = info.reportingtime.ToString();
                }

                // txtRollNo.Text = string.IsNullOrEmpty(info.rollno.ToString()) == true ? "NULL" : info.rollno.ToString();
                //txtExamCentreName.Text = info.examcentrename.ToString();
                // txtExamCentreAddress.Text = info.examcentreaddress.ToString();
                //  txtExamDate.Text = info.examdate.ToString().Substring(0,9);
                // txtbatchNumber.Text = info.batchnumber.ToString();
                //txtReportingTime.Text = string.IsNullOrEmpty(info.reportingtime.ToString()) == true ? "NA" : info.reportingtime.ToString(); //info.reportingtime.ToString();
                txtResultGradeId.Text = string.IsNullOrEmpty(info.resultgradeid.ToString()) == true ? "NA" : info.resultgradeid.ToString();
            }                   
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
        finally
        {
          
        }
    }
    protected void ddlCheck_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlCheck.SelectedValue) == 0)
        {
            divCentreDetails.Visible = false;
            divRCDetails.Visible = false;
        }

        if (Convert.ToInt32(ddlCheck.SelectedValue) == 1)
        {
            divRCDetails.Visible = true;
            divCentreDetails.Visible = false;  
        }

        if (Convert.ToInt32(ddlCheck.SelectedValue) == 2)
        {
            divCentreDetails.Visible = true;
            divRCDetails.Visible = false;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {

        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        try
        {
            SqlCommand cmd = new SqlCommand("Inter_RC_Admit_Card_Request_For_Change", con);
           
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
           
            con.Open();

            string appNumberRc = txtAppNum.Text;
            string rollNumberRc = txtRcRollNum.Text;
            string centreNameRc = txtECC.Text;
            string centreAddress = txtECA.Text;
            DateTime examDateRc = Convert.ToDateTime(txtRcED.Text);
            int batchNumberRc = Convert.ToInt32(txtRcBN.Text);
            string reportingTimeRc = txtRcRT.Text;
            Int32 userNo = loginUserNo;

            cmd.Parameters.Add(new SqlParameter("@Pnumber", SqlDbType.NVarChar));
            cmd.Parameters["@Pnumber"].Value = appNumberRc;

            cmd.Parameters.Add(new SqlParameter("@Pnew_roll_no", SqlDbType.NVarChar));
            cmd.Parameters["@Pnew_roll_no"].Value = rollNumberRc;

            cmd.Parameters.Add(new SqlParameter("@Pnew_Exam_Centre_Name", SqlDbType.NVarChar));
            cmd.Parameters["@Pnew_Exam_Centre_Name"].Value = centreNameRc;

            cmd.Parameters.Add(new SqlParameter("@Pnew_Exam_Centre_Address", SqlDbType.NVarChar));
            cmd.Parameters["@Pnew_Exam_Centre_Address"].Value = centreAddress;

            cmd.Parameters.Add(new SqlParameter("@Pnew_Date_of_Exam", SqlDbType.Date));
            cmd.Parameters["@Pnew_Date_of_Exam"].Value = examDateRc;

            cmd.Parameters.Add(new SqlParameter("@Pnew_Exam_Batch_Number", SqlDbType.Int));
            cmd.Parameters["@Pnew_Exam_Batch_Number"].Value = batchNumberRc;

            cmd.Parameters.Add(new SqlParameter("@Pnew_Reporting_Time", SqlDbType.NVarChar));
            cmd.Parameters["@Pnew_Reporting_Time"].Value = reportingTimeRc;

            cmd.Parameters.Add(new SqlParameter("@PUser_no", SqlDbType.Int));
            cmd.Parameters["@PUser_no"].Value = userNo;
                
            cmd.ExecuteNonQuery();
            ShowAlert(" Records Updated Successfully");
 
            con.Close();
           
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void btnSaveCr_Click(object sender, EventArgs e)
    {
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        try
        {
            SqlCommand cmd = new SqlCommand("Within_RC_Admit_Card_Request_For_Delete_privious_data", con);

            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();

            string appNoCr = txtAppNum.Text;
            string rollNoCr = txtRcRollNum.Text;
            Int32 userNo = loginUserNo;

            cmd.Parameters.Add(new SqlParameter("@Pnumber", SqlDbType.NVarChar));
            cmd.Parameters["@Pnumber"].Value = appNoCr;

            cmd.Parameters.Add(new SqlParameter("@Proll_no", SqlDbType.NVarChar));
            cmd.Parameters["@Proll_no"].Value = rollNoCr;

            cmd.Parameters.Add(new SqlParameter("@PUser_no", SqlDbType.Int));
            cmd.Parameters["@PUser_no"].Value = userNo;

            cmd.ExecuteNonQuery();
            ShowAlert(" Records Updated Successfully");

            con.Close();

            Lblerror.Visible = true;
            Lblerror.Text = "Previous Admit Card Data has been removed, Now you can proceed to upload new Admit Card.";
        }
        catch( Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
  
  
}