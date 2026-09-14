using System;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.UI;
using EConnect;
using System.Net;
using EConnect.DAL;
using EConnect.Utils.Data;
using System.Data.SqlClient;
using System.Configuration;
using EConnect.NIELIT;
using EConnect.URM;
//using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web;
using System.Transactions;
using EConnect.Utils.Common;
using System.Drawing;
using ClosedXML.Excel;
//using Microsoft.Office.Interop.Word;
using Independentsoft.Office;
using Independentsoft.Office.Word;
using Independentsoft.Office.Word.Sections;
using Independentsoft.Office.Word.Tables;
using Independentsoft.Office.Drawing;
using Independentsoft.Office.Word.Drawing;
using Independentsoft.Office.Word.Styles;
using System.Text;

public partial class PurskarCommitteeReport : BasePage
{
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;
    string examName = "";
    System.Web.UI.WebControls.Table tbl = new System.Web.UI.WebControls.Table();
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
           // currentRoleId = 6;
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                ExamName();
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Purskar Committee Report", "HO/PurskarCommitteeReport.aspx", ""));
                 
                 
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("PurskarCommitteeReport.aspx", true);
    }
      

    public System.Data.DataTable GetDataForPurskarApplicationCommitteeReport(int ViewCode)
    {
        string ExamMonthYear = ddlflExam.SelectedValue;
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        System.Data.DataTable myDt = new System.Data.DataTable();
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("GetDataForPurskarApplicationCommitteeReport", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ViewRecord", SqlDbType.Int));
                cmd.Parameters["@ViewRecord"].Value = ViewCode;
                cmd.Parameters.Add(new SqlParameter("@ExamMonthYear", SqlDbType.VarChar));
                cmd.Parameters["@ExamMonthYear"].Value = ExamMonthYear;

                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(myDt);
                }
            }
        }
        return myDt;
    }   
   
    
    protected void btnDownload3_Click(object sender, EventArgs e)
    {
        CreateDocument();
    }
    public System.Data.DataTable GetDataForPurskarApplicationCommitteeReportT(int ViewCode)
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
    public void ExamName()
    {
        ddlflExam.Items.Clear();
        System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
        using (System.Data.DataTable dt = GetDataForPurskarApplicationCommitteeReportT(1))
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
    //public void ExamName()
    //{
    //    System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--All--", "0");
    //    using (System.Data.DataTable dt = GetDataForPurskarApplicationCommitteeReport(1))
    //    {
    //        if (dt.Rows.Count > 0)
    //        {
    //            int k;
    //            for (k = 0; k < 1; k++)
    //            {
    //                ddlflExam.DataSource = dt;
    //                ddlflExam.DataTextField = "ExamName";
    //                ddlflExam.DataValueField = "ExamID";
    //                ddlflExam.DataBind();
                    
    //            }
    //        }
    //        else
    //        {
    //            ddlflExam.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select One--", "0"));
    //        }
    //    }

    //}
    private void CreateDocument()
    {
        try
        {
            #region--DATABASE VALUE--------------------------------------------
            // Data fetch from database for report on 28 jan 2022
            int courseID = 0, OExamId = 0, AExamId = 0, BExamId = 0, CExamId = 0;
            string OLevelExamName = string.Empty, ALevelExamName = string.Empty, BLevelExamName = string.Empty, CLevelExamName = string.Empty;
            string OExamdate = string.Empty, AExamdate = string.Empty, BExamdate = string.Empty, CExamdate = string.Empty;
            string OResultdate = string.Empty, AResultdate = string.Empty, BResultdate = string.Empty, CResultdate = string.Empty;
            string OLastdate = string.Empty, ALastdate = string.Empty, BLastdate = string.Empty, CLastdate = string.Empty;

            using (System.Data.DataTable dt2 = GetDataForPurskarApplicationCommitteeReport(1))
            {
                if (dt2.Rows.Count > 0)
                {
                    int k;
                    for (k = 0; k < dt2.Rows.Count; k++)
                    {
                        courseID = Convert.ToInt32(dt2.Rows[k]["CourseID"].ToString());
                        if (courseID == 1)
                        {
                            OLevelExamName = dt2.Rows[k]["ExamName"].ToString();
                          DateTime  OExamdate1 = Convert.ToDateTime( dt2.Rows[k]["Exam_Start_Date"]);
                          OExamdate = OExamdate1.ToString("dd-MMM-yyyy");
                          DateTime  OResultdate1 =Convert.ToDateTime( dt2.Rows[k]["Result_Publish_Date"]);
                          OResultdate = OResultdate1.ToString("dd-MMM-yyyy");
                            DateTime OOResultdate = Convert.ToDateTime(OResultdate);
                            DateTime LastDate = OOResultdate.AddDays(45);
                            OLastdate = LastDate.ToString("dd-MMM-yyyy");
                        }

                        if (courseID == 2)
                        {
                            ALevelExamName = dt2.Rows[k]["ExamName"].ToString();
                            DateTime AExamdate1 = Convert.ToDateTime(dt2.Rows[k]["Exam_Start_Date"]);
                            AExamdate = AExamdate1.ToString("dd-MMM-yyyy");
                            DateTime AResultdate1 = Convert.ToDateTime(dt2.Rows[k]["Result_Publish_Date"]);
                            AResultdate = AResultdate1.ToString("dd-MMM-yyyy");
                            DateTime AAResultdate = Convert.ToDateTime(AResultdate);
                            DateTime LastDate = AAResultdate.AddDays(45);
                            ALastdate = LastDate.ToString("dd-MMM-yyyy");
                        }
                        if (courseID == 3)
                        {
                            BLevelExamName = dt2.Rows[k]["ExamName"].ToString();
                            DateTime BExamdate1 = Convert.ToDateTime(dt2.Rows[k]["Exam_Start_Date"]);
                            BExamdate = BExamdate1.ToString("dd-MMM-yyyy");
                            DateTime BResultdate1 = Convert.ToDateTime(dt2.Rows[k]["Result_Publish_Date"]);
                            BResultdate = BResultdate1.ToString("dd-MMM-yyyy");
                            DateTime BBResultdate = Convert.ToDateTime(BResultdate);
                            DateTime LastDate = BBResultdate.AddDays(45);
                            BLastdate = LastDate.ToString("dd-MMM-yyyy");
                        }
                        if (courseID == 4)
                        {
                            CLevelExamName = dt2.Rows[k]["ExamName"].ToString();
                            DateTime CExamdate1 = Convert.ToDateTime(dt2.Rows[k]["Exam_Start_Date"]);
                            CExamdate = CExamdate1.ToString("dd-MMM-yyyy");
                            DateTime CResultdate1 = Convert.ToDateTime(dt2.Rows[k]["Result_Publish_Date"]);
                            CResultdate = CResultdate1.ToString("dd-MMM-yyyy");
                            DateTime CCResultdate = Convert.ToDateTime(CResultdate);
                            DateTime LastDate = CCResultdate.AddDays(45);
                            CLastdate = LastDate.ToString("dd-MMM-yyyy");
                        }
                        
                    }
                }
            }

            // no of application recieved by course id wise
            string  ORec = "", ARec = "", BRec = "", CRec = "";
            string OWithhold = "", AWithhold = "", BWithhold = "", CWithhold = "";
            string ORejected = "", ARejected = "", BRejected = "", CRejected = "";
            int TotalApplication = 0,Oapp=0,Aapp=0,Bapp=0,Capp=0;
            using (System.Data.DataTable dt2 = GetDataForPurskarApplicationCommitteeReport(2))
            {
                if (dt2.Rows.Count > 0)
                {
                    int k;
                    for (k = 0; k < dt2.Rows.Count; k++)
                    {
                        courseID = Convert.ToInt32(dt2.Rows[k]["CourseID"].ToString());
                        if (courseID == 1)
                        {
                            ORec = dt2.Rows[k]["Recommended"].ToString();
                            OWithhold = dt2.Rows[k]["Withhold"].ToString();
                            ORejected = dt2.Rows[k]["Rejected"].ToString();
                            Oapp = Convert.ToInt32(ORec) + Convert.ToInt32(OWithhold) + Convert.ToInt32(ORejected);
                        }

                        if (courseID == 2)
                        {
                            ARec = dt2.Rows[k]["Recommended"].ToString();
                            AWithhold = dt2.Rows[k]["Withhold"].ToString();
                            ARejected = dt2.Rows[k]["Rejected"].ToString();
                            Aapp = Convert.ToInt32(ARec) + Convert.ToInt32(AWithhold) + Convert.ToInt32(ARejected);
                        }
                        if (courseID == 3)
                        {
                            BRec = dt2.Rows[k]["Recommended"].ToString();
                            BWithhold = dt2.Rows[k]["Withhold"].ToString();
                            BRejected = dt2.Rows[k]["Rejected"].ToString();
                            Bapp = Convert.ToInt32(BRec) + Convert.ToInt32(BWithhold) + Convert.ToInt32(BRejected);
                        }
                        if (courseID == 4)
                        {
                            CRec = dt2.Rows[k]["Recommended"].ToString();
                            CWithhold = dt2.Rows[k]["Withhold"].ToString();
                            CRejected = dt2.Rows[k]["Rejected"].ToString();
                            Capp = Convert.ToInt32(CRec) + Convert.ToInt32(CWithhold) + Convert.ToInt32(CRejected);
                        }

                    }
                    TotalApplication = Oapp + Aapp + Bapp + Capp;

                }
            }


            // SC/ST, PwD & Female Candidates
            //CourseID,  MaleSC , MaleST  , MalePWD , MaleGen , MaleOBC , FeMaleSC , FeMaleST  , FeMalePWD , FeMaleGen , FeMaleOBC
            string OMaleSC = "", OMaleST = "", OMalePWD = "", OMaleGen = "", OMaleOBC = "", OFeMaleSC = "", OFeMaleST = "", OFeMalePWD = "", OFeMaleGen = "", OFeMaleOBC = "";
            string AMaleSC = "", AMaleST = "", AMalePWD = "", AMaleGen = "", AMaleOBC = "", AFeMaleSC = "", AFeMaleST = "", AFeMalePWD = "", AFeMaleGen = "", AFeMaleOBC = "";
            string BMaleSC = "", BMaleST = "", BMalePWD = "", BMaleGen = "", BMaleOBC = "", BFeMaleSC = "", BFeMaleST = "", BFeMalePWD = "", BFeMaleGen = "", BFeMaleOBC = "";
            string CMaleSC = "", CMaleST = "", CMalePWD = "", CMaleGen = "", CMaleOBC = "", CFeMaleSC = "", CFeMaleST = "", CFeMalePWD = "", CFeMaleGen = "", CFeMaleOBC = "";
            string TotalApplicationRO = "",  TotalApplicationRA = "", TotalApplicationRB = "", TotalApplicationRC = "";
            using (System.Data.DataTable dt2 = GetDataForPurskarApplicationCommitteeReport(3))
            {
                if (dt2.Rows.Count > 0)
                {
                    int k;
                    for (k = 0; k < dt2.Rows.Count; k++)
                    {
                        courseID = Convert.ToInt32(dt2.Rows[k]["CourseID"].ToString());
                        if (courseID == 1)
                        {
                            OMaleSC = dt2.Rows[k]["MaleSC"].ToString();
                            OMaleST = dt2.Rows[k]["MaleST"].ToString();
                            OMalePWD = dt2.Rows[k]["MalePWD"].ToString();
                            OMaleGen = dt2.Rows[k]["MaleGen"].ToString();
                            OMaleOBC = dt2.Rows[k]["MaleOBC"].ToString();

                            OFeMaleSC = dt2.Rows[k]["FeMaleSC"].ToString();
                            OFeMaleST = dt2.Rows[k]["FeMaleST"].ToString();
                            OFeMalePWD = dt2.Rows[k]["FeMalePWD"].ToString();
                            OFeMaleGen = dt2.Rows[k]["FeMaleGen"].ToString();
                            OFeMaleOBC = dt2.Rows[k]["FeMaleOBC"].ToString();

                            TotalApplicationRO = (Convert.ToInt32(OMaleSC) + Convert.ToInt32(OMaleST) + Convert.ToInt32(OMalePWD) + Convert.ToInt32(OMaleGen) + Convert.ToInt32(OMaleOBC) +
                                Convert.ToInt32(OFeMaleSC) + Convert.ToInt32(OFeMaleST) + Convert.ToInt32(OFeMalePWD) + Convert.ToInt32(OFeMaleGen) + Convert.ToInt32(OFeMaleOBC)).ToString();
                        }

                        if (courseID == 2)
                        {
                            AMaleSC = dt2.Rows[k]["MaleSC"].ToString();
                            AMaleST = dt2.Rows[k]["MaleST"].ToString();
                            AMalePWD = dt2.Rows[k]["MalePWD"].ToString();
                            AMaleGen = dt2.Rows[k]["MaleGen"].ToString();
                            AMaleOBC = dt2.Rows[k]["MaleOBC"].ToString();

                            AFeMaleSC = dt2.Rows[k]["FeMaleSC"].ToString();
                            AFeMaleST = dt2.Rows[k]["FeMaleST"].ToString();
                            AFeMalePWD = dt2.Rows[k]["FeMalePWD"].ToString();
                            AFeMaleGen = dt2.Rows[k]["FeMaleGen"].ToString();
                            AFeMaleOBC = dt2.Rows[k]["FeMaleOBC"].ToString();

                            TotalApplicationRA = (Convert.ToInt32(AMaleSC) + Convert.ToInt32(AMaleST) + Convert.ToInt32(AMalePWD) + Convert.ToInt32(AMaleGen) + Convert.ToInt32(AMaleOBC) +
                                Convert.ToInt32(AFeMaleSC) + Convert.ToInt32(AFeMaleST) + Convert.ToInt32(AFeMalePWD) + Convert.ToInt32(AFeMaleGen) + Convert.ToInt32(AFeMaleOBC)).ToString();
                        }
                        if (courseID == 3)
                        {
                            BMaleSC = dt2.Rows[k]["MaleSC"].ToString();
                            BMaleST = dt2.Rows[k]["MaleST"].ToString();
                            BMalePWD = dt2.Rows[k]["MalePWD"].ToString();
                            BMaleGen = dt2.Rows[k]["MaleGen"].ToString();
                            BMaleOBC = dt2.Rows[k]["MaleOBC"].ToString();

                            BFeMaleSC = dt2.Rows[k]["FeMaleSC"].ToString();
                            BFeMaleST = dt2.Rows[k]["FeMaleST"].ToString();
                            BFeMalePWD = dt2.Rows[k]["FeMalePWD"].ToString();
                            BFeMaleGen = dt2.Rows[k]["FeMaleGen"].ToString();
                            BFeMaleOBC = dt2.Rows[k]["FeMaleOBC"].ToString();

                            TotalApplicationRB = (Convert.ToInt32(BMaleSC) + Convert.ToInt32(BMaleST) + Convert.ToInt32(BMalePWD) + Convert.ToInt32(BMaleGen) + Convert.ToInt32(BMaleOBC) +
                                Convert.ToInt32(BFeMaleSC) + Convert.ToInt32(BFeMaleST) + Convert.ToInt32(BFeMalePWD) + Convert.ToInt32(BFeMaleGen) + Convert.ToInt32(BFeMaleOBC)).ToString();
                        }
                        if (courseID == 4)
                        {
                            CMaleSC = dt2.Rows[k]["MaleSC"].ToString();
                            CMaleST = dt2.Rows[k]["MaleST"].ToString();
                            CMalePWD = dt2.Rows[k]["MalePWD"].ToString();
                            CMaleGen = dt2.Rows[k]["MaleGen"].ToString();
                            CMaleOBC = dt2.Rows[k]["MaleOBC"].ToString();

                            CFeMaleSC = dt2.Rows[k]["FeMaleSC"].ToString();
                            CFeMaleST = dt2.Rows[k]["FeMaleST"].ToString();
                            CFeMalePWD = dt2.Rows[k]["FeMalePWD"].ToString();
                            CFeMaleGen = dt2.Rows[k]["FeMaleGen"].ToString();
                            CFeMaleOBC = dt2.Rows[k]["FeMaleOBC"].ToString();

                            TotalApplicationRC = (Convert.ToInt32(CMaleSC) + Convert.ToInt32(CMaleST) + Convert.ToInt32(CMalePWD) + Convert.ToInt32(CMaleGen) + Convert.ToInt32(CMaleOBC) +
                                Convert.ToInt32(CFeMaleSC) + Convert.ToInt32(CFeMaleST) + Convert.ToInt32(CFeMalePWD) + Convert.ToInt32(CFeMaleGen) + Convert.ToInt32(CFeMaleOBC)).ToString();
                        }

                    }
                   

                }
            }

            // FOR WITHHOLD CASE DISPLAY DATA ON REPORT
            // SC/ST, PwD & Female Candidates
            //CourseID,  MaleSC , MaleST  , MalePWD , MaleGen , MaleOBC , FeMaleSC , FeMaleST  , FeMalePWD , FeMaleGen , FeMaleOBC
            string OMaleSC1 = "", OMaleST1 = "", OMalePWD1 = "", OMaleGen1 = "", OMaleOBC1 = "", OFeMaleSC1 = "", OFeMaleST1 = "", OFeMalePWD1 = "", OFeMaleGen1 = "", OFeMaleOBC1 = "";
            string AMaleSC1 = "", AMaleST1 = "", AMalePWD1 = "", AMaleGen1 = "", AMaleOBC1 = "", AFeMaleSC1 = "", AFeMaleST1 = "", AFeMalePWD1 = "", AFeMaleGen1 = "", AFeMaleOBC1 = "";
            string BMaleSC1 = "", BMaleST1 = "", BMalePWD1 = "", BMaleGen1 = "", BMaleOBC1 = "", BFeMaleSC1 = "", BFeMaleST1 = "", BFeMalePWD1 = "", BFeMaleGen1 = "", BFeMaleOBC1 = "";
            string CMaleSC1 = "", CMaleST1 = "", CMalePWD1= "", CMaleGen1 = "", CMaleOBC1 = "", CFeMaleSC1 = "", CFeMaleST1 = "", CFeMalePWD1 = "", CFeMaleGen1 = "", CFeMaleOBC1 = "";
            string TotalApplicationRO1 = "", TotalApplicationRA1 = "", TotalApplicationRB1 = "", TotalApplicationRC1 = "";
            using (System.Data.DataTable dt5 = GetDataForPurskarApplicationCommitteeReport(4))
            {
                if (dt5.Rows.Count > 0)
                {
                    int k;
                    for (k = 0; k < dt5.Rows.Count; k++)
                    {
                        courseID = Convert.ToInt32(dt5.Rows[k]["CourseID"].ToString());
                        if (courseID == 1)
                        {
                            OMaleSC1 = dt5.Rows[k]["MaleSC1"].ToString();
                            OMaleST1 = dt5.Rows[k]["MaleST1"].ToString();
                            OMalePWD1 = dt5.Rows[k]["MalePWD1"].ToString();
                            OMaleGen1 = dt5.Rows[k]["MaleGen1"].ToString();
                            OMaleOBC1 = dt5.Rows[k]["MaleOBC1"].ToString();

                            OFeMaleSC1 = dt5.Rows[k]["FeMaleSC1"].ToString();
                            OFeMaleST1 = dt5.Rows[k]["FeMaleST1"].ToString();
                            OFeMalePWD1 = dt5.Rows[k]["FeMalePWD1"].ToString();
                            OFeMaleGen1 = dt5.Rows[k]["FeMaleGen1"].ToString();
                            OFeMaleOBC1 = dt5.Rows[k]["FeMaleOBC1"].ToString();

                            TotalApplicationRO1 = (Convert.ToInt32(OMaleSC1) + Convert.ToInt32(OMaleST1) + Convert.ToInt32(OMalePWD1) + Convert.ToInt32(OMaleGen1) + Convert.ToInt32(OMaleOBC1) +
                                Convert.ToInt32(OFeMaleSC1) + Convert.ToInt32(OFeMaleST1) + Convert.ToInt32(OFeMalePWD1) + Convert.ToInt32(OFeMaleGen1) + Convert.ToInt32(OFeMaleOBC1)).ToString();
                        }

                        if (courseID == 2)
                        {
                            AMaleSC1 = dt5.Rows[k]["MaleSC1"].ToString();
                            AMaleST1 = dt5.Rows[k]["MaleST1"].ToString();
                            AMalePWD1 = dt5.Rows[k]["MalePWD1"].ToString();
                            AMaleGen1 = dt5.Rows[k]["MaleGen1"].ToString();
                            AMaleOBC1 = dt5.Rows[k]["MaleOBC1"].ToString();

                            AFeMaleSC1 = dt5.Rows[k]["FeMaleSC1"].ToString();
                            AFeMaleST1 = dt5.Rows[k]["FeMaleST1"].ToString();
                            AFeMalePWD1 = dt5.Rows[k]["FeMalePWD1"].ToString();
                            AFeMaleGen1 = dt5.Rows[k]["FeMaleGen1"].ToString();
                            AFeMaleOBC1 = dt5.Rows[k]["FeMaleOBC1"].ToString();

                            TotalApplicationRA1 = (Convert.ToInt32(AMaleSC1) + Convert.ToInt32(AMaleST1) + Convert.ToInt32(AMalePWD1) + Convert.ToInt32(AMaleGen1) + Convert.ToInt32(AMaleOBC1) +
                                Convert.ToInt32(AFeMaleSC1) + Convert.ToInt32(AFeMaleST1) + Convert.ToInt32(AFeMalePWD1) + Convert.ToInt32(AFeMaleGen1) + Convert.ToInt32(AFeMaleOBC1)).ToString();
                        }
                        if (courseID == 3)
                        {
                            BMaleSC1 = dt5.Rows[k]["MaleSC1"].ToString();
                            BMaleST1 = dt5.Rows[k]["MaleST1"].ToString();
                            BMalePWD1 = dt5.Rows[k]["MalePWD1"].ToString();
                            BMaleGen1 = dt5.Rows[k]["MaleGen1"].ToString();
                            BMaleOBC1 = dt5.Rows[k]["MaleOBC1"].ToString();

                            BFeMaleSC1 = dt5.Rows[k]["FeMaleSC1"].ToString();
                            BFeMaleST1 = dt5.Rows[k]["FeMaleST1"].ToString();
                            BFeMalePWD1 = dt5.Rows[k]["FeMalePWD1"].ToString();
                            BFeMaleGen1 = dt5.Rows[k]["FeMaleGen1"].ToString();
                            BFeMaleOBC1 = dt5.Rows[k]["FeMaleOBC1"].ToString();

                            TotalApplicationRB1 = (Convert.ToInt32(BMaleSC1) + Convert.ToInt32(BMaleST1) + Convert.ToInt32(BMalePWD1) + Convert.ToInt32(BMaleGen1) + Convert.ToInt32(BMaleOBC1) +
                                Convert.ToInt32(BFeMaleSC1) + Convert.ToInt32(BFeMaleST1) + Convert.ToInt32(BFeMalePWD1) + Convert.ToInt32(BFeMaleGen1) + Convert.ToInt32(BFeMaleOBC1)).ToString();
                        }
                        if (courseID == 4)
                        {
                            CMaleSC1 = dt5.Rows[k]["MaleSC1"].ToString();
                            CMaleST1 = dt5.Rows[k]["MaleST1"].ToString();
                            CMalePWD1 = dt5.Rows[k]["MalePWD1"].ToString();
                            CMaleGen1 = dt5.Rows[k]["MaleGen1"].ToString();
                            CMaleOBC1 = dt5.Rows[k]["MaleOBC1"].ToString();

                            CFeMaleSC1 = dt5.Rows[k]["FeMaleSC1"].ToString();
                            CFeMaleST1 = dt5.Rows[k]["FeMaleST1"].ToString();
                            CFeMalePWD1 = dt5.Rows[k]["FeMalePWD1"].ToString();
                            CFeMaleGen1 = dt5.Rows[k]["FeMaleGen1"].ToString();
                            CFeMaleOBC1 = dt5.Rows[k]["FeMaleOBC1"].ToString();

                            TotalApplicationRC1 = (Convert.ToInt32(CMaleSC1) + Convert.ToInt32(CMaleST1) + Convert.ToInt32(CMalePWD1) + Convert.ToInt32(CMaleGen1) + Convert.ToInt32(CMaleOBC1) +
                                Convert.ToInt32(CFeMaleSC1) + Convert.ToInt32(CFeMaleST1) + Convert.ToInt32(CFeMalePWD1) + Convert.ToInt32(CFeMaleGen1) + Convert.ToInt32(CFeMaleOBC1)).ToString();
                        }

                    }
                    TotalApplication = Oapp + Aapp + Bapp + Capp;

                }
            }
            //END FOR WITHHOLD CASE DISPALY DATA ON REPORT
            // data fetch end on 28 jan 2022         
            #endregion ----------------------------------------------------------------------------------------------------------

            #region ---------- Page Margin ------------------------------------------------------
            //Create a missing variable for missing value  
            object missing = System.Reflection.Missing.Value;

            //Create an instance for word app                     
            WordDocument doc = new WordDocument();

            PageMargins margins = new PageMargins();
            margins.Bottom = 450;// 1440; // 1 inch 
            margins.Left = 1080; // 1 inch
            margins.Right = 1080; // 1 inch
            margins.Top = 450; // 1 inch
            margins.Footer = 90; // 1/2 inch 
            margins.Header = 90; // 1/2 inch

            PageSize pageSize = new PageSize(12140, 16416); //8.5 x 11 inch
            pageSize.PageOrientation = PageOrientation.Portrait;

            Section section = new Section();
            section.PageSize = pageSize;
            section.PageMargins = margins;            
            doc.Body.Section = section;

            #endregion -----------------------------------------------------------------------------

            #region--HEADER OF REPORT--------------------------------------------
            // Heading add started
            string folderPath1 = Server.MapPath("~/images/");
            string filePath1 = folderPath1 + Path.GetFileName("LetterHeader.jpg");

            //Picture picture = new Picture("../images/LetterHeader.jpg");
            Picture picture = new Picture(filePath1);
            Unit pictureWidth = new Unit(640, UnitType.Pixel);
            Unit pictureHeight = new Unit(30, UnitType.Pixel);

            Offset offset = new Offset(0, 0);
            Extents extents = new Extents(pictureWidth, pictureHeight);

            picture.ShapeProperties.PresetGeometry = new PresetGeometry(ShapeType.Rectangle);
            picture.ShapeProperties.Transform2D = new Transform2D(offset, extents);
            picture.ID = "1";
            picture.Name = "LetterHeader.jpg";

            Stretch stretch = new Stretch(); //important to scale image
            stretch.FillRectangle = new FillRectangle();
            picture.Stretch = stretch;

            Inline inline = new Inline(picture);
            inline.Size = new DrawingObjectSize(pictureWidth, pictureHeight);
            inline.ID = "1";
            inline.Name = "Picture 1";
            inline.Description = "LetterHeader.jpg";

            DrawingObject drawingObject = new DrawingObject(inline);

            Run imageRun = new Run();
            imageRun.Add(drawingObject);

            Paragraph imageParagraph = new Paragraph();
            imageParagraph.Add(imageRun);
            imageParagraph.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            imageParagraph.Spacing = new Spacing();
            imageParagraph.Spacing.After = 2;
            doc.Body.Add(imageParagraph);
           

            Run R1 = new Run();
            R1.AddText("Dated: "+System.DateTime.Now.ToString("dd-MMM-yyyy"));
            R1.FontSize = 18; //12 points
            R1.AsciiFont = "Century Gothic";
            //R1.Bold = ExtendedBoolean.True; // for bold  
            Paragraph P1 = new Paragraph();
            P1.Add(R1);
            P1.HorizontalTextAlignment = HorizontalAlignmentType.Right;          
            doc.Body.Add(P1);

            // Heading add end
            #endregion -----------------------------------------------------------------------------

            #region -----SUBJECT OF REPORT ------------------------------
            // Subject add
            Run R2 = new Run();
            R2.AddText("Subject : Protsahan Puraskar (formerly Scholarship) payment for SC/ST/PwD and Female candidates appeared in NIELIT Examination held in ........ Exams. ");
            R2.FontSize = 18; //12 points
            R2.AsciiFont = "Century Gothic";
            R2.Bold = ExtendedBoolean.True; // for bold  
            Paragraph P2 = new Paragraph();
            P2.Add(R2);
            P2.HorizontalTextAlignment = HorizontalAlignmentType.Left;
            doc.Body.Add(P2);
            //Subject add en
            #endregion -----------------------------------------------------------------------------

            #region --- Point first ------------------------------------
            // First Line add
            Run R3 = new Run();
            R3.AddText(" With the approval of Governing Council in its meeting held on .............. with effect from " +
                  " ............ examinations onwards, NIELIT (formerly DOEACC Society) has introduced a Scholarship Scheme for SC/ST,  " +
                  "PwD and Female candidates appearing through accredited Institutes which has now been renamed as Protsahan " +
                  "Puraskar w.e.f ............ Examinations.");
            R3.FontSize = 18; //12 points
            R3.AsciiFont = "Century Gothic"; 
            Paragraph P3 = new Paragraph();
            P3.Add(R3);
            P3.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            doc.Body.Add(P3);
            //First Line add end
            #endregion -----------------------------------------------------------------------------

            #region --- Point second  with table 1 records----------------------------------------------
            
            // Second Line add
            Run R4 = new Run();
            R4.AddText("2.  Last date for submission of Protsahan Puraskar  "+
            "(formerly Scholarship) application forms for " + OLevelExamName + " O/A/B/C Level Examination was " + OExamdate + " i.e. 45-days from the date " +
            "of declaration of result, as per details given below: -");
            R4.FontSize = 18; //12 points
            R4.AsciiFont = "Century Gothic";
            Paragraph P4 = new Paragraph();
            P4.Add(R4);
            P4.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            doc.Body.Add(P4);
            //Second Line add end

            #region ---------- Table 1 for examination Details -----------------------------------------------------
            //First Table with Merge cell Start
            TableGrid tableGrid = new TableGrid();
            tableGrid.Columns.Add(new TableGridColumn(498));
            tableGrid.Columns.Add(new TableGridColumn(1200));
            tableGrid.Columns.Add(new TableGridColumn(1200));

            #region ---------- Row 1 Heading -------------------------------------------------------
            // First row start 
            Run CellValue = new Run(OLevelExamName + " Examinations");
            CellValue.Bold = ExtendedBoolean.True;
            CellValue.FontSize = 18; //18 points
            CellValue.AsciiFont = "Century Gothic";
            Paragraph CellAddValue = new Paragraph();
            CellAddValue.Spacing = new Spacing();
            CellAddValue.Spacing.After = 0;
            CellAddValue.Add(CellValue);
            CellAddValue.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell11 = new Cell();
            cell11.VerticallyMergedCell = new VerticallyMergedCell();
            cell11.GridSpan = 3;
            cell11.VerticallyMergedCell.Type = MergeCellType.Restart;
            cell11.Width = new Width(TableWidthUnit.Point, 900);
            cell11.Shading = new Shading(ShadingPattern.Percent10);
            cell11.VerticalAlignment = VerticalAlignmentType.Center;
            cell11.Add(CellAddValue);
            Row row1 = new Row();
            row1.Add(cell11);
            row1.Alignment = HorizontalAlignmentType.Center;
            // First row End 
            #endregion -----------------------------------------------------------------------------

            #region -----------  Row 2  ----------------------------------------------------

            // Second row start 
            Run CellValue21 = new Run("Level");
            CellValue21.FontSize = 18; //18 points
            CellValue21.AsciiFont = "Century Gothic";
            Paragraph CellAddValue21 = new Paragraph();
            CellAddValue21.Spacing = new Spacing();
            CellAddValue21.Spacing.After = 0;
            CellAddValue21.Add(CellValue21);
            CellAddValue21.HorizontalTextAlignment = HorizontalAlignmentType.Center;

            Cell cell21 = new Cell();
            cell21.Width = new Width(TableWidthUnit.Point, 900);
            cell21.VerticalAlignment = VerticalAlignmentType.Center;
            cell21.Add(CellAddValue21);

            Run CellValue22 = new Run("Declaration of Results");
            CellValue22.FontSize = 18; //18 points
            CellValue22.AsciiFont = "Century Gothic";
            Paragraph CellAddValue22 = new Paragraph();
            CellAddValue22.Spacing = new Spacing();
            CellAddValue22.Spacing.After = 0;
            CellAddValue22.Add(CellValue22);
            CellAddValue22.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell22 = new Cell();
            cell22.Width = new Width(TableWidthUnit.Point, 2400);
            cell22.VerticalAlignment = VerticalAlignmentType.Center;
            cell22.Add(CellAddValue22);

            Run CellValue23 = new Run("Last Date for Receipt of Scholarship Forms");
            CellValue23.FontSize = 18; //18 points
            CellValue23.AsciiFont = "Century Gothic";
            Paragraph CellAddValue23 = new Paragraph();
            CellAddValue23.Spacing = new Spacing();
            CellAddValue23.Spacing.After = 0;
            CellAddValue23.Add(CellValue23);
            CellAddValue23.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell23 = new Cell();
            cell23.Width = new Width(TableWidthUnit.Point, 2400);
            cell23.VerticalAlignment = VerticalAlignmentType.Center;
            cell23.Add(CellAddValue23);
            Row row2 = new Row();
            row2.Add(cell21);
            row2.Add(cell22);
            row2.Add(cell23);
            // Second row End
            #endregion -----------------------------------------------------------------------------

            #region --------- Row 3 --------------------------------------------------------
            // Third row start
            Run CellValue31 = new Run("'O'");
            CellValue31.FontSize = 18; //18 points
            CellValue31.AsciiFont = "Century Gothic";
            Paragraph CellAddValue31 = new Paragraph();
            CellAddValue31.Spacing = new Spacing();
            CellAddValue31.Spacing.After = 0;
            CellAddValue31.Add(CellValue31);
            CellAddValue31.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell31 = new Cell();
            cell31.Width = new Width(TableWidthUnit.Point, 900);
            cell31.VerticalAlignment = VerticalAlignmentType.Center;
            cell31.Add(CellAddValue31);

            Run CellValue32 = new Run(OResultdate); // Result Date
            CellValue32.FontSize = 18; //18 points
            CellValue32.AsciiFont = "Century Gothic";
            Paragraph CellAddValue32 = new Paragraph();
            CellAddValue32.Spacing = new Spacing();
            CellAddValue32.Spacing.After = 0;
            CellAddValue32.Add(CellValue32);
            CellAddValue32.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell32 = new Cell();
            cell32.Width = new Width(TableWidthUnit.Point, 2400);
            cell32.VerticalAlignment = VerticalAlignmentType.Center;
            cell32.Add(CellAddValue32);

            Run CellValue33 = new Run(OLastdate); // Last Date of Puraskar application
            CellValue33.FontSize = 18; //18 points
            CellValue33.AsciiFont = "Century Gothic";
            Paragraph CellAddValue33 = new Paragraph();
            CellAddValue33.Spacing = new Spacing();
            CellAddValue33.Spacing.After = 0;
            CellAddValue33.Add(CellValue33);
            CellAddValue33.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell33 = new Cell();
            cell33.Width = new Width(TableWidthUnit.Point, 2400);
            cell33.VerticalAlignment = VerticalAlignmentType.Center;
            cell33.Add(CellAddValue33);

            Row row3 = new Row();
            row3.Add(cell31);
            row3.Add(cell32);
            row3.Add(cell33);
            // Third row End
            #endregion -----------------------------------------------------------------------------

            #region --------- Row 4 --------------------------------------------------------
            //Fourth row start
            Run CellValue41 = new Run("'A'");
            CellValue41.FontSize = 18; //18 points
            CellValue41.AsciiFont = "Century Gothic";
            Paragraph CellAddValue41 = new Paragraph();
            CellAddValue41.Spacing = new Spacing();
            CellAddValue41.Spacing.After = 0;
            CellAddValue41.Add(CellValue41);
            CellAddValue41.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell41 = new Cell();
            cell41.Width = new Width(TableWidthUnit.Point, 900);
            cell41.VerticalAlignment = VerticalAlignmentType.Center;
            cell41.Add(CellAddValue41);


            Run CellValue42 = new Run(AResultdate);
            CellValue42.FontSize = 18; //18 points
            CellValue42.AsciiFont = "Century Gothic";
            Paragraph CellAddValue42 = new Paragraph();
            CellAddValue42.Spacing = new Spacing();
            CellAddValue42.Spacing.After = 0;
            CellAddValue42.Add(CellValue42);
            CellAddValue42.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell42 = new Cell();
            cell42.Width = new Width(TableWidthUnit.Point, 2400);
            cell42.VerticalAlignment = VerticalAlignmentType.Center;
            cell42.Add(CellAddValue42);

            Run CellValue43 = new Run(ALastdate);
            CellValue43.FontSize = 18; //18 points
            CellValue43.AsciiFont = "Century Gothic";
            Paragraph CellAddValue43 = new Paragraph();
            CellAddValue43.Spacing = new Spacing();
            CellAddValue43.Spacing.After = 0;
            CellAddValue43.Add(CellValue43);
            CellAddValue43.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell43 = new Cell();
            cell43.Width = new Width(TableWidthUnit.Point, 2400);
            cell43.VerticalAlignment = VerticalAlignmentType.Center;
            cell43.Add(CellAddValue43);

            Row row4 = new Row();
            row4.Add(cell41);
            row4.Add(cell42);
            row4.Add(cell43);
            //Fourth row End
            #endregion -----------------------------------------------------------------------------

            #region -------- Row 5 ------------------------------------------------------
            // 5th row start
            Run CellValue51 = new Run("'B'");
            CellValue51.FontSize = 18; //18 points
            CellValue51.AsciiFont = "Century Gothic";
            Paragraph CellAddValue51 = new Paragraph();
            CellAddValue51.Spacing = new Spacing();
            CellAddValue51.Spacing.After = 0;
            CellAddValue51.Add(CellValue51);
            CellAddValue51.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell51 = new Cell();
            cell51.Width = new Width(TableWidthUnit.Point, 900);
            cell51.VerticalAlignment = VerticalAlignmentType.Center;
            cell51.Add(CellAddValue51);


            Run CellValue52 = new Run(BResultdate);
            CellValue52.FontSize = 18; //18 points
            CellValue52.AsciiFont = "Century Gothic";
            Paragraph CellAddValue52 = new Paragraph();
            CellAddValue52.Spacing = new Spacing();
            CellAddValue52.Spacing.After = 0;
            CellAddValue52.Add(CellValue52);
            CellAddValue52.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell52 = new Cell();
            cell52.Width = new Width(TableWidthUnit.Point, 2400);
            cell52.VerticalAlignment = VerticalAlignmentType.Center;
            cell52.Add(CellAddValue52);

            Run CellValue53 = new Run(BLastdate);
            CellValue53.FontSize = 18; //18 points
            CellValue53.AsciiFont = "Century Gothic";
            Paragraph CellAddValue53 = new Paragraph();
            CellAddValue53.Spacing = new Spacing();
            CellAddValue53.Spacing.After = 0;
            CellAddValue53.Add(CellValue53);
            CellAddValue53.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell53 = new Cell();
            cell53.Width = new Width(TableWidthUnit.Point, 2400);
            cell53.VerticalAlignment = VerticalAlignmentType.Center;
            cell53.Add(CellAddValue53);

            Row row5 = new Row();
            row5.Add(cell51);
            row5.Add(cell52);
            row5.Add(cell53);
            // 5th row End
            #endregion -----------------------------------------------------------------------------

            #region ------- Row 6   ----------------------------------------------------------------------

            // 6th row start
            Run CellValue61 = new Run("'C'");
            CellValue61.FontSize = 18; //18 points
            CellValue61.AsciiFont = "Century Gothic";
            Paragraph CellAddValue61 = new Paragraph();
            CellAddValue61.Spacing = new Spacing();
            CellAddValue61.Spacing.After = 0;
            CellAddValue61.Add(CellValue61);
            CellAddValue61.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell61 = new Cell();
            cell61.Width = new Width(TableWidthUnit.Point, 900);
            cell61.VerticalAlignment = VerticalAlignmentType.Center;
            cell61.Add(CellAddValue61);

            Run CellValue62 = new Run(CResultdate);
            CellValue62.FontSize = 18; //18 points
            CellValue62.AsciiFont = "Century Gothic";
            Paragraph CellAddValue62 = new Paragraph();
            CellAddValue62.Spacing = new Spacing();
            CellAddValue62.Spacing.After = 0;
            CellAddValue62.Add(CellValue62);
            CellAddValue62.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell62 = new Cell();
            cell62.Width = new Width(TableWidthUnit.Point, 2400);
            cell62.VerticalAlignment = VerticalAlignmentType.Center;
            cell62.Add(CellAddValue62);

            Run CellValue63 = new Run(CLastdate);
            CellValue63.FontSize = 18; //18 points
            CellValue63.AsciiFont = "Century Gothic";
            Paragraph CellAddValue63 = new Paragraph();
            CellAddValue63.Spacing = new Spacing();
            CellAddValue63.Spacing.After = 0;
            CellAddValue63.Add(CellValue63);
            CellAddValue63.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell63 = new Cell();
            cell63.Width = new Width(TableWidthUnit.Point, 2400);
            cell63.VerticalAlignment = VerticalAlignmentType.Center;
            cell63.Add(CellAddValue63);

            Row row6 = new Row();
            row6.Height = new RowHeight();
            row6.Height.Value = 1;
            row6.Add(cell61);
            row6.Add(cell62);
            row6.Add(cell63);
            // 6th row End  

            #endregion -----------------------------------------------------------------------------

            Table table1 = new Table(StandardBorderStyle.SingleLine);
            table1.Width = new Width(TableWidthUnit.Percent, 55);
            table1.Alignment = HorizontalAlignmentType.Center;
            table1.Grid = tableGrid;
            table1.Add(row1);
            table1.Add(row2);
            table1.Add(row3);
            table1.Add(row4);
            table1.Add(row5);
            table1.Add(row6);
            doc.Body.Add(table1);
            //First Table with Merge cell End
            #endregion -----------------------------------------------------------------------------
            
            #endregion -----------------------------------------------------------------------------

            #region --------------- Point third -------------
            // 3rd Line add 
            Run R5 = new Run();
            R5.AddText("3.  With the approval of the competent authority vide Office Order no.  " +
            "NIELIT/HQ/GEN/153/2019-20 dated 25th Nov,2019 a committee with following members constituted for   " +
            "scrutinizing the Protsahan Puraskar (formerly scholarship) forms for O/A/B/C Levels Examinations:");
            R5.FontSize = 18; //12 points
            R5.AsciiFont = "Century Gothic";
            Paragraph P5 = new Paragraph();
            P5.Add(R5);
            P5.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            P5.Spacing = new Spacing();
            P5.Spacing.Before = 120;
            doc.Body.Add(P5);

            Run R6 = new Run();
            R6.AddText("\ti)    Representative of Technical Wing");
            R6.AddBreak();
            R6.AddText("\tii)   Representative of Examination Wing");
            R6.AddBreak();
            R6.AddText("\tiii)  Representative of Finance Wing");
            R6.FontSize = 18; //12 points
            R6.AsciiFont = "Century Gothic";
            Paragraph P6 = new Paragraph();
            P6.Add(R6);
            P6.HorizontalTextAlignment = HorizontalAlignmentType.Left;            
            doc.Body.Add(P6);
            //3rd Line add end
            #endregion -----------------------------------------------------------------------------

            #region  --------------- Point Fouth ------------------------------------------------
            // 4th Line add 
            Run R7 = new Run();
            R7.AddText("4.  With respect to payment of Protsahan Puraskar (formerly Scholarship) for SC/ST, PwD and Female  " +
            "candidates who had appeared in NIELIT O/A/B/C Examinations held in " + OLevelExamName + " is in process. Totals of " + TotalApplication + " Protsahan Puraskar (formerly Scholarship) forms were received in the office of NIELIT by due date.    " +
            "Further, the Finance Wing has processed and verified the Protsahan Puraskar (formerly Scholarship) application forms "+
            "and subsequently Examination Wing has cross verified the same as per lists placed at pg.79/c to 117/c. The details of the same are as under:");
            R7.FontSize = 18; //12 points
            R7.AsciiFont = "Century Gothic";
            Paragraph P7 = new Paragraph();
            P7.Add(R7);
            P7.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            P7.Spacing = new Spacing();
            P7.Spacing.Before = 5;
            doc.Body.Add(P7);
            // 4th Line  End
            #endregion -----------------------------------------------------------------------------

            #region  ------------- Classification of Recommended / withhold & Rejected Candidates with Table 2 --------------------------------

            Run R8 = new Run();
            R8.AddText("\t a: Classification of Recommended/Withhold & Rejected Candidates"); 
            R8.FontSize = 18; //12 points
            R8.AsciiFont = "Century Gothic";
            Paragraph P8 = new Paragraph();
            P8.Add(R8);
            P8.HorizontalTextAlignment = HorizontalAlignmentType.Left;
            doc.Body.Add(P8);
           
            #region --------- Tabels --------------------------------------------------
            //Second Table with Merge cell Start
            TableGrid tableGrid2 = new TableGrid();
            tableGrid2.Columns.Add(new TableGridColumn(498));
            tableGrid2.Columns.Add(new TableGridColumn(2200));
            tableGrid2.Columns.Add(new TableGridColumn(260));
            tableGrid2.Columns.Add(new TableGridColumn(260));
            tableGrid2.Columns.Add(new TableGridColumn(260));
            tableGrid2.Columns.Add(new TableGridColumn(260));
            tableGrid2.Columns.Add(new TableGridColumn(260));

            #region -----------Row 1 for heading 1 of Table ----------------------------------------------------
            // First row start
            Run CellValue02 = new Run("Sl.No");
            CellValue02.Bold = ExtendedBoolean.True;
            CellValue02.FontSize = 18; //18 points
            CellValue02.AsciiFont = "Century Gothic";
            Paragraph CellAddValue02 = new Paragraph();
            CellAddValue02.Spacing = new Spacing();
            CellAddValue02.Spacing.After = 0;
            CellAddValue02.Add(CellValue02);
            CellAddValue02.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell211 = new Cell();

            cell211.VerticallyMergedCell = new VerticallyMergedCell();
            cell211.VerticallyMergedCell.Type = MergeCellType.Restart;

          

            cell211.Width = new Width(TableWidthUnit.Point, 900);
            cell211.Shading = new Shading(ShadingPattern.Percent10);
            cell211.VerticalAlignment = VerticalAlignmentType.Center;
            cell211.Add(CellAddValue02);


            Run CellValue2 = new Run(" Particulars");
            CellValue2.Bold = ExtendedBoolean.True;
            CellValue2.FontSize = 18; //18 points
            CellValue2.AsciiFont = "Century Gothic";
            Paragraph CellAddValue2 = new Paragraph();
            CellAddValue2.Spacing = new Spacing();
            CellAddValue2.Spacing.After = 0;
            CellAddValue2.Add(CellValue2);
            CellAddValue2.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell212 = new Cell();

            cell212.VerticallyMergedCell = new VerticallyMergedCell();
            cell212.VerticallyMergedCell.Type = MergeCellType.Restart;

            cell212.Width = new Width(TableWidthUnit.Point, 900);
            cell212.Shading = new Shading(ShadingPattern.Percent10);
            cell212.VerticalAlignment = VerticalAlignmentType.Center;
            cell212.Add(CellAddValue2);

            Run CellValue3 = new Run(" No. of Applications");
            CellValue3.Bold = ExtendedBoolean.True;
            CellValue3.FontSize = 18; //18 points
            CellValue3.AsciiFont = "Century Gothic";
            Paragraph CellAddValue3 = new Paragraph();
            CellAddValue3.Spacing = new Spacing();
            CellAddValue3.Spacing.After = 0;
            CellAddValue3.Add(CellValue3);
            CellAddValue3.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell213 = new Cell();
            cell213.VerticallyMergedCell = new VerticallyMergedCell();
            cell213.GridSpan = 4;
            cell213.VerticallyMergedCell.Type = MergeCellType.Restart;
            cell213.Width = new Width(TableWidthUnit.Point, 1500);
            cell213.Shading = new Shading(ShadingPattern.Percent10);
            cell213.VerticalAlignment = VerticalAlignmentType.Center;
            cell213.Add(CellAddValue3);

            Run CellValue4 = new Run("Total");
            CellValue4.Bold = ExtendedBoolean.True;
            CellValue4.FontSize = 18; //18 points
            CellValue4.AsciiFont = "Century Gothic";
            Paragraph CellAddValue4 = new Paragraph();
            CellAddValue4.Spacing = new Spacing();
            CellAddValue4.Spacing.After = 0;
            CellAddValue4.Add(CellValue4);
            CellAddValue4.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell214 = new Cell();
            cell214.VerticallyMergedCell = new VerticallyMergedCell();
            cell214.VerticallyMergedCell.Type = MergeCellType.Restart;
            cell214.Width = new Width(TableWidthUnit.Point, 700);
            cell214.Shading = new Shading(ShadingPattern.Percent10);
            cell214.VerticalAlignment = VerticalAlignmentType.Center;
            cell214.Add(CellAddValue4);

            Row row21 = new Row();
            row21.Add(cell211);
            row21.Add(cell212);
            row21.Add(cell213);
            row21.Add(cell214);
            row21.Alignment = HorizontalAlignmentType.Center;
            // First row End 
            #endregion -----------------------------------------------------------------------------

            #region --------Row 2 for Heading of table-----------------------------------------

            // Second row start 
            //Run CellValue221 = new Run("a)");
            //CellValue221.FontSize = 18; //18 points
            //CellValue221.AsciiFont = "Century Gothic";
            //Paragraph CellAddValue221 = new Paragraph();
            //CellAddValue221.Spacing = new Spacing();
            //CellAddValue221.Spacing.After = 0;
            //CellAddValue221.Add(CellValue221);
            //CellAddValue221.HorizontalTextAlignment = HorizontalAlignmentType.Center;

            Cell cell221 = new Cell();
            cell221.VerticallyMergedCell = new VerticallyMergedCell();
            cell221.Width = new Width(TableWidthUnit.Point, 900);
            cell221.VerticalAlignment = VerticalAlignmentType.Center;
            //cell221.Add(CellAddValue221);
            //cell221.Add();

           // Run CellValue222 = new Run("Recommended Candidates");
           // CellValue222.FontSize = 18; //18 points
           // CellValue222.AsciiFont = "Century Gothic";
           // Paragraph CellAddValue222 = new Paragraph();
           // CellAddValue222.Spacing = new Spacing();
           // CellAddValue222.Spacing.After = 0;
           //CellAddValue222.Add(CellValue222);           
           // CellAddValue222.HorizontalTextAlignment = HorizontalAlignmentType.Center;

            Cell cell222 = new Cell();
            cell222.VerticallyMergedCell = new VerticallyMergedCell();
            cell222.Width = new Width(TableWidthUnit.Point, 2200);
            cell222.VerticalAlignment = VerticalAlignmentType.Center;
           // cell222.Add(CellAddValue222);

            Run CellValue223 = new Run("O");
            CellValue223.FontSize = 18; //18 points
            CellValue223.AsciiFont = "Century Gothic";
            Paragraph CellAddValue223 = new Paragraph();
            CellAddValue223.Spacing = new Spacing();
            CellAddValue223.Spacing.After = 0;
            CellAddValue223.Add(CellValue223);
            CellAddValue223.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell223 = new Cell();
            cell223.Width = new Width(TableWidthUnit.Point, 500);
            cell223.VerticalAlignment = VerticalAlignmentType.Center;
            cell223.Add(CellAddValue223);

            Run CellValue224 = new Run("A");
            CellValue224.FontSize = 18; //18 points
            CellValue224.AsciiFont = "Century Gothic";
            Paragraph CellAddValue224 = new Paragraph();
            CellAddValue224.Spacing = new Spacing();
            CellAddValue224.Spacing.After = 0;
            CellAddValue224.Add(CellValue224);
            CellAddValue224.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell224 = new Cell();
            cell224.Width = new Width(TableWidthUnit.Point, 500);
            cell224.VerticalAlignment = VerticalAlignmentType.Center;
            cell224.Add(CellAddValue224);

            Run CellValue225 = new Run("B");
            CellValue225.FontSize = 18; //18 points
            CellValue225.AsciiFont = "Century Gothic";
            Paragraph CellAddValue225 = new Paragraph();
            CellAddValue225.Spacing = new Spacing();
            CellAddValue225.Spacing.After = 0;
            CellAddValue225.Add(CellValue225);
            CellAddValue225.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell225 = new Cell();
            cell225.Width = new Width(TableWidthUnit.Point, 500);
            cell225.VerticalAlignment = VerticalAlignmentType.Center;
            cell225.Add(CellAddValue225);

            Run CellValue226 = new Run("C");
            CellValue226.FontSize = 18; //18 points
            CellValue226.AsciiFont = "Century Gothic";
            Paragraph CellAddValue226 = new Paragraph();
            CellAddValue226.Spacing = new Spacing();
            CellAddValue226.Spacing.After = 0;
            CellAddValue226.Add(CellValue226);
            CellAddValue226.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell226 = new Cell();
            cell226.Width = new Width(TableWidthUnit.Point, 500);
            cell226.VerticalAlignment = VerticalAlignmentType.Center;
            cell226.Add(CellAddValue226);

            //Run CellValue227 = new Run("Last");
            //CellValue227.FontSize = 18; //18 points
            //CellValue227.AsciiFont = "Century Gothic";
            //Paragraph CellAddValue227 = new Paragraph();
            //CellAddValue227.Spacing = new Spacing();
            //CellAddValue227.Spacing.After = 0;
            //CellAddValue227.Add(CellValue227);
            //CellAddValue227.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell227 = new Cell();
            cell227.VerticallyMergedCell = new VerticallyMergedCell();
            cell227.Width = new Width(TableWidthUnit.Point, 500);
            cell227.VerticalAlignment = VerticalAlignmentType.Center;
            //cell227.Add(CellAddValue227);

            Row row22 = new Row();
            row22.Add(cell221);
            row22.Add(cell222);
            row22.Add(cell223);
            row22.Add(cell224);
            row22.Add(cell225);
            row22.Add(cell226);
            row22.Add(cell227);
            // Second row End
            #endregion -----------------------------------------------------------------------------

            #region ----------- Row 3 for Recommended candidates Point a --------------------------------------------------

            // Third row start
            Run CellValue231 = new Run("a)");
            CellValue231.FontSize = 18; //18 points
            CellValue231.AsciiFont = "Century Gothic";
            Paragraph CellAddValue231 = new Paragraph();
            CellAddValue231.Spacing = new Spacing();
            CellAddValue231.Spacing.After = 0;
            CellAddValue231.Add(CellValue231);
            CellAddValue231.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell231 = new Cell();
            cell231.Width = new Width(TableWidthUnit.Point, 900);
            cell231.VerticalAlignment = VerticalAlignmentType.Center;
            cell231.Add(CellAddValue231);

            Run CellValue232 = new Run("Recommended Candidates"); 
            CellValue232.FontSize = 18; //18 points
            CellValue232.AsciiFont = "Century Gothic";
            Paragraph CellAddValue232 = new Paragraph();
            CellAddValue232.Spacing = new Spacing();
            CellAddValue232.Spacing.After = 0;
            CellAddValue232.Add(CellValue232);
            CellAddValue232.HorizontalTextAlignment = HorizontalAlignmentType.Left;
            Cell cell232 = new Cell();
            cell232.Width = new Width(TableWidthUnit.Point, 2200);
            cell232.VerticalAlignment = VerticalAlignmentType.Center;
            cell232.Add(CellAddValue232);

            Run CellValue233 = new Run(ORec != "0" ? ORec : "-"); 
            CellValue233.FontSize = 18; //18 points
            CellValue233.AsciiFont = "Century Gothic";
            Paragraph CellAddValue233 = new Paragraph();
            CellAddValue233.Spacing = new Spacing();
            CellAddValue233.Spacing.After = 0;
            CellAddValue233.Add(CellValue233);
            CellAddValue233.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell233 = new Cell();
            cell233.Width = new Width(TableWidthUnit.Point, 500);
            cell233.VerticalAlignment = VerticalAlignmentType.Center;
            cell233.Add(CellAddValue233);

            Run CellValue234 = new Run(ARec != "0" ? ARec : "-"); // Last Date of Puraskar application
            CellValue234.FontSize = 18; //18 points
            CellValue234.AsciiFont = "Century Gothic";
            Paragraph CellAddValue234 = new Paragraph();
            CellAddValue234.Spacing = new Spacing();
            CellAddValue234.Spacing.After = 0;
            CellAddValue234.Add(CellValue234);
            CellAddValue234.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell234 = new Cell();
            cell234.Width = new Width(TableWidthUnit.Point, 500);
            cell234.VerticalAlignment = VerticalAlignmentType.Center;
            cell234.Add(CellAddValue234);

            Run CellValue235 = new Run(BRec != "0" ? BRec : "-"); // Last Date of Puraskar application
            CellValue235.FontSize = 18; //18 points
            CellValue235.AsciiFont = "Century Gothic";
            Paragraph CellAddValue235 = new Paragraph();
            CellAddValue235.Spacing = new Spacing();
            CellAddValue235.Spacing.After = 0;
            CellAddValue235.Add(CellValue235);
            CellAddValue235.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell235 = new Cell();
            cell235.Width = new Width(TableWidthUnit.Point, 500);
            cell235.VerticalAlignment = VerticalAlignmentType.Center;
            cell235.Add(CellAddValue235);

            Run CellValue236 = new Run(CRec != "0" ? CRec : "-"); // Last Date of Puraskar application
            CellValue236.FontSize = 18; //18 points
            CellValue236.AsciiFont = "Century Gothic";
            Paragraph CellAddValue236 = new Paragraph();
            CellAddValue236.Spacing = new Spacing();
            CellAddValue236.Spacing.After = 0;
            CellAddValue236.Add(CellValue236);
            CellAddValue236.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell236 = new Cell();
            cell236.Width = new Width(TableWidthUnit.Point, 500);
            cell236.VerticalAlignment = VerticalAlignmentType.Center;
            cell236.Add(CellAddValue236);

            string total2 = (Convert.ToInt32(ORec) + Convert.ToInt32(ARec) + Convert.ToInt32(BRec) + Convert.ToInt32(CRec)).ToString();            
            Run CellValue237 = new Run(total2 != "0" ? total2 : "-"); // Last Date of Puraskar application
            CellValue237.FontSize = 18; //18 points
            CellValue237.AsciiFont = "Century Gothic";
            Paragraph CellAddValue237 = new Paragraph();
            CellAddValue237.Spacing = new Spacing();
            CellAddValue237.Spacing.After = 0;
            CellAddValue237.Add(CellValue237);
            CellAddValue237.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell237 = new Cell();
            cell237.Width = new Width(TableWidthUnit.Point, 500);
            cell237.VerticalAlignment = VerticalAlignmentType.Center;
            cell237.Add(CellAddValue237);

            Row row23 = new Row();
            row23.Add(cell231);
            row23.Add(cell232);
            row23.Add(cell233);
            row23.Add(cell234);
            row23.Add(cell235);
            row23.Add(cell236);
            row23.Add(cell237);
            // Third row End
            #endregion -----------------------------------------------------------------------------
            
            #region ----------- Row 4 for Withhold candidates Point b ----------------------------------------------------
            ////////Fourth row start
            Run CellValue241 = new Run("b)");
            CellValue241.FontSize = 18; //18 points
            CellValue241.AsciiFont = "Century Gothic";
            Paragraph CellAddValue241 = new Paragraph();
            CellAddValue241.Spacing = new Spacing();
            CellAddValue241.Spacing.After = 0;
            CellAddValue241.Add(CellValue241);
            CellAddValue241.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell241 = new Cell();
            cell241.Width = new Width(TableWidthUnit.Point, 900);
            cell241.VerticalAlignment = VerticalAlignmentType.Center;
            cell241.Add(CellAddValue241);

            Run CellValue242 = new Run("Withhold Candidates"); // Result Date
            CellValue242.FontSize = 18; //18 points
            CellValue242.AsciiFont = "Century Gothic";
            Paragraph CellAddValue242 = new Paragraph();
            CellAddValue242.Spacing = new Spacing();
            CellAddValue242.Spacing.After = 0;
            CellAddValue242.Add(CellValue242);
            CellAddValue242.HorizontalTextAlignment = HorizontalAlignmentType.Left;
            Cell cell242 = new Cell();
            cell242.Width = new Width(TableWidthUnit.Point, 2200);
            cell242.VerticalAlignment = VerticalAlignmentType.Center;
            cell242.Add(CellAddValue242);

            Run CellValue243 = new Run(OWithhold != "0" ? OWithhold : "-"); 
            CellValue243.FontSize = 18; //18 points
            CellValue243.AsciiFont = "Century Gothic";
            Paragraph CellAddValue243 = new Paragraph();
            CellAddValue243.Spacing = new Spacing();
            CellAddValue243.Spacing.After = 0;
            CellAddValue243.Add(CellValue243);
            CellAddValue243.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell243 = new Cell();
            cell243.Width = new Width(TableWidthUnit.Point, 260);
            cell243.VerticalAlignment = VerticalAlignmentType.Center;
            cell243.Add(CellAddValue243);

            Run CellValue244 = new Run(AWithhold != "0" ? AWithhold : "-"); 
            CellValue244.FontSize = 18; //18 points
            CellValue244.AsciiFont = "Century Gothic";
            Paragraph CellAddValue244 = new Paragraph();
            CellAddValue244.Spacing = new Spacing();
            CellAddValue244.Spacing.After = 0;
            CellAddValue244.Add(CellValue244);
            CellAddValue244.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell244 = new Cell();
            cell244.Width = new Width(TableWidthUnit.Point, 260);
            cell244.VerticalAlignment = VerticalAlignmentType.Center;
            cell244.Add(CellAddValue244);

            Run CellValue245 = new Run(BWithhold != "0" ? BWithhold : "-"); 
            CellValue245.FontSize = 18; //18 points
            CellValue245.AsciiFont = "Century Gothic";
            Paragraph CellAddValue245 = new Paragraph();
            CellAddValue245.Spacing = new Spacing();
            CellAddValue245.Spacing.After = 0;
            CellAddValue245.Add(CellValue245);
            CellAddValue245.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell245 = new Cell();
            cell245.Width = new Width(TableWidthUnit.Point, 260);
            cell245.VerticalAlignment = VerticalAlignmentType.Center;
            cell245.Add(CellAddValue245);

            Run CellValue246 = new Run(CWithhold != "0" ? CWithhold : "-"); 
            CellValue246.FontSize = 18; //18 points
            CellValue246.AsciiFont = "Century Gothic";
            Paragraph CellAddValue246 = new Paragraph();
            CellAddValue246.Spacing = new Spacing();
            CellAddValue246.Spacing.After = 0;
            CellAddValue246.Add(CellValue246);
            CellAddValue246.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell246 = new Cell();
            cell246.Width = new Width(TableWidthUnit.Point, 260);
            cell246.VerticalAlignment = VerticalAlignmentType.Center;
            cell246.Add(CellAddValue246);
            string total1 = (Convert.ToInt32(OWithhold) + Convert.ToInt32(AWithhold) + Convert.ToInt32(BWithhold) + Convert.ToInt32(CWithhold)).ToString();
            Run CellValue247 = new Run(total1 != "0" ? total1 : "-"); 
            CellValue247.FontSize = 18; //18 points
            CellValue247.AsciiFont = "Century Gothic";
            Paragraph CellAddValue247 = new Paragraph();
            CellAddValue247.Spacing = new Spacing();
            CellAddValue247.Spacing.After = 0;
            CellAddValue247.Add(CellValue247);
            CellAddValue247.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell247 = new Cell();
            cell247.Width = new Width(TableWidthUnit.Point, 260);
            cell247.VerticalAlignment = VerticalAlignmentType.Center;
            cell247.Add(CellAddValue247);

            Row row24 = new Row();
            row24.Add(cell241);
            row24.Add(cell242);
            row24.Add(cell243);
            row24.Add(cell244);
            row24.Add(cell245);
            row24.Add(cell246);
            row24.Add(cell247);
            ////////Fourth row End
            #endregion -----------------------------------------------------------------------------

            #region ------------ Row 5 for Rejected Candidates Count, Point c --------------------------------------------------------


            //////// 5th row start
            Run CellValue251 = new Run("c)");
            CellValue251.FontSize = 18; //18 points
            CellValue251.AsciiFont = "Century Gothic";
            Paragraph CellAddValue251 = new Paragraph();
            CellAddValue251.Spacing = new Spacing();
            CellAddValue251.Spacing.After = 0;
            CellAddValue251.Add(CellValue251);
            CellAddValue251.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell251 = new Cell();
            cell251.Width = new Width(TableWidthUnit.Point, 900);
            cell251.VerticalAlignment = VerticalAlignmentType.Center;
            cell251.Add(CellAddValue251);

            Run CellValue252 = new Run("Rejected Candidates"); // Result Date
            CellValue252.FontSize = 18; //18 points
            CellValue252.AsciiFont = "Century Gothic";
            Paragraph CellAddValue252 = new Paragraph();
            CellAddValue252.Spacing = new Spacing();
            CellAddValue252.Spacing.After = 0;
            CellAddValue252.Add(CellValue252);
            CellAddValue252.HorizontalTextAlignment = HorizontalAlignmentType.Left;
            Cell cell252 = new Cell();
            cell252.Width = new Width(TableWidthUnit.Point, 2200);
            cell252.VerticalAlignment = VerticalAlignmentType.Center;
            cell252.Add(CellAddValue252);

            Run CellValue253 = new Run(ORejected != "0" ? ORejected : "-"); 
            CellValue253.FontSize = 18; //18 points
            CellValue253.AsciiFont = "Century Gothic";
            Paragraph CellAddValue253 = new Paragraph();
            CellAddValue253.Spacing = new Spacing();
            CellAddValue253.Spacing.After = 0;
            CellAddValue253.Add(CellValue253);
            CellAddValue253.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell253 = new Cell();
            cell253.Width = new Width(TableWidthUnit.Point, 260);
            cell253.VerticalAlignment = VerticalAlignmentType.Center;
            cell253.Add(CellAddValue253);

            Run CellValue254 = new Run(ARejected != "0" ? ARejected : "-"); 
            CellValue254.FontSize = 18; //18 points
            CellValue254.AsciiFont = "Century Gothic";
            Paragraph CellAddValue254 = new Paragraph();
            CellAddValue254.Spacing = new Spacing();
            CellAddValue254.Spacing.After = 0;
            CellAddValue254.Add(CellValue254);
            CellAddValue254.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell254 = new Cell();
            cell254.Width = new Width(TableWidthUnit.Point, 260);
            cell254.VerticalAlignment = VerticalAlignmentType.Center;
            cell254.Add(CellAddValue254);

            Run CellValue255 = new Run(BRejected != "0" ? BRejected : "-"); 
            CellValue255.FontSize = 18; //18 points
            CellValue255.AsciiFont = "Century Gothic";
            Paragraph CellAddValue255 = new Paragraph();
            CellAddValue255.Spacing = new Spacing();
            CellAddValue255.Spacing.After = 0;
            CellAddValue255.Add(CellValue255);
            CellAddValue255.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell255 = new Cell();
            cell255.Width = new Width(TableWidthUnit.Point, 260);
            cell255.VerticalAlignment = VerticalAlignmentType.Center;
            cell255.Add(CellAddValue255);

            Run CellValue256 = new Run(CRejected != "0" ? CRejected : "-"); 
            CellValue256.FontSize = 18; //18 points
            CellValue256.AsciiFont = "Century Gothic";
            Paragraph CellAddValue256 = new Paragraph();
            CellAddValue256.Spacing = new Spacing();
            CellAddValue256.Spacing.After = 0;
            CellAddValue256.Add(CellValue256);
            CellAddValue256.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell256 = new Cell();
            cell256.Width = new Width(TableWidthUnit.Point, 260);
            cell256.VerticalAlignment = VerticalAlignmentType.Center;
            cell256.Add(CellAddValue256);
            string TOTAL = (Convert.ToInt32(ORejected) + Convert.ToInt32(ARejected) + Convert.ToInt32(BRejected) + Convert.ToInt32(CRejected)).ToString();
            Run CellValue257 = new Run(TOTAL != "0" ? TOTAL : "-"); 
            CellValue257.FontSize = 18; //18 points
            CellValue257.AsciiFont = "Century Gothic";
            Paragraph CellAddValue257 = new Paragraph();
            CellAddValue257.Spacing = new Spacing();
            CellAddValue257.Spacing.After = 0;
            CellAddValue257.Add(CellValue257);
            CellAddValue257.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell257 = new Cell();
            cell257.Width = new Width(TableWidthUnit.Point, 260);
            cell257.VerticalAlignment = VerticalAlignmentType.Center;
            cell257.Add(CellAddValue257);

            Row row25 = new Row();
            row25.Add(cell251);
            row25.Add(cell252);
            row25.Add(cell253);
            row25.Add(cell254);
            row25.Add(cell255);
            row25.Add(cell256);
            row25.Add(cell257);
            //////// 5th row End
            #endregion -----------------------------------------------------------------------------

            #region ---------- Row 6 for Total ----------------------------------------------------
            ////// 6th row start
            Run CellValue261 = new Run("Total");
            CellValue261.FontSize = 18; //18 points
            CellValue261.AsciiFont = "Century Gothic";
            Paragraph CellAddValue261 = new Paragraph();
            CellAddValue261.Spacing = new Spacing();
            CellAddValue261.Spacing.After = 0;
            CellAddValue261.Add(CellValue261);
            CellAddValue261.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell261 = new Cell();
             cell261.VerticallyMergedCell = new VerticallyMergedCell();
             cell261.VerticallyMergedCell.Type = MergeCellType.Restart;
             cell261.GridSpan = 2;
            cell261.Width = new Width(TableWidthUnit.Point, 900);
            cell261.VerticalAlignment = VerticalAlignmentType.Center;
            cell261.Add(CellAddValue261);

            //Run CellValue262 = new Run("Rejected Candidates"); // Result Date
            //CellValue262.FontSize = 18; //18 points
            //CellValue262.AsciiFont = "Century Gothic";
            //Paragraph CellAddValue262 = new Paragraph();
            //CellAddValue262.Spacing = new Spacing();
            //CellAddValue262.Spacing.After = 0;
            //CellAddValue262.Add(CellValue262);
            //CellAddValue262.HorizontalTextAlignment = HorizontalAlignmentType.Left;
           // Cell cell262 = new Cell();
            //cell262.Width = new Width(TableWidthUnit.Point, 2200);
            //cell262.VerticalAlignment = VerticalAlignmentType.Center;
            //cell262.Add(CellAddValue262);

            Run CellValue263 = new Run((Convert.ToInt32(ORec) + Convert.ToInt32(OWithhold) + Convert.ToInt32(ORejected)).ToString() != "0" ? (Convert.ToInt32(ORec) + Convert.ToInt32(OWithhold) + Convert.ToInt32(ORejected)).ToString() : "-"); 
            CellValue263.FontSize = 18; //18 points
            CellValue263.AsciiFont = "Century Gothic";
            Paragraph CellAddValue263 = new Paragraph();
            CellAddValue263.Spacing = new Spacing();
            CellAddValue263.Spacing.After = 0;
            CellAddValue263.Add(CellValue263);
            CellAddValue263.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell263 = new Cell();
            cell263.Width = new Width(TableWidthUnit.Point, 260);
            cell263.VerticalAlignment = VerticalAlignmentType.Center;
            cell263.Add(CellAddValue263);

            Run CellValue264 = new Run((Convert.ToInt32(ARec) + Convert.ToInt32(AWithhold) + Convert.ToInt32(ARejected)).ToString() != "0" ? (Convert.ToInt32(ARec) + Convert.ToInt32(AWithhold) + Convert.ToInt32(ARejected)).ToString() : "-"); // Last Date of Puraskar application
            CellValue264.FontSize = 18; //18 points
            CellValue264.AsciiFont = "Century Gothic";
            Paragraph CellAddValue264 = new Paragraph();
            CellAddValue264.Spacing = new Spacing();
            CellAddValue264.Spacing.After = 0;
            CellAddValue264.Add(CellValue264);
            CellAddValue264.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell264 = new Cell();
            cell264.Width = new Width(TableWidthUnit.Point, 260);
            cell264.VerticalAlignment = VerticalAlignmentType.Center;
            cell264.Add(CellAddValue264);

            Run CellValue265 = new Run((Convert.ToInt32(BRec) + Convert.ToInt32(BWithhold) + Convert.ToInt32(BRejected)).ToString() != "0" ? (Convert.ToInt32(ARec) + Convert.ToInt32(AWithhold) + Convert.ToInt32(ARejected)).ToString() : "-"); 
            CellValue265.FontSize = 18; //18 points
            CellValue265.AsciiFont = "Century Gothic";
            Paragraph CellAddValue265 = new Paragraph();
            CellAddValue265.Spacing = new Spacing();
            CellAddValue265.Spacing.After = 0;
            CellAddValue265.Add(CellValue265);
            CellAddValue265.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell265 = new Cell();
            cell265.Width = new Width(TableWidthUnit.Point, 260);
            cell265.VerticalAlignment = VerticalAlignmentType.Center;
            cell265.Add(CellAddValue265);

            Run CellValue266 = new Run((Convert.ToInt32(CRec) + Convert.ToInt32(CWithhold) + Convert.ToInt32(CRejected)).ToString() != "0" ? (Convert.ToInt32(CRec) + Convert.ToInt32(CWithhold) + Convert.ToInt32(CRejected)).ToString() : "-"); 
            CellValue266.FontSize = 18; //18 points
            CellValue266.AsciiFont = "Century Gothic";
            Paragraph CellAddValue266 = new Paragraph();
            CellAddValue266.Spacing = new Spacing();
            CellAddValue266.Spacing.After = 0;
            CellAddValue266.Add(CellValue266);
            CellAddValue266.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell266 = new Cell();
            cell266.Width = new Width(TableWidthUnit.Point, 260);
            cell266.VerticalAlignment = VerticalAlignmentType.Center;
            cell266.Add(CellAddValue266);

            Run CellValue267 = new Run((Convert.ToInt32(ORec) + Convert.ToInt32(ARec) + Convert.ToInt32(BRec) + Convert.ToInt32(CRec) + Convert.ToInt32(OWithhold) + Convert.ToInt32(AWithhold) + Convert.ToInt32(BWithhold) + Convert.ToInt32(CWithhold) + Convert.ToInt32(ORejected) + Convert.ToInt32(ARejected) + Convert.ToInt32(BRejected) + Convert.ToInt32(CRejected)).ToString()); 
            CellValue267.FontSize = 18; //18 points
            CellValue267.AsciiFont = "Century Gothic";
            Paragraph CellAddValue267 = new Paragraph();
            CellAddValue267.Spacing = new Spacing();
            CellAddValue267.Spacing.After = 0;
            CellAddValue267.Add(CellValue267);
            CellAddValue267.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell267 = new Cell();
            cell267.Width = new Width(TableWidthUnit.Point, 260);
            cell267.VerticalAlignment = VerticalAlignmentType.Center;
            cell267.Add(CellAddValue267);

            Row row26 = new Row();
            row26.Add(cell261);
           // row26.Add(cell262);
            row26.Add(cell263);
            row26.Add(cell264);
            row26.Add(cell265);
            row26.Add(cell266);
            row26.Add(cell267);
            ////// 6th row End  
            #endregion -----------------------------------------------------------------------------

            Table table2 = new Table(StandardBorderStyle.SingleLine);
            table2.Width = new Width(TableWidthUnit.Percent, 75);
            table2.Alignment = HorizontalAlignmentType.Center;
            table2.Grid = tableGrid2;
            table2.Add(row21);
            table2.Add(row22);
            table2.Add(row23);
            table2.Add(row24);
            table2.Add(row25);
            table2.Add(row26);
            doc.Body.Add(table2);
            //Second Table with Merge cell End
            #endregion -----------------------------------------------------------------------------

            #endregion -----------------------------------------------------------------------------

            #region  -------------- Part b Classification of SC/ST,PWD & Female Candidates with Table 3 ------------------------
            Run R9 = new Run();
            R9.AddText("\t b: Classification of SC/ST, PwD & Female Candidates:");
            R9.FontSize = 19; //12 points
            R9.AsciiFont = "Century Gothic";
            Paragraph P9 = new Paragraph();
            P9.Spacing = new Spacing();
            P9.Spacing.Before = 400;
            P9.Add(R9);
            P9.HorizontalTextAlignment = HorizontalAlignmentType.Left;
            doc.Body.Add(P9);

            // 3rd Table start Classification of SC/ST, PwD & Female Candidates RECOMMENDED COUNT

            //Second Table with Merge cell Start
            TableGrid tableGrid3 = new TableGrid();
            tableGrid3.Columns.Add(new TableGridColumn(1000));
            tableGrid3.Columns.Add(new TableGridColumn(2200));
            tableGrid3.Columns.Add(new TableGridColumn(260));
            tableGrid3.Columns.Add(new TableGridColumn(260));
            tableGrid3.Columns.Add(new TableGridColumn(260));
            tableGrid3.Columns.Add(new TableGridColumn(260));
            tableGrid3.Columns.Add(new TableGridColumn(260));

            // First row start
            Run CellValue402 = new Run("Recommended");
            CellValue402.Bold = ExtendedBoolean.True;
            CellValue402.FontSize = 18; //18 points
            CellValue402.AsciiFont = "Century Gothic";
            Paragraph CellAddValue402 = new Paragraph();
            CellAddValue402.Spacing = new Spacing();
            CellAddValue402.Spacing.After = 10;
            CellAddValue402.Add(CellValue402);
            CellAddValue402.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            CellAddValue402.VerticalTextAlignment = VerticalTextAlignment.Center;
            Cell cell311 = new Cell();

            //cell311.VerticallyMergedCell = new VerticallyMergedCell();
            //cell311.VerticallyMergedCell.Type = MergeCellType.Restart;



            cell311.Width = new Width(TableWidthUnit.Point, 1500);
            cell311.Shading = new Shading(ShadingPattern.Percent10);
            cell311.VerticalAlignment = VerticalAlignmentType.Center;            
            cell311.Add(CellAddValue402);


            Run CellValue302 = new Run(" Male");
            CellValue302.Bold = ExtendedBoolean.True;
            CellValue302.FontSize = 18; //18 points
            CellValue302.AsciiFont = "Century Gothic";
            Paragraph CellAddValue302 = new Paragraph();
            CellAddValue302.Spacing = new Spacing();
            CellAddValue302.Spacing.Before = 200;
            CellAddValue302.Spacing.After = 200;
            CellAddValue302.Add(CellValue302);
            CellAddValue302.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            //CellAddValue302.VerticalTextAlignment = VerticalTextAlignment.Center;
            Cell cell312 = new Cell();

            cell312.VerticallyMergedCell = new VerticallyMergedCell();
            cell312.GridSpan = 5;
            cell312.VerticallyMergedCell.Type = MergeCellType.Restart;

            cell312.Width = new Width(TableWidthUnit.Point, 1500);
            cell312.Shading = new Shading(ShadingPattern.Percent10);
            cell312.VerticalAlignment = VerticalAlignmentType.Center;
            cell312.Add(CellAddValue302);

            Run CellValue303 = new Run(" Female");
            CellValue303.Bold = ExtendedBoolean.True;
            CellValue303.FontSize = 18; //18 points
            CellValue303.AsciiFont = "Century Gothic";
            Paragraph CellAddValue303 = new Paragraph();
            CellAddValue303.Spacing = new Spacing();
            CellAddValue303.Spacing.Before = 200;
            CellAddValue303.Spacing.After =200;
            CellAddValue303.Add(CellValue303);
            CellAddValue303.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell313 = new Cell();
            cell313.VerticallyMergedCell = new VerticallyMergedCell();
            cell313.GridSpan = 5;
            cell313.VerticallyMergedCell.Type = MergeCellType.Restart;
            cell313.Width = new Width(TableWidthUnit.Point, 1500);
            cell313.Shading = new Shading(ShadingPattern.Percent10);
            cell313.VerticalAlignment = VerticalAlignmentType.Center;
            cell313.Add(CellAddValue303);

            Run CellValue304 = new Run("Total");
            CellValue304.Bold = ExtendedBoolean.True;
            CellValue304.FontSize = 18; //18 points
            CellValue304.AsciiFont = "Century Gothic";
            Paragraph CellAddValue304 = new Paragraph();
            //CellAddValue304.Spacing = new Spacing();
            //CellAddValue304.Spacing.After = 0;
            CellAddValue304.Add(CellValue304);
            CellAddValue304.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell314 = new Cell();
            cell314.VerticallyMergedCell = new VerticallyMergedCell();
            cell314.VerticallyMergedCell.Type = MergeCellType.Restart;
            cell314.Width = new Width(TableWidthUnit.Point, 700);
            cell314.Shading = new Shading(ShadingPattern.Percent10);
            cell314.VerticalAlignment = VerticalAlignmentType.Center;
            cell314.Add(CellAddValue304);

            Row row31 = new Row();
            row31.Add(cell311);
            row31.Add(cell312);
            row31.Add(cell313);
            row31.Add(cell314);
            row31.Alignment = HorizontalAlignmentType.Center;
            // First row End 

            // Second row start 
            Run CellValue321 = new Run("Level");
            CellValue321.FontSize = 18; //18 points
            CellValue321.AsciiFont = "Century Gothic";
            Paragraph CellAddValue321 = new Paragraph();
            CellAddValue321.Spacing = new Spacing();
            CellAddValue321.Spacing.After = 0;
            CellAddValue321.Add(CellValue321);
            CellAddValue321.HorizontalTextAlignment = HorizontalAlignmentType.Center;

            Cell cell321 = new Cell();           
            cell321.Width = new Width(TableWidthUnit.Point, 1500);
            cell321.VerticalAlignment = VerticalAlignmentType.Center;
            cell321.Add(CellAddValue321);
            

            // Run CellValue322 = new Run("Recommended Candidates");
            // CellValue322.FontSize = 18; //18 points
            // CellValue322.AsciiFont = "Century Gothic";
            // Paragraph CellAddValue322 = new Paragraph();
            // CellAddValue322.Spacing = new Spacing();
            // CellAddValue322.Spacing.After = 0;
            //CellAddValue322.Add(CellValue322);           
            // CellAddValue322.HorizontalTextAlignment = HorizontalAlignmentType.Center;

            //Cell cell322 = new Cell();
            ////cell322.VerticallyMergedCell = new VerticallyMergedCell();
            //cell322.Width = new Width(TableWidthUnit.Point, 2200);
            //cell322.VerticalAlignment = VerticalAlignmentType.Center;
            //// cell322.Add(CellAddValue322);

            // for male
            Run CellValue1323 = new Run("SC");
            CellValue1323.FontSize = 18; //18 points
            CellValue1323.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1323 = new Paragraph();
            CellAddValue1323.Spacing = new Spacing();
            CellAddValue1323.Spacing.After = 0;
            CellAddValue1323.Add(CellValue1323);
            CellAddValue1323.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1323 = new Cell();
            cell1323.Width = new Width(TableWidthUnit.Point, 500);
            cell1323.VerticalAlignment = VerticalAlignmentType.Center;
            cell1323.Add(CellAddValue1323);

            Run CellValue1324 = new Run("ST");
            CellValue1324.FontSize = 18; //18 points
            CellValue1324.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1324 = new Paragraph();
            CellAddValue1324.Spacing = new Spacing();
            CellAddValue1324.Spacing.After = 0;
            CellAddValue1324.Add(CellValue1324);
            CellAddValue1324.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1324 = new Cell();
            cell1324.Width = new Width(TableWidthUnit.Point, 500);
            cell1324.VerticalAlignment = VerticalAlignmentType.Center;
            cell1324.Add(CellAddValue1324);

            Run CellValue1325 = new Run("PwD");
            CellValue1325.FontSize = 18; //18 points
            CellValue1325.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1325 = new Paragraph();
            CellAddValue1325.Spacing = new Spacing();
            CellAddValue1325.Spacing.After = 0;
            CellAddValue1325.Add(CellValue1325);
            CellAddValue1325.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1325 = new Cell();
            cell1325.Width = new Width(TableWidthUnit.Point, 500);
            cell1325.VerticalAlignment = VerticalAlignmentType.Center;
            cell1325.Add(CellAddValue1325);

            Run CellValue1326 = new Run("Gen");
            CellValue1326.FontSize = 18; //18 points
            CellValue1326.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1326 = new Paragraph();
            CellAddValue1326.Spacing = new Spacing();
            CellAddValue1326.Spacing.After = 0;
            CellAddValue1326.Add(CellValue1326);
            CellAddValue1326.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1326 = new Cell();
            cell1326.Width = new Width(TableWidthUnit.Point, 500);
            cell1326.VerticalAlignment = VerticalAlignmentType.Center;
            cell1326.Add(CellAddValue1326);

            Run CellValue1328 = new Run("OBC");
            CellValue1328.FontSize = 18; //18 points
            CellValue1328.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1328 = new Paragraph();
            CellAddValue1328.Spacing = new Spacing();
            CellAddValue1328.Spacing.After = 0;
            CellAddValue1328.Add(CellValue1328);
            CellAddValue1328.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1328 = new Cell();
            cell1328.Width = new Width(TableWidthUnit.Point, 500);
            cell1328.VerticalAlignment = VerticalAlignmentType.Center;
            cell1328.Add(CellAddValue1328);
            //formale end

            Run CellValue323 = new Run("SC");
            CellValue323.FontSize = 18; //18 points
            CellValue323.AsciiFont = "Century Gothic";
            Paragraph CellAddValue323 = new Paragraph();
            CellAddValue323.Spacing = new Spacing();
            CellAddValue323.Spacing.After = 0;
            CellAddValue323.Add(CellValue323);
            CellAddValue323.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell323 = new Cell();
            cell323.Width = new Width(TableWidthUnit.Point, 500);
            cell323.VerticalAlignment = VerticalAlignmentType.Center;
            cell323.Add(CellAddValue323);

            Run CellValue324 = new Run("ST");
            CellValue324.FontSize = 18; //18 points
            CellValue324.AsciiFont = "Century Gothic";
            Paragraph CellAddValue324 = new Paragraph();
            CellAddValue324.Spacing = new Spacing();
            CellAddValue324.Spacing.After = 0;
            CellAddValue324.Add(CellValue324);
            CellAddValue324.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell324 = new Cell();
            cell324.Width = new Width(TableWidthUnit.Point, 500);
            cell324.VerticalAlignment = VerticalAlignmentType.Center;
            cell324.Add(CellAddValue324);

            Run CellValue325 = new Run("PwD");
            CellValue325.FontSize = 18; //18 points
            CellValue325.AsciiFont = "Century Gothic";
            Paragraph CellAddValue325 = new Paragraph();
            CellAddValue325.Spacing = new Spacing();
            CellAddValue325.Spacing.After = 0;
            CellAddValue325.Add(CellValue325);
            CellAddValue325.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell325 = new Cell();
            cell325.Width = new Width(TableWidthUnit.Point, 500);
            cell325.VerticalAlignment = VerticalAlignmentType.Center;
            cell325.Add(CellAddValue325);

            Run CellValue326 = new Run("Gen");
            CellValue326.FontSize = 18; //18 points
            CellValue326.AsciiFont = "Century Gothic";
            Paragraph CellAddValue326 = new Paragraph();
            CellAddValue326.Spacing = new Spacing();
            CellAddValue326.Spacing.After = 0;
            CellAddValue326.Add(CellValue326);
            CellAddValue326.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell326 = new Cell();
            cell326.Width = new Width(TableWidthUnit.Point, 500);
            cell326.VerticalAlignment = VerticalAlignmentType.Center;
            cell326.Add(CellAddValue326);

            Run CellValue328 = new Run("OBC");
            CellValue328.FontSize = 18; //18 points
            CellValue328.AsciiFont = "Century Gothic";
            Paragraph CellAddValue328 = new Paragraph();
            CellAddValue328.Spacing = new Spacing();
            CellAddValue328.Spacing.After = 0;
            CellAddValue328.Add(CellValue328);
            CellAddValue328.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell328 = new Cell();
            cell328.Width = new Width(TableWidthUnit.Point, 500);
            cell328.VerticalAlignment = VerticalAlignmentType.Center;
            cell328.Add(CellAddValue328);

            //Run CellValue327 = new Run("Last");
            //CellValue327.FontSize = 18; //18 points
            //CellValue327.AsciiFont = "Century Gothic";
            //Paragraph CellAddValue327 = new Paragraph();
            //CellAddValue327.Spacing = new Spacing();
            //CellAddValue327.Spacing.After = 0;
            //CellAddValue327.Add(CellValue327);
            //CellAddValue327.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell327 = new Cell();
            cell327.VerticallyMergedCell = new VerticallyMergedCell();
            cell327.Width = new Width(TableWidthUnit.Point, 500);
            cell327.VerticalAlignment = VerticalAlignmentType.Center;
            //cell327.Add(CellAddValue327);

            Row row32 = new Row();
            row32.Add(cell321);          
            row32.Add(cell1323);
            row32.Add(cell1324);
            row32.Add(cell1325);
            row32.Add(cell1326);
            row32.Add(cell1328);
            row32.Add(cell323);
            row32.Add(cell324);
            row32.Add(cell325);
            row32.Add(cell326);
            row32.Add(cell328);
            row32.Add(cell327);
            
            // Second row End

            // Third row start   for O LEVEL
            Run CellValue331 = new Run("O");
            CellValue331.FontSize = 18; //18 points
            CellValue331.AsciiFont = "Century Gothic";
            Paragraph CellAddValue331 = new Paragraph();
            CellAddValue331.Spacing = new Spacing();
            CellAddValue331.Spacing.After = 0;
            CellAddValue331.Add(CellValue331);
            CellAddValue331.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell331 = new Cell();
            cell331.Width = new Width(TableWidthUnit.Point, 1500);
            cell331.VerticalAlignment = VerticalAlignmentType.Center;
            cell331.Add(CellAddValue331);
           
            // for male 3r
            Run CellValue1333 = new Run(OMaleSC != "0" ? OMaleSC : "-"); // SC MALE COUNT
            CellValue1333.FontSize = 18; //18 points
            CellValue1333.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1333 = new Paragraph();
            CellAddValue1333.Spacing = new Spacing();
            CellAddValue1333.Spacing.After = 0;
            CellAddValue1333.Add(CellValue1333);
            CellAddValue1333.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1333 = new Cell();
            cell1333.Width = new Width(TableWidthUnit.Point, 500);
            cell1333.VerticalAlignment = VerticalAlignmentType.Center;
            cell1333.Add(CellAddValue1333);

            Run CellValue1334 = new Run(OMaleST != "0" ? OMaleST : "-"); // ST MALE COUNT
            CellValue1334.FontSize = 18; //18 points
            CellValue1334.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1334 = new Paragraph();
            CellAddValue1334.Spacing = new Spacing();
            CellAddValue1334.Spacing.After = 0;
            CellAddValue1334.Add(CellValue1334);
            CellAddValue1334.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1334 = new Cell();
            cell1334.Width = new Width(TableWidthUnit.Point, 500);
            cell1334.VerticalAlignment = VerticalAlignmentType.Center;
            cell1334.Add(CellAddValue1334);

            Run CellValue1335 = new Run(OMalePWD != "0" ? OMalePWD : "-"); // PWD MALE COUNT
            CellValue1335.FontSize = 18; //18 points
            CellValue1335.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1335 = new Paragraph();
            CellAddValue1335.Spacing = new Spacing();
            CellAddValue1335.Spacing.After = 0;
            CellAddValue1335.Add(CellValue1335);
            CellAddValue1335.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1335 = new Cell();
            cell1335.Width = new Width(TableWidthUnit.Point, 500);
            cell1335.VerticalAlignment = VerticalAlignmentType.Center;
            cell1335.Add(CellAddValue1335);

            Run CellValue1336 = new Run(OMaleGen != "0" ? OMaleGen : "-"); // GEN MALE COUNT
            CellValue1336.FontSize = 18; //18 points
            CellValue1336.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1336 = new Paragraph();
            CellAddValue1336.Spacing = new Spacing();
            CellAddValue1336.Spacing.After = 0;
            CellAddValue1336.Add(CellValue1336);
            CellAddValue1336.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1336 = new Cell();
            cell1336.Width = new Width(TableWidthUnit.Point, 500);
            cell1336.VerticalAlignment = VerticalAlignmentType.Center;
            cell1336.Add(CellAddValue1336);

            Run CellValue1338 = new Run(OMaleOBC != "0" ? OMaleOBC : "-");// OBC MALE COUNT
            CellValue1338.FontSize = 18; //18 points
            CellValue1338.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1338 = new Paragraph();
            CellAddValue1338.Spacing = new Spacing();
            CellAddValue1338.Spacing.After = 0;
            CellAddValue1338.Add(CellValue1338);
            CellAddValue1338.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1338 = new Cell();
            cell1338.Width = new Width(TableWidthUnit.Point, 500);
            cell1338.VerticalAlignment = VerticalAlignmentType.Center;
            cell1338.Add(CellAddValue1338);
            // for male 3r
            Run CellValue333 = new Run(OFeMaleSC != "0" ? OFeMaleSC : "-"); // FEMALE SC COUNT
            CellValue333.FontSize = 18; //18 points
            CellValue333.AsciiFont = "Century Gothic";
            Paragraph CellAddValue333 = new Paragraph();
            CellAddValue333.Spacing = new Spacing();
            CellAddValue333.Spacing.After = 0;
            CellAddValue333.Add(CellValue333);
            CellAddValue333.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell333 = new Cell();
            cell333.Width = new Width(TableWidthUnit.Point, 500);
            cell333.VerticalAlignment = VerticalAlignmentType.Center;
            cell333.Add(CellAddValue333);

            Run CellValue334 = new Run(OFeMaleST != "0" ? OFeMaleST : "-"); //FEMALE ST COUNT
            CellValue334.FontSize = 18; //18 points
            CellValue334.AsciiFont = "Century Gothic";
            Paragraph CellAddValue334 = new Paragraph();
            CellAddValue334.Spacing = new Spacing();
            CellAddValue334.Spacing.After = 0;
            CellAddValue334.Add(CellValue334);
            CellAddValue334.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell334 = new Cell();
            cell334.Width = new Width(TableWidthUnit.Point, 500);
            cell334.VerticalAlignment = VerticalAlignmentType.Center;
            cell334.Add(CellAddValue334);

            Run CellValue335 = new Run(OFeMalePWD != "0" ? OFeMalePWD : "-"); // FEMALE PWD COUNT
            CellValue335.FontSize = 18; //18 points
            CellValue335.AsciiFont = "Century Gothic";
            Paragraph CellAddValue335 = new Paragraph();
            CellAddValue335.Spacing = new Spacing();
            CellAddValue335.Spacing.After = 0;
            CellAddValue335.Add(CellValue335);
            CellAddValue335.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell335 = new Cell();
            cell335.Width = new Width(TableWidthUnit.Point, 500);
            cell335.VerticalAlignment = VerticalAlignmentType.Center;
            cell335.Add(CellAddValue335);

            Run CellValue336 = new Run(OFeMaleGen != "0" ? OFeMaleGen : "-"); // FEMALE GEN COUNT
            CellValue336.FontSize = 18; //18 points
            CellValue336.AsciiFont = "Century Gothic";
            Paragraph CellAddValue336 = new Paragraph();
            CellAddValue336.Spacing = new Spacing();
            CellAddValue336.Spacing.After = 0;
            CellAddValue336.Add(CellValue336);
            CellAddValue336.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell336 = new Cell();
            cell336.Width = new Width(TableWidthUnit.Point, 500);
            cell336.VerticalAlignment = VerticalAlignmentType.Center;
            cell336.Add(CellAddValue336);

            Run CellValue338 = new Run(OFeMaleOBC != "0" ? OFeMaleOBC : "-"); // FEMAL OBC COUNT
            CellValue338.FontSize = 18; //18 points
            CellValue338.AsciiFont = "Century Gothic";
            Paragraph CellAddValue338 = new Paragraph();
            CellAddValue338.Spacing = new Spacing();
            CellAddValue338.Spacing.After = 0;
            CellAddValue338.Add(CellValue338);
            CellAddValue338.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell338 = new Cell();
            cell338.Width = new Width(TableWidthUnit.Point, 500);
            cell338.VerticalAlignment = VerticalAlignmentType.Center;
            cell338.Add(CellAddValue338);


            Run CellValue337 = new Run(TotalApplicationRO != "0" ? TotalApplicationRO : "-"); 
            CellValue337.FontSize = 18; //18 points
            CellValue337.AsciiFont = "Century Gothic";
            Paragraph CellAddValue337 = new Paragraph();
            CellAddValue337.Spacing = new Spacing();
            CellAddValue337.Spacing.After = 0;
            CellAddValue337.Add(CellValue337);
            CellAddValue337.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell337 = new Cell();
            cell337.Width = new Width(TableWidthUnit.Point, 500);
            cell337.VerticalAlignment = VerticalAlignmentType.Center;
            cell337.Add(CellAddValue337);

            Row row33 = new Row();
            row33.Add(cell331);          
            row33.Add(cell1333);
            row33.Add(cell1334);
            row33.Add(cell1335);
            row33.Add(cell1336);
            row33.Add(cell1338);
            row33.Add(cell333);
            row33.Add(cell334);
            row33.Add(cell335);
            row33.Add(cell336);
            row33.Add(cell338);
            row33.Add(cell337);

            // Third row End   for O LEVEL

            ////////////Fourth row start   for A LEVEL
            Run CellValue341 = new Run("A");
            CellValue341.FontSize = 18; //18 points
            CellValue341.AsciiFont = "Century Gothic";
            Paragraph CellAddValue341 = new Paragraph();
            CellAddValue341.Spacing = new Spacing();
            CellAddValue341.Spacing.After = 0;
            CellAddValue341.Add(CellValue341);
            CellAddValue341.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell341 = new Cell();
            cell341.Width = new Width(TableWidthUnit.Point, 1500);
            cell341.VerticalAlignment = VerticalAlignmentType.Center;
            cell341.Add(CellAddValue341);
           
            // for male 3r
            Run CellValue1343 = new Run(AMaleSC != "0" ? AMaleSC : "-"); // A LEVEL MALE SC COUNT
            CellValue1343.FontSize = 18; //18 points
            CellValue1343.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1343 = new Paragraph();
            CellAddValue1343.Spacing = new Spacing();
            CellAddValue1343.Spacing.After = 0;
            CellAddValue1343.Add(CellValue1343);
            CellAddValue1343.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1343 = new Cell();
            cell1343.Width = new Width(TableWidthUnit.Point, 500);
            cell1343.VerticalAlignment = VerticalAlignmentType.Center;
            cell1343.Add(CellAddValue1343);

            Run CellValue1344 = new Run(AMaleST != "0" ? AMaleST : "-");// A LEVEL MALE ST COUNT
            CellValue1344.FontSize = 18; //18 points
            CellValue1344.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1344 = new Paragraph();
            CellAddValue1344.Spacing = new Spacing();
            CellAddValue1344.Spacing.After = 0;
            CellAddValue1344.Add(CellValue1344);
            CellAddValue1344.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1344 = new Cell();
            cell1344.Width = new Width(TableWidthUnit.Point, 500);
            cell1344.VerticalAlignment = VerticalAlignmentType.Center;
            cell1344.Add(CellAddValue1344);

            Run CellValue1345 = new Run(AMalePWD != "0" ? AMalePWD : "-");// A LEVEL MALE PWD COUNT
            CellValue1345.FontSize = 18; //18 points
            CellValue1345.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1345 = new Paragraph();
            CellAddValue1345.Spacing = new Spacing();
            CellAddValue1345.Spacing.After = 0;
            CellAddValue1345.Add(CellValue1345);
            CellAddValue1345.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1345 = new Cell();
            cell1345.Width = new Width(TableWidthUnit.Point, 500);
            cell1345.VerticalAlignment = VerticalAlignmentType.Center;
            cell1345.Add(CellAddValue1345);

            Run CellValue1346 = new Run(AMaleGen != "0" ? AMaleGen : "-"); // A LEVEL MALE GEN COUNT
            CellValue1346.FontSize = 18; //18 points
            CellValue1346.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1346 = new Paragraph();
            CellAddValue1346.Spacing = new Spacing();
            CellAddValue1346.Spacing.After = 0;
            CellAddValue1346.Add(CellValue1346);
            CellAddValue1346.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1346 = new Cell();
            cell1346.Width = new Width(TableWidthUnit.Point, 500);
            cell1346.VerticalAlignment = VerticalAlignmentType.Center;
            cell1346.Add(CellAddValue1346);

            Run CellValue1348 = new Run(AMaleOBC != "0" ? AMaleOBC : "-"); // A LEVEL MALE OBC COUNT
            CellValue1348.FontSize = 18; //18 points
            CellValue1348.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1348 = new Paragraph();
            CellAddValue1348.Spacing = new Spacing();
            CellAddValue1348.Spacing.After = 0;
            CellAddValue1348.Add(CellValue1348);
            CellAddValue1348.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1348 = new Cell();
            cell1348.Width = new Width(TableWidthUnit.Point, 500);
            cell1348.VerticalAlignment = VerticalAlignmentType.Center;
            cell1348.Add(CellAddValue1348);
            // for male 3r
            Run CellValue343 = new Run(AFeMaleSC != "0" ? AFeMaleSC : "-");// A LEVEL FEMALE SC COUNT
            CellValue343.FontSize = 18; //18 points
            CellValue343.AsciiFont = "Century Gothic";
            Paragraph CellAddValue343 = new Paragraph();
            CellAddValue343.Spacing = new Spacing();
            CellAddValue343.Spacing.After = 0;
            CellAddValue343.Add(CellValue343);
            CellAddValue343.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell343 = new Cell();
            cell343.Width = new Width(TableWidthUnit.Point, 500);
            cell343.VerticalAlignment = VerticalAlignmentType.Center;
            cell343.Add(CellAddValue343);

            Run CellValue344 = new Run(AFeMaleST != "0" ? AFeMaleST : "-"); // A LEVEL FEMALE ST COUNT
            CellValue344.FontSize = 18; //18 points
            CellValue344.AsciiFont = "Century Gothic";
            Paragraph CellAddValue344 = new Paragraph();
            CellAddValue344.Spacing = new Spacing();
            CellAddValue344.Spacing.After = 0;
            CellAddValue344.Add(CellValue344);
            CellAddValue344.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell344 = new Cell();
            cell344.Width = new Width(TableWidthUnit.Point, 500);
            cell344.VerticalAlignment = VerticalAlignmentType.Center;
            cell344.Add(CellAddValue344);

            Run CellValue345 = new Run(AFeMalePWD != "0" ? AFeMalePWD : "-"); // A LEVEL FEMALE PWD COUNT
            CellValue345.FontSize = 18; //18 points
            CellValue345.AsciiFont = "Century Gothic";
            Paragraph CellAddValue345 = new Paragraph();
            CellAddValue345.Spacing = new Spacing();
            CellAddValue345.Spacing.After = 0;
            CellAddValue345.Add(CellValue345);
            CellAddValue345.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell345 = new Cell();
            cell345.Width = new Width(TableWidthUnit.Point, 500);
            cell345.VerticalAlignment = VerticalAlignmentType.Center;
            cell345.Add(CellAddValue345);

            Run CellValue346 = new Run(AFeMaleGen != "0" ? AFeMaleGen : "-");// A LEVEL FEMALE GEN COUNT
            CellValue346.FontSize = 18; //18 points
            CellValue346.AsciiFont = "Century Gothic";
            Paragraph CellAddValue346 = new Paragraph();
            CellAddValue346.Spacing = new Spacing();
            CellAddValue346.Spacing.After = 0;
            CellAddValue346.Add(CellValue346);
            CellAddValue346.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell346 = new Cell();
            cell346.Width = new Width(TableWidthUnit.Point, 500);
            cell346.VerticalAlignment = VerticalAlignmentType.Center;
            cell346.Add(CellAddValue346);

            Run CellValue348 = new Run(AFeMaleOBC != "0" ? AFeMaleOBC : "-");// A LEVEL FEMALE OBC COUNT
            CellValue348.FontSize = 18; //18 points
            CellValue348.AsciiFont = "Century Gothic";
            Paragraph CellAddValue348 = new Paragraph();
            CellAddValue348.Spacing = new Spacing();
            CellAddValue348.Spacing.After = 0;
            CellAddValue348.Add(CellValue348);
            CellAddValue348.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell348 = new Cell();
            cell348.Width = new Width(TableWidthUnit.Point, 500);
            cell348.VerticalAlignment = VerticalAlignmentType.Center;
            cell348.Add(CellAddValue348);


            Run CellValue347 = new Run(TotalApplicationRA != "0" ? TotalApplicationRA : "-");
            CellValue347.FontSize = 18; //18 points
            CellValue347.AsciiFont = "Century Gothic";
            Paragraph CellAddValue347 = new Paragraph();
            CellAddValue347.Spacing = new Spacing();
            CellAddValue347.Spacing.After = 0;
            CellAddValue347.Add(CellValue347);
            CellAddValue347.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell347 = new Cell();
            cell347.Width = new Width(TableWidthUnit.Point, 500);
            cell347.VerticalAlignment = VerticalAlignmentType.Center;
            cell347.Add(CellAddValue347);

            Row row34 = new Row();
            row34.Add(cell341);
            row34.Add(cell1343);
            row34.Add(cell1344);
            row34.Add(cell1345);
            row34.Add(cell1346);
            row34.Add(cell1348);
            row34.Add(cell343);
            row34.Add(cell344);
            row34.Add(cell345);
            row34.Add(cell346);
            row34.Add(cell348);
            row34.Add(cell347);
            ////////////Fourth row End for A LEVEL

            // 5th row start FOR B LEVEL
            Run CellValue441 = new Run("B");
            CellValue441.FontSize = 18; //18 points
            CellValue441.AsciiFont = "Century Gothic";
            Paragraph CellAddValue441 = new Paragraph();
            CellAddValue441.Spacing = new Spacing();
            CellAddValue441.Spacing.After = 0;
            CellAddValue441.Add(CellValue441);
            CellAddValue441.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell441 = new Cell();
            cell441.Width = new Width(TableWidthUnit.Point, 1500);
            cell441.VerticalAlignment = VerticalAlignmentType.Center;
            cell441.Add(CellAddValue441);

            // for male 3r
            Run CellValue1443 = new Run(BMaleSC != "0" ? BMaleSC : "-"); // B LEVEL MALE SC COUNT
            CellValue1443.FontSize = 18; //18 points
            CellValue1443.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1443 = new Paragraph();
            CellAddValue1443.Spacing = new Spacing();
            CellAddValue1443.Spacing.After = 0;
            CellAddValue1443.Add(CellValue1443);
            CellAddValue1443.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1443 = new Cell();
            cell1443.Width = new Width(TableWidthUnit.Point, 500);
            cell1443.VerticalAlignment = VerticalAlignmentType.Center;
            cell1443.Add(CellAddValue1443);

            Run CellValue1444 = new Run(BMaleST != "0" ? BMaleST : "-");// B LEVEL MALE ST COUNT
            CellValue1444.FontSize = 18; //18 points
            CellValue1444.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1444 = new Paragraph();
            CellAddValue1444.Spacing = new Spacing();
            CellAddValue1444.Spacing.After = 0;
            CellAddValue1444.Add(CellValue1444);
            CellAddValue1444.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1444 = new Cell();
            cell1444.Width = new Width(TableWidthUnit.Point, 500);
            cell1444.VerticalAlignment = VerticalAlignmentType.Center;
            cell1444.Add(CellAddValue1444);

            Run CellValue1445 = new Run(BMalePWD != "0" ? BMalePWD : "-");// B LEVEL MALE PWD COUNT
            CellValue1445.FontSize = 18; //18 points
            CellValue1445.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1445 = new Paragraph();
            CellAddValue1445.Spacing = new Spacing();
            CellAddValue1445.Spacing.After = 0;
            CellAddValue1445.Add(CellValue1445);
            CellAddValue1445.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1445 = new Cell();
            cell1445.Width = new Width(TableWidthUnit.Point, 500);
            cell1445.VerticalAlignment = VerticalAlignmentType.Center;
            cell1445.Add(CellAddValue1445);

            Run CellValue1446 = new Run(BMaleGen != "0" ? BMaleGen : "-"); // B LEVEL MALE GEN COUNT
            CellValue1446.FontSize = 18; //18 points
            CellValue1446.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1446 = new Paragraph();
            CellAddValue1446.Spacing = new Spacing();
            CellAddValue1446.Spacing.After = 0;
            CellAddValue1446.Add(CellValue1446);
            CellAddValue1446.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1446 = new Cell();
            cell1446.Width = new Width(TableWidthUnit.Point, 500);
            cell1446.VerticalAlignment = VerticalAlignmentType.Center;
            cell1446.Add(CellAddValue1446);

            Run CellValue1448 = new Run(BMaleOBC != "0" ? BMaleOBC : "-"); // B LEVEL MALE OBC COUNT
            CellValue1448.FontSize = 18; //18 points
            CellValue1448.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1448 = new Paragraph();
            CellAddValue1448.Spacing = new Spacing();
            CellAddValue1448.Spacing.After = 0;
            CellAddValue1448.Add(CellValue1448);
            CellAddValue1448.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1448 = new Cell();
            cell1448.Width = new Width(TableWidthUnit.Point, 500);
            cell1448.VerticalAlignment = VerticalAlignmentType.Center;
            cell1448.Add(CellAddValue1448);
            // for male 3r
            Run CellValue443 = new Run(BFeMaleSC != "0" ? BFeMaleSC : "-");// B LEVEL FEMALE SC COUNT
            CellValue443.FontSize = 18; //18 points
            CellValue443.AsciiFont = "Century Gothic";
            Paragraph CellAddValue443 = new Paragraph();
            CellAddValue443.Spacing = new Spacing();
            CellAddValue443.Spacing.After = 0;
            CellAddValue443.Add(CellValue443);
            CellAddValue443.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell443 = new Cell();
            cell443.Width = new Width(TableWidthUnit.Point, 500);
            cell443.VerticalAlignment = VerticalAlignmentType.Center;
            cell443.Add(CellAddValue443);

            Run CellValue444 = new Run(BFeMaleST != "0" ? BFeMaleST : "-"); // B LEVEL FEMALE ST COUNT
            CellValue444.FontSize = 18; //18 points
            CellValue444.AsciiFont = "Century Gothic";
            Paragraph CellAddValue444 = new Paragraph();
            CellAddValue444.Spacing = new Spacing();
            CellAddValue444.Spacing.After = 0;
            CellAddValue444.Add(CellValue444);
            CellAddValue444.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell444 = new Cell();
            cell444.Width = new Width(TableWidthUnit.Point, 500);
            cell444.VerticalAlignment = VerticalAlignmentType.Center;
            cell444.Add(CellAddValue444);

            Run CellValue445 = new Run(BFeMalePWD != "0" ? BFeMalePWD : "-"); // B LEVEL FEMALE PWD COUNT
            CellValue445.FontSize = 18; //18 points
            CellValue445.AsciiFont = "Century Gothic";
            Paragraph CellAddValue445 = new Paragraph();
            CellAddValue445.Spacing = new Spacing();
            CellAddValue445.Spacing.After = 0;
            CellAddValue445.Add(CellValue445);
            CellAddValue445.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell445 = new Cell();
            cell445.Width = new Width(TableWidthUnit.Point, 500);
            cell445.VerticalAlignment = VerticalAlignmentType.Center;
            cell445.Add(CellAddValue445);

            Run CellValue446 = new Run(BFeMaleGen != "0" ? BFeMaleGen : "-");// B LEVEL FEMALE GEN COUNT
            CellValue446.FontSize = 18; //18 points
            CellValue446.AsciiFont = "Century Gothic";
            Paragraph CellAddValue446 = new Paragraph();
            CellAddValue446.Spacing = new Spacing();
            CellAddValue446.Spacing.After = 0;
            CellAddValue446.Add(CellValue446);
            CellAddValue446.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell446 = new Cell();
            cell446.Width = new Width(TableWidthUnit.Point, 500);
            cell446.VerticalAlignment = VerticalAlignmentType.Center;
            cell446.Add(CellAddValue446);

            Run CellValue448 = new Run(BFeMaleOBC != "0" ? BFeMaleOBC : "-");// B LEVEL FEMALE OBC COUNT
            CellValue448.FontSize = 18; //18 points
            CellValue448.AsciiFont = "Century Gothic";
            Paragraph CellAddValue448 = new Paragraph();
            CellAddValue448.Spacing = new Spacing();
            CellAddValue448.Spacing.After = 0;
            CellAddValue448.Add(CellValue448);
            CellAddValue448.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell448 = new Cell();
            cell448.Width = new Width(TableWidthUnit.Point, 500);
            cell448.VerticalAlignment = VerticalAlignmentType.Center;
            cell448.Add(CellAddValue448);


            Run CellValue447 = new Run(TotalApplicationRB != "0" ? TotalApplicationRB : "-");
            CellValue447.FontSize = 18; //18 points
            CellValue447.AsciiFont = "Century Gothic";
            Paragraph CellAddValue447 = new Paragraph();
            CellAddValue447.Spacing = new Spacing();
            CellAddValue447.Spacing.After = 0;
            CellAddValue447.Add(CellValue447);
            CellAddValue447.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell447 = new Cell();
            cell447.Width = new Width(TableWidthUnit.Point, 500);
            cell447.VerticalAlignment = VerticalAlignmentType.Center;
            cell447.Add(CellAddValue447);

            Row row35 = new Row();
            row35.Add(cell441);
            row35.Add(cell1443);
            row35.Add(cell1444);
            row35.Add(cell1445);
            row35.Add(cell1446);
            row35.Add(cell1448);
            row35.Add(cell443);
            row35.Add(cell444);
            row35.Add(cell445);
            row35.Add(cell446);
            row35.Add(cell448);
            row35.Add(cell447);
            //////////// 5th row End FOR B LEVEL

            ////////// 6th row start FOR C LEVEL
            Run CellValue541 = new Run("C");
            CellValue541.FontSize = 18; //18 points
            CellValue541.AsciiFont = "Century Gothic";
            Paragraph CellAddValue541 = new Paragraph();
            CellAddValue541.Spacing = new Spacing();
            CellAddValue541.Spacing.After = 0;
            CellAddValue541.Add(CellValue541);
            CellAddValue541.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell541 = new Cell();
            cell541.Width = new Width(TableWidthUnit.Point, 1500);
            cell541.VerticalAlignment = VerticalAlignmentType.Center;
            cell541.Add(CellAddValue541);

            // for male 3r
            Run CellValue1543 = new Run(CMaleSC != "0" ? CMaleSC : "-"); // C LEVEL MALE SC COUNT
            CellValue1543.FontSize = 18; //18 points
            CellValue1543.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1543 = new Paragraph();
            CellAddValue1543.Spacing = new Spacing();
            CellAddValue1543.Spacing.After = 0;
            CellAddValue1543.Add(CellValue1543);
            CellAddValue1543.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1543 = new Cell();
            cell1543.Width = new Width(TableWidthUnit.Point, 500);
            cell1543.VerticalAlignment = VerticalAlignmentType.Center;
            cell1543.Add(CellAddValue1543);

            Run CellValue1544 = new Run(CMaleST != "0" ? CMaleST : "-");// C LEVEL MALE ST COUNT
            CellValue1544.FontSize = 18; //18 points
            CellValue1544.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1544 = new Paragraph();
            CellAddValue1544.Spacing = new Spacing();
            CellAddValue1544.Spacing.After = 0;
            CellAddValue1544.Add(CellValue1544);
            CellAddValue1544.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1544 = new Cell();
            cell1544.Width = new Width(TableWidthUnit.Point, 500);
            cell1544.VerticalAlignment = VerticalAlignmentType.Center;
            cell1544.Add(CellAddValue1544);

            Run CellValue1545 = new Run(CMalePWD != "0" ? CMalePWD : "-");// C LEVEL MALE PWD COUNT
            CellValue1545.FontSize = 18; //18 points
            CellValue1545.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1545 = new Paragraph();
            CellAddValue1545.Spacing = new Spacing();
            CellAddValue1545.Spacing.After = 0;
            CellAddValue1545.Add(CellValue1545);
            CellAddValue1545.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1545 = new Cell();
            cell1545.Width = new Width(TableWidthUnit.Point, 500);
            cell1545.VerticalAlignment = VerticalAlignmentType.Center;
            cell1545.Add(CellAddValue1545);

            Run CellValue1546 = new Run(CMaleGen != "0" ? CMaleGen : "-"); // C LEVEL MALE GEN COUNT
            CellValue1546.FontSize = 18; //18 points
            CellValue1546.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1546 = new Paragraph();
            CellAddValue1546.Spacing = new Spacing();
            CellAddValue1546.Spacing.After = 0;
            CellAddValue1546.Add(CellValue1546);
            CellAddValue1546.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1546 = new Cell();
            cell1546.Width = new Width(TableWidthUnit.Point, 500);
            cell1546.VerticalAlignment = VerticalAlignmentType.Center;
            cell1546.Add(CellAddValue1546);

            Run CellValue1548 = new Run(CMaleOBC != "0" ? BMaleOBC : "-"); //C LEVEL MALE OBC COUNT
            CellValue1548.FontSize = 18; //18 points
            CellValue1548.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1548 = new Paragraph();
            CellAddValue1548.Spacing = new Spacing();
            CellAddValue1548.Spacing.After = 0;
            CellAddValue1548.Add(CellValue1548);
            CellAddValue1548.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1548 = new Cell();
            cell1548.Width = new Width(TableWidthUnit.Point, 500);
            cell1548.VerticalAlignment = VerticalAlignmentType.Center;
            cell1548.Add(CellAddValue1548);
            // for male 3r
            Run CellValue543 = new Run(CFeMaleSC != "0" ? CFeMaleSC : "-");// C LEVEL FEMALE SC COUNT
            CellValue543.FontSize = 18; //18 points
            CellValue543.AsciiFont = "Century Gothic";
            Paragraph CellAddValue543 = new Paragraph();
            CellAddValue543.Spacing = new Spacing();
            CellAddValue543.Spacing.After = 0;
            CellAddValue543.Add(CellValue543);
            CellAddValue543.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell543 = new Cell();
            cell543.Width = new Width(TableWidthUnit.Point, 500);
            cell543.VerticalAlignment = VerticalAlignmentType.Center;
            cell543.Add(CellAddValue543);

            Run CellValue544 = new Run(CFeMaleST != "0" ? CFeMaleST : "-"); // C LEVEL FEMALE ST COUNT
            CellValue544.FontSize = 18; //18 points
            CellValue544.AsciiFont = "Century Gothic";
            Paragraph CellAddValue544 = new Paragraph();
            CellAddValue544.Spacing = new Spacing();
            CellAddValue544.Spacing.After = 0;
            CellAddValue544.Add(CellValue544);
            CellAddValue544.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell544 = new Cell();
            cell544.Width = new Width(TableWidthUnit.Point, 500);
            cell544.VerticalAlignment = VerticalAlignmentType.Center;
            cell544.Add(CellAddValue544);

            Run CellValue545 = new Run(CFeMalePWD != "0" ? CFeMalePWD : "-"); // C LEVEL FEMALE PWD COUNT
            CellValue545.FontSize = 18; //18 points
            CellValue545.AsciiFont = "Century Gothic";
            Paragraph CellAddValue545 = new Paragraph();
            CellAddValue545.Spacing = new Spacing();
            CellAddValue545.Spacing.After = 0;
            CellAddValue545.Add(CellValue545);
            CellAddValue545.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell545 = new Cell();
            cell545.Width = new Width(TableWidthUnit.Point, 500);
            cell545.VerticalAlignment = VerticalAlignmentType.Center;
            cell545.Add(CellAddValue545);

            Run CellValue546 = new Run(CFeMaleGen != "0" ? CFeMaleGen : "-");// C LEVEL FEMALE GEN COUNT
            CellValue546.FontSize = 18; //18 points
            CellValue546.AsciiFont = "Century Gothic";
            Paragraph CellAddValue546 = new Paragraph();
            CellAddValue546.Spacing = new Spacing();
            CellAddValue546.Spacing.After = 0;
            CellAddValue546.Add(CellValue546);
            CellAddValue546.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell546 = new Cell();
            cell546.Width = new Width(TableWidthUnit.Point, 500);
            cell546.VerticalAlignment = VerticalAlignmentType.Center;
            cell546.Add(CellAddValue546);

            Run CellValue548 = new Run(CFeMaleOBC != "0" ? CFeMaleOBC : "-");// C LEVEL FEMALE OBC COUNT
            CellValue548.FontSize = 18; //18 points
            CellValue548.AsciiFont = "Century Gothic";
            Paragraph CellAddValue548 = new Paragraph();
            CellAddValue548.Spacing = new Spacing();
            CellAddValue548.Spacing.After = 0;
            CellAddValue548.Add(CellValue548);
            CellAddValue548.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell548 = new Cell();
            cell548.Width = new Width(TableWidthUnit.Point, 500);
            cell548.VerticalAlignment = VerticalAlignmentType.Center;
            cell548.Add(CellAddValue548);


            Run CellValue547 = new Run(TotalApplicationRC != "0" ? TotalApplicationRC : "-");
            CellValue547.FontSize = 18; //18 points
            CellValue547.AsciiFont = "Century Gothic";
            Paragraph CellAddValue547 = new Paragraph();
            CellAddValue547.Spacing = new Spacing();
            CellAddValue547.Spacing.After = 0;
            CellAddValue547.Add(CellValue547);
            CellAddValue547.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell547 = new Cell();
            cell547.Width = new Width(TableWidthUnit.Point, 500);
            cell547.VerticalAlignment = VerticalAlignmentType.Center;
            cell547.Add(CellAddValue547);

            Row row36 = new Row();
            row36.Add(cell541);
            row36.Add(cell1543);
            row36.Add(cell1544);
            row36.Add(cell1545);
            row36.Add(cell1546);
            row36.Add(cell1548);
            row36.Add(cell543);
            row36.Add(cell544);
            row36.Add(cell545);
            row36.Add(cell546);
            row36.Add(cell548);
            row36.Add(cell547);
            ////////// 6th row End  FOR C LEVEL

            // FOR TOTAL O A B C LEVEL START
            Run CellValue641 = new Run("Total");
            CellValue641.FontSize = 18; //18 points
            CellValue641.AsciiFont = "Century Gothic";
            Paragraph CellAddValue641 = new Paragraph();
            CellAddValue641.Spacing = new Spacing();
            CellAddValue641.Spacing.After = 0;
            CellAddValue641.Add(CellValue641);
            CellAddValue641.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell641 = new Cell();
            cell641.Width = new Width(TableWidthUnit.Point, 1500);
            cell641.VerticalAlignment = VerticalAlignmentType.Center;
            cell641.Add(CellAddValue641);

            // for male 3r
            Run CellValue1643 = new Run((Convert.ToInt32(OMaleSC) + Convert.ToInt32(AMaleSC) + Convert.ToInt32(BMaleSC) + Convert.ToInt32(CMaleSC)).ToString() != "0" ? (Convert.ToInt32(OMaleSC) + Convert.ToInt32(AMaleSC) + Convert.ToInt32(BMaleSC) + Convert.ToInt32(CMaleSC)).ToString() : "-"); // TOTAL COUNT
            CellValue1643.FontSize = 18; //18 points
            CellValue1643.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1643 = new Paragraph();
            CellAddValue1643.Spacing = new Spacing();
            CellAddValue1643.Spacing.After = 0;
            CellAddValue1643.Add(CellValue1643);
            CellAddValue1643.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1643 = new Cell();
            cell1643.Width = new Width(TableWidthUnit.Point, 500);
            cell1643.VerticalAlignment = VerticalAlignmentType.Center;
            cell1643.Add(CellAddValue1643);

            Run CellValue1644 = new Run((Convert.ToInt32(OMaleST) + Convert.ToInt32(AMaleST) + Convert.ToInt32(BMaleST) + Convert.ToInt32(CMaleST)).ToString() != "0" ? (Convert.ToInt32(OMaleST) + Convert.ToInt32(AMaleST) + Convert.ToInt32(BMaleST) + Convert.ToInt32(CMaleST)).ToString() : "-");// TOTAL COUNT
            CellValue1644.FontSize = 18; //18 points
            CellValue1644.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1644 = new Paragraph();
            CellAddValue1644.Spacing = new Spacing();
            CellAddValue1644.Spacing.After = 0;
            CellAddValue1644.Add(CellValue1644);
            CellAddValue1644.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1644 = new Cell();
            cell1644.Width = new Width(TableWidthUnit.Point, 500);
            cell1644.VerticalAlignment = VerticalAlignmentType.Center;
            cell1644.Add(CellAddValue1644);

            Run CellValue1645 = new Run((Convert.ToInt32(OMalePWD) + Convert.ToInt32(AMalePWD) + Convert.ToInt32(BMalePWD) + Convert.ToInt32(CMalePWD)).ToString() != "0" ? (Convert.ToInt32(OMalePWD) + Convert.ToInt32(AMalePWD) + Convert.ToInt32(BMalePWD) + Convert.ToInt32(CMalePWD)).ToString() : "-");// TOTAL COUNT
            CellValue1645.FontSize = 18; //18 points
            CellValue1645.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1645 = new Paragraph();
            CellAddValue1645.Spacing = new Spacing();
            CellAddValue1645.Spacing.After = 0;
            CellAddValue1645.Add(CellValue1645);
            CellAddValue1645.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1645 = new Cell();
            cell1645.Width = new Width(TableWidthUnit.Point, 500);
            cell1645.VerticalAlignment = VerticalAlignmentType.Center;
            cell1645.Add(CellAddValue1645);

            Run CellValue1646 = new Run((Convert.ToInt32(OMaleGen) + Convert.ToInt32(AMaleGen) + Convert.ToInt32(BMaleGen) + Convert.ToInt32(CMaleGen)).ToString() != "0" ? (Convert.ToInt32(OMaleGen) + Convert.ToInt32(AMaleGen) + Convert.ToInt32(BMaleGen) + Convert.ToInt32(CMaleGen)).ToString() : "-"); // TOTAL COUNT
            CellValue1646.FontSize = 18; //18 points
            CellValue1646.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1646 = new Paragraph();
            CellAddValue1646.Spacing = new Spacing();
            CellAddValue1646.Spacing.After = 0;
            CellAddValue1646.Add(CellValue1646);
            CellAddValue1646.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1646 = new Cell();
            cell1646.Width = new Width(TableWidthUnit.Point, 500);
            cell1646.VerticalAlignment = VerticalAlignmentType.Center;
            cell1646.Add(CellAddValue1646);

            Run CellValue1648 = new Run((Convert.ToInt32(OMaleOBC) + Convert.ToInt32(AMaleOBC) + Convert.ToInt32(BMaleOBC) + Convert.ToInt32(CMaleOBC)).ToString() != "0" ? (Convert.ToInt32(OMaleOBC) + Convert.ToInt32(AMaleOBC) + Convert.ToInt32(BMaleOBC) + Convert.ToInt32(CMaleOBC)).ToString() : "-"); //TOTAL COUNT
            CellValue1648.FontSize = 18; //18 points
            CellValue1648.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1648 = new Paragraph();
            CellAddValue1648.Spacing = new Spacing();
            CellAddValue1648.Spacing.After = 0;
            CellAddValue1648.Add(CellValue1648);
            CellAddValue1648.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1648 = new Cell();
            cell1648.Width = new Width(TableWidthUnit.Point, 500);
            cell1648.VerticalAlignment = VerticalAlignmentType.Center;
            cell1648.Add(CellAddValue1648);
            // for male 3r
            Run CellValue643 = new Run((Convert.ToInt32(OFeMaleSC) + Convert.ToInt32(AFeMaleSC) + Convert.ToInt32(BFeMaleSC) + Convert.ToInt32(CFeMaleSC)).ToString() != "0" ? (Convert.ToInt32(OFeMaleSC) + Convert.ToInt32(AFeMaleSC) + Convert.ToInt32(BFeMaleSC) + Convert.ToInt32(CFeMaleSC)).ToString() : "-");// TOTAL COUNT
            CellValue643.FontSize = 18; //18 points
            CellValue643.AsciiFont = "Century Gothic";
            Paragraph CellAddValue643 = new Paragraph();
            CellAddValue643.Spacing = new Spacing();
            CellAddValue643.Spacing.After = 0;
            CellAddValue643.Add(CellValue643);
            CellAddValue643.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell643 = new Cell();
            cell643.Width = new Width(TableWidthUnit.Point, 500);
            cell643.VerticalAlignment = VerticalAlignmentType.Center;
            cell643.Add(CellAddValue643);

            Run CellValue644 = new Run((Convert.ToInt32(OFeMaleST) + Convert.ToInt32(AFeMaleST) + Convert.ToInt32(BFeMaleST) + Convert.ToInt32(CFeMaleST)).ToString() != "0" ? (Convert.ToInt32(OFeMaleST) + Convert.ToInt32(AFeMaleST) + Convert.ToInt32(BFeMaleST) + Convert.ToInt32(CFeMaleST)).ToString() : "-"); // TOTAL COUNT
            CellValue644.FontSize = 18; //18 points
            CellValue644.AsciiFont = "Century Gothic";
            Paragraph CellAddValue644 = new Paragraph();
            CellAddValue644.Spacing = new Spacing();
            CellAddValue644.Spacing.After = 0;
            CellAddValue644.Add(CellValue644);
            CellAddValue644.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell644 = new Cell();
            cell644.Width = new Width(TableWidthUnit.Point, 500);
            cell644.VerticalAlignment = VerticalAlignmentType.Center;
            cell644.Add(CellAddValue644);

            Run CellValue645 = new Run((Convert.ToInt32(OFeMalePWD) + Convert.ToInt32(AFeMalePWD) + Convert.ToInt32(BFeMalePWD) + Convert.ToInt32(CFeMalePWD)).ToString() != "0" ? (Convert.ToInt32(OFeMalePWD) + Convert.ToInt32(AFeMalePWD) + Convert.ToInt32(BFeMalePWD) + Convert.ToInt32(CFeMalePWD)).ToString() : "-"); // TOTAL COUNT
            CellValue645.FontSize = 18; //18 points
            CellValue645.AsciiFont = "Century Gothic";
            Paragraph CellAddValue645 = new Paragraph();
            CellAddValue645.Spacing = new Spacing();
            CellAddValue645.Spacing.After = 0;
            CellAddValue645.Add(CellValue645);
            CellAddValue645.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell645 = new Cell();
            cell645.Width = new Width(TableWidthUnit.Point, 500);
            cell645.VerticalAlignment = VerticalAlignmentType.Center;
            cell645.Add(CellAddValue645);

            Run CellValue646 = new Run((Convert.ToInt32(OFeMaleGen) + Convert.ToInt32(AFeMaleGen) + Convert.ToInt32(BFeMaleGen) + Convert.ToInt32(CFeMaleGen)).ToString() != "0" ? (Convert.ToInt32(OFeMaleGen) + Convert.ToInt32(AFeMaleGen) + Convert.ToInt32(BFeMaleGen) + Convert.ToInt32(CFeMaleGen)).ToString() : "-");// TOTAL COUNT
            CellValue646.FontSize = 18; //18 points
            CellValue646.AsciiFont = "Century Gothic";
            Paragraph CellAddValue646 = new Paragraph();
            CellAddValue646.Spacing = new Spacing();
            CellAddValue646.Spacing.After = 0;
            CellAddValue646.Add(CellValue646);
            CellAddValue646.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell646 = new Cell();
            cell646.Width = new Width(TableWidthUnit.Point, 500);
            cell646.VerticalAlignment = VerticalAlignmentType.Center;
            cell646.Add(CellAddValue646);

            Run CellValue648 = new Run((Convert.ToInt32(OFeMaleOBC) + Convert.ToInt32(AFeMaleOBC) + Convert.ToInt32(BFeMaleOBC) + Convert.ToInt32(CFeMaleOBC)).ToString() != "0" ? (Convert.ToInt32(OFeMaleOBC) + Convert.ToInt32(AFeMaleOBC) + Convert.ToInt32(BFeMaleOBC) + Convert.ToInt32(CFeMaleOBC)).ToString() : "-");// TOTAL COUNT
            CellValue648.FontSize = 18; //18 points
            CellValue648.AsciiFont = "Century Gothic";
            Paragraph CellAddValue648 = new Paragraph();
            CellAddValue648.Spacing = new Spacing();
            CellAddValue648.Spacing.After = 0;
            CellAddValue648.Add(CellValue648);
            CellAddValue648.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell648 = new Cell();
            cell648.Width = new Width(TableWidthUnit.Point, 500);
            cell648.VerticalAlignment = VerticalAlignmentType.Center;
            cell648.Add(CellAddValue648);


            Run CellValue647 = new Run((Convert.ToInt32(TotalApplicationRO) + Convert.ToInt32(TotalApplicationRA) + Convert.ToInt32(TotalApplicationRB) + Convert.ToInt32(TotalApplicationRC)).ToString() != "0" ? (Convert.ToInt32(TotalApplicationRO) + Convert.ToInt32(TotalApplicationRA) + Convert.ToInt32(TotalApplicationRB) + Convert.ToInt32(TotalApplicationRC)).ToString() : "-"); // GRAND TOTAL COUNT
            CellValue647.FontSize = 18; //18 points
            CellValue647.AsciiFont = "Century Gothic";
            Paragraph CellAddValue647 = new Paragraph();
            CellAddValue647.Spacing = new Spacing();
            CellAddValue647.Spacing.After = 0;
            CellAddValue647.Add(CellValue647);
            CellAddValue647.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell647 = new Cell();
            cell647.Width = new Width(TableWidthUnit.Point, 500);
            cell647.VerticalAlignment = VerticalAlignmentType.Center;
            cell647.Add(CellAddValue647);

            Row row37 = new Row();
            row37.Add(cell641);
            row37.Add(cell1643);
            row37.Add(cell1644);
            row37.Add(cell1645);
            row37.Add(cell1646);
            row37.Add(cell1648);
            row37.Add(cell643);
            row37.Add(cell644);
            row37.Add(cell645);
            row37.Add(cell546);
            row37.Add(cell648);
            row37.Add(cell647);
            // FOR TOTAL O A B C LEVEL END

            Table table3 = new Table(StandardBorderStyle.SingleLine);
            table3.Width = new Width(TableWidthUnit.Percent, 90);
            table3.Alignment = HorizontalAlignmentType.Center;
            table3.Grid = tableGrid2;
            table3.Add(row31);
            table3.Add(row32);
            table3.Add(row33);
            table3.Add(row34);
            table3.Add(row35);
            table3.Add(row36);
            table3.Add(row37);
            doc.Body.Add(table3);
            // 3rd Table END
            #endregion -----------------------------------------------------------------------------

            #region  -----------Second Page of Report Start Notifications ------------------------------------------
            Run R10 = new Run();
            R10.AddText("Contd…2");
            R10.FontSize = 19; //12 points
            R10.AsciiFont = "Century Gothic";
            Paragraph P10 = new Paragraph();
            P10.Spacing = new Spacing();
            P10.Spacing.Before = 350;
            P10.Spacing.After = 400;
            P10.Add(R10);
            P10.HorizontalTextAlignment = HorizontalAlignmentType.Right;
            doc.Body.Add(P10);

            #endregion -----------------------------------------------------------------------------

            #region  ------------- WithHold Count , SC/ST, PWD & Female candidates With Table 4
            // WITHHOLD COUNT START
            // 4th Table start Classification of SC/ST, PwD & Female Candidates WithHold COUNT           
            TableGrid tableGrid4 = new TableGrid();
            tableGrid4.Columns.Add(new TableGridColumn(1000));
            tableGrid4.Columns.Add(new TableGridColumn(2200));
            tableGrid4.Columns.Add(new TableGridColumn(260));
            tableGrid4.Columns.Add(new TableGridColumn(260));
            tableGrid4.Columns.Add(new TableGridColumn(260));
            tableGrid4.Columns.Add(new TableGridColumn(260));
            tableGrid4.Columns.Add(new TableGridColumn(260));

            #region  ----------- Row 1 for Heading 1 of Withhold, Male, Female , Total -------------------------
            // First row start
            Run CellValueW402 = new Run("Withhold");
            CellValueW402.Bold = ExtendedBoolean.True;
            CellValueW402.FontSize = 18; //18 points
            CellValueW402.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW402 = new Paragraph();
            CellAddValueW402.Spacing = new Spacing();
            CellAddValueW402.Spacing.After = 10;
            CellAddValueW402.Add(CellValueW402);
            CellAddValueW402.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            CellAddValueW402.VerticalTextAlignment = VerticalTextAlignment.Center;
            Cell cellW311 = new Cell();

            //cellW311.VerticallyMergedCell = new VerticallyMergedCell();
            //cellW311.VerticallyMergedCell.Type = MergeCellType.Restart;



            cellW311.Width = new Width(TableWidthUnit.Point, 1500);
            cellW311.Shading = new Shading(ShadingPattern.Percent10);
            cellW311.VerticalAlignment = VerticalAlignmentType.Center;
            cellW311.Add(CellAddValueW402);


            Run CellValueW302 = new Run(" Male");
            CellValueW302.Bold = ExtendedBoolean.True;
            CellValueW302.FontSize = 18; //18 points
            CellValueW302.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW302 = new Paragraph();
            CellAddValueW302.Spacing = new Spacing();
            CellAddValueW302.Spacing.Before = 200;
            CellAddValueW302.Spacing.After = 200;
            CellAddValueW302.Add(CellValueW302);
            CellAddValueW302.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            //CellAddValue302.VerticalTextAlignment = VerticalTextAlignment.Center;
            Cell cellW312 = new Cell();

            cellW312.VerticallyMergedCell = new VerticallyMergedCell();
            cellW312.GridSpan = 5;
            cellW312.VerticallyMergedCell.Type = MergeCellType.Restart;

            cellW312.Width = new Width(TableWidthUnit.Point, 1500);
            cellW312.Shading = new Shading(ShadingPattern.Percent10);
            cellW312.VerticalAlignment = VerticalAlignmentType.Center;
            cellW312.Add(CellAddValueW302);

            Run CellValueW303 = new Run(" Female");
            CellValueW303.Bold = ExtendedBoolean.True;
            CellValueW303.FontSize = 18; //18 points
            CellValueW303.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW303 = new Paragraph();
            CellAddValueW303.Spacing = new Spacing();
            CellAddValueW303.Spacing.Before = 200;
            CellAddValueW303.Spacing.After = 200;
            CellAddValueW303.Add(CellValueW303);
            CellAddValueW303.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW313 = new Cell();
            cellW313.VerticallyMergedCell = new VerticallyMergedCell();
            cellW313.GridSpan = 5;
            cellW313.VerticallyMergedCell.Type = MergeCellType.Restart;
            cellW313.Width = new Width(TableWidthUnit.Point, 1500);
            cellW313.Shading = new Shading(ShadingPattern.Percent10);
            cellW313.VerticalAlignment = VerticalAlignmentType.Center;
            cellW313.Add(CellAddValueW303);

            Run CellValueW304 = new Run("Total");
            CellValueW304.Bold = ExtendedBoolean.True;
            CellValueW304.FontSize = 18; //18 points
            CellValueW304.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW304 = new Paragraph();
            //CellAddValueW304.Spacing = new Spacing();
            //CellAddValueW304.Spacing.After = 0;
            CellAddValueW304.Add(CellValueW304);
            CellAddValueW304.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW314 = new Cell();
            cellW314.VerticallyMergedCell = new VerticallyMergedCell();
            cellW314.VerticallyMergedCell.Type = MergeCellType.Restart;
            cellW314.Width = new Width(TableWidthUnit.Point, 700);
            cellW314.Shading = new Shading(ShadingPattern.Percent10);
            cellW314.VerticalAlignment = VerticalAlignmentType.Center;
            cellW314.Add(CellAddValueW304);

            Row rowW31 = new Row();
            rowW31.Add(cellW311);
            rowW31.Add(cellW312);
            rowW31.Add(cellW313);
            rowW31.Add(cellW314);
            rowW31.Alignment = HorizontalAlignmentType.Center;
            // First row End 
             #endregion -----------------------------------------------------------------------------

            #region ----- Row 2 for Heading 2 Level, SC, ST,PWD , GEN, OBC , TOTAL ---------------------------
            // Second row start 
            Run CellValueW321 = new Run("Level");
            CellValueW321.FontSize = 18; //18 points
            CellValueW321.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW321 = new Paragraph();
            CellAddValueW321.Spacing = new Spacing();
            CellAddValueW321.Spacing.After = 0;
            CellAddValueW321.Add(CellValueW321);
            CellAddValueW321.HorizontalTextAlignment = HorizontalAlignmentType.Center;

            Cell cellW321 = new Cell();
            cellW321.Width = new Width(TableWidthUnit.Point, 1500);
            cellW321.VerticalAlignment = VerticalAlignmentType.Center;
            cellW321.Add(CellAddValueW321);
                       
            // for male
            Run CellValue1W323 = new Run("SC");
            CellValue1W323.FontSize = 18; //18 points
            CellValue1W323.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W323 = new Paragraph();
            CellAddValue1W323.Spacing = new Spacing();
            CellAddValue1W323.Spacing.After = 0;
            CellAddValue1W323.Add(CellValue1W323);
            CellAddValue1W323.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W323 = new Cell();
            cell1W323.Width = new Width(TableWidthUnit.Point, 500);
            cell1W323.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W323.Add(CellAddValue1W323);

            Run CellValue1W324 = new Run("ST");
            CellValue1W324.FontSize = 18; //18 points
            CellValue1W324.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W324 = new Paragraph();
            CellAddValue1W324.Spacing = new Spacing();
            CellAddValue1W324.Spacing.After = 0;
            CellAddValue1W324.Add(CellValue1W324);
            CellAddValue1W324.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W324 = new Cell();
            cell1W324.Width = new Width(TableWidthUnit.Point, 500);
            cell1W324.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W324.Add(CellAddValue1W324);

            Run CellValue1W325 = new Run("PwD");
            CellValue1W325.FontSize = 18; //18 points
            CellValue1W325.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W325 = new Paragraph();
            CellAddValue1W325.Spacing = new Spacing();
            CellAddValue1W325.Spacing.After = 0;
            CellAddValue1W325.Add(CellValue1W325);
            CellAddValue1W325.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W325 = new Cell();
            cell1W325.Width = new Width(TableWidthUnit.Point, 500);
            cell1W325.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W325.Add(CellAddValue1W325);

            Run CellValue1W326 = new Run("Gen");
            CellValue1W326.FontSize = 18; //18 points
            CellValue1W326.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W326 = new Paragraph();
            CellAddValue1W326.Spacing = new Spacing();
            CellAddValue1W326.Spacing.After = 0;
            CellAddValue1W326.Add(CellValue1W326);
            CellAddValue1W326.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W326 = new Cell();
            cell1W326.Width = new Width(TableWidthUnit.Point, 500);
            cell1W326.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W326.Add(CellAddValue1W326);

            Run CellValue1W328 = new Run("OBC");
            CellValue1W328.FontSize = 18; //18 points
            CellValue1W328.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W328 = new Paragraph();
            CellAddValue1W328.Spacing = new Spacing();
            CellAddValue1W328.Spacing.After = 0;
            CellAddValue1W328.Add(CellValue1W328);
            CellAddValue1W328.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W328 = new Cell();
            cell1W328.Width = new Width(TableWidthUnit.Point, 500);
            cell1W328.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W328.Add(CellAddValue1W328);
            //formale end

            Run CellValueW323 = new Run("SC");
            CellValueW323.FontSize = 18; //18 points
            CellValueW323.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW323 = new Paragraph();
            CellAddValueW323.Spacing = new Spacing();
            CellAddValueW323.Spacing.After = 0;
            CellAddValueW323.Add(CellValueW323);
            CellAddValueW323.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW323 = new Cell();
            cellW323.Width = new Width(TableWidthUnit.Point, 500);
            cellW323.VerticalAlignment = VerticalAlignmentType.Center;
            cellW323.Add(CellAddValueW323);

            Run CellValueW324 = new Run("ST");
            CellValueW324.FontSize = 18; //18 points
            CellValueW324.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW324 = new Paragraph();
            CellAddValueW324.Spacing = new Spacing();
            CellAddValueW324.Spacing.After = 0;
            CellAddValueW324.Add(CellValueW324);
            CellAddValueW324.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW324 = new Cell();
            cellW324.Width = new Width(TableWidthUnit.Point, 500);
            cellW324.VerticalAlignment = VerticalAlignmentType.Center;
            cellW324.Add(CellAddValueW324);

            Run CellValueW325 = new Run("PwD");
            CellValueW325.FontSize = 18; //18 points
            CellValueW325.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW325 = new Paragraph();
            CellAddValueW325.Spacing = new Spacing();
            CellAddValueW325.Spacing.After = 0;
            CellAddValueW325.Add(CellValueW325);
            CellAddValueW325.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW325 = new Cell();
            cellW325.Width = new Width(TableWidthUnit.Point, 500);
            cellW325.VerticalAlignment = VerticalAlignmentType.Center;
            cellW325.Add(CellAddValueW325);

            Run CellValueW326 = new Run("Gen");
            CellValueW326.FontSize = 18; //18 points
            CellValueW326.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW326 = new Paragraph();
            CellAddValueW326.Spacing = new Spacing();
            CellAddValueW326.Spacing.After = 0;
            CellAddValueW326.Add(CellValueW326);
            CellAddValueW326.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW326 = new Cell();
            cellW326.Width = new Width(TableWidthUnit.Point, 500);
            cellW326.VerticalAlignment = VerticalAlignmentType.Center;
            cellW326.Add(CellAddValueW326);

            Run CellValueW328 = new Run("OBC");
            CellValueW328.FontSize = 18; //18 points
            CellValueW328.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW328 = new Paragraph();
            CellAddValueW328.Spacing = new Spacing();
            CellAddValueW328.Spacing.After = 0;
            CellAddValueW328.Add(CellValueW328);
            CellAddValueW328.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW328 = new Cell();
            cellW328.Width = new Width(TableWidthUnit.Point, 500);
            cellW328.VerticalAlignment = VerticalAlignmentType.Center;
            cellW328.Add(CellAddValueW328);

            //Run CellValueW327 = new Run("Last");
            //CellValueW327.FontSize = 18; //18 points
            //CellValueW327.AsciiFont = "Century Gothic";
            //Paragraph CellAddValueW327 = new Paragraph();
            //CellAddValueW327.Spacing = new Spacing();
            //CellAddValueW327.Spacing.After = 0;
            //CellAddValueW327.Add(CellValueW327);
            //CellAddValueW327.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW327 = new Cell();
            cellW327.VerticallyMergedCell = new VerticallyMergedCell();
            cellW327.Width = new Width(TableWidthUnit.Point, 500);
            cellW327.VerticalAlignment = VerticalAlignmentType.Center;
            //cellW327.Add(CellAddValueW327);

            Row rowW32 = new Row();
            rowW32.Add(cellW321);
            rowW32.Add(cell1W323);
            rowW32.Add(cell1W324);
            rowW32.Add(cell1W325);
            rowW32.Add(cell1W326);
            rowW32.Add(cell1W328);
            rowW32.Add(cellW323);
            rowW32.Add(cellW324);
            rowW32.Add(cellW325);
            rowW32.Add(cellW326);
            rowW32.Add(cellW328);
            rowW32.Add(cellW327);
            // Second row End
             #endregion -----------------------------------------------------------------------------

            #region  -------- Row 3 for O Level count --------------------------------
            // Third row start   for O LEVEL
            Run CellValueW331 = new Run("O");
            CellValueW331.FontSize = 18; //18 points
            CellValueW331.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW331 = new Paragraph();
            CellAddValueW331.Spacing = new Spacing();
            CellAddValueW331.Spacing.After = 0;
            CellAddValueW331.Add(CellValueW331);
            CellAddValueW331.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW331 = new Cell();
            cellW331.Width = new Width(TableWidthUnit.Point, 1500);
            cellW331.VerticalAlignment = VerticalAlignmentType.Center;
            cellW331.Add(CellAddValueW331);

            // for male 3r
            Run CellValue1W333 = new Run(OMaleSC1 != "0" ? OMaleSC1 : "-"); // SC MALE COUNT
            CellValue1W333.FontSize = 18; //18 points
            CellValue1W333.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W333 = new Paragraph();
            CellAddValue1W333.Spacing = new Spacing();
            CellAddValue1W333.Spacing.After = 0;
            CellAddValue1W333.Add(CellValue1W333);
            CellAddValue1W333.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W333 = new Cell();
            cell1W333.Width = new Width(TableWidthUnit.Point, 500);
            cell1W333.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W333.Add(CellAddValue1W333);

            Run CellValue1W334 = new Run(OMaleST1 != "0" ? OMaleST1 : "-"); // ST MALE COUNT
            CellValue1W334.FontSize = 18; //18 points
            CellValue1W334.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W334 = new Paragraph();
            CellAddValue1W334.Spacing = new Spacing();
            CellAddValue1W334.Spacing.After = 0;
            CellAddValue1W334.Add(CellValue1W334);
            CellAddValue1W334.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W334 = new Cell();
            cell1W334.Width = new Width(TableWidthUnit.Point, 500);
            cell1W334.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W334.Add(CellAddValue1W334);

            Run CellValue1W335 = new Run(OMalePWD1 != "0" ? OMalePWD1 : "-"); // PWD MALE COUNT
            CellValue1W335.FontSize = 18; //18 points
            CellValue1W335.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W335 = new Paragraph();
            CellAddValue1W335.Spacing = new Spacing();
            CellAddValue1W335.Spacing.After = 0;
            CellAddValue1W335.Add(CellValue1W335);
            CellAddValue1W335.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W335 = new Cell();
            cell1W335.Width = new Width(TableWidthUnit.Point, 500);
            cell1W335.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W335.Add(CellAddValue1W335);

            Run CellValue1W336 = new Run(OMaleGen1 != "0" ? OMaleGen1 : "-"); // GEN MALE COUNT
            CellValue1W336.FontSize = 18; //18 points
            CellValue1W336.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W336 = new Paragraph();
            CellAddValue1W336.Spacing = new Spacing();
            CellAddValue1W336.Spacing.After = 0;
            CellAddValue1W336.Add(CellValue1W336);
            CellAddValue1W336.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W336 = new Cell();
            cell1W336.Width = new Width(TableWidthUnit.Point, 500);
            cell1W336.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W336.Add(CellAddValue1W336);

            Run CellValue1W338 = new Run(OMaleOBC1 != "0" ? OMaleOBC1 : "-");// OBC MALE COUNT
            CellValue1W338.FontSize = 18; //18 points
            CellValue1W338.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W338 = new Paragraph();
            CellAddValue1W338.Spacing = new Spacing();
            CellAddValue1W338.Spacing.After = 0;
            CellAddValue1W338.Add(CellValue1W338);
            CellAddValue1W338.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W338 = new Cell();
            cell1W338.Width = new Width(TableWidthUnit.Point, 500);
            cell1W338.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W338.Add(CellAddValue1W338);
            // for male 3r
            Run CellValueW333 = new Run(OFeMaleSC1 != "0" ? OFeMaleSC1 : "-"); // FEMALE SC COUNT
            CellValueW333.FontSize = 18; //18 points
            CellValueW333.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW333 = new Paragraph();
            CellAddValueW333.Spacing = new Spacing();
            CellAddValueW333.Spacing.After = 0;
            CellAddValueW333.Add(CellValueW333);
            CellAddValueW333.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW333 = new Cell();
            cellW333.Width = new Width(TableWidthUnit.Point, 500);
            cellW333.VerticalAlignment = VerticalAlignmentType.Center;
            cellW333.Add(CellAddValueW333);

            Run CellValueW334 = new Run(OFeMaleST1 != "0" ? OFeMaleST1 : "-"); //FEMALE ST COUNT
            CellValueW334.FontSize = 18; //18 points
            CellValueW334.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW334 = new Paragraph();
            CellAddValueW334.Spacing = new Spacing();
            CellAddValueW334.Spacing.After = 0;
            CellAddValueW334.Add(CellValueW334);
            CellAddValueW334.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW334 = new Cell();
            cellW334.Width = new Width(TableWidthUnit.Point, 500);
            cellW334.VerticalAlignment = VerticalAlignmentType.Center;
            cellW334.Add(CellAddValueW334);

            Run CellValueW335 = new Run(OFeMalePWD1 != "0" ? OFeMalePWD1 : "-"); // FEMALE PWD COUNT
            CellValueW335.FontSize = 18; //18 points
            CellValueW335.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW335 = new Paragraph();
            CellAddValueW335.Spacing = new Spacing();
            CellAddValueW335.Spacing.After = 0;
            CellAddValueW335.Add(CellValueW335);
            CellAddValueW335.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW335 = new Cell();
            cellW335.Width = new Width(TableWidthUnit.Point, 500);
            cellW335.VerticalAlignment = VerticalAlignmentType.Center;
            cellW335.Add(CellAddValueW335);

            Run CellValueW336 = new Run(OFeMaleGen1 != "0" ? OFeMaleGen1 : "-"); // FEMALE GEN COUNT
            CellValueW336.FontSize = 18; //18 points
            CellValueW336.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW336 = new Paragraph();
            CellAddValueW336.Spacing = new Spacing();
            CellAddValueW336.Spacing.After = 0;
            CellAddValueW336.Add(CellValueW336);
            CellAddValueW336.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW336 = new Cell();
            cellW336.Width = new Width(TableWidthUnit.Point, 500);
            cellW336.VerticalAlignment = VerticalAlignmentType.Center;
            cellW336.Add(CellAddValueW336);

            Run CellValueW338 = new Run(OFeMaleOBC1 != "0" ? OFeMaleOBC1 : "-"); // FEMAL OBC COUNT
            CellValueW338.FontSize = 18; //18 points
            CellValueW338.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW338 = new Paragraph();
            CellAddValueW338.Spacing = new Spacing();
            CellAddValueW338.Spacing.After = 0;
            CellAddValueW338.Add(CellValueW338);
            CellAddValueW338.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW338 = new Cell();
            cellW338.Width = new Width(TableWidthUnit.Point, 500);
            cellW338.VerticalAlignment = VerticalAlignmentType.Center;
            cellW338.Add(CellAddValueW338);


            Run CellValueW337 = new Run(TotalApplicationRO1 != "0" ? TotalApplicationRO1 : "-");
            CellValueW337.FontSize = 18; //18 points
            CellValueW337.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW337 = new Paragraph();
            CellAddValueW337.Spacing = new Spacing();
            CellAddValueW337.Spacing.After = 0;
            CellAddValueW337.Add(CellValueW337);
            CellAddValueW337.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW337 = new Cell();
            cellW337.Width = new Width(TableWidthUnit.Point, 500);
            cellW337.VerticalAlignment = VerticalAlignmentType.Center;
            cellW337.Add(CellAddValueW337);

            Row rowW33 = new Row();
            rowW33.Add(cellW331);
            rowW33.Add(cell1W333);
            rowW33.Add(cell1W334);
            rowW33.Add(cell1W335);
            rowW33.Add(cell1W336);
            rowW33.Add(cell1W338);
            rowW33.Add(cellW333);
            rowW33.Add(cellW334);
            rowW33.Add(cellW335);
            rowW33.Add(cellW336);
            rowW33.Add(cellW338);
            rowW33.Add(cellW337);
            // Third row End   for O LEVEL
             #endregion -----------------------------------------------------------------------------

            #region ----- Row 4 for A Level Count -----------------------------------------------
            ////////////Fourth row start   for A LEVEL
            Run CellValueW341 = new Run("A");
            CellValueW341.FontSize = 18; //18 points
            CellValueW341.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW341 = new Paragraph();
            CellAddValueW341.Spacing = new Spacing();
            CellAddValueW341.Spacing.After = 0;
            CellAddValueW341.Add(CellValueW341);
            CellAddValueW341.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW341 = new Cell();
            cellW341.Width = new Width(TableWidthUnit.Point, 1500);
            cellW341.VerticalAlignment = VerticalAlignmentType.Center;
            cellW341.Add(CellAddValueW341);

            // for male 3r
            Run CellValue1W343 = new Run(AMaleSC1 != "0" ? AMaleSC1 : "-"); // A LEVEL MALE SC COUNT
            CellValue1W343.FontSize = 18; //18 points
            CellValue1W343.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W343 = new Paragraph();
            CellAddValue1W343.Spacing = new Spacing();
            CellAddValue1W343.Spacing.After = 0;
            CellAddValue1W343.Add(CellValue1W343);
            CellAddValue1W343.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W343 = new Cell();
            cell1W343.Width = new Width(TableWidthUnit.Point, 500);
            cell1W343.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W343.Add(CellAddValue1W343);

            Run CellValue1W344 = new Run(AMaleST1 != "0" ? AMaleST1 : "-");// A LEVEL MALE ST COUNT
            CellValue1W344.FontSize = 18; //18 points
            CellValue1W344.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W344 = new Paragraph();
            CellAddValue1W344.Spacing = new Spacing();
            CellAddValue1W344.Spacing.After = 0;
            CellAddValue1W344.Add(CellValue1W344);
            CellAddValue1W344.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W344 = new Cell();
            cell1W344.Width = new Width(TableWidthUnit.Point, 500);
            cell1W344.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W344.Add(CellAddValue1W344);

            Run CellValue1W345 = new Run(AMalePWD1 != "0" ? AMalePWD1 : "-");// A LEVEL MALE PWD COUNT
            CellValue1W345.FontSize = 18; //18 points
            CellValue1W345.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W345 = new Paragraph();
            CellAddValue1W345.Spacing = new Spacing();
            CellAddValue1W345.Spacing.After = 0;
            CellAddValue1W345.Add(CellValue1W345);
            CellAddValue1W345.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W345 = new Cell();
            cell1W345.Width = new Width(TableWidthUnit.Point, 500);
            cell1W345.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W345.Add(CellAddValue1W345);

            Run CellValue1W346 = new Run(AMaleGen1 != "0" ? AMaleGen1 : "-"); // A LEVEL MALE GEN COUNT
            CellValue1W346.FontSize = 18; //18 points
            CellValue1W346.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W346 = new Paragraph();
            CellAddValue1W346.Spacing = new Spacing();
            CellAddValue1W346.Spacing.After = 0;
            CellAddValue1W346.Add(CellValue1W346);
            CellAddValue1W346.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W346 = new Cell();
            cell1W346.Width = new Width(TableWidthUnit.Point, 500);
            cell1W346.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W346.Add(CellAddValue1W346);

            Run CellValue1W348 = new Run(AMaleOBC1 != "0" ? AMaleOBC1 : "-"); // A LEVEL MALE OBC COUNT
            CellValue1W348.FontSize = 18; //18 points
            CellValue1W348.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W348 = new Paragraph();
            CellAddValue1W348.Spacing = new Spacing();
            CellAddValue1W348.Spacing.After = 0;
            CellAddValue1W348.Add(CellValue1W348);
            CellAddValue1W348.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W348 = new Cell();
            cell1W348.Width = new Width(TableWidthUnit.Point, 500);
            cell1W348.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W348.Add(CellAddValue1W348);
            // for male 3r
            Run CellValueW343 = new Run(AFeMaleSC1 != "0" ? AFeMaleSC1 : "-");// A LEVEL FEMALE SC COUNT
            CellValueW343.FontSize = 18; //18 points
            CellValueW343.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW343 = new Paragraph();
            CellAddValueW343.Spacing = new Spacing();
            CellAddValueW343.Spacing.After = 0;
            CellAddValueW343.Add(CellValueW343);
            CellAddValueW343.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW343 = new Cell();
            cellW343.Width = new Width(TableWidthUnit.Point, 500);
            cellW343.VerticalAlignment = VerticalAlignmentType.Center;
            cellW343.Add(CellAddValueW343);

            Run CellValueW344 = new Run(AFeMaleST1 != "0" ? AFeMaleST1 : "-"); // A LEVEL FEMALE ST COUNT
            CellValueW344.FontSize = 18; //18 points
            CellValueW344.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW344 = new Paragraph();
            CellAddValueW344.Spacing = new Spacing();
            CellAddValueW344.Spacing.After = 0;
            CellAddValueW344.Add(CellValueW344);
            CellAddValueW344.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW344 = new Cell();
            cellW344.Width = new Width(TableWidthUnit.Point, 500);
            cellW344.VerticalAlignment = VerticalAlignmentType.Center;
            cellW344.Add(CellAddValueW344);

            Run CellValueW345 = new Run(AFeMalePWD1 != "0" ? AFeMalePWD1 : "-"); // A LEVEL FEMALE PWD COUNT
            CellValueW345.FontSize = 18; //18 points
            CellValueW345.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW345 = new Paragraph();
            CellAddValueW345.Spacing = new Spacing();
            CellAddValueW345.Spacing.After = 0;
            CellAddValueW345.Add(CellValueW345);
            CellAddValueW345.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW345 = new Cell();
            cellW345.Width = new Width(TableWidthUnit.Point, 500);
            cellW345.VerticalAlignment = VerticalAlignmentType.Center;
            cellW345.Add(CellAddValueW345);

            Run CellValueW346 = new Run(AFeMaleGen1 != "0" ? AFeMaleGen1 : "-");// A LEVEL FEMALE GEN COUNT
            CellValueW346.FontSize = 18; //18 points
            CellValueW346.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW346 = new Paragraph();
            CellAddValueW346.Spacing = new Spacing();
            CellAddValueW346.Spacing.After = 0;
            CellAddValueW346.Add(CellValueW346);
            CellAddValueW346.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW346 = new Cell();
            cellW346.Width = new Width(TableWidthUnit.Point, 500);
            cellW346.VerticalAlignment = VerticalAlignmentType.Center;
            cellW346.Add(CellAddValueW346);

            Run CellValueW348 = new Run(AFeMaleOBC1 != "0" ? AFeMaleOBC1 : "-");// A LEVEL FEMALE OBC COUNT
            CellValueW348.FontSize = 18; //18 points
            CellValueW348.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW348 = new Paragraph();
            CellAddValueW348.Spacing = new Spacing();
            CellAddValueW348.Spacing.After = 0;
            CellAddValueW348.Add(CellValueW348);
            CellAddValueW348.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW348 = new Cell();
            cellW348.Width = new Width(TableWidthUnit.Point, 500);
            cellW348.VerticalAlignment = VerticalAlignmentType.Center;
            cellW348.Add(CellAddValueW348);


            Run CellValueW347 = new Run(TotalApplicationRA1 != "0" ? TotalApplicationRA1 : "-");
            CellValueW347.FontSize = 18; //18 points
            CellValueW347.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW347 = new Paragraph();
            CellAddValueW347.Spacing = new Spacing();
            CellAddValueW347.Spacing.After = 0;
            CellAddValueW347.Add(CellValueW347);
            CellAddValueW347.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW347 = new Cell();
            cellW347.Width = new Width(TableWidthUnit.Point, 500);
            cellW347.VerticalAlignment = VerticalAlignmentType.Center;
            cellW347.Add(CellAddValueW347);

            Row rowW34 = new Row();
            rowW34.Add(cellW341);
            rowW34.Add(cell1W343);
            rowW34.Add(cell1W344);
            rowW34.Add(cell1W345);
            rowW34.Add(cell1W346);
            rowW34.Add(cell1W348);
            rowW34.Add(cellW343);
            rowW34.Add(cellW344);
            rowW34.Add(cellW345);
            rowW34.Add(cellW346);
            rowW34.Add(cellW348);
            rowW34.Add(cellW347);
            ////////////Fourth row End for A LEVEL
             #endregion -----------------------------------------------------------------------------

            #region ---- Row 5 for B Level Count ------------------------------------------------
            // 5th row start FOR B LEVEL
            Run CellValueW441 = new Run("B");
            CellValueW441.FontSize = 18; //18 points
            CellValueW441.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW441 = new Paragraph();
            CellAddValueW441.Spacing = new Spacing();
            CellAddValueW441.Spacing.After = 0;
            CellAddValueW441.Add(CellValueW441);
            CellAddValueW441.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW441 = new Cell();
            cellW441.Width = new Width(TableWidthUnit.Point, 1500);
            cellW441.VerticalAlignment = VerticalAlignmentType.Center;
            cellW441.Add(CellAddValueW441);

            // for male 3r
            Run CellValue1W443 = new Run(BMaleSC1 != "0" ? BMaleSC1 : "-"); // B LEVEL MALE SC COUNT
            CellValue1W443.FontSize = 18; //18 points
            CellValue1W443.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W443 = new Paragraph();
            CellAddValue1W443.Spacing = new Spacing();
            CellAddValue1W443.Spacing.After = 0;
            CellAddValue1W443.Add(CellValue1W443);
            CellAddValue1W443.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W443 = new Cell();
            cell1W443.Width = new Width(TableWidthUnit.Point, 500);
            cell1W443.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W443.Add(CellAddValue1W443);

            Run CellValue1W444 = new Run(BMaleST1 != "0" ? BMaleST1 : "-");// B LEVEL MALE ST COUNT
            CellValue1W444.FontSize = 18; //18 points
            CellValue1W444.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W444 = new Paragraph();
            CellAddValue1W444.Spacing = new Spacing();
            CellAddValue1W444.Spacing.After = 0;
            CellAddValue1W444.Add(CellValue1W444);
            CellAddValue1W444.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W444 = new Cell();
            cell1W444.Width = new Width(TableWidthUnit.Point, 500);
            cell1W444.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W444.Add(CellAddValue1W444);

            Run CellValue1W445 = new Run(BMalePWD1 != "0" ? BMalePWD1 : "-");// B LEVEL MALE PWD COUNT
            CellValue1W445.FontSize = 18; //18 points
            CellValue1W445.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W445 = new Paragraph();
            CellAddValue1W445.Spacing = new Spacing();
            CellAddValue1W445.Spacing.After = 0;
            CellAddValue1W445.Add(CellValue1W445);
            CellAddValue1W445.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W445 = new Cell();
            cell1W445.Width = new Width(TableWidthUnit.Point, 500);
            cell1W445.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W445.Add(CellAddValue1W445);

            Run CellValue1W446 = new Run(BMaleGen1 != "0" ? BMaleGen1 : "-"); // B LEVEL MALE GEN COUNT
            CellValue1W446.FontSize = 18; //18 points
            CellValue1W446.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W446 = new Paragraph();
            CellAddValue1W446.Spacing = new Spacing();
            CellAddValue1W446.Spacing.After = 0;
            CellAddValue1W446.Add(CellValue1W446);
            CellAddValue1W446.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W446 = new Cell();
            cell1W446.Width = new Width(TableWidthUnit.Point, 500);
            cell1W446.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W446.Add(CellAddValue1W446);

            Run CellValue1W448 = new Run(BMaleOBC1 != "0" ? BMaleOBC1 : "-"); // B LEVEL MALE OBC COUNT
            CellValue1W448.FontSize = 18; //18 points
            CellValue1W448.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W448 = new Paragraph();
            CellAddValue1W448.Spacing = new Spacing();
            CellAddValue1W448.Spacing.After = 0;
            CellAddValue1W448.Add(CellValue1W448);
            CellAddValue1W448.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W448 = new Cell();
            cell1W448.Width = new Width(TableWidthUnit.Point, 500);
            cell1W448.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W448.Add(CellAddValue1W448);
            // for male 3r
            Run CellValueW443 = new Run(BFeMaleSC1 != "0" ? BFeMaleSC1 : "-");// B LEVEL FEMALE SC COUNT
            CellValueW443.FontSize = 18; //18 points
            CellValueW443.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW443 = new Paragraph();
            CellAddValueW443.Spacing = new Spacing();
            CellAddValueW443.Spacing.After = 0;
            CellAddValueW443.Add(CellValueW443);
            CellAddValueW443.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW443 = new Cell();
            cellW443.Width = new Width(TableWidthUnit.Point, 500);
            cellW443.VerticalAlignment = VerticalAlignmentType.Center;
            cellW443.Add(CellAddValueW443);

            Run CellValueW444 = new Run(BFeMaleST1 != "0" ? BFeMaleST1 : "-"); // B LEVEL FEMALE ST COUNT
            CellValueW444.FontSize = 18; //18 points
            CellValueW444.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW444 = new Paragraph();
            CellAddValueW444.Spacing = new Spacing();
            CellAddValueW444.Spacing.After = 0;
            CellAddValueW444.Add(CellValueW444);
            CellAddValueW444.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW444 = new Cell();
            cellW444.Width = new Width(TableWidthUnit.Point, 500);
            cellW444.VerticalAlignment = VerticalAlignmentType.Center;
            cellW444.Add(CellAddValueW444);

            Run CellValueW445 = new Run(BFeMalePWD1 != "0" ? BFeMalePWD1 : "-"); // B LEVEL FEMALE PWD COUNT
            CellValueW445.FontSize = 18; //18 points
            CellValueW445.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW445 = new Paragraph();
            CellAddValueW445.Spacing = new Spacing();
            CellAddValueW445.Spacing.After = 0;
            CellAddValueW445.Add(CellValueW445);
            CellAddValueW445.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW445 = new Cell();
            cellW445.Width = new Width(TableWidthUnit.Point, 500);
            cellW445.VerticalAlignment = VerticalAlignmentType.Center;
            cellW445.Add(CellAddValueW445);

            Run CellValueW446 = new Run(BFeMaleGen1 != "0" ? BFeMaleGen1 : "-");// B LEVEL FEMALE GEN COUNT
            CellValueW446.FontSize = 18; //18 points
            CellValueW446.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW446 = new Paragraph();
            CellAddValueW446.Spacing = new Spacing();
            CellAddValueW446.Spacing.After = 0;
            CellAddValueW446.Add(CellValueW446);
            CellAddValueW446.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW446 = new Cell();
            cellW446.Width = new Width(TableWidthUnit.Point, 500);
            cellW446.VerticalAlignment = VerticalAlignmentType.Center;
            cellW446.Add(CellAddValueW446);

            Run CellValueW448 = new Run(BFeMaleOBC1 != "0" ? BFeMaleOBC1 : "-");// B LEVEL FEMALE OBC COUNT
            CellValueW448.FontSize = 18; //18 points
            CellValueW448.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW448 = new Paragraph();
            CellAddValueW448.Spacing = new Spacing();
            CellAddValueW448.Spacing.After = 0;
            CellAddValueW448.Add(CellValueW448);
            CellAddValueW448.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW448 = new Cell();
            cellW448.Width = new Width(TableWidthUnit.Point, 500);
            cellW448.VerticalAlignment = VerticalAlignmentType.Center;
            cellW448.Add(CellAddValueW448);


            Run CellValueW447 = new Run(TotalApplicationRB1 != "0" ? TotalApplicationRB1 : "-");
            CellValueW447.FontSize = 18; //18 points
            CellValueW447.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW447 = new Paragraph();
            CellAddValueW447.Spacing = new Spacing();
            CellAddValueW447.Spacing.After = 0;
            CellAddValueW447.Add(CellValueW447);
            CellAddValueW447.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW447 = new Cell();
            cellW447.Width = new Width(TableWidthUnit.Point, 500);
            cellW447.VerticalAlignment = VerticalAlignmentType.Center;
            cellW447.Add(CellAddValueW447);

            Row rowW35 = new Row();
            rowW35.Add(cellW441);
            rowW35.Add(cell1W443);
            rowW35.Add(cell1W444);
            rowW35.Add(cell1W445);
            rowW35.Add(cell1446);
            rowW35.Add(cell1W448);
            rowW35.Add(cellW443);
            rowW35.Add(cellW444);
            rowW35.Add(cellW445);
            rowW35.Add(cellW446);
            rowW35.Add(cellW448);
            rowW35.Add(cellW447);
            //////////// 5th row End FOR B LEVEL
             #endregion -----------------------------------------------------------------------------

            #region --- Row 6 for C Level count -------------------------------------------
            ////////// 6th row start FOR C LEVEL
            Run CellValueW541 = new Run("C");
            CellValueW541.FontSize = 18; //18 points
            CellValueW541.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW541 = new Paragraph();
            CellAddValueW541.Spacing = new Spacing();
            CellAddValueW541.Spacing.After = 0;
            CellAddValueW541.Add(CellValueW541);
            CellAddValueW541.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW541 = new Cell();
            cellW541.Width = new Width(TableWidthUnit.Point, 1500);
            cellW541.VerticalAlignment = VerticalAlignmentType.Center;
            cellW541.Add(CellAddValueW541);

            // for male 3r
            Run CellValue1W543 = new Run(CMaleSC1 != "0" ? CMaleSC1 : "-"); // C LEVEL MALE SC COUNT
            CellValue1W543.FontSize = 18; //18 points
            CellValue1W543.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W543 = new Paragraph();
            CellAddValue1W543.Spacing = new Spacing();
            CellAddValue1W543.Spacing.After = 0;
            CellAddValue1W543.Add(CellValue1W543);
            CellAddValue1W543.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W543 = new Cell();
            cell1W543.Width = new Width(TableWidthUnit.Point, 500);
            cell1W543.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W543.Add(CellAddValue1W543);

            Run CellValue1W544 = new Run(CMaleST1 != "0" ? CMaleST1 : "-");// C LEVEL MALE ST COUNT
            CellValue1W544.FontSize = 18; //18 points
            CellValue1W544.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W544 = new Paragraph();
            CellAddValue1W544.Spacing = new Spacing();
            CellAddValue1W544.Spacing.After = 0;
            CellAddValue1W544.Add(CellValue1W544);
            CellAddValue1W544.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W544 = new Cell();
            cell1W544.Width = new Width(TableWidthUnit.Point, 500);
            cell1W544.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W544.Add(CellAddValue1W544);

            Run CellValue1W545 = new Run(CMalePWD1 != "0" ? CMalePWD1 : "-");// C LEVEL MALE PWD COUNT
            CellValue1W545.FontSize = 18; //18 points
            CellValue1W545.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W545 = new Paragraph();
            CellAddValue1W545.Spacing = new Spacing();
            CellAddValue1W545.Spacing.After = 0;
            CellAddValue1W545.Add(CellValue1W545);
            CellAddValue1W545.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W545 = new Cell();
            cell1W545.Width = new Width(TableWidthUnit.Point, 500);
            cell1W545.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W545.Add(CellAddValue1W545);

            Run CellValue1W546 = new Run(CMaleGen1 != "0" ? CMaleGen1 : "-"); // C LEVEL MALE GEN COUNT
            CellValue1W546.FontSize = 18; //18 points
            CellValue1W546.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W546 = new Paragraph();
            CellAddValue1W546.Spacing = new Spacing();
            CellAddValue1W546.Spacing.After = 0;
            CellAddValue1W546.Add(CellValue1W546);
            CellAddValue1W546.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W546 = new Cell();
            cell1W546.Width = new Width(TableWidthUnit.Point, 500);
            cell1W546.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W546.Add(CellAddValue1W546);

            Run CellValue1W548 = new Run(CMaleOBC1 != "0" ? BMaleOBC1 : "-"); //C LEVEL MALE OBC COUNT
            CellValue1W548.FontSize = 18; //18 points
            CellValue1W548.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W548 = new Paragraph();
            CellAddValue1W548.Spacing = new Spacing();
            CellAddValue1W548.Spacing.After = 0;
            CellAddValue1W548.Add(CellValue1W548);
            CellAddValue1W548.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W548 = new Cell();
            cell1W548.Width = new Width(TableWidthUnit.Point, 500);
            cell1W548.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W548.Add(CellAddValue1W548);
            // for male 3r
            Run CellValueW543 = new Run(CFeMaleSC1 != "0" ? CFeMaleSC1 : "-");// C LEVEL FEMALE SC COUNT
            CellValueW543.FontSize = 18; //18 points
            CellValueW543.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW543 = new Paragraph();
            CellAddValueW543.Spacing = new Spacing();
            CellAddValueW543.Spacing.After = 0;
            CellAddValueW543.Add(CellValueW543);
            CellAddValueW543.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW543 = new Cell();
            cellW543.Width = new Width(TableWidthUnit.Point, 500);
            cellW543.VerticalAlignment = VerticalAlignmentType.Center;
            cellW543.Add(CellAddValueW543);

            Run CellValueW544 = new Run(CFeMaleST1 != "0" ? CFeMaleST1 : "-"); // C LEVEL FEMALE ST COUNT
            CellValueW544.FontSize = 18; //18 points
            CellValueW544.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW544 = new Paragraph();
            CellAddValueW544.Spacing = new Spacing();
            CellAddValueW544.Spacing.After = 0;
            CellAddValueW544.Add(CellValueW544);
            CellAddValueW544.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW544 = new Cell();
            cellW544.Width = new Width(TableWidthUnit.Point, 500);
            cellW544.VerticalAlignment = VerticalAlignmentType.Center;
            cellW544.Add(CellAddValueW544);

            Run CellValueW545 = new Run(CFeMalePWD1 != "0" ? CFeMalePWD1 : "-"); // C LEVEL FEMALE PWD COUNT
            CellValueW545.FontSize = 18; //18 points
            CellValueW545.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW545 = new Paragraph();
            CellAddValueW545.Spacing = new Spacing();
            CellAddValueW545.Spacing.After = 0;
            CellAddValueW545.Add(CellValueW545);
            CellAddValueW545.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW545 = new Cell();
            cellW545.Width = new Width(TableWidthUnit.Point, 500);
            cellW545.VerticalAlignment = VerticalAlignmentType.Center;
            cellW545.Add(CellAddValueW545);

            Run CellValueW546 = new Run(CFeMaleGen1 != "0" ? CFeMaleGen1 : "-");// C LEVEL FEMALE GEN COUNT
            CellValueW546.FontSize = 18; //18 points
            CellValueW546.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW546 = new Paragraph();
            CellAddValueW546.Spacing = new Spacing();
            CellAddValueW546.Spacing.After = 0;
            CellAddValueW546.Add(CellValueW546);
            CellAddValueW546.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW546 = new Cell();
            cellW546.Width = new Width(TableWidthUnit.Point, 500);
            cellW546.VerticalAlignment = VerticalAlignmentType.Center;
            cellW546.Add(CellAddValueW546);

            Run CellValueW548 = new Run(CFeMaleOBC1 != "0" ? CFeMaleOBC1 : "-");// C LEVEL FEMALE OBC COUNT
            CellValueW548.FontSize = 18; //18 points
            CellValueW548.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW548 = new Paragraph();
            CellAddValueW548.Spacing = new Spacing();
            CellAddValueW548.Spacing.After = 0;
            CellAddValueW548.Add(CellValueW548);
            CellAddValueW548.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW548 = new Cell();
            cellW548.Width = new Width(TableWidthUnit.Point, 500);
            cellW548.VerticalAlignment = VerticalAlignmentType.Center;
            cellW548.Add(CellAddValueW548);


            Run CellValueW547 = new Run(TotalApplicationRC1 != "0" ? TotalApplicationRC1 : "-");
            CellValueW547.FontSize = 18; //18 points
            CellValueW547.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW547 = new Paragraph();
            CellAddValueW547.Spacing = new Spacing();
            CellAddValueW547.Spacing.After = 0;
            CellAddValueW547.Add(CellValueW547);
            CellAddValueW547.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW547 = new Cell();
            cellW547.Width = new Width(TableWidthUnit.Point, 500);
            cellW547.VerticalAlignment = VerticalAlignmentType.Center;
            cellW547.Add(CellAddValueW547);

            Row rowW36 = new Row();
            rowW36.Add(cellW541);
            rowW36.Add(cell1W543);
            rowW36.Add(cell1W544);
            rowW36.Add(cell1W545);
            rowW36.Add(cell1W546);
            rowW36.Add(cell1W548);
            rowW36.Add(cellW543);
            rowW36.Add(cellW544);
            rowW36.Add(cellW545);
            rowW36.Add(cellW546);
            rowW36.Add(cellW548);
            rowW36.Add(cellW547);
            ////////// 6th row End  FOR C LEVEL
             #endregion -----------------------------------------------------------------------------

            #region ----  Row 7 for Total Count -----------------------------------------------
            // FOR TOTAL O A B C LEVEL START
            Run CellValueW641 = new Run("Total");
            CellValueW641.FontSize = 18; //18 points
            CellValueW641.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW641 = new Paragraph();
            CellAddValueW641.Spacing = new Spacing();
            CellAddValueW641.Spacing.After = 0;
            CellAddValueW641.Add(CellValueW641);
            CellAddValueW641.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW641 = new Cell();
            cellW641.Width = new Width(TableWidthUnit.Point, 1500);
            cellW641.VerticalAlignment = VerticalAlignmentType.Center;
            cellW641.Add(CellAddValueW641);

            // for male 3r
            Run CellValue1W643 = new Run((Convert.ToInt32(OMaleSC1) + Convert.ToInt32(AMaleSC1) + Convert.ToInt32(BMaleSC1) + Convert.ToInt32(CMaleSC1)).ToString() != "0" ? (Convert.ToInt32(OMaleSC1) + Convert.ToInt32(AMaleSC1) + Convert.ToInt32(BMaleSC1) + Convert.ToInt32(CMaleSC1)).ToString() : "-"); // TOTAL COUNT
            CellValue1W643.FontSize = 18; //18 points
            CellValue1W643.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W643 = new Paragraph();
            CellAddValue1W643.Spacing = new Spacing();
            CellAddValue1W643.Spacing.After = 0;
            CellAddValue1W643.Add(CellValue1W643);
            CellAddValue1W643.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W643 = new Cell();
            cell1W643.Width = new Width(TableWidthUnit.Point, 500);
            cell1W643.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W643.Add(CellAddValue1W643);

            Run CellValue1W644 = new Run((Convert.ToInt32(OMaleST1) + Convert.ToInt32(AMaleST1) + Convert.ToInt32(BMaleST1) + Convert.ToInt32(CMaleST1)).ToString() != "0" ? (Convert.ToInt32(OMaleST1) + Convert.ToInt32(AMaleST1) + Convert.ToInt32(BMaleST1) + Convert.ToInt32(CMaleST1)).ToString() : "-");// TOTAL COUNT
            CellValue1W644.FontSize = 18; //18 points
            CellValue1W644.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W644 = new Paragraph();
            CellAddValue1W644.Spacing = new Spacing();
            CellAddValue1W644.Spacing.After = 0;
            CellAddValue1W644.Add(CellValue1W644);
            CellAddValue1W644.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W644 = new Cell();
            cell1W644.Width = new Width(TableWidthUnit.Point, 500);
            cell1W644.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W644.Add(CellAddValue1W644);

            Run CellValue1W645 = new Run((Convert.ToInt32(OMalePWD1) + Convert.ToInt32(AMalePWD1) + Convert.ToInt32(BMalePWD1) + Convert.ToInt32(CMalePWD1)).ToString() != "0" ? (Convert.ToInt32(OMalePWD1) + Convert.ToInt32(AMalePWD1) + Convert.ToInt32(BMalePWD1) + Convert.ToInt32(CMalePWD1)).ToString() : "-");// TOTAL COUNT
            CellValue1W645.FontSize = 18; //18 points
            CellValue1W645.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W645 = new Paragraph();
            CellAddValue1W645.Spacing = new Spacing();
            CellAddValue1W645.Spacing.After = 0;
            CellAddValue1W645.Add(CellValue1W645);
            CellAddValue1W645.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W645 = new Cell();
            cell1W645.Width = new Width(TableWidthUnit.Point, 500);
            cell1W645.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W645.Add(CellAddValue1W645);

            Run CellValue1W646 = new Run((Convert.ToInt32(OMaleGen1) + Convert.ToInt32(AMaleGen1) + Convert.ToInt32(BMaleGen1) + Convert.ToInt32(CMaleGen1)).ToString() != "0" ? (Convert.ToInt32(OMaleGen1) + Convert.ToInt32(AMaleGen1) + Convert.ToInt32(BMaleGen1) + Convert.ToInt32(CMaleGen1)).ToString() : "-"); // TOTAL COUNT
            CellValue1W646.FontSize = 18; //18 points
            CellValue1W646.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W646 = new Paragraph();
            CellAddValue1W646.Spacing = new Spacing();
            CellAddValue1W646.Spacing.After = 0;
            CellAddValue1W646.Add(CellValue1W646);
            CellAddValue1W646.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W646 = new Cell();
            cell1W646.Width = new Width(TableWidthUnit.Point, 500);
            cell1W646.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W646.Add(CellAddValue1W646);

            Run CellValue1W648 = new Run((Convert.ToInt32(OMaleOBC1) + Convert.ToInt32(AMaleOBC1) + Convert.ToInt32(BMaleOBC1) + Convert.ToInt32(CMaleOBC1)).ToString() != "0" ? (Convert.ToInt32(OMaleOBC1) + Convert.ToInt32(AMaleOBC1) + Convert.ToInt32(BMaleOBC1) + Convert.ToInt32(CMaleOBC1)).ToString() : "-"); //TOTAL COUNT
            CellValue1W648.FontSize = 18; //18 points
            CellValue1W648.AsciiFont = "Century Gothic";
            Paragraph CellAddValue1W648 = new Paragraph();
            CellAddValue1W648.Spacing = new Spacing();
            CellAddValue1W648.Spacing.After = 0;
            CellAddValue1W648.Add(CellValue1W648);
            CellAddValue1W648.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cell1W648 = new Cell();
            cell1W648.Width = new Width(TableWidthUnit.Point, 500);
            cell1W648.VerticalAlignment = VerticalAlignmentType.Center;
            cell1W648.Add(CellAddValue1W648);
            // for male 3r
            Run CellValueW643 = new Run((Convert.ToInt32(OFeMaleSC1) + Convert.ToInt32(AFeMaleSC1) + Convert.ToInt32(BFeMaleSC1) + Convert.ToInt32(CFeMaleSC1)).ToString() != "0" ? (Convert.ToInt32(OFeMaleSC1) + Convert.ToInt32(AFeMaleSC1) + Convert.ToInt32(BFeMaleSC1) + Convert.ToInt32(CFeMaleSC1)).ToString() : "-");// TOTAL COUNT
            CellValueW643.FontSize = 18; //18 points
            CellValueW643.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW643 = new Paragraph();
            CellAddValueW643.Spacing = new Spacing();
            CellAddValueW643.Spacing.After = 0;
            CellAddValueW643.Add(CellValueW643);
            CellAddValueW643.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW643 = new Cell();
            cellW643.Width = new Width(TableWidthUnit.Point, 500);
            cellW643.VerticalAlignment = VerticalAlignmentType.Center;
            cellW643.Add(CellAddValueW643);

            Run CellValueW644 = new Run((Convert.ToInt32(OFeMaleST1) + Convert.ToInt32(AFeMaleST1) + Convert.ToInt32(BFeMaleST1) + Convert.ToInt32(CFeMaleST1)).ToString() != "0" ? (Convert.ToInt32(OFeMaleST1) + Convert.ToInt32(AFeMaleST1) + Convert.ToInt32(BFeMaleST1) + Convert.ToInt32(CFeMaleST1)).ToString() : "-"); // TOTAL COUNT
            CellValueW644.FontSize = 18; //18 points
            CellValueW644.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW644 = new Paragraph();
            CellAddValueW644.Spacing = new Spacing();
            CellAddValueW644.Spacing.After = 0;
            CellAddValueW644.Add(CellValueW644);
            CellAddValueW644.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW644 = new Cell();
            cellW644.Width = new Width(TableWidthUnit.Point, 500);
            cellW644.VerticalAlignment = VerticalAlignmentType.Center;
            cellW644.Add(CellAddValueW644);

            Run CellValueW645 = new Run((Convert.ToInt32(OFeMalePWD1) + Convert.ToInt32(AFeMalePWD1) + Convert.ToInt32(BFeMalePWD1) + Convert.ToInt32(CFeMalePWD1)).ToString() != "0" ? (Convert.ToInt32(OFeMalePWD1) + Convert.ToInt32(AFeMalePWD1) + Convert.ToInt32(BFeMalePWD1) + Convert.ToInt32(CFeMalePWD1)).ToString() : "-"); // TOTAL COUNT
            CellValueW645.FontSize = 18; //18 points
            CellValueW645.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW645 = new Paragraph();
            CellAddValueW645.Spacing = new Spacing();
            CellAddValueW645.Spacing.After = 0;
            CellAddValueW645.Add(CellValueW645);
            CellAddValueW645.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW645 = new Cell();
            cellW645.Width = new Width(TableWidthUnit.Point, 500);
            cellW645.VerticalAlignment = VerticalAlignmentType.Center;
            cellW645.Add(CellAddValueW645);

            Run CellValueW646 = new Run((Convert.ToInt32(OFeMaleGen1) + Convert.ToInt32(AFeMaleGen1) + Convert.ToInt32(BFeMaleGen1) + Convert.ToInt32(CFeMaleGen1)).ToString() != "0" ? (Convert.ToInt32(OFeMaleGen1) + Convert.ToInt32(AFeMaleGen1) + Convert.ToInt32(BFeMaleGen1) + Convert.ToInt32(CFeMaleGen1)).ToString() : "-");// TOTAL COUNT
            CellValueW646.FontSize = 18; //18 points
            CellValueW646.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW646 = new Paragraph();
            CellAddValueW646.Spacing = new Spacing();
            CellAddValueW646.Spacing.After = 0;
            CellAddValueW646.Add(CellValueW646);
            CellAddValueW646.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW646 = new Cell();
            cellW646.Width = new Width(TableWidthUnit.Point, 500);
            cellW646.VerticalAlignment = VerticalAlignmentType.Center;
            cellW646.Add(CellAddValueW646);

            Run CellValueW648 = new Run((Convert.ToInt32(OFeMaleOBC1) + Convert.ToInt32(AFeMaleOBC1) + Convert.ToInt32(BFeMaleOBC1) + Convert.ToInt32(CFeMaleOBC1)).ToString() != "0" ? (Convert.ToInt32(OFeMaleOBC1) + Convert.ToInt32(AFeMaleOBC1) + Convert.ToInt32(BFeMaleOBC1) + Convert.ToInt32(CFeMaleOBC1)).ToString() : "-");// TOTAL COUNT
            CellValueW648.FontSize = 18; //18 points
            CellValueW648.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW648 = new Paragraph();
            CellAddValueW648.Spacing = new Spacing();
            CellAddValueW648.Spacing.After = 0;
            CellAddValueW648.Add(CellValueW648);
            CellAddValueW648.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW648 = new Cell();
            cellW648.Width = new Width(TableWidthUnit.Point, 500);
            cellW648.VerticalAlignment = VerticalAlignmentType.Center;
            cellW648.Add(CellAddValueW648);


            Run CellValueW647 = new Run((Convert.ToInt32(TotalApplicationRO1) + Convert.ToInt32(TotalApplicationRA1) + Convert.ToInt32(TotalApplicationRB1) + Convert.ToInt32(TotalApplicationRC1)).ToString() != "0" ? (Convert.ToInt32(TotalApplicationRO1) + Convert.ToInt32(TotalApplicationRA1) + Convert.ToInt32(TotalApplicationRB1) + Convert.ToInt32(TotalApplicationRC1)).ToString() : "-"); // GRAND TOTAL COUNT
            CellValueW647.FontSize = 18; //18 points
            CellValueW647.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW647 = new Paragraph();
            CellAddValueW647.Spacing = new Spacing();
            CellAddValueW647.Spacing.After = 0;
            CellAddValueW647.Add(CellValueW647);
            CellAddValueW647.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW647 = new Cell();
            cellW647.Width = new Width(TableWidthUnit.Point, 500);
            cellW647.VerticalAlignment = VerticalAlignmentType.Center;
            cellW647.Add(CellAddValueW647);

            Row rowW37 = new Row();
            rowW37.Add(cellW641);
            rowW37.Add(cell1W643);
            rowW37.Add(cell1W644);
            rowW37.Add(cell1W645);
            rowW37.Add(cell1W646);
            rowW37.Add(cell1W648);
            rowW37.Add(cellW643);
            rowW37.Add(cellW644);
            rowW37.Add(cellW645);
            rowW37.Add(cellW546);
            rowW37.Add(cellW648);
            rowW37.Add(cellW647);
            // FOR TOTAL O A B C LEVEL END
             #endregion -----------------------------------------------------------------------------

            Table table4 = new Table(StandardBorderStyle.SingleLine);
            table4.Width = new Width(TableWidthUnit.Percent, 90);
            table4.Alignment = HorizontalAlignmentType.Center;
            table4.Grid = tableGrid4;
            table4.Add(rowW31);
            table4.Add(rowW32);
            table4.Add(rowW33);
            table4.Add(rowW34);
            table4.Add(rowW35);
            table4.Add(rowW36);
            table4.Add(rowW37);
            doc.Body.Add(table4);           
            // 4th Table END
            // WITHHOLD COUNT END

            #endregion -----------------------------------------------------------------------------

            #region --- Point Fifth ------------------------------------
            // First Line add
            Run R11 = new Run();
            R11.AddText("5.  The status of the Protsahan Puraskar (formerly Scholarship) applications under consideration in respect of   " +
            "O/A/B & C Levels Examination held  in ........ is as stated under:");
            R11.FontSize = 18; //12 points
            R11.AsciiFont = "Century Gothic";
            Paragraph P11 = new Paragraph();
            P11.Spacing = new Spacing();
            P11.Spacing.Before = 100;
            P11.Add(R11);
            P11.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            doc.Body.Add(P11);
            //First Line add end
            #endregion -----------------------------------------------------------------------------

            #region --- Point Fifth  a  with table------------------------------------

            Run R124 = new Run();
            R124.AddText("\t");
            Run R123 = new Run();
            R123.AddText("(a)");           
            Run R122 = new Run();
            R123.Bold = ExtendedBoolean.True;
            R123.FontSize = 18; //12 points
            R123.AsciiFont = "Century Gothic";
            R122.Bold = ExtendedBoolean.True;
            R122.FontSize = 18; //12 points
            R122.AsciiFont = "Century Gothic";
            R122.AddText("Recommended:");
            R122.Underline = new Underline(UnderlinePattern.Single);
            Run R12 = new Run();
            R12.AddText(" There are ............ applications, found to be eligible for Protsahan Puraskar for ...... no. of" +
                          " modules, submitted for recommendations. The details are placed at pg. ................ to ................. The total Protsahan " +
           " Puraskar to be released for ............ examination has been worked-out as Rs. ........./- as detailed below:");         
            R12.FontSize = 18; //12 points
            R12.AsciiFont = "Century Gothic";         
            Paragraph P12 = new Paragraph();
            P12.Spacing = new Spacing();
            P12.Spacing.Before = 300;
            P12.HorizontalTextAlignment = HorizontalAlignmentType.Both;          
            //P12.Add(R124);
            P12.Add(R123);
            P12.Add(R122);
            P12.Add(R12);
           
            doc.Body.Add(P12);

            #region ---------- Table 5 for Recommended Details -----------------------------------------------------
            //First Table with Merge cell Start
            TableGrid tableGrid5 = new TableGrid();
            tableGrid5.Columns.Add(new TableGridColumn(498));
            tableGrid5.Columns.Add(new TableGridColumn(1200));
            tableGrid5.Columns.Add(new TableGridColumn(1200));

            #region ---------- Row 1 Heading -------------------------------------------------------
            // First row start 
            Run CellValueR = new Run("SL. No");
            CellValueR.Bold = ExtendedBoolean.True;
            CellValueR.FontSize = 18; //18 points
            CellValueR.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR = new Paragraph();
            CellAddValueR.Spacing = new Spacing();
            CellAddValueR.Spacing.After = 0;
            CellAddValueR.Add(CellValueR);
            CellAddValueR.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR11 = new Cell();
            //cellR11.VerticallyMergedCell = new VerticallyMergedCell();
            //cellR11.GridSpan = 3;         
            cellR11.Shading = new Shading(ShadingPattern.Percent10);
            cellR11.Width = new Width(TableWidthUnit.Point, 900);
            cellR11.VerticalAlignment = VerticalAlignmentType.Center;
            cellR11.Add(CellAddValueR);

            Run CellValueR12 = new Run("Level");
            CellValueR12.Bold = ExtendedBoolean.True;
            CellValueR12.FontSize = 18; //18 points
            CellValueR12.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR12 = new Paragraph();
            CellAddValueR12.Spacing = new Spacing();
            CellAddValueR12.Spacing.After = 0;
            CellAddValueR12.Add(CellValueR12);            
            CellAddValueR12.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR12 = new Cell();
            cellR12.Shading = new Shading(ShadingPattern.Percent10);
            cellR12.Width = new Width(TableWidthUnit.Point, 900);
            cellR12.VerticalAlignment = VerticalAlignmentType.Center;
            cellR12.Add(CellAddValueR12);

            Run CellValueR13 = new Run("Application Recommended");
            CellValueR13.Bold = ExtendedBoolean.True;
            CellValueR13.FontSize = 18; //18 points
            CellValueR13.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR13 = new Paragraph();
            CellAddValueR13.Spacing = new Spacing();
            CellAddValueR13.Spacing.After = 0;
            CellAddValueR13.Add(CellValueR13);            
            CellAddValueR13.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR13 = new Cell();
            cellR13.Shading = new Shading(ShadingPattern.Percent10);
            cellR13.Width = new Width(TableWidthUnit.Point, 1200);
            cellR13.VerticalAlignment = VerticalAlignmentType.Center;
            cellR13.Add(CellAddValueR13);

            Run CellValueR14 = new Run("Total No. of Modules  \n     (a)");
            CellValueR14.Bold = ExtendedBoolean.True;
            CellValueR14.FontSize = 18; //18 points
            CellValueR14.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR14 = new Paragraph();
            CellAddValueR14.Spacing = new Spacing();
            CellAddValueR14.Spacing.After = 0;
            CellAddValueR14.Add(CellValueR14);
            CellAddValueR14.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR14 = new Cell();
            cellR14.Shading = new Shading(ShadingPattern.Percent10);
            cellR14.Width = new Width(TableWidthUnit.Point, 1200);
            cellR14.VerticalAlignment = VerticalAlignmentType.Center;
            cellR14.Add(CellAddValueR14);

            Run CellValueR15 = new Run("Scholarship Amt per module \n        (b)");
            CellValueR15.Bold = ExtendedBoolean.True;
            CellValueR15.FontSize = 18; //18 points
            CellValueR15.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR15 = new Paragraph();
            CellAddValueR15.Spacing = new Spacing();
            CellAddValueR15.Spacing.After = 0;
            CellAddValueR15.Add(CellValueR15);
            CellAddValueR15.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR15 = new Cell();
            cellR15.Shading = new Shading(ShadingPattern.Percent10);
            cellR15.Width = new Width(TableWidthUnit.Point, 1200);
            cellR15.VerticalAlignment = VerticalAlignmentType.Center;
            cellR15.Add(CellAddValueR15);

            Run CellValueR16 = new Run("Total amount \n (in Rs)");
            CellValueR16.Bold = ExtendedBoolean.True;
            CellValueR16.FontSize = 18; //18 points
            CellValueR16.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR16 = new Paragraph();
            CellAddValueR16.Spacing = new Spacing();
            CellAddValueR16.Spacing.After = 0;
            CellAddValueR16.Add(CellValueR16);
            CellAddValueR16.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR16 = new Cell();
            cellR16.Shading = new Shading(ShadingPattern.Percent10);
            cellR16.Width = new Width(TableWidthUnit.Point, 1200);
            cellR16.VerticalAlignment = VerticalAlignmentType.Center;
            cellR16.Add(CellAddValueR16);

            Row rowR1 = new Row();
            rowR1.Add(cellR11);           
            rowR1.Add(cellR12);
            rowR1.Add(cellR13);
            rowR1.Add(cellR14);
            rowR1.Add(cellR15);
            rowR1.Add(cellR16);



            // First row End 
            #endregion -----------------------------------------------------------------------------

            #region -----------  Row 2  ----------------------------------------------------

            // Second row start 
            Run CellValueR21 = new Run("1");
            CellValueR21.FontSize = 18; //18 points
            CellValueR21.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR21 = new Paragraph();
            CellAddValueR21.Spacing = new Spacing();
            CellAddValueR21.Spacing.After = 0;
            CellAddValueR21.Add(CellValueR21);
            CellAddValueR21.HorizontalTextAlignment = HorizontalAlignmentType.Center;

            Cell cellR21 = new Cell();
            cellR21.Width = new Width(TableWidthUnit.Point, 900);
            cellR21.VerticalAlignment = VerticalAlignmentType.Center;
            cellR21.Add(CellAddValueR21);

            Run CellValueR22 = new Run("'O'");
            CellValueR22.FontSize = 18; //18 points
            CellValueR22.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR22 = new Paragraph();
            CellAddValueR22.Spacing = new Spacing();
            CellAddValueR22.Spacing.After = 0;
            CellAddValueR22.Add(CellValueR22);
            CellAddValueR22.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR22 = new Cell();
            cellR22.Width = new Width(TableWidthUnit.Point, 900);
            cellR22.VerticalAlignment = VerticalAlignmentType.Center;
            cellR22.Add(CellAddValueR22);

            Run CellValueR23 = new Run("628");
            CellValueR23.FontSize = 18; //18 points
            CellValueR23.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR23 = new Paragraph();
            CellAddValueR23.Spacing = new Spacing();
            CellAddValueR23.Spacing.After = 0;
            CellAddValueR23.Add(CellValueR23);
            CellAddValueR23.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR23 = new Cell();
            cellR23.Width = new Width(TableWidthUnit.Point, 1200);
            cellR23.VerticalAlignment = VerticalAlignmentType.Center;
            cellR23.Add(CellAddValueR23);

            Run CellValueR24 = new Run("628");
            CellValueR24.FontSize = 18; //18 points
            CellValueR24.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR24 = new Paragraph();
            CellAddValueR24.Spacing = new Spacing();
            CellAddValueR24.Spacing.After = 0;
            CellAddValueR24.Add(CellValueR24);
            CellAddValueR24.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR24 = new Cell();
            cellR24.Width = new Width(TableWidthUnit.Point, 1200);
            cellR24.VerticalAlignment = VerticalAlignmentType.Center;
            cellR24.Add(CellAddValueR24);

            Run CellValueR25 = new Run("628");
            CellValueR25.FontSize = 18; //18 points
            CellValueR25.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR25 = new Paragraph();
            CellAddValueR25.Spacing = new Spacing();
            CellAddValueR25.Spacing.After = 0;
            CellAddValueR25.Add(CellValueR25);
            CellAddValueR25.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR25 = new Cell();
            cellR25.Width = new Width(TableWidthUnit.Point, 1200);
            cellR25.VerticalAlignment = VerticalAlignmentType.Center;
            cellR25.Add(CellAddValueR25);

            Run CellValueR26 = new Run("628");
            CellValueR26.FontSize = 18; //18 points
            CellValueR26.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR26 = new Paragraph();
            CellAddValueR26.Spacing = new Spacing();
            CellAddValueR26.Spacing.After = 0;
            CellAddValueR26.Add(CellValueR26);
            CellAddValueR26.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR26 = new Cell();
            cellR26.Width = new Width(TableWidthUnit.Point, 1200);
            cellR26.VerticalAlignment = VerticalAlignmentType.Center;
            cellR26.Add(CellAddValueR26);
            Row rowR2 = new Row();
            rowR2.Add(cellR21);
            rowR2.Add(cellR22);
            rowR2.Add(cellR23);
            rowR2.Add(cellR24);
            rowR2.Add(cellR25);
            rowR2.Add(cellR26);
            // Second row End
            #endregion -----------------------------------------------------------------------------

            #region --------- Row 3 --------------------------------------------------------
            // Third row start
            Run CellValueR31 = new Run("2");
            CellValueR31.FontSize = 18; //18 points
            CellValueR31.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR31 = new Paragraph();
            CellAddValueR31.Spacing = new Spacing();
            CellAddValueR31.Spacing.After = 0;
            CellAddValueR31.Add(CellValueR31);
            CellAddValueR31.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR31 = new Cell();
            cellR31.Width = new Width(TableWidthUnit.Point, 900);
            cellR31.VerticalAlignment = VerticalAlignmentType.Center;
            cellR31.Add(CellAddValueR31);

            Run CellValueR32 = new Run("'A'"); // 
            CellValueR32.FontSize = 18; //18 points
            CellValueR32.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR32 = new Paragraph();
            CellAddValueR32.Spacing = new Spacing();
            CellAddValueR32.Spacing.After = 0;
            CellAddValueR32.Add(CellValueR32);
            CellAddValueR32.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR32 = new Cell();
            cellR32.Width = new Width(TableWidthUnit.Point, 900);
            cellR32.VerticalAlignment = VerticalAlignmentType.Center;
            cellR32.Add(CellAddValueR32);

            Run CellValueR33 = new Run("-"); // A LEVEL COUNT
            CellValueR33.FontSize = 18; //18 points
            CellValueR33.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR33 = new Paragraph();
            CellAddValueR33.Spacing = new Spacing();
            CellAddValueR33.Spacing.After = 0;
            CellAddValueR33.Add(CellValueR33);
            CellAddValueR33.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR33 = new Cell();
            cellR33.Width = new Width(TableWidthUnit.Point, 1200);
            cellR33.VerticalAlignment = VerticalAlignmentType.Center;
            cellR33.Add(CellAddValueR33);

            Run CellValueR34 = new Run("-"); // A LEVEL COUNT
            CellValueR34.FontSize = 18; //18 points
            CellValueR34.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR34 = new Paragraph();
            CellAddValueR34.Spacing = new Spacing();
            CellAddValueR34.Spacing.After = 0;
            CellAddValueR34.Add(CellValueR34);
            CellAddValueR34.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR34 = new Cell();
            cellR34.Width = new Width(TableWidthUnit.Point, 1200);
            cellR34.VerticalAlignment = VerticalAlignmentType.Center;
            cellR34.Add(CellAddValueR34);

            Run CellValueR35 = new Run("-"); // A LEVEL COUNT
            CellValueR35.FontSize = 18; //18 points
            CellValueR35.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR35 = new Paragraph();
            CellAddValueR35.Spacing = new Spacing();
            CellAddValueR35.Spacing.After = 0;
            CellAddValueR35.Add(CellValueR35);
            CellAddValueR35.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR35 = new Cell();
            cellR35.Width = new Width(TableWidthUnit.Point, 1200);
            cellR35.VerticalAlignment = VerticalAlignmentType.Center;
            cellR35.Add(CellAddValueR35);

            Run CellValueR36 = new Run("-"); // A LEVEL COUNT
            CellValueR36.FontSize = 18; //18 points
            CellValueR36.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR36 = new Paragraph();
            CellAddValueR36.Spacing = new Spacing();
            CellAddValueR36.Spacing.After = 0;
            CellAddValueR36.Add(CellValueR36);
            CellAddValueR36.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR36 = new Cell();
            cellR36.Width = new Width(TableWidthUnit.Point, 1200);
            cellR36.VerticalAlignment = VerticalAlignmentType.Center;
            cellR36.Add(CellAddValueR36);


            Row rowR3 = new Row();
            rowR3.Add(cellR31);
            rowR3.Add(cellR32);
            rowR3.Add(cellR33);
            rowR3.Add(cellR34);
            rowR3.Add(cellR35);
            rowR3.Add(cellR36);
            // Third row End
            #endregion -----------------------------------------------------------------------------

            #region --------- Row 4 --------------------------------------------------------
            //Fourth row start
            Run CellValueR41 = new Run("3");
            CellValueR41.FontSize = 18; //18 points
            CellValueR41.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR41 = new Paragraph();
            CellAddValueR41.Spacing = new Spacing();
            CellAddValueR41.Spacing.After = 0;
            CellAddValueR41.Add(CellValueR41);
            CellAddValueR41.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR41 = new Cell();
            cellR41.Width = new Width(TableWidthUnit.Point, 900);
            cellR41.VerticalAlignment = VerticalAlignmentType.Center;
            cellR41.Add(CellAddValueR41);


            Run CellValueR42 = new Run("'B'");
            CellValueR42.FontSize = 18; //18 points
            CellValueR42.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR42 = new Paragraph();
            CellAddValueR42.Spacing = new Spacing();
            CellAddValueR42.Spacing.After = 0;
            CellAddValueR42.Add(CellValueR42);
            CellAddValueR42.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR42 = new Cell();
            cellR42.Width = new Width(TableWidthUnit.Point, 900);
            cellR42.VerticalAlignment = VerticalAlignmentType.Center;
            cellR42.Add(CellAddValueR42);

            Run CellValueR43 = new Run("-");
            CellValueR43.FontSize = 18; //18 points
            CellValueR43.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR43 = new Paragraph();
            CellAddValueR43.Spacing = new Spacing();
            CellAddValueR43.Spacing.After = 0;
            CellAddValueR43.Add(CellValueR43);
            CellAddValueR43.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR43 = new Cell();
            cellR43.Width = new Width(TableWidthUnit.Point, 1200);
            cellR43.VerticalAlignment = VerticalAlignmentType.Center;
            cellR43.Add(CellAddValueR43);

            Run CellValueR44 = new Run("-");
            CellValueR44.FontSize = 18; //18 points
            CellValueR44.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR44 = new Paragraph();
            CellAddValueR44.Spacing = new Spacing();
            CellAddValueR44.Spacing.After = 0;
            CellAddValueR44.Add(CellValueR44);
            CellAddValueR44.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR44 = new Cell();
            cellR44.Width = new Width(TableWidthUnit.Point, 1200);
            cellR44.VerticalAlignment = VerticalAlignmentType.Center;
            cellR44.Add(CellAddValueR44);

            Run CellValueR45 = new Run("-");
            CellValueR45.FontSize = 18; //18 points
            CellValueR45.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR45 = new Paragraph();
            CellAddValueR45.Spacing = new Spacing();
            CellAddValueR45.Spacing.After = 0;
            CellAddValueR45.Add(CellValueR45);
            CellAddValueR45.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR45 = new Cell();
            cellR45.Width = new Width(TableWidthUnit.Point, 1200);
            cellR45.VerticalAlignment = VerticalAlignmentType.Center;
            cellR45.Add(CellAddValueR45);

            Run CellValueR46 = new Run("-");
            CellValueR46.FontSize = 18; //18 points
            CellValueR46.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR46 = new Paragraph();
            CellAddValueR46.Spacing = new Spacing();
            CellAddValueR46.Spacing.After = 0;
            CellAddValueR46.Add(CellValueR46);
            CellAddValueR46.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR46 = new Cell();
            cellR46.Width = new Width(TableWidthUnit.Point, 1200);
            cellR46.VerticalAlignment = VerticalAlignmentType.Center;
            cellR46.Add(CellAddValueR46);


            Row rowR4 = new Row();
            rowR4.Add(cellR41);
            rowR4.Add(cellR42);
            rowR4.Add(cellR43);
            rowR4.Add(cellR44);
            rowR4.Add(cellR45);
            rowR4.Add(cellR46);
            //Fourth row End
            #endregion -----------------------------------------------------------------------------

            #region -------- Row 5 ------------------------------------------------------
            // 5th row start
            Run CellValueR51 = new Run("4");
            CellValueR51.FontSize = 18; //18 points
            CellValueR51.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR51 = new Paragraph();
            CellAddValueR51.Spacing = new Spacing();
            CellAddValueR51.Spacing.After = 0;
            CellAddValueR51.Add(CellValueR51);
            CellAddValueR51.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR51 = new Cell();
            cellR51.Width = new Width(TableWidthUnit.Point, 900);
            cellR51.VerticalAlignment = VerticalAlignmentType.Center;
            cellR51.Add(CellAddValueR51);


            Run CellValueR52 = new Run("'C'");
            CellValueR52.FontSize = 18; //18 points
            CellValueR52.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR52 = new Paragraph();
            CellAddValueR52.Spacing = new Spacing();
            CellAddValueR52.Spacing.After = 0;
            CellAddValueR52.Add(CellValueR52);
            CellAddValueR52.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR52 = new Cell();
            cellR52.Width = new Width(TableWidthUnit.Point, 900);
            cellR52.VerticalAlignment = VerticalAlignmentType.Center;
            cellR52.Add(CellAddValueR52);

            Run CellValueR53 = new Run("-");
            CellValueR53.FontSize = 18; //18 points
            CellValueR53.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR53 = new Paragraph();
            CellAddValueR53.Spacing = new Spacing();
            CellAddValueR53.Spacing.After = 0;
            CellAddValueR53.Add(CellValueR53);
            CellAddValueR53.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR53 = new Cell();
            cellR53.Width = new Width(TableWidthUnit.Point, 1200);
            cellR53.VerticalAlignment = VerticalAlignmentType.Center;
            cellR53.Add(CellAddValueR53);

            Run CellValueR54 = new Run("-");
            CellValueR54.FontSize = 18; //18 points
            CellValueR54.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR54 = new Paragraph();
            CellAddValueR54.Spacing = new Spacing();
            CellAddValueR54.Spacing.After = 0;
            CellAddValueR54.Add(CellValueR54);
            CellAddValueR54.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR54 = new Cell();
            cellR54.Width = new Width(TableWidthUnit.Point, 1200);
            cellR54.VerticalAlignment = VerticalAlignmentType.Center;
            cellR54.Add(CellAddValueR54);

            Run CellValueR55 = new Run("-");
            CellValueR55.FontSize = 18; //18 points
            CellValueR55.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR55 = new Paragraph();
            CellAddValueR55.Spacing = new Spacing();
            CellAddValueR55.Spacing.After = 0;
            CellAddValueR55.Add(CellValueR55);
            CellAddValueR55.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR55 = new Cell();
            cellR55.Width = new Width(TableWidthUnit.Point, 1200);
            cellR55.VerticalAlignment = VerticalAlignmentType.Center;
            cellR55.Add(CellAddValueR55);

            Run CellValueR56 = new Run("-");
            CellValueR56.FontSize = 18; //18 points
            CellValueR56.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR56 = new Paragraph();
            CellAddValueR56.Spacing = new Spacing();
            CellAddValueR56.Spacing.After = 0;
            CellAddValueR56.Add(CellValueR56);
            CellAddValueR56.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR56 = new Cell();
            cellR56.Width = new Width(TableWidthUnit.Point, 1200);
            cellR56.VerticalAlignment = VerticalAlignmentType.Center;
            cellR56.Add(CellAddValueR56);


            Row rowR5 = new Row();
            rowR5.Add(cellR51);
            rowR5.Add(cellR52);
            rowR5.Add(cellR53);
            rowR5.Add(cellR54);
            rowR5.Add(cellR55);
            rowR5.Add(cellR56);
            // 5th row End
            #endregion -----------------------------------------------------------------------------

            #region ------- Row 6   ----------------------------------------------------------------------

            // 6th row start
            Run CellValueR61 = new Run("Total:-->");
            CellValueR61.FontSize = 18; //18 points
            CellValueR61.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR61 = new Paragraph();
            CellAddValueR61.Spacing = new Spacing();
            CellAddValueR61.Spacing.After = 0;
            CellAddValueR61.Add(CellValueR61);
            CellAddValueR61.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR61 = new Cell();
            cell61.VerticallyMergedCell = new VerticallyMergedCell();
            cell61.VerticallyMergedCell.Type = MergeCellType.Restart;
            cellR61.GridSpan = 2;
            cellR61.Width = new Width(TableWidthUnit.Point, 900);
            cellR61.VerticalAlignment = VerticalAlignmentType.Center;
            cellR61.Add(CellAddValueR61);

            //Run CellValueR62 = new Run(CResultdate);
            //CellValueR62.FontSize = 18; //18 points
            //CellValueR62.AsciiFont = "Century Gothic";
            //Paragraph CellAddValueR62 = new Paragraph();
            //CellAddValueR62.Spacing = new Spacing();
            //CellAddValueR62.Spacing.After = 0;
            //CellAddValueR62.Add(CellValueR62);
            //CellAddValueR62.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            //Cell cellR62 = new Cell();
            //cellR62.Width = new Width(TableWidthUnit.Point, 2400);
            //cellR62.VerticalAlignment = VerticalAlignmentType.Center;
            //cellR62.Add(CellAddValueR62);

            Run CellValueR63 = new Run("666");
            CellValueR63.FontSize = 18; //18 points
            CellValueR63.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR63 = new Paragraph();
            CellAddValueR63.Spacing = new Spacing();
            CellAddValueR63.Spacing.After = 0;
            CellAddValueR63.Add(CellValueR63);
            CellAddValueR63.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR63 = new Cell();
            cellR63.Width = new Width(TableWidthUnit.Point, 1200);
            cellR63.VerticalAlignment = VerticalAlignmentType.Center;
            cellR63.Add(CellAddValueR63);

            Run CellValueR64 = new Run("666");
            CellValueR64.FontSize = 18; //18 points
            CellValueR64.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR64 = new Paragraph();
            CellAddValueR64.Spacing = new Spacing();
            CellAddValueR64.Spacing.After = 0;
            CellAddValueR64.Add(CellValueR64);
            CellAddValueR64.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR64 = new Cell();
            cellR64.Width = new Width(TableWidthUnit.Point, 1200);
            cellR64.VerticalAlignment = VerticalAlignmentType.Center;
            cellR64.Add(CellAddValueR64);

            Run CellValueR65 = new Run("666");
            CellValueR65.FontSize = 18; //18 points
            CellValueR65.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR65 = new Paragraph();
            CellAddValueR65.Spacing = new Spacing();
            CellAddValueR65.Spacing.After = 0;
            CellAddValueR65.Add(CellValueR65);
            CellAddValueR65.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR65 = new Cell();
            cellR65.Width = new Width(TableWidthUnit.Point, 1200);
            cellR65.VerticalAlignment = VerticalAlignmentType.Center;
            cellR65.Add(CellAddValueR65);

            Run CellValueR66 = new Run("666");
            CellValueR66.FontSize = 18; //18 points
            CellValueR66.AsciiFont = "Century Gothic";
            Paragraph CellAddValueR66 = new Paragraph();
            CellAddValueR66.Spacing = new Spacing();
            CellAddValueR66.Spacing.After = 0;
            CellAddValueR66.Add(CellValueR66);
            CellAddValueR66.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellR66 = new Cell();
            cellR66.Width = new Width(TableWidthUnit.Point, 1200);
            cellR66.VerticalAlignment = VerticalAlignmentType.Center;
            cellR66.Add(CellAddValueR66);

            Row rowR6 = new Row();
            rowR6.Height = new RowHeight();
            rowR6.Height.Value = 1;
            rowR6.Add(cellR61);
            //rowR6.Add(cellR62);
            rowR6.Add(cellR63);
            rowR6.Add(cellR64);
            rowR6.Add(cellR65);
            rowR6.Add(cellR66);
            // 6th row End  

            #endregion -----------------------------------------------------------------------------

            Table table5 = new Table(StandardBorderStyle.SingleLine);
            table5.Width = new Width(TableWidthUnit.Percent, 80);
            table5.Alignment = HorizontalAlignmentType.Center;
            table5.Grid = tableGrid5;
            table5.Add(rowR1);
            table5.Add(rowR2);
            table5.Add(rowR3);
            table5.Add(rowR4);
            table5.Add(rowR5);
            table5.Add(rowR6);
            doc.Body.Add(table5);
            //First Table with Merge cell End
            #endregion -----------------------------------------------------------------------------
           
            #endregion -----------------------------------------------------------------------------
            
            #region --- Point Fifth  b ------------------------------------

            Run R125 = new Run();
            R125.AddText("\t");
            Run R126 = new Run();
            R126.AddText("(b)");
            Run R127 = new Run();
            R126.Bold = ExtendedBoolean.True;
            R126.FontSize = 18; //12 points
            R126.AsciiFont = "Century Gothic";
            R127.Bold = ExtendedBoolean.True;
            R127.FontSize = 18; //12 points
            R127.AsciiFont = "Century Gothic";
            R127.AddText("Rejected Cases:");
            R127.Underline = new Underline(UnderlinePattern.Single);
            Run R13 = new Run();
            R13.AddText(" ...... (...............) application forms as per list of candidates placed at " +
                          " pg. ................ to ............ for ......, are recommended for rejection, as the candidates do not fulfill the criteria for claim of Protsahan Puraskar  " +
                          " (formerly Scholarship) laid down by the GC in its ...... meeting held on ............");
            R13.FontSize = 18; //12 points
            R13.AsciiFont = "Century Gothic";          
            Paragraph P13 = new Paragraph();
            P13.Spacing = new Spacing();
            P13.Spacing.Before = 200;
            P13.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            //P13.Add(R125);
            P13.Add(R126);
            P13.Add(R127);
            P13.Add(R13);
           
            doc.Body.Add(P13);
            #endregion -----------------------------------------------------------------------------

            #region --- Point Fifth  c  with table ------------------------------------

            Run R128 = new Run();
            R128.AddText("\t");
            Run R129 = new Run();
            R129.AddText("(c)");
            Run R130 = new Run();
            R130.Bold = ExtendedBoolean.True;
            R130.FontSize = 18; //12 points
            R130.AsciiFont = "Century Gothic";
            R129.Bold = ExtendedBoolean.True;
            R129.FontSize = 18; //12 points
            R129.AsciiFont = "Century Gothic";
            R130.AddText("Withheld Cases:");
            R130.Underline = new Underline(UnderlinePattern.Single);
            Run R14 = new Run();
            R14.AddText("  ...... (.....) application forms as per list of candidates placed at pg.\t ........... to ........... for .........., Examinations " +
                          " are put on hold as they have not submitted the relevant documents as proof of meeting the criteria. It is recommended that  " +
           " these applicants may be send emails to submit the relevant document within ...... days followed by ...... reminders, otherwise their claim may be rejected.");
            R14.FontSize = 18; //12 points
            R14.AsciiFont = "Century Gothic";
            Paragraph P14 = new Paragraph();
            P14.Spacing = new Spacing();
            P14.Spacing.Before = 200;
            P14.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            //P14.Add(R128);
            P14.Add(R129);
            P14.Add(R130);
            P14.Add(R14);
            
            doc.Body.Add(P14);

            #region ---------- Table 6 for Withheld Cases -----------------------------------------------------
            //First Table with Merge cell Start
            TableGrid tableGrid6 = new TableGrid();
            tableGrid6.Columns.Add(new TableGridColumn(498));
            tableGrid6.Columns.Add(new TableGridColumn(1200));
            tableGrid6.Columns.Add(new TableGridColumn(1200));

            #region ---------- Row 1 Heading -------------------------------------------------------
            // First row start 
            Run CellValueW = new Run("SL. No");
            CellValueW.Bold = ExtendedBoolean.True;
            CellValueW.FontSize = 18; //18 points
            CellValueW.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW = new Paragraph();
            CellAddValueW.Spacing = new Spacing();
            CellAddValueW.Spacing.After = 0;
            CellAddValueW.Add(CellValueW);
            CellAddValueW.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW11 = new Cell();
            //cellR11.VerticallyMergedCell = new VerticallyMergedCell();
            //cellR11.GridSpan = 3;         
            cellW11.Shading = new Shading(ShadingPattern.Percent10);
            cellW11.Width = new Width(TableWidthUnit.Point, 500);
            cellW11.VerticalAlignment = VerticalAlignmentType.Center;
            cellW11.Add(CellAddValueW);

            Run CellValueW12 = new Run("Particulars");
            CellValueW12.Bold = ExtendedBoolean.True;
            CellValueW12.FontSize = 18; //18 points
            CellValueW12.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW12 = new Paragraph();
            CellAddValueW12.Spacing = new Spacing();
            CellAddValueW12.Spacing.After = 0;
            CellAddValueW12.Add(CellValueW12);
            CellAddValueW12.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW12 = new Cell();
            cellW12.Shading = new Shading(ShadingPattern.Percent10);
            cellW12.Width = new Width(TableWidthUnit.Point, 4000);
            cellW12.VerticalAlignment = VerticalAlignmentType.Center;
            cellW12.Add(CellAddValueW12);

            Run CellValueW13 = new Run("O     Level");
            CellValueW13.Bold = ExtendedBoolean.True;
            CellValueW13.FontSize = 18; //18 points
            CellValueW13.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW13 = new Paragraph();
            CellAddValueW13.Spacing = new Spacing();
            CellAddValueW13.Spacing.After = 0;
            CellAddValueW13.Add(CellValueW13);
            CellAddValueW13.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW13 = new Cell();
            cellW13.Shading = new Shading(ShadingPattern.Percent10);
            cellW13.Width = new Width(TableWidthUnit.Point, 500);
            cellW13.VerticalAlignment = VerticalAlignmentType.Center;
            cellW13.Add(CellAddValueW13);

            Run CellValueW14 = new Run("A     Level");
            CellValueW14.Bold = ExtendedBoolean.True;
            CellValueW14.FontSize = 18; //18 points
            CellValueW14.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW14 = new Paragraph();
            CellAddValueW14.Spacing = new Spacing();
            CellAddValueW14.Spacing.After = 0;
            CellAddValueW14.Add(CellValueW14);
            CellAddValueW14.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW14 = new Cell();
            cellW14.Shading = new Shading(ShadingPattern.Percent10);
            cellW14.Width = new Width(TableWidthUnit.Point, 500);
            cellW14.VerticalAlignment = VerticalAlignmentType.Center;
            cellW14.Add(CellAddValueW14);

            Run CellValueW15 = new Run("Total");
            CellValueW15.Bold = ExtendedBoolean.True;
            CellValueW15.FontSize = 18; //18 points
            CellValueW15.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW15 = new Paragraph();
            CellAddValueW15.Spacing = new Spacing();
            CellAddValueW15.Spacing.After = 0;
            CellAddValueW15.Add(CellValueW15);
            CellAddValueW15.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW15 = new Cell();
            cellW15.Shading = new Shading(ShadingPattern.Percent10);
            cellW15.Width = new Width(TableWidthUnit.Point, 500);
            cellW15.VerticalAlignment = VerticalAlignmentType.Center;
            cellW15.Add(CellAddValueW15);

            Row rowW1 = new Row();
            rowW1.Add(cellW11);
            rowW1.Add(cellW12);
            rowW1.Add(cellW13);
            rowW1.Add(cellW14);
            rowW1.Add(cellW15);  
            // First row End 
            #endregion -----------------------------------------------------------------------------

            #region -----------  Row 2  ----------------------------------------------------

            // Second row start 
            Run CellValueW21 = new Run("a)");
            CellValueW21.FontSize = 18; //18 points
            CellValueW21.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW21 = new Paragraph();
            CellAddValueW21.Spacing = new Spacing();
            CellAddValueW21.Spacing.After = 0;
            CellAddValueW21.Add(CellValueW21);
            CellAddValueW21.HorizontalTextAlignment = HorizontalAlignmentType.Center;

            Cell cellW21 = new Cell();
            cellW21.Width = new Width(TableWidthUnit.Point, 500);
            cellW21.VerticalAlignment = VerticalAlignmentType.Center;
            cellW21.Add(CellAddValueW21);

            Run CellValueW22 = new Run("Aadhaar number has not been produced by the Candidate");
            CellValueW22.FontSize = 18; //18 points
            CellValueW22.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW22 = new Paragraph();
            CellAddValueW22.Spacing = new Spacing();
            CellAddValueW22.Spacing.After = 0;
            CellAddValueW22.Add(CellValueW22);
            CellAddValueW22.HorizontalTextAlignment = HorizontalAlignmentType.Left;
            Cell cellW22 = new Cell();
            cellW22.Width = new Width(TableWidthUnit.Point, 4000);
            cellW22.VerticalAlignment = VerticalAlignmentType.None;
            cellW22.Add(CellAddValueW22);

            Run CellValueW23 = new Run("019");
            CellValueW23.FontSize = 18; //18 points
            CellValueW23.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW23 = new Paragraph();
            CellAddValueW23.Spacing = new Spacing();
            CellAddValueW23.Spacing.After = 0;
            CellAddValueW23.Add(CellValueW23);
            CellAddValueW23.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW23 = new Cell();
            cellW23.Width = new Width(TableWidthUnit.Point, 500);
            cellW23.VerticalAlignment = VerticalAlignmentType.Center;
            cellW23.Add(CellAddValueW23);

            Run CellValueW24 = new Run("-");
            CellValueW24.FontSize = 18; //18 points
            CellValueW24.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW24 = new Paragraph();
            CellAddValueW24.Spacing = new Spacing();
            CellAddValueW24.Spacing.After = 0;
            CellAddValueW24.Add(CellValueW24);
            CellAddValueW24.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW24 = new Cell();
            cellW24.Width = new Width(TableWidthUnit.Point, 500);
            cellW24.VerticalAlignment = VerticalAlignmentType.Center;
            cellW24.Add(CellAddValueW24);

            Run CellValueW25 = new Run("019");
            CellValueW25.FontSize = 18; //18 points
            CellValueW25.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW25 = new Paragraph();
            CellAddValueW25.Spacing = new Spacing();
            CellAddValueW25.Spacing.After = 0;
            CellAddValueW25.Add(CellValueW25);
            CellAddValueW25.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW25 = new Cell();
            cellW25.Width = new Width(TableWidthUnit.Point, 500);
            cellW25.VerticalAlignment = VerticalAlignmentType.Center;
            cellW25.Add(CellAddValueW25);
            
            Row rowW2 = new Row();
            rowW2.Add(cellW21);
            rowW2.Add(cellW22);
            rowW2.Add(cellW23);
            rowW2.Add(cellW24);
            rowW2.Add(cellW25);          
            // Second row End
            #endregion -----------------------------------------------------------------------------

            #region --------- Row 3 --------------------------------------------------------
            // Third row start
            Run CellValueW31 = new Run("b)");
            CellValueW31.FontSize = 18; //18 points
            CellValueW31.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW31 = new Paragraph();
            CellAddValueW31.Spacing = new Spacing();
            CellAddValueW31.Spacing.After = 0;
            CellAddValueW31.Add(CellValueW31);
            CellAddValueW31.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW31 = new Cell();
            cellW31.Width = new Width(TableWidthUnit.Point, 500);
            cellW31.VerticalAlignment = VerticalAlignmentType.Center;
            cellW31.Add(CellAddValueW31);

            Run CellValueW32 = new Run("Income certificate has not been issued by the Comp. Authority"); // 
            CellValueW32.FontSize = 18; //18 points
            CellValueW32.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW32 = new Paragraph();
            CellAddValueW32.Spacing = new Spacing();
            CellAddValueW32.Spacing.After = 0;
            CellAddValueW32.Add(CellValueW32);
            CellAddValueW32.HorizontalTextAlignment = HorizontalAlignmentType.Left;
            Cell cellW32 = new Cell();
            cellW32.Width = new Width(TableWidthUnit.Point, 4000);
            cellW32.VerticalAlignment = VerticalAlignmentType.None;
            cellW32.Add(CellAddValueW32);

            Run CellValueW33 = new Run("01"); // A LEVEL COUNT
            CellValueW33.FontSize = 18; //18 points
            CellValueW33.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW33 = new Paragraph();
            CellAddValueW33.Spacing = new Spacing();
            CellAddValueW33.Spacing.After = 0;
            CellAddValueW33.Add(CellValueW33);
            CellAddValueW33.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW33 = new Cell();
            cellW33.Width = new Width(TableWidthUnit.Point, 500);
            cellW33.VerticalAlignment = VerticalAlignmentType.Center;
            cellW33.Add(CellAddValueW33);

            Run CellValueW34 = new Run("-"); // A LEVEL COUNT
            CellValueW34.FontSize = 18; //18 points
            CellValueW34.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW34 = new Paragraph();
            CellAddValueW34.Spacing = new Spacing();
            CellAddValueW34.Spacing.After = 0;
            CellAddValueW34.Add(CellValueW34);
            CellAddValueW34.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW34 = new Cell();
            cellW34.Width = new Width(TableWidthUnit.Point, 500);
            cellW34.VerticalAlignment = VerticalAlignmentType.Center;
            cellW34.Add(CellAddValueW34);

            Run CellValueW35 = new Run("01"); // A LEVEL COUNT
            CellValueW35.FontSize = 18; //18 points
            CellValueW35.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW35 = new Paragraph();
            CellAddValueW35.Spacing = new Spacing();
            CellAddValueW35.Spacing.After = 0;
            CellAddValueW35.Add(CellValueW35);
            CellAddValueW35.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW35 = new Cell();
            cellW35.Width = new Width(TableWidthUnit.Point, 500);
            cellW35.VerticalAlignment = VerticalAlignmentType.Center;
            cellW35.Add(CellAddValueW35);

            Row rowW3 = new Row();
            rowW3.Add(cellW31);
            rowW3.Add(cellW32);
            rowW3.Add(cellW33);
            rowW3.Add(cellW34);
            rowW3.Add(cellW35);           
            // Third row End
            #endregion -----------------------------------------------------------------------------

            #region --------- Row 4 --------------------------------------------------------
            //Fourth row start
            Run CellValueW41 = new Run("c)");
            CellValueW41.FontSize = 18; //18 points
            CellValueW41.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW41 = new Paragraph();
            CellAddValueW41.Spacing = new Spacing();
            CellAddValueW41.Spacing.After = 0;
            CellAddValueW41.Add(CellValueW41);
            CellAddValueW41.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW41 = new Cell();
            cellW41.Width = new Width(TableWidthUnit.Point, 500);
            cellW41.VerticalAlignment = VerticalAlignmentType.Center;
            cellW41.Add(CellAddValueW41);


            Run CellValueW42 = new Run("Latest Income Certificate has not been produced by candidate");
            CellValueW42.FontSize = 18; //18 points
            CellValueW42.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW42 = new Paragraph();
            CellAddValueW42.Spacing = new Spacing();
            CellAddValueW42.Spacing.After = 0;
            CellAddValueW42.Add(CellValueW42);
            CellAddValueW42.HorizontalTextAlignment = HorizontalAlignmentType.Left;
            Cell cellW42 = new Cell();
            cellW42.Width = new Width(TableWidthUnit.Point, 4000);
            cellW42.VerticalAlignment = VerticalAlignmentType.None;
            cellW42.Add(CellAddValueW42);

            Run CellValueW43 = new Run("03");
            CellValueW43.FontSize = 18; //18 points
            CellValueW43.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW43 = new Paragraph();
            CellAddValueW43.Spacing = new Spacing();
            CellAddValueW43.Spacing.After = 0;
            CellAddValueW43.Add(CellValueW43);
            CellAddValueW43.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW43 = new Cell();
            cellW43.Width = new Width(TableWidthUnit.Point, 500);
            cellW43.VerticalAlignment = VerticalAlignmentType.Center;
            cellW43.Add(CellAddValueW43);

            Run CellValueW44 = new Run("-");
            CellValueW44.FontSize = 18; //18 points
            CellValueW44.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW44 = new Paragraph();
            CellAddValueW44.Spacing = new Spacing();
            CellAddValueW44.Spacing.After = 0;
            CellAddValueW44.Add(CellValueW44);
            CellAddValueW44.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW44 = new Cell();
            cellW44.Width = new Width(TableWidthUnit.Point, 500);
            cellW44.VerticalAlignment = VerticalAlignmentType.Center;
            cellW44.Add(CellAddValueW44);

            Run CellValueW45 = new Run("03");
            CellValueW45.FontSize = 18; //18 points
            CellValueW45.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW45 = new Paragraph();
            CellAddValueW45.Spacing = new Spacing();
            CellAddValueW45.Spacing.After = 0;
            CellAddValueW45.Add(CellValueW45);
            CellAddValueW45.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW45 = new Cell();
            cellW45.Width = new Width(TableWidthUnit.Point, 500);
            cellW45.VerticalAlignment = VerticalAlignmentType.Center;
            cellW45.Add(CellAddValueW45);

            Row rowW4 = new Row();
            rowW4.Add(cellW41);
            rowW4.Add(cellW42);
            rowW4.Add(cellW43);
            rowW4.Add(cellW44);
            rowW4.Add(cellW45);          
            //Fourth row End
            #endregion -----------------------------------------------------------------------------

            #region -------- Row 5 ------------------------------------------------------
            // 5th row start
            Run CellValueW51 = new Run("d)");
            CellValueW51.FontSize = 18; //18 points
            CellValueW51.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW51 = new Paragraph();
            CellAddValueW51.Spacing = new Spacing();
            CellAddValueW51.Spacing.After = 0;
            CellAddValueW51.Add(CellValueW51);
            CellAddValueW51.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW51 = new Cell();
            cellW51.Width = new Width(TableWidthUnit.Point, 500);
            cellW51.VerticalAlignment = VerticalAlignmentType.Center;
            cellW51.Add(CellAddValueW51);


            Run CellValueW52 = new Run("Caste Certificate has not been produced by the Candidate");
            CellValueW52.FontSize = 18; //18 points
            CellValueW52.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW52 = new Paragraph();
            CellAddValueW52.Spacing = new Spacing();
            CellAddValueW52.Spacing.After = 0;
            CellAddValueW52.Add(CellValueW52);
            CellAddValueW52.HorizontalTextAlignment = HorizontalAlignmentType.Left;
            Cell cellW52 = new Cell();
            cellW52.Width = new Width(TableWidthUnit.Point, 4000);
            cellW52.VerticalAlignment = VerticalAlignmentType.None;
            cellW52.Add(CellAddValueW52);

            Run CellValueW53 = new Run("03");
            CellValueW53.FontSize = 18; //18 points
            CellValueW53.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW53 = new Paragraph();
            CellAddValueW53.Spacing = new Spacing();
            CellAddValueW53.Spacing.After = 0;
            CellAddValueW53.Add(CellValueW53);
            CellAddValueW53.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW53 = new Cell();
            cellW53.Width = new Width(TableWidthUnit.Point, 500);
            cellW53.VerticalAlignment = VerticalAlignmentType.Center;
            cellW53.Add(CellAddValueW53);

            Run CellValueW54 = new Run("-");
            CellValueW54.FontSize = 18; //18 points
            CellValueW54.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW54 = new Paragraph();
            CellAddValueW54.Spacing = new Spacing();
            CellAddValueW54.Spacing.After = 0;
            CellAddValueW54.Add(CellValueW54);
            CellAddValueW54.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW54 = new Cell();
            cellW54.Width = new Width(TableWidthUnit.Point, 500);
            cellW54.VerticalAlignment = VerticalAlignmentType.Center;
            cellW54.Add(CellAddValueW54);

            Run CellValueW55 = new Run("03");
            CellValueW55.FontSize = 18; //18 points
            CellValueW55.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW55 = new Paragraph();
            CellAddValueW55.Spacing = new Spacing();
            CellAddValueW55.Spacing.After = 0;
            CellAddValueW55.Add(CellValueW55);
            CellAddValueW55.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW55 = new Cell();
            cellW55.Width = new Width(TableWidthUnit.Point, 500);
            cellW55.VerticalAlignment = VerticalAlignmentType.Center;
            cellW55.Add(CellAddValueW55);

            Row rowW5 = new Row();
            rowW5.Add(cellW51);
            rowW5.Add(cellW52);
            rowW5.Add(cellW53);
            rowW5.Add(cellW54);
            rowW5.Add(cellW55);         
            // 5th row End
            #endregion -----------------------------------------------------------------------------

            #region ------- Row 6   ----------------------------------------------------------------------

            // 6th row start
            Run CellValueW61 = new Run("Total withhold forms");
            CellValueW61.Bold = ExtendedBoolean.True;
            CellValueW61.FontSize = 18; //18 points
            CellValueW61.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW61 = new Paragraph();
            CellAddValueW61.Spacing = new Spacing();
            CellAddValueW61.Spacing.After = 0;
            CellAddValueW61.Add(CellValueW61);
            CellAddValueW61.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW61 = new Cell();
            cell61.VerticallyMergedCell = new VerticallyMergedCell();
            cell61.VerticallyMergedCell.Type = MergeCellType.Restart;
            cellW61.GridSpan = 2;
            cellW61.Width = new Width(TableWidthUnit.Point, 500);
            cellW61.VerticalAlignment = VerticalAlignmentType.Center;
            cellW61.Add(CellAddValueW61);          

            Run CellValueW63 = new Run("06");
            CellValueW63.FontSize = 18; //18 points
            CellValueW63.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW63 = new Paragraph();
            CellAddValueW63.Spacing = new Spacing();
            CellAddValueW63.Spacing.After = 0;
            CellAddValueW63.Add(CellValueW63);
            CellAddValueW63.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW63 = new Cell();
            cellW63.Width = new Width(TableWidthUnit.Point, 500);
            cellW63.VerticalAlignment = VerticalAlignmentType.Center;
            cellW63.Add(CellAddValueW63);

            Run CellValueW64 = new Run("-");
            CellValueW64.FontSize = 18; //18 points
            CellValueW64.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW64 = new Paragraph();
            CellAddValueW64.Spacing = new Spacing();
            CellAddValueW64.Spacing.After = 0;
            CellAddValueW64.Add(CellValueW64);
            CellAddValueW64.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW64 = new Cell();
            cellW64.Width = new Width(TableWidthUnit.Point, 500);
            cellW64.VerticalAlignment = VerticalAlignmentType.Center;
            cellW64.Add(CellAddValueW64);

            Run CellValueW65 = new Run("06");
            CellValueW65.FontSize = 18; //18 points
            CellValueW65.AsciiFont = "Century Gothic";
            Paragraph CellAddValueW65 = new Paragraph();
            CellAddValueW65.Spacing = new Spacing();
            CellAddValueW65.Spacing.After = 0;
            CellAddValueW65.Add(CellValueW65);
            CellAddValueW65.HorizontalTextAlignment = HorizontalAlignmentType.Center;
            Cell cellW65 = new Cell();
            cellW65.Width = new Width(TableWidthUnit.Point, 500);
            cellW65.VerticalAlignment = VerticalAlignmentType.Center;
            cellW65.Add(CellAddValueW65);

            Row rowW6 = new Row();
            rowW6.Height = new RowHeight();
            rowW6.Height.Value = 1;
            rowW6.Add(cellW61);           
            rowW6.Add(cellW63);
            rowW6.Add(cellW64);
            rowW6.Add(cellW65);          
            // 6th row End  

            #endregion -----------------------------------------------------------------------------

            Table table6 = new Table(StandardBorderStyle.SingleLine);          
            table6.Width = new Width(TableWidthUnit.Percent, 90);
            table6.Alignment = HorizontalAlignmentType.Center;
            table6.Grid = tableGrid6;
            table6.Add(rowW1);
            table6.Add(rowW2);
            table6.Add(rowW3);
            table6.Add(rowW4);
            table6.Add(rowW5);
            table6.Add(rowW6);
            doc.Body.Add(table6);
            //First Table with Merge cell End
            #endregion -----------------------------------------------------------------------------

            #endregion -----------------------------------------------------------------------------

            #region --------------- Point Fifth d -------------
         
            Run R15 = new Run();
            R15.AddText("(d) As per norms the payment of Protsahan Puruskar to be disbursed is as given below:");
            R15.FontSize = 18; //12 points
            R15.AsciiFont = "Century Gothic";
            Paragraph P15 = new Paragraph();
            P15.Add(R15);
            P15.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            P15.Spacing = new Spacing();
            P15.Spacing.Before = 10;
            doc.Body.Add(P15);

            Run R16 = new Run();
            R16.AddText("\ti)    ‘O’ Level (Two Instalments) 2/2");
            R16.AddBreak();
            R16.AddText("\tii)   'A’ Level (Three Instalments) 3/3/4");
            R16.AddBreak();
            R16.AddText("\tiii)  ‘B’ Level (Four Instalments) 3/3/3/6");
            R16.AddBreak();
            R16.AddText("\tiv)  ‘C’ Level (Three Instalments) 4/4/4");
            R16.FontSize = 18; //12 points
            R16.AsciiFont = "Century Gothic";
            Paragraph P16 = new Paragraph();
            P16.Spacing = new Spacing();
            P16.Spacing.Before = 0;
            P16.Add(R16);
            P16.HorizontalTextAlignment = HorizontalAlignmentType.Left;
           
            doc.Body.Add(P16);

            Run R17 = new Run();
            R17.AddText("Based on the above, ....... application forms with ....... modules (Rs..........) as per list of candidates placed "+
            "at pg............... to ..............for .........., are recommended for carrying forward to ........ Examination Cycle,");
            R17.FontSize = 18; //12 points
            R17.AsciiFont = "Century Gothic";
            Paragraph P17 = new Paragraph();
            P17.Add(R17);
            P17.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            P17.Spacing = new Spacing();
            P17.Spacing.Before = 0;
            doc.Body.Add(P17);
           
            #endregion -----------------------------------------------------------------------------

            #region --- Point Fifth  e   ------------------------------------
                        
            Run R132 = new Run();
            R132.AddText("(e)");
            Run R133 = new Run();
            R133.Bold = ExtendedBoolean.True;
            R133.FontSize = 18; //12 points
            R133.AsciiFont = "Century Gothic";
            R132.Bold = ExtendedBoolean.True;
            R132.FontSize = 18; //12 points
            R132.AsciiFont = "Century Gothic";
            R133.AddText("Lapsed Papers Cases:");
            R133.Underline = new Underline(UnderlinePattern.Single);
            Run R18 = new Run();
            R18.AddText("  ...... application forms with ..... modules as per list of candidates placed at pg................ for .........., are recommended for"+
            " lapsed paper cases for ........ The suitable remarks is written along in the list for references.");
            R18.FontSize = 18; //12 points
            R18.AsciiFont = "Century Gothic";
            Paragraph P18 = new Paragraph();
            P18.Spacing = new Spacing();
            P18.Spacing.Before = 200;
            P18.HorizontalTextAlignment = HorizontalAlignmentType.Both;         
            P18.Add(R132);
            P18.Add(R133);
            P18.Add(R18);

            doc.Body.Add(P18);

              #endregion ------------------------------------------   
          
            #region  ----------- Third Page of Report Start Notifications ------------------------------------------
            Run R19 = new Run();
            R19.AddText("Contd…3");
            R19.FontSize = 19; //12 points
            R19.AsciiFont = "Century Gothic";
            Paragraph P19 = new Paragraph();
            P19.Spacing = new Spacing();
            P19.Spacing.Before = 550;
            P19.Spacing.After = 400;
            P19.Add(R19);
            P19.HorizontalTextAlignment = HorizontalAlignmentType.Right;
            doc.Body.Add(P19);

            #endregion -----------------------------------------------------------------------------
            
            #region --- Heading of page On the basis of figures as mentioned above  -----------------
              
            Run R134 = new Run();
            R134.AddText("On the basis of figures as mentioned above the following is recommended by the committee.");
            R134.Bold = ExtendedBoolean.True;
            R134.FontSize = 18; //12 points
            R134.AsciiFont = "Century Gothic";
            R134.Underline = new Underline(UnderlinePattern.Single);
            Paragraph P20 = new Paragraph();
            P20.Spacing = new Spacing();
            P20.Spacing.Before = 2000;
            P20.HorizontalTextAlignment = HorizontalAlignmentType.Left;
            P20.Add(R134);
            doc.Body.Add(P20);

            #endregion ------------------------------------------   
                        
            #region ---------- Table 6 for Point 6 descriptions last -----------------------------------------------------
            //First Table with Merge cell Start
            TableGrid tableGrid7 = new TableGrid();
            tableGrid7.Columns.Add(new TableGridColumn(498));
            tableGrid7.Columns.Add(new TableGridColumn(1200));
            tableGrid7.Columns.Add(new TableGridColumn(1200));

            #region ---------- Row 1 Heading -------------------------------------------------------
            // First row start 
            Run CellValueL = new Run("On the basis of figures as mentioned above the following is recommended by the committee.");
            CellValueL.Bold = ExtendedBoolean.True;
            CellValueL.Underline = new Underline(UnderlinePattern.Single);
            CellValueL.FontSize = 18; //18 points
            CellValueL.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL = new Paragraph();
            CellAddValueL.Spacing = new Spacing();
            CellAddValueL.Spacing.After = 0;
            CellAddValueL.Add(CellValueL);
            CellAddValueL.HorizontalTextAlignment = HorizontalAlignmentType.Left;
            Cell cellL11 = new Cell();
            cellL11.VerticallyMergedCell = new VerticallyMergedCell();
            cellL11.VerticallyMergedCell.Type = MergeCellType.Restart;
            cellL11.GridSpan = 3;           
            cellL11.Width = new Width(TableWidthUnit.Point, 500);
            cellL11.VerticalAlignment = VerticalAlignmentType.None;
            cellL11.Add(CellAddValueL);
                       
            Row rowL1 = new Row();
            rowL1.Add(cellL11);           
          
            // First row End 
            #endregion -----------------------------------------------------------------------------

            #region -----------  Row 2  ----------------------------------------------------

            // Second row start 
            Run CellValueL21 = new Run("6.");
            CellValueL21.FontSize = 18; //18 points
            CellValueL21.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL21 = new Paragraph();          
            CellAddValueL21.Add(CellValueL21);
            CellAddValueL21.HorizontalTextAlignment = HorizontalAlignmentType.None;

            Cell cellL21 = new Cell();
            cellL21.Width = new Width(TableWidthUnit.Point, 500);
            cellL21.VerticalAlignment = VerticalAlignmentType.Top;
            cellL21.Add(CellAddValueL21);

            Run CellValueL22 = new Run("a) ");
            CellValueL22.FontSize = 18; //18 points
            CellValueL22.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL22 = new Paragraph();
            CellAddValueL22.Add(CellValueL22);
            CellAddValueL22.HorizontalTextAlignment = HorizontalAlignmentType.None;

            Cell cellL22 = new Cell();
            cellL22.Width = new Width(TableWidthUnit.Point, 500);
            cellL22.VerticalAlignment = VerticalAlignmentType.Top;
            cellL22.Add(CellAddValueL22);



            Run CellValueL23 = new Run("The committee Recommends ....... applications for release of payment Rs ........./- (............ " +
                          " .......................) towards the payment of Protsahan Puraskar (formerly Scholarship) to" +
                          " SC/ST/Female & PwD candidates for ........ Examinations as per list placed in " +
                          "the file at pg. ............. to ............. .");
            CellValueL23.FontSize = 18; //18 points
            CellValueL23.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL23 = new Paragraph();         
            CellAddValueL23.Add(CellValueL23);
            CellAddValueL23.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            Cell cellL23 = new Cell();
            cellL23.Width = new Width(TableWidthUnit.Point, 10000);
            cellL23.VerticalAlignment = VerticalAlignmentType.None;
            cellL23.Add(CellAddValueL23);
          
            Row rowL2 = new Row();
            rowL2.Add(cellL21);
            rowL2.Add(cellL22);
            rowL2.Add(cellL23);
            // Second row End
            #endregion -----------------------------------------------------------------------------

            #region --------- Row 3 --------------------------------------------------------
            // Third row start
            Run CellValueL31 = new Run(" ");
            CellValueL31.FontSize = 18; 
            CellValueL31.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL31 = new Paragraph();          
            CellAddValueL31.Add(CellValueL31);
            CellAddValueL31.HorizontalTextAlignment = HorizontalAlignmentType.None;
            Cell cellL31 = new Cell();
            cellL31.Width = new Width(TableWidthUnit.Point, 500);
            cellL31.VerticalAlignment = VerticalAlignmentType.Top;
            cellL31.Add(CellAddValueL31);

            Run CellValueL32 = new Run("b) ");
            CellValueL32.FontSize = 18; //18 points
            CellValueL32.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL32 = new Paragraph();
            CellAddValueL32.Add(CellValueL32);
            CellAddValueL32.HorizontalTextAlignment = HorizontalAlignmentType.None;

            Cell cellL32 = new Cell();
            cellL32.Width = new Width(TableWidthUnit.Point, 500);
            cellL32.VerticalAlignment = VerticalAlignmentType.Top;
            cellL32.Add(CellAddValueL32);

            Run CellValueL33 = new Run("The committee recommends to Withhold ..... applications for ...... examination for resubmission of  " +
                                     "relevant documents as per list placed at pg. ............. to ............. ."); 
            CellValueL33.FontSize = 18; //18 points
            CellValueL33.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL33 = new Paragraph();           
            CellAddValueL33.Add(CellValueL33);
            CellAddValueL33.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            Cell cellL33 = new Cell();
            cellL33.Width = new Width(TableWidthUnit.Point, 10000);
            cellL33.VerticalAlignment = VerticalAlignmentType.None;
            cellL33.Add(CellAddValueL33);

            Row rowL3 = new Row();
            rowL3.Add(cellL31);
            rowL3.Add(cellL32);
            rowL3.Add(cellL33);
           
            // Third row End
            #endregion -----------------------------------------------------------------------------

            #region --------- Row 4 --------------------------------------------------------
            ////Fourth row start
            Run CellValueL41 = new Run(" ");
            CellValueL41.FontSize = 18;
            CellValueL41.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL41 = new Paragraph();
            CellAddValueL41.Add(CellValueL41);
            CellAddValueL41.HorizontalTextAlignment = HorizontalAlignmentType.None;
            Cell cellL41 = new Cell();
            cellL41.Width = new Width(TableWidthUnit.Point, 500);
            cellL41.VerticalAlignment = VerticalAlignmentType.Top;
            cellL41.Add(CellAddValueL41);

            Run CellValueL42 = new Run("C) ");
            CellValueL42.FontSize = 18; //18 points
            CellValueL42.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL42 = new Paragraph();
            CellAddValueL42.Add(CellValueL42);
            CellAddValueL42.HorizontalTextAlignment = HorizontalAlignmentType.None;

            Cell cellL42 = new Cell();
            cellL42.Width = new Width(TableWidthUnit.Point, 500);
            cellL42.VerticalAlignment = VerticalAlignmentType.Top;
            cellL42.Add(CellAddValueL42);

            Run CellValueL43 = new Run("The committee recommends to Reject ........ applications for all the levels of ......... examination as they do not fulfill Protsahan Puraskar (formerly Scholarship)  " +
                                    "norms of the NIELIT (Formerly DOEACC Society) as per list placed at pg. ............. to ............. .");
            CellValueL43.FontSize = 18; //18 points
            CellValueL43.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL43 = new Paragraph();
            CellAddValueL43.Add(CellValueL43);
            CellAddValueL43.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            Cell cellL43 = new Cell();
            cellL43.Width = new Width(TableWidthUnit.Point, 10000);
            cellL43.VerticalAlignment = VerticalAlignmentType.None;
            cellL43.Add(CellAddValueL43);

            Row rowL4 = new Row();
            rowL4.Add(cellL41);
            rowL4.Add(cellL42);
            rowL4.Add(cellL43);
            ////Fourth row End
            #endregion -----------------------------------------------------------------------------

            #region -------- Row 5 ------------------------------------------------------
            //// 5th row start
            Run CellValueL51 = new Run(" ");
            CellValueL51.FontSize = 18;
            CellValueL51.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL51 = new Paragraph();
            CellAddValueL51.Add(CellValueL51);
            CellAddValueL51.HorizontalTextAlignment = HorizontalAlignmentType.None;
            Cell cellL51 = new Cell();
            cellL51.Width = new Width(TableWidthUnit.Point, 500);
            cellL51.VerticalAlignment = VerticalAlignmentType.Top;
            cellL51.Add(CellAddValueL51);

            Run CellValueL52 = new Run("d) ");
            CellValueL52.FontSize = 18; //18 points
            CellValueL52.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL52 = new Paragraph();
            CellAddValueL52.Add(CellValueL52);
            CellAddValueL52.HorizontalTextAlignment = HorizontalAlignmentType.None;

            Cell cellL52 = new Cell();
            cellL52.Width = new Width(TableWidthUnit.Point, 500);
            cellL52.VerticalAlignment = VerticalAlignmentType.Top;
            cellL52.Add(CellAddValueL52);

            Run CellValueL53 = new Run("The committee recommends for carrying forward of ..... applications with ......-modules for payment of Rs.......... for consideration  " +
                                     "while processing for ...... Protsahan Puruskar as per list placed  at pg. ............. to ............. .");
            CellValueL53.FontSize = 18; //18 points
            CellValueL53.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL53 = new Paragraph();
            CellAddValueL53.Add(CellValueL53);
            CellAddValueL53.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            Cell cellL53 = new Cell();
            cellL53.Width = new Width(TableWidthUnit.Point, 10000);
            cellL53.VerticalAlignment = VerticalAlignmentType.None;
            cellL53.Add(CellAddValueL53);

            Row rowL5 = new Row();
            rowL5.Add(cellL51);
            rowL5.Add(cellL52);
            rowL5.Add(cellL53);
            //// 5th row End
            #endregion -----------------------------------------------------------------------------

            #region ------- Row 6   ----------------------------------------------------------------------

            //// 6th row start
            Run CellValueL61 = new Run(" ");
            CellValueL61.FontSize = 18;
            CellValueL61.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL61 = new Paragraph();
            CellAddValueL61.Add(CellValueL61);
            CellAddValueL61.HorizontalTextAlignment = HorizontalAlignmentType.None;
            Cell cellL61 = new Cell();
            cellL61.Width = new Width(TableWidthUnit.Point, 500);
            cellL61.VerticalAlignment = VerticalAlignmentType.Top;
            cellL61.Add(CellAddValueL61);

            Run CellValueL62 = new Run("e) ");
            CellValueL62.FontSize = 18; //18 points
            CellValueL62.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL62 = new Paragraph();
            CellAddValueL62.Add(CellValueL62);
            CellAddValueL62.HorizontalTextAlignment = HorizontalAlignmentType.None;

            Cell cellL62 = new Cell();
            cellL62.Width = new Width(TableWidthUnit.Point, 500);
            cellL62.VerticalAlignment = VerticalAlignmentType.Top;
            cellL62.Add(CellAddValueL62);

            Run CellValueL63 = new Run("The committee recommends for ...... Application with ......-module for consideration while processing for   " +
                    "while processing for ...... Protsahan Puruskar as per list placed  at pg. ............. to ............. .");
            CellValueL63.FontSize = 18; //18 points
            CellValueL63.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL63 = new Paragraph();
            CellAddValueL63.Add(CellValueL63);
            CellAddValueL63.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            Cell cellL63 = new Cell();
            cellL63.Width = new Width(TableWidthUnit.Point, 10000);
            cellL63.VerticalAlignment = VerticalAlignmentType.None;
            cellL63.Add(CellAddValueL63);

            Row rowL6 = new Row();
            rowL6.Add(cellL61);
            rowL6.Add(cellL62);
            rowL6.Add(cellL63);
            //// 6th row End  

            #endregion -----------------------------------------------------------------------------

            #region ------- Row 7   ----------------------------------------------------------------------

            //// 7th row start
            Run CellValueL71 = new Run(" ");
            CellValueL71.FontSize = 18;
            CellValueL71.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL71 = new Paragraph();
            CellAddValueL71.Add(CellValueL71);
            CellAddValueL71.HorizontalTextAlignment = HorizontalAlignmentType.None;
            Cell cellL71 = new Cell();
            cellL71.Width = new Width(TableWidthUnit.Point, 500);
            cellL71.VerticalAlignment = VerticalAlignmentType.Top;
            cellL71.Add(CellAddValueL71);

            Run CellValueL72 = new Run("f) ");
            CellValueL72.FontSize = 18; //18 points
            CellValueL72.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL72 = new Paragraph();
            CellAddValueL72.Add(CellValueL72);
            CellAddValueL72.HorizontalTextAlignment = HorizontalAlignmentType.None;

            Cell cellL72 = new Cell();
            cellL72.Width = new Width(TableWidthUnit.Point, 500);
            cellL72.VerticalAlignment = VerticalAlignmentType.Top;
            cellL72.Add(CellAddValueL72);

            Run CellValueL73 = new Run();
            Run CellValueL74 = new Run();
            CellValueL73.AddText("The committee recommends for ..... Application with ....-modules for consideration which was inadvertently "+
            " put into debarred in ...... Details follows: " );
           
            CellValueL74.AddText("(The candidate has passed the papers in two consecutive attempts in ....... and ....... In ....... candidate cleared  " +
                "one module and ........ cleared ...... module. As per policy statement it is not mentioned that candidate has to clear ....... paper in each cycle.  " +
                "Committee members were of the view in the absence of any clear cut norms in r/o number of papers to be cleared in each cycle, decision has to be given in favour of candidate as  " +
                "candidate has clear all the ....... modules in first attempt and in one year. The committee also recommends that this may be incorporated   " +
                "in policy for processing of all such cases in present or future also.)");
            
            CellValueL74.FontSize = 18; //18 points
            CellValueL74.AsciiFont = "Century Gothic";

            CellValueL73.FontSize = 18; //18 points
            CellValueL73.AsciiFont = "Century Gothic";

            Paragraph CellAddValueL73 = new Paragraph();
            CellAddValueL73.Spacing = new Spacing();
            CellAddValueL73.Spacing.After = 0;
            Paragraph CellAddValueL74 = new Paragraph();
            CellAddValueL74.Spacing = new Spacing();
            CellAddValueL74.Spacing.Before = 0;
            CellAddValueL73.Add(CellValueL73);
            CellAddValueL74.Add(CellValueL74);
            CellAddValueL73.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            CellAddValueL74.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            Cell cellL73 = new Cell();
            cellL73.Width = new Width(TableWidthUnit.Point, 10000);
            cellL73.VerticalAlignment = VerticalAlignmentType.None;
            cellL73.Add(CellAddValueL73);
            cellL73.Add(CellAddValueL74);

            Row rowL7 = new Row();
            rowL7.Add(cellL71);
            rowL7.Add(cellL72);
            rowL7.Add(cellL73);
            //// 7th row End  
            #endregion -----------------------------------------------------------------------------

            #region ------- Row 8   ----------------------------------------------------------------------

            //// 8th row start
            Run CellValueL81 = new Run(" ");
            CellValueL81.FontSize = 18;
            CellValueL81.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL81 = new Paragraph();
            CellAddValueL81.Add(CellValueL81);
            CellAddValueL81.HorizontalTextAlignment = HorizontalAlignmentType.None;
            Cell cellL81 = new Cell();
            cellL81.Width = new Width(TableWidthUnit.Point, 500);
            cellL81.VerticalAlignment = VerticalAlignmentType.Top;
            cellL81.Add(CellAddValueL81);

            Run CellValueL82 = new Run("g) ");
            CellValueL82.FontSize = 18; //18 points
            CellValueL82.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL82 = new Paragraph();
            CellAddValueL82.Add(CellValueL82);
            CellAddValueL82.HorizontalTextAlignment = HorizontalAlignmentType.None;

            Cell cellL82 = new Cell();
            cellL82.Width = new Width(TableWidthUnit.Point, 500);
            cellL82.VerticalAlignment = VerticalAlignmentType.Top;
            cellL82.Add(CellAddValueL82);

            Run CellValueL83 = new Run();
            Run CellValueL84 = new Run();
            CellValueL83.AddText("The committee recommends for lapsed paper cases for ...... Application with ....... module(.........)for "+
            " ........ as per list placed at ....... Details follows:");

            CellValueL84.AddText("(The candidate has passed the papers in two consecutive attempts, ......-module in ........ but not applied, " +
            "......-modules in ....... and applied for Protsahan Puruskar,  " +
            "as per norms the payments to be disbursed in the mode ......... in two consecutive attempts, hence paid for ........ "+
            " and lapsed for ......-paper.)");

            CellValueL84.FontSize = 18; //18 points
            CellValueL84.AsciiFont = "Century Gothic";

            CellValueL83.FontSize = 18; //18 points
            CellValueL83.AsciiFont = "Century Gothic";

            Paragraph CellAddValueL83 = new Paragraph();
            CellAddValueL83.Spacing = new Spacing();
            CellAddValueL83.Spacing.After = 0;
            Paragraph CellAddValueL84 = new Paragraph();
            CellAddValueL84.Spacing = new Spacing();
            CellAddValueL84.Spacing.Before = 0;
            CellAddValueL83.Add(CellValueL83);
            CellAddValueL84.Add(CellValueL84);
            CellAddValueL83.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            CellAddValueL84.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            Cell cellL83 = new Cell();
            cellL83.Width = new Width(TableWidthUnit.Point, 10000);
            cellL83.VerticalAlignment = VerticalAlignmentType.None;
            cellL83.Add(CellAddValueL83);
            cellL83.Add(CellAddValueL84);

            Row rowL8 = new Row();
            rowL8.Add(cellL81);
            rowL8.Add(cellL82);
            rowL8.Add(cellL83);
            //// 8th row End  

            #endregion -----------------------------------------------------------------------------

            #region ------- Row 9   ----------------------------------------------------------------------

            //// 9th row start
            Run CellValueL91 = new Run(" ");
            CellValueL91.FontSize = 18;
            CellValueL91.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL91 = new Paragraph();
            CellAddValueL91.Add(CellValueL91);
            CellAddValueL91.HorizontalTextAlignment = HorizontalAlignmentType.None;
            Cell cellL91 = new Cell();
            cellL91.Width = new Width(TableWidthUnit.Point, 500);
            cellL91.VerticalAlignment = VerticalAlignmentType.Top;
            cellL91.Add(CellAddValueL91);

            Run CellValueL92 = new Run("h) ");
            CellValueL92.FontSize = 18; //18 points
            CellValueL92.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL92 = new Paragraph();
            CellAddValueL92.Add(CellValueL92);
            CellAddValueL92.HorizontalTextAlignment = HorizontalAlignmentType.None;

            Cell cellL92 = new Cell();
            cellL92.Width = new Width(TableWidthUnit.Point, 500);
            cellL92.VerticalAlignment = VerticalAlignmentType.Top;
            cellL92.Add(CellAddValueL92);

            Run CellValueL93 = new Run("The committee recommends that the protsahan puraskar should be applicable to those candidates only, who had submitted examination " +
                 "fees from their own resources and not claimed/obtained reimbursement (directly/indirectly) " +
                "from any other sources. This clause to be applicable from ........... Examination cycle.");
            CellValueL93.FontSize = 18; //18 points
            CellValueL93.AsciiFont = "Century Gothic";
            Paragraph CellAddValueL93 = new Paragraph();
            CellAddValueL93.Add(CellValueL93);
            CellAddValueL93.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            Cell cellL93 = new Cell();
            cellL93.Width = new Width(TableWidthUnit.Point, 10000);
            cellL93.VerticalAlignment = VerticalAlignmentType.None;
            cellL93.Add(CellAddValueL93);

            Row rowL9 = new Row();
            rowL9.Add(cellL91);
            rowL9.Add(cellL92);
            rowL9.Add(cellL93);
            //// 9th row End  
            #endregion --------------------------------------------------------


            //Table table7 = new Table(StandardBorderStyle.SingleLine);
            Table table7 = new Table(StandardBorderStyle.None);
            table7.Width = new Width(TableWidthUnit.Percent, 100);
            table7.Alignment = HorizontalAlignmentType.Center;
            table7.Grid = tableGrid7;
           // table7.Add(rowL1);
            table7.Add(rowL2);
            table7.Add(rowL3);
            table7.Add(rowL4);
            table7.Add(rowL5);
            table7.Add(rowL6);
            table7.Add(rowL7);
            table7.Add(rowL8);
            table7.Add(rowL9);
            doc.Body.Add(table7);
            //First Table with Merge cell End
            #endregion -----------------------------------------------------------------------------
            
            #region  ----------- Space between two tables ------------------------------------------
            Run R59 = new Run();
            R59.AddText("");
            R59.FontSize = 19; //12 points
            R59.AsciiFont = "Century Gothic";
            Paragraph P59 = new Paragraph();
            P59.Spacing = new Spacing();
            P59.Spacing.Before = 350;
            P59.Spacing.After = 400;
            P59.Add(R59);
            P59.HorizontalTextAlignment = HorizontalAlignmentType.Right;
            doc.Body.Add(P59);

            #endregion -----------------------------------------------------------------------------
            
            #region ---------- Table 8 for Last Page context, Name of wing  -----------------------------------------------------
           
            TableGrid tableGrid8 = new TableGrid();
            tableGrid8.Columns.Add(new TableGridColumn(2000));
            tableGrid8.Columns.Add(new TableGridColumn(2000));
            tableGrid8.Columns.Add(new TableGridColumn(2000));

           
            #region -----------  Row 1  ----------------------------------------------------

            // First row start 
            Run CellValueS21 = new Run("(Anurag Shah, CoE)");
            CellValueS21.FontSize = 18; //18 points
            CellValueS21.Bold = ExtendedBoolean.True;
            CellValueS21.AsciiFont = "Century Gothic";
            Paragraph CellAddValueS21 = new Paragraph();
            CellAddValueS21.Spacing = new Spacing();
            CellAddValueS21.Spacing.Before = 0;
            CellAddValueS21.Spacing.After = 0;
            CellAddValueS21.Add(CellValueS21);
            CellAddValueS21.HorizontalTextAlignment = HorizontalAlignmentType.None;

            Cell cellS21 = new Cell();
            cellS21.Width = new Width(TableWidthUnit.Point, 2000);
            cellS21.VerticalAlignment = VerticalAlignmentType.Top;
            cellS21.Add(CellAddValueS21);

            Run CellValueS22 = new Run("(Rajneesh Kr.Asthana, JD(Acad.)");
            CellValueS22.FontSize = 18; //18 points
            CellValueS22.Bold = ExtendedBoolean.True;
            CellValueS22.AsciiFont = "Century Gothic";
            Paragraph CellAddValueS22 = new Paragraph();
            CellAddValueS22.Spacing = new Spacing();
            CellAddValueS22.Spacing.Before = 0;
            CellAddValueS22.Spacing.After = 0;
            CellAddValueS22.Add(CellValueS22);
            CellAddValueS22.HorizontalTextAlignment = HorizontalAlignmentType.None;

            Cell cellS22 = new Cell();
            cellS22.Width = new Width(TableWidthUnit.Point, 2000);
            cellS22.VerticalAlignment = VerticalAlignmentType.Top;
            cellS22.Add(CellAddValueS22);

            Run CellValueS23 = new Run("(Rabindra Prasad, AO)");
            CellValueS23.FontSize = 18; //18 points
            CellValueS23.Bold = ExtendedBoolean.True;
            CellValueS23.AsciiFont = "Century Gothic";
            Paragraph CellAddValueS23 = new Paragraph();
            CellAddValueS23.Spacing = new Spacing();
            CellAddValueS23.Spacing.Before = 0;
            CellAddValueS23.Spacing.After = 0;
            CellAddValueS23.Add(CellValueS23);
            CellAddValueS23.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            Cell cellS23 = new Cell();
            cellS23.Width = new Width(TableWidthUnit.Point, 2000);
            cellS23.VerticalAlignment = VerticalAlignmentType.None;
            cellS23.Add(CellAddValueS23);

            Row rowS2 = new Row();
            rowS2.Add(cellS21);
            rowS2.Add(cellS22);
            rowS2.Add(cellS23);
            // First row End
            #endregion -----------------------------------------------------------------------------

            #region --------- Row 2 --------------------------------------------------------
            // Second row start
            Run CellValueS31 = new Run("Representative(Exam.Wing)");
            CellValueS31.FontSize = 18;
            CellValueS31.Bold = ExtendedBoolean.True;
            CellValueS31.AsciiFont = "Century Gothic";
            Paragraph CellAddValueS31 = new Paragraph();
            CellAddValueS31.Spacing = new Spacing();
            CellAddValueS31.Spacing.Before = 0;
            CellAddValueS31.Spacing.After = 0;
            CellAddValueS31.Add(CellValueS31);
            CellAddValueS31.HorizontalTextAlignment = HorizontalAlignmentType.None;
            Cell cellS31 = new Cell();
            cellS31.Width = new Width(TableWidthUnit.Point, 2000);
            cellS31.VerticalAlignment = VerticalAlignmentType.Top;
            cellS31.Add(CellAddValueS31);

            Run CellValueS32 = new Run("Representative(Tech.Wing)");
            CellValueS32.FontSize = 18; //18 points
            CellValueS32.Bold = ExtendedBoolean.True;
            CellValueS32.AsciiFont = "Century Gothic";
            Paragraph CellAddValueS32 = new Paragraph();
            CellAddValueS32.Spacing = new Spacing();
            CellAddValueS32.Spacing.Before = 0;
            CellAddValueS32.Spacing.After = 0;
            CellAddValueS32.Add(CellValueS32);
            CellAddValueS32.HorizontalTextAlignment = HorizontalAlignmentType.None;

            Cell cellS32 = new Cell();
            cellS32.Width = new Width(TableWidthUnit.Point, 2000);
            cellS32.VerticalAlignment = VerticalAlignmentType.Top;
            cellS32.Add(CellAddValueS32);

            Run CellValueS33 = new Run("Representative(Fin.Wing)");
            CellValueS33.FontSize = 18; //18 points
            CellValueS33.Bold = ExtendedBoolean.True;
            CellValueS33.AsciiFont = "Century Gothic";
            Paragraph CellAddValueS33 = new Paragraph();
            CellAddValueS33.Spacing = new Spacing();
            CellAddValueS33.Spacing.Before = 0;
            CellAddValueS33.Spacing.After = 0;
            CellAddValueS33.Add(CellValueS33);
            CellAddValueS33.HorizontalTextAlignment = HorizontalAlignmentType.Both;
            Cell cellS33 = new Cell();
            cellS33.Width = new Width(TableWidthUnit.Point, 2000);
            cellS33.VerticalAlignment = VerticalAlignmentType.None;
            cellS33.Add(CellAddValueS33);

            Row rowS3 = new Row();
            rowS3.Add(cellS31);
            rowS3.Add(cellS32);
            rowS3.Add(cellS33);

            // Second row End
            #endregion -----------------------------------------------------------------------------
                      
            Table table8 = new Table(StandardBorderStyle.None);
            table8.Width = new Width(TableWidthUnit.Percent, 100);
            table8.Alignment = HorizontalAlignmentType.Center;
            table8.Grid = tableGrid8;          
            table8.Add(rowS2);
            table8.Add(rowS3);          
            doc.Body.Add(table8);
          // signature and name wing  END
            #endregion -----------------------------------------------------------------------------
                     
            #region -------- DownLoad Report ----------------------------------------------
            //create new folder and save the file and download         
            string folderPath = Server.MapPath("~/PuraskarDocument/");
            string filePath = folderPath + Path.GetFileName("temp26.docx");
            if (!Directory.Exists(folderPath))
            { Directory.CreateDirectory(folderPath); }
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            doc.Save(folderPath + "temp26.docx");
            WebClient wc = new WebClient();

            ShowAlert("Document created successfully !");

            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            string filePath2 = "~/PuraskarDocument/" + "temp26.docx";
            response.Clear();
            response.ClearContent();
            response.ClearHeaders();
            response.Buffer = true;
            response.AddHeader("Content-Disposition", "attachment;filename=ProtsahanPuraskarReport.docx");
            byte[] data = req.DownloadData(Server.MapPath(filePath2));
            response.BinaryWrite(data);
            // delete the file which created after download
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            response.End();
           
      #endregion -----------------------------------------------------------------------------

          

            #region for delete -------------
            //////  Microsoft.Office.Interop.Word.Paragraph para26 = document.Content.Paragraphs.Add(ref missing);
          //////  object styleHeading26 = "Normal";
          //////  para26.Range.set_Style(ref styleHeading18);
          //////  para26.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
          //////  Microsoft.Office.Interop.Word.Table objTable5 = (Microsoft.Office.Interop.Word.Table)document.Tables.Add(para26.Range, 6, 6, ref missing, ref missing);
          //////  objTable5.Rows.Alignment = Microsoft.Office.Interop.Word.WdRowAlignment.wdAlignRowCenter;

          //////  objTable5.Borders.Enable = 1;
          //////  objTable5.Rows[1].Range.Text = strText;
          //////  objTable5.Rows[1].Range.Font.Bold = 1;
          //////  objTable5.Rows[1].Range.Font.Size = 9;
          //////  objTable5.Rows[1].Range.Font.Position = 1;
          //////  objTable5.Rows[1].Range.Font.Name = "Century Gothic";
          //////  objTable5.Cell(1, 1).Range.Text = "Sl.No";
          //////  objTable5.Cell(1, 1).Width = 35;
          //////  objTable5.Cell(1, 2).Range.Text = "Level";

          //////  objTable5.Cell(1, 3).Range.Text = " Application Recommended";
          //////  objTable5.Cell(1, 4).Range.Text = "Total No. of Modules   \n (a)";
          //////  objTable5.Cell(1, 5).Range.Text = "Scholarship Amt per module \n (b)";
          //////  objTable5.Cell(1, 6).Range.Text = "Total amount \n (in Rs)";
           

          //////  objTable5.Cell(1, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(1, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(1, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(1, 4).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(1, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(1, 6).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;           
          //////  objTable5.Range.Rows[1].Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;


          //////  objTable5.Rows[2].Range.Font.Italic = 1;
          //////  objTable5.Rows[2].Range.Font.Size = 9;
          //////  objTable5.Rows[2].Range.Font.Name = "Century Gothic";
          //////  objTable5.Cell(2, 1).Range.Text = "1";
          //////  objTable5.Cell(2, 1).Width = 35;
          //////  objTable5.Cell(2, 2).Range.Text = "'O'";           
          //////  objTable5.Cell(2, 3).Range.Text = "628";
          //////  objTable5.Cell(2, 4).Range.Text = "1254";
          //////  objTable5.Cell(2, 5).Range.Text = "3000";
          //////  objTable5.Cell(2, 6).Range.Text = "37,62,000"; 

          //////  objTable5.Cell(2, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(2, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
          //////  objTable5.Cell(2, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(2, 4).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(2, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(2, 6).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
           
          //////  objTable5.Range.Rows[2].Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

          //////  objTable5.Rows[3].Range.Font.Size = 9;
          //////  objTable5.Rows[3].Range.Font.Name = "Century Gothic";
          //////  objTable5.Cell(3, 1).Range.Text = "2";
          //////  objTable5.Cell(3, 1).Width = 35;
          //////  objTable5.Cell(3, 2).Range.Text = "'A'";            
          //////  objTable5.Cell(3, 3).Range.Text = "- ";
          //////  objTable5.Cell(3, 4).Range.Text = "-";
          //////  objTable5.Cell(3, 5).Range.Text = "3000";
          //////  objTable5.Cell(3, 6).Range.Text = "Total";
           
          //////  objTable5.Cell(3, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(3, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
          //////  objTable5.Cell(3, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(3, 4).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(3, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(3, 6).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
         
          //////  objTable5.Range.Rows[3].Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

          //////  objTable5.Rows[4].Range.Font.Size = 9;
          //////  objTable5.Rows[4].Range.Font.Name = "Century Gothic";
          //////  objTable5.Cell(4, 1).Range.Text = "3";
          //////  objTable5.Cell(4, 1).Width = 35;
          //////  objTable5.Cell(4, 2).Range.Text = "'B'";           
          //////  objTable5.Cell(4, 3).Range.Text = "- ";
          //////  objTable5.Cell(4, 4).Range.Text = "-";
          //////  objTable5.Cell(4, 5).Range.Text = "-";
          //////  objTable5.Cell(4, 6).Range.Text = "-";           
         
          //////  objTable5.Cell(4, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(4, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
          //////  objTable5.Cell(4, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(4, 4).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(4, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(4, 6).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
           
          //////  objTable5.Range.Rows[4].Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

          //////  objTable5.Rows[5].Range.Font.Size = 9;
          //////  objTable5.Rows[5].Range.Font.Name = "Century Gothic";
          //////  objTable5.Cell(5, 1).Range.Text = "4";           
          //////  objTable5.Cell(5, 1).Width = 35;           
          //////  //objTable5.Cell(5, 2).Width = 150;
          //////  //objTable5.Rows[5].Cells[1].Merge(objTable5.Rows[5].Cells[2]);
          //////  objTable5.Cell(5, 2).Range.Text = "'C'";
          //////  objTable5.Cell(5, 3).Range.Text = "-";
          //////  objTable5.Cell(5, 4).Range.Text = "-";
          //////  objTable5.Cell(5, 5).Range.Text = "-";
          //////  objTable5.Cell(5, 6).Range.Text = "-";

          //////  objTable5.Cell(5, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  // objTable5.Cell(5, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
          //////  objTable5.Cell(5, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(5, 4).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(5, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(5, 6).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;          
          //////  objTable5.Range.Rows[5].Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;


          //////  objTable5.Rows[6].Range.Font.Size = 9;
          //////  objTable5.Rows[6].Range.Font.Name = "Century Gothic";
          //////  objTable5.Cell(6, 1).Range.Text = "Total:->";
          //////  objTable5.Cell(6, 1).Width = 35;
          //////  //objTable5.Cell(5, 2).Width = 150;
          //////  objTable5.Rows[6].Cells[1].Merge(objTable5.Rows[6].Cells[2]);
          //////  objTable5.Cell(6, 2).Range.Text = "'C'";
          //////  objTable5.Cell(6, 3).Range.Text = "-";
          //////  objTable5.Cell(6, 4).Range.Text = "-";
          //////  objTable5.Cell(6, 5).Range.Text = "-";
          //////  //objTable5.Cell(6, 6).Range.Text = "-";

          //////  objTable5.Cell(6, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  // objTable5.Cell(5, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
          //////  objTable5.Cell(6, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(6, 4).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Cell(6, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          ////// // objTable5.Cell(6, 6).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable5.Range.Rows[6].Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

          //////  para26.Format.SpaceAfter = 5;
          //////  para26.Range.InsertParagraphAfter();


          //////  //Rejected Cases
          //////  Microsoft.Office.Interop.Word.Paragraph para27 = document.Content.Paragraphs.Add(ref missing);
          //////  object styleHeading27 = "Normal";
          //////  para27.Range.set_Style(ref styleHeading27);

          //////  para27.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  para27.Range.Font.Name = "Century Gothic";
          //////  para27.Range.Font.Size = 9;
          //////  para27.Range.Font.Bold = 0;
          //////  para27.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  // para25.Range.Text = "      (a)Recommended: There are 628 applications, found to be eligible for Protsahan Puraskar for 1254 no. of";
          //////  String text1 = "\t(b)" + "Rejected Cases: 116 (One hundred Sixteen) application forms as per list of candidates placed at " +
          //////                " pg. ................ to ............ for Jan-2021, are recommended for rejection, as the candidates do not fulfill the criteria for claim of Protsahan Puraskar  " +
          ////// " (formerly Scholarship) laid down by the GC in its 16th meeting held on 16th April, 2002.";
          //////  para27.Range.Text = text1;
          //////  para27.Range.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
          //////  Microsoft.Office.Interop.Word.Range underlinedRanges = document.Range(para27.Range.Start + text1.IndexOf("Rejec"), para27.Range.Start + text1.IndexOf(":"));
          //////  underlinedRanges.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineSingle;

          //////  para27.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  para27.Range.Font.Name = "Century Gothic";
          //////  para27.Range.Font.Size = 9;
          //////  para27.Range.Font.Bold = 0;
          //////  para27.Format.SpaceAfter = 5;
          //////  para27.Range.InsertParagraphAfter();

          //////  //Withheld Cases
          //////  Microsoft.Office.Interop.Word.Paragraph para28 = document.Content.Paragraphs.Add(ref missing);
          //////  object styleHeading28 = "Normal";
          //////  para28.Range.set_Style(ref styleHeading28);

          //////  para28.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  para28.Range.Font.Name = "Century Gothic";
          //////  para28.Range.Font.Size = 9;
          //////  para28.Range.Font.Bold = 0;
          //////  para28.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  String text2 = "\t(c)" + "Withheld Cases: 06 (Six) application forms as per list of candidates placed at pg. ........... to ........... for July, 2019, Examinations " +
          //////                " are put on hold as they have not submitted the relevant documents as proof of meeting the criteria. It is recommended that  " +
          ////// " these applicants may be send emails to submit the relevant document within 15 days followed by 3 reminders, otherwise their claim may be rejected.";
          //////  para28.Range.Text = text2;
          //////  para28.Range.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
          //////  Microsoft.Office.Interop.Word.Range underlinedRangess = document.Range(para28.Range.Start + text2.IndexOf("Withhe"), para28.Range.Start + text2.IndexOf(":"));
          //////  underlinedRangess.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineSingle;

          //////  para28.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  para28.Range.Font.Name = "Century Gothic";
          //////  para28.Range.Font.Size = 9;
          //////  para28.Range.Font.Bold = 0;
          //////  para28.Format.SpaceAfter = 5;
          //////  para28.Range.InsertParagraphAfter();

          //////  Microsoft.Office.Interop.Word.Paragraph para29 = document.Content.Paragraphs.Add(ref missing);
          //////  object styleHeading29 = "Normal";
          //////  para29.Range.set_Style(ref styleHeading18);
          //////  para29.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
          //////  Microsoft.Office.Interop.Word.Table objTable6 = (Microsoft.Office.Interop.Word.Table)document.Tables.Add(para29.Range, 6,5, ref missing, ref missing);
          //////  objTable6.Rows.Alignment = Microsoft.Office.Interop.Word.WdRowAlignment.wdAlignRowCenter;

          //////  objTable6.Borders.Enable = 1;
          //////  objTable6.Rows[1].Range.Text = strText;
          //////  objTable6.Rows[1].Range.Font.Bold = 1;
          //////  objTable6.Rows[1].Range.Font.Size = 9;
          //////  objTable6.Rows[1].Range.Font.Position = 1;
          //////  objTable6.Rows[1].Range.Font.Name = "Century Gothic";
          //////  objTable6.Cell(1, 1).Range.Text = "Sl.No";
          //////  objTable6.Cell(1, 1).Width = 35;
          //////  objTable6.Cell(1, 2).Range.Text = "Particulars";
          //////  objTable6.Cell(1, 2).Width = 300;
          //////  objTable6.Cell(1, 3).Range.Text = " O \n Level";
          //////  objTable6.Cell(1, 3).Width = 40;
          //////  objTable6.Cell(1, 4).Width = 40;
          //////  objTable6.Cell(1, 5).Width = 40;
          //////  objTable6.Cell(1, 4).Range.Text = "A \n Level";
            
          //////  objTable6.Cell(1, 5).Range.Text = "Total";
            
          ////// // objTable6.Cell(1, 6).Range.Text = "Total amount \n (in Rs)";


          //////  objTable6.Cell(1, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(1, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(1, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(1, 4).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(1, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          ////// // objTable6.Cell(1, 6).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Range.Rows[1].Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;


          //////  objTable6.Rows[2].Range.Font.Italic = 1;
          //////  objTable6.Rows[2].Range.Font.Size = 9;
          //////  objTable6.Rows[2].Range.Font.Name = "Century Gothic";
          //////  objTable6.Cell(2, 1).Range.Text = "a)";
          //////  objTable6.Cell(2, 1).Width = 35;
          //////  objTable6.Cell(2, 2).Range.Text = "Aadhaar number has not been produced by the Candidate";
          //////  objTable6.Cell(2, 2).Width = 300;
          //////  objTable6.Cell(2, 3).Width = 40;
          //////  objTable6.Cell(2, 4).Width = 40;
          //////  objTable6.Cell(2, 5).Width = 40;
          //////  objTable6.Cell(2, 3).Range.Text = "01";
          //////  objTable6.Cell(2, 4).Range.Text = "-";
          //////  objTable6.Cell(2, 5).Range.Text = "01";
          ////// // objTable6.Cell(2, 6).Range.Text = "37,62,000";

          //////  objTable6.Cell(2, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(2, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
          //////  objTable6.Cell(2, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(2, 4).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(2, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  //objTable6.Cell(2, 6).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

          //////  objTable6.Range.Rows[2].Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

          //////  objTable6.Rows[3].Range.Font.Size = 9;
          //////  objTable6.Rows[3].Range.Font.Name = "Century Gothic";
          //////  objTable6.Cell(3, 1).Range.Text = "b)";
          //////  objTable6.Cell(3, 1).Width = 35;
          //////  objTable6.Cell(3, 2).Range.Text = "Income certificate has not been issued by the Comp. Authority";
          //////  objTable6.Cell(3, 2).Width = 300;
          //////  objTable6.Cell(3, 3).Width = 40;
          //////  objTable6.Cell(3, 4).Width = 40;
          //////  objTable6.Cell(3, 5).Width = 40;
          //////  objTable6.Cell(3, 3).Range.Text = "01 ";
          //////  objTable6.Cell(3, 4).Range.Text = "-";
          //////  objTable6.Cell(3, 5).Range.Text = "01";
          //////  //objTable6.Cell(3, 6).Range.Text = "Total";

          //////  objTable6.Cell(3, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(3, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
          //////  objTable6.Cell(3, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(3, 4).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(3, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  //objTable6.Cell(3, 6).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

          //////  objTable6.Range.Rows[3].Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

          //////  objTable6.Rows[4].Range.Font.Size = 9;
          //////  objTable6.Rows[4].Range.Font.Name = "Century Gothic";
          //////  objTable6.Cell(4, 1).Range.Text = "c)";
          //////  objTable6.Cell(4, 1).Width = 35;
          //////  objTable6.Cell(4, 2).Range.Text = "Latest Income Certificate has not been produced by candidate";
          //////  objTable6.Cell(4, 2).Width = 300;
          //////  objTable6.Cell(4, 3).Width = 40;
          //////  objTable6.Cell(4, 4).Width = 40;
          //////  objTable6.Cell(4, 5).Width = 40;
          //////  objTable6.Cell(4, 3).Range.Text = "03 ";
          //////  objTable6.Cell(4, 4).Range.Text = "-";
          //////  objTable6.Cell(4, 5).Range.Text = "03";
          //////  //objTable6.Cell(4, 6).Range.Text = "-";

          //////  objTable6.Cell(4, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(4, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
          //////  objTable6.Cell(4, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(4, 4).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(4, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          ////// // objTable6.Cell(4, 6).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

          //////  objTable6.Range.Rows[4].Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

          //////  objTable6.Rows[5].Range.Font.Size = 9;
          //////  objTable6.Rows[5].Range.Font.Name = "Century Gothic";
          //////  objTable6.Cell(5, 1).Range.Text = "d)";
          //////  objTable6.Cell(5, 1).Width = 35;
          //////  //objTable6.Cell(5, 2).Width = 150;
          //////  //objTable6.Rows[5].Cells[1].Merge(objTable6.Rows[5].Cells[2]);
          //////  objTable6.Cell(5, 2).Range.Text = "Caste Certificate has not been produced by the Candidate";
          //////  objTable6.Cell(5, 2).Width = 300;
          //////  objTable6.Cell(5, 3).Width = 40;
          //////  objTable6.Cell(5, 4).Width = 40;
          //////  objTable6.Cell(5, 5).Width = 40;
          //////  objTable6.Cell(5, 3).Range.Text = "01";
          //////  objTable6.Cell(5, 4).Range.Text = "-";
          //////  objTable6.Cell(5, 5).Range.Text = "01";
          ////// // objTable6.Cell(5, 6).Range.Text = "-";

          //////  objTable6.Cell(5, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  // objTable6.Cell(5, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
          //////  objTable6.Cell(5, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(5, 4).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(5, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  //objTable6.Cell(5, 6).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Range.Rows[5].Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;


          //////  objTable6.Rows[6].Range.Font.Size = 9;
          //////  objTable6.Rows[6].Range.Font.Name = "Century Gothic";
          //////  objTable6.Cell(6, 1).Range.Text = "Total withhold forms";
          //////  objTable6.Cell(6, 1).Width = 35;
          //////  objTable6.Cell(6, 2).Width = 300;
          //////  objTable6.Cell(6, 3).Width = 40;
          //////  objTable6.Cell(6, 4).Width = 40;
          //////  objTable6.Cell(6, 5).Width = 40;
          //////  objTable6.Rows[6].Cells[1].Merge(objTable6.Rows[6].Cells[2]);
          //////  objTable6.Cell(6, 2).Range.Text = "06";
          //////  objTable6.Cell(6, 3).Range.Text = "-";
          //////  objTable6.Cell(6, 4).Range.Text = "06";
          //////  //objTable6.Cell(6, 5).Range.Text = "-";
          //////  //objTable6.Cell(6, 6).Range.Text = "-";

          //////  objTable6.Cell(6, 1).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(6, 2).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(6, 3).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Cell(6, 4).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          ////// // objTable6.Cell(6, 5).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  // objTable6.Cell(6, 6).Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
          //////  objTable6.Range.Rows[6].Cells.VerticalAlignment = WdCellVerticalAlignment.wdCellAlignVerticalCenter;

          //////  para29.Format.SpaceAfter = 1;
          //////  para29.Range.InsertParagraphAfter();


          //////  //d part
          //////  Microsoft.Office.Interop.Word.Paragraph para30 = document.Content.Paragraphs.Add(ref missing);
          //////  object styleHeading30 = "Normal";
          //////  para30.Range.set_Style(ref styleHeading30);

          //////  para30.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  para30.Range.Font.Name = "Century Gothic";
          //////  para30.Range.Font.Size = 9;
          //////  para30.Range.Font.Bold = 0;
          //////  para30.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  String text3 = "\t(d)" + "As per norms the payment of Protsahan Puruskar to be disbursed is as given below: \n \t\t" +
          //////                " i)	‘O’ Level (Two Instalments) 2/2 \n \t\t" +
          //////                " ii)	‘A’ Level (Three Instalments) 3/3/4  \n \t\t" +
          //////                " iii)	‘B’ Level (Four Instalments) 3/3/3/6 \n \t\t" +
          //////                " iv)	‘C’ Level (Three Instalments) 4/4/4  \n \t" +
          //////                " Based on the above, 44 application forms with 84 modules (Rs.252000) as per list of candidates placed \t\t " +
          ////// "  at pg. ............. to ............. forJan-2021, are recommended for carrying forward to July-21 Examination Cycle,";
          //////  para30.Range.Text = text3;
          //////  para30.Range.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
          //////  Microsoft.Office.Interop.Word.Range underlinedRangesss = document.Range(para30.Range.Start + text3.IndexOf("Withhe"), para30.Range.Start + text3.IndexOf(":"));
          //////  underlinedRangess.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineSingle;

          //////  para30.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  para30.Range.Font.Name = "Century Gothic";
          //////  para30.Range.Font.Size = 9;
          //////  para30.Range.Font.Bold = 0;
          //////  para30.Format.SpaceAfter = 6;
          //////  para30.Range.InsertParagraphAfter();

          //////  //on the basis of 
          //////  Microsoft.Office.Interop.Word.Paragraph para31 = document.Content.Paragraphs.Add(ref missing);
          //////  object styleHeading31 = "Normal";
          //////  para31.Range.set_Style(ref styleHeading31);

          //////  para31.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  para31.Range.Font.Name = "Century Gothic";
          //////  para31.Range.Font.Size = 9;
          //////  para31.Range.Font.Bold = 1;
          //////  para31.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  String text4 = "On the basis of figures as mentioned above the following is recommended by the committee.";
          //////  para31.Range.Text = text4;
          //////  para31.Range.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
          //////  Microsoft.Office.Interop.Word.Range underlinedRangessss = document.Range(para31.Range.Start + text4.IndexOf("On"), para31.Range.Start + text4.IndexOf("."));
          //////  underlinedRangessss.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineSingle;

          //////  para31.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  para31.Range.Font.Name = "Century Gothic";
          //////  para31.Range.Font.Size = 9;
          //////  para31.Range.Font.Bold = 1;
          //////  para31.Format.SpaceAfter = 5;
          //////  para31.Range.InsertParagraphAfter();

          //////  //6 a
          //////  Microsoft.Office.Interop.Word.Paragraph para32 = document.Content.Paragraphs.Add(ref missing);
          //////  object styleHeading32= "Normal";
          //////  para32.Range.set_Style(ref styleHeading32);

          //////  para32.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  para32.Range.Font.Name = "Century Gothic";
          //////  para32.Range.Font.Size = 9;
            //////  para32.Range.Font.Bold = 0;
          
            //////  para32.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  String text5 = "6.  a)	The committee Recommends 628 applications for release of payment Rs 37,62,000/- (Thirty Seven " +
          //////                " Lakhs Sixty Two thousand only) towards the payment of Protsahan Puraskar (formerly Scholarship) to" +
          //////                " SC/ST/Female & PwD candidates for Jan, 2021 Examinations as per list placed in " +
          ////// "the file at pg. ............. to ............. .";
          //////  para32.Range.Text = text5;
          //////  para32.Range.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
          //////  Microsoft.Office.Interop.Word.Range underlinedRange1 = document.Range(para32.Range.Start + text5.IndexOf("Reco"), para32.Range.Start + text5.IndexOf("applications"));
          //////  underlinedRange1.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineSingle;
            
          //////  para32.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  para32.Range.Font.Name = "Century Gothic";
          //////  para32.Range.Font.Size = 9;
          //////  para32.Range.Font.Bold = 0;
          //////  para32.Format.SpaceAfter = 5;
          //////  para32.Range.InsertParagraphAfter();

           

          //////  Microsoft.Office.Interop.Word.Paragraph para34 = document.Content.Paragraphs.Add(ref missing);
          //////  object styleHeading34 = "Normal";
          //////  para34.Range.set_Style(ref styleHeading34);
          //////  para34.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
          //////  para34.Range.Text = "\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tContd…3";

          //////  para34.Format.SpaceAfter = 15;
          //////  para34.Range.InsertParagraphAfter();

          //////  para20.Format.SpaceAfter = 4;
          //////  para20.Range.InsertParagraphAfter();

          //////  //6 b
          //////  Microsoft.Office.Interop.Word.Paragraph para33 = document.Content.Paragraphs.Add(ref missing);
          //////  object styleHeading33 = "Normal";
          //////  para33.Range.set_Style(ref styleHeading33);

          //////  para33.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  para33.Range.Font.Name = "Century Gothic";
          //////  para33.Range.Font.Size = 9;
          //////  para33.Range.Font.Bold = 0;
            //////  para33.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
           
            //////  String text6 = "6.  b)	The committee recommends to Withhold 06 applications for Jan, 2021 examination for resubmission of  " +
          //////                           "relevant documents as per list placed at pg. ............. to ............. .";
          //////  para33.Range.Text = text6;
          //////  para33.Range.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
          //////  Microsoft.Office.Interop.Word.Range underlinedRange2 = document.Range(para33.Range.Start + text6.IndexOf("Wi"), para33.Range.Start + text6.IndexOf("applications"));
          //////  underlinedRange2.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineSingle;

          //////  para33.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  para33.Range.Font.Name = "Century Gothic";
          //////  para33.Range.Font.Size = 9;
          //////  para33.Range.Font.Bold = 0;
          //////  para33.Format.SpaceAfter = 5;
            //////  para33.Range.InsertParagraphAfter();
            #endregion ----------------------------

            // "Representative(Exam.Wing)\t\tRepresentative(Tech.Wing) \t\t Representative(Fin.Wing)\n Representative(Exam.Wing)\t\tRepresentative(Tech.Wing) \t\t Representative(Fin.Wing)"
          //////  para40.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  String text12 = "6.  h)	The committee recommends that the protsahan puraskar should be applicable to those candidates only, who had submitted examination " +
          //////       "fees from their own resources and not claimed/obtained reimbursement (directly/indirectly) " +
          //////      "from any other sources. This clause to be applicable from July-2021 Examination cycle.";
          //////  para40.Range.Text = text12;
          //////  para40.Range.Font.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineNone;
          //////  //Microsoft.Office.Interop.Word.Range underlinedRange8 = document.Range(para40.Range.Start + text12.IndexOf("la"), para40.Range.Start + text12.IndexOf("for 01"));
          //////  //underlinedRange8.Underline = Microsoft.Office.Interop.Word.WdUnderline.wdUnderlineSingle;

          //////  para40.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  para40.Range.Font.Name = "Century Gothic";
          //////  para40.Range.Font.Size = 9;
          //////  para40.Range.Font.Bold = 0;
          //////  para40.Format.SpaceAfter = 12;
          //////  para40.Range.InsertParagraphAfter();


          //////  para39.Format.SpaceAfter = 0;
          //////  para39.Range.InsertParagraphAfter();

          //////  Microsoft.Office.Interop.Word.Paragraph para5 = document.Content.Paragraphs.Add(ref missing);
          //////  object styleHeading5 = "Normal";
          //////  para5.Range.set_Style(ref styleHeading5);
          //////   para5.Range.Font.Bold = 2;
          //////  para5.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
          //////  para5.Range.Text = "Representative(Exam.Wing)\t\tRepresentative(Tech.Wing) \t\t Representative(Fin.Wing)\n Representative(Exam.Wing)\t\tRepresentative(Tech.Wing) \t\t Representative(Fin.Wing)" + Environment.NewLine;
          ////// // para4.Range.InsertParagraphAfter();

          //////  //Save the document  
          ////// // object filename = @"d:\temp22.docx";
          ////// // document.SaveAs2(ref filename);                   
           
         ////////create new folder and save the file and download         
         //////   string folderPath = Server.MapPath("~/PuraskarDocument/");
         //////   string filePath = folderPath + Path.GetFileName("temp26.docx");
         //////   if (!Directory.Exists(folderPath))
         //////   { Directory.CreateDirectory(folderPath);}
         //////   if (File.Exists(filePath))
         //////   {
         //////       File.Delete(filePath);
         //////   }
         //////   document.SaveAs2(folderPath + "temp26.docx");           
         //////   WebClient wc = new WebClient();
          
         //////   document.Close(ref missing, ref missing, ref missing);
         //////   document = null;
         //////   winword.Quit(ref missing, ref missing, ref missing);
         //////   winword = null;
         //////   ShowAlert("Document created successfully !");

         //////   WebClient req = new WebClient();
         //////   HttpResponse response = HttpContext.Current.Response;
         //////   string filePath2 = "~/PuraskarDocument/" + "temp26.docx"; 
         //////   response.Clear();
         //////   response.ClearContent();
         //////   response.ClearHeaders();
         //////   response.Buffer = true;
         //////   response.AddHeader("Content-Disposition", "attachment;filename=ProtsahanPuraskarReport.docx");
         //////   byte[] data = req.DownloadData(Server.MapPath(filePath2));
         //////   response.BinaryWrite(data);
         //////   // delete the file which created after download
         //////   if (File.Exists(filePath))
         //////   {
         //////       File.Delete(filePath);
         //////   }
         //////   response.End();
         //////   //create new folder and save the file and download  End            
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }    
}  
