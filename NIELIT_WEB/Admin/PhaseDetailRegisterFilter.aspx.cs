using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using EConnect.Utils.Data;
using System.Data.SqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web;
using System.Configuration;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using ThoughtWorks.QRCode.Codec.Util;
using ZXing;
using System.Text;
using System.Drawing;
using Ionic.Zip;


public partial class PhaseDetailRegisterFilter : BasePage
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
                    BindCourseCategory();
                    ddlCourseCategry.SelectedValue = "6";
                    BindDataDownloadedSequence();
                    //  BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Photographs For Certificate", "Admin/DownloadImagesForCertificate.aspx", ""));

                    lblNoRecord.Text = "";
                    lblNoRecord.Visible = false;
                }
            }
            else
            {
                //  BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Photographs For Certificate", "#", ""));
                //  BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Download Photographs For Certificate", "Admin/DownloadImagesForCertificate.aspx", ""));
                btnView.Visible = false;
                btnReset.Visible = false;
                Lblerror.Text = "You can not download  Phase Detail Register Data.";
                Lblerror.Visible = true;
            }
            BreadCrumb1.Render();
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
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
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
        //try
        //{
        //    BreadCrumb1.Render();
        //    lblNoRecord.Text = "";
        //    int courseCatId = Convert.ToInt32(ddlCourseCategry.SelectedValue);
        //    using (var context = new EConnectContext())
        //    {
        //        ListItem lst = new ListItem("--Select One--", "0");
        //        if (courseCatId > 0)
        //        {

        //            var DataDownloadedSequence = from p in context.NSQFCertPhasePrintDetails
        //                                         join c in context.CourseMappingWithNSQFCoursecodes on p.course_code equals c.NSQFCourseCode
        //                                         join k in context.Courses on c.CourseID equals k.ID
        //                                         where k.CourseCategoryID == courseCatId && p.data_downloaded_sequence != null
        //                                         orderby (p.data_downloaded_sequence)
        //                                         select new { ValueField = p.data_downloaded_sequence, TextField = p.data_downloaded_sequence };

        //            DataDownloadedSequence = DataDownloadedSequence.Distinct().Take(20);
        //            DataDownloadedSequence = DataDownloadedSequence.Distinct().OrderByDescending(a => a.TextField);
        //            EConnect.Utils.Common.ControlUtility.BindListObject(ddlDataDownloadedSequence, DataDownloadedSequence, lst);
        //        }
        //        else
        //        {
        //            ddlDataDownloadedSequence.Items.Clear();
        //            ddlDataDownloadedSequence.Items.Insert(0, lst);
        //        }
        //        ddlDataDownloadedSequence.SelectedValue = "0";
        //        ddlDataDownloadedSequence_SelectedIndexChanged(ddlDataDownloadedSequence, EventArgs.Empty);
        //    };

        //}
        //catch (Exception ex)
        //{
        //    ShowAlert(ex.Message, true);
        //}
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
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("-- All --", "0");
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
            System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("-- All --", "0");
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
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
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
    private DataTable GetData(Int64 dataDownloadedSequenceId, string courseCode)
    {
        DataTable dt1 = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

        string sql = "SELECT DISTINCT CM.REGISTRATION_NO,C.NAME AS COURSE_NAME,PD.NAME,PD.F_NAME,PD.M_NAME, PD.PHASE ,PD.GRADE_CODE, PD.PERCENTAGE,PD.PHASE_DATE  from  NSQF_CERT_PHASE_PRINT_DETAIL PD,NSQF_MODULE_CANDIDATE_MARKS CM,COURSE C ,MODULE M where " +
                     "PD.REGISTRATION_NO = CM.REGISTRATION_NO AND C.ID = M.COURSE_ID AND  CM.COURSE_ID = M.COURSE_ID AND M.COURSE_CATEGORY_ID = 6 AND M.COURSE_ID > 102 AND CM.RESULT = 'PASS' AND PD.DATA_DOWNLOADED_SEQUENCE = '" + dataDownloadedSequenceId + "' AND PD.COURSE_CODE = '" + courseCode + "' ORDER BY REGISTRATION_NO";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt1);
                }
            }
        }

        return dt1;
    }
    private DataTable GetDataPhase(Int64 dataDownloadedSequenceId, string courseCode, Int64 pId)
    {
        DataTable dt2 = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        string sql = "SELECT DISTINCT CM.REGISTRATION_NO,C.NAME AS COURSE_NAME,PD.NAME,PD.inst_name,PD.F_NAME,PD.M_NAME,PD.G_NAME,PD.PHASE,PD.GRADE_CODE, PD.PERCENTAGE,PD.PHASE_DATE,ISNULL(can.Gender,'na') AS Gender from  NSQF_CERT_PHASE_PRINT_DETAIL PD,NSQF_MODULE_CANDIDATE_MARKS CM,COURSE C ,MODULE M,candidate can where " +
                     "PD.REGISTRATION_NO = CM.REGISTRATION_NO AND C.ID = M.COURSE_ID AND  CM.COURSE_ID = M.COURSE_ID AND M.COURSE_CATEGORY_ID = 6 and can.ID = cm.Candidate_id AND M.COURSE_ID > 102 AND CM.RESULT = 'PASS' AND PD.DATA_DOWNLOADED_SEQUENCE = '" + dataDownloadedSequenceId + "' AND PD.COURSE_CODE = '" + courseCode + "' AND PD.PHASE = '" + pId + "' ORDER BY REGISTRATION_NO";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt2);
                }
            }
        }

        return dt2;
    }
    private DataTable GetDatamodule(Int64 regno)
    {
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        string sql = " SELECT M.SHORT_NAME,M.NAME AS MOD_TYPE,CONVERT(VARCHAR,CM.EXAM_DATE,113) EXAM_DATE,CM.NSQF_ROLL_NO,CM.TOTAL_MARKS,CM.RESULT,PD.GRADE_CODE AS OVERALL_GRADE,PD.PERCENTAGE," +
                     " (SELECT ( CASE WHEN MODULE_TYPE = 1 THEN THEORYPAPERMAXMARKS WHEN MODULE_TYPE = 3 THEN PRACTICALPAPERMAXMARKS WHEN MODULE_TYPE IN (4,7,8) THEN PROJECT_PRESENTATION_ASSIGNMENTMAXMARKS WHEN MODULE_TYPE = 6" +
                     " THEN INTERNALASSESSMENTMAXMARKS WHEN MODULE_TYPE IN (9,10) THEN  MAJORPROJECT_DISSERTATIONMAXMARKS END ) FROM NSQFTOTALPAPERS WHERE NSQFLEARNHOURTYPE = MAP.NSQFLEARNHOURTYPE ) MAX_MARKS " +
                     " FROM MODULE M,NSQF_CERT_PHASE_PRINT_DETAIL PD,COURSEMAPPINGWITHNSQFCOURSECODE MAP,MODULE_TYPE MT,NSQF_MODULE_CANDIDATE_MARKS CM WHERE   m.ID = cm.Module_id and  m.Code= cm.Module_code and  M.MODULE_TYPE_ID = MT.ID " +
                     " AND M.COURSE_CATEGORY_ID = 6 AND M.COURSE_ID > 102 AND CM.COURSE_ID = M.COURSE_ID AND CM.COURSE_ID = MAP.COURSEID AND MAP.NSQFCOURSECODE = PD.COURSE_CODE AND PD.REGISTRATION_NO = CM.REGISTRATION_NO " +
                     " AND MT.ID = CM.MODULE_TYPE AND CM.RESULT = 'PASS' AND CM.REGISTRATION_NO = '" + regno + "' ORDER BY PD.PHASE";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
        }

        return dt;
    }
    private DataTable candidatesSummary(Int64 pId, String courseCode, Int64 dataDownloadedSequenceId)
    {
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        string sql = " SELECT (SELECT COUNT(*) FROM NSQF_CERT_PHASE_PRINT_DETAIL WHERE COURSE_CODE = '" + courseCode + "'  AND PHASE <=  '" + pId + "' AND (DATA_DOWNLOADED_SEQUENCE <= '" + dataDownloadedSequenceId + "' OR " +
                     " DATA_DOWNLOADED_SEQUENCE IS NULL) ) AS TOTAL_QUALIFIED_CANDIDATES,COUNT(*) TOTAL_QULAIFIED_CANDIDATES_PHASEWISE, " +
                     " SUM(CASE  GRADE_CODE  WHEN 'A' THEN 1 ELSE 0 END) CANDIDATES_WITH_GRADE_A,SUM(CASE GRADE_CODE WHEN 'B' THEN 1 ELSE 0 END) CANDIDATES_WITH_GRADE_B, " +
                     " SUM(CASE  GRADE_CODE  WHEN 'C' THEN 1 ELSE 0 END) CANDIDATES_WITH_GRADE_C,SUM(CASE GRADE_CODE WHEN 'D' THEN 1 ELSE 0 END) CANDIDATES_WITH_GRADE_D, " +
                     " SUM(CASE  GRADE_CODE  WHEN 'S' THEN 1 ELSE 0 END) CANDIDATES_WITH_GRADE_S FROM NSQF_CERT_PHASE_PRINT_DETAIL WHERE COURSE_CODE = '" + courseCode + "' " +
                     " AND PHASE = '" + pId + "' AND DATA_DOWNLOADED_SEQUENCE = '" + dataDownloadedSequenceId + "'";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
        }

        return dt;
    }
    private PdfPCell GetCell(Phrase phrase)
    {
        PdfPCell cell = new PdfPCell(phrase);
        cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //cell.PaddingBottom = 2f;
        cell.PaddingTop = 3f;
        //cell.BackgroundColor = new iTextSharp.text.BaseColor(60, 60, 60);
        //cell.HorizontalAlignment = 
        //cell.Width = Unit.Percentage(100);
        // cell.BorderWidth = PdfPCell.BOTTOM_BORDER;
        cell.VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED;
        return cell;
    }
    private DataTable candidateInfo(Int64 rId)
    {
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

        string sql = " SELECT REGISTRATION_NO,NAME,F_NAME,M_NAME,G_NAME  FROM NSQF_CERT_PHASE_PRINT_DETAIL WHERE REGISTRATION_NO = '" + rId + "'";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
        }

        return dt;
    }
    private PdfPCell GetCell1(Phrase phrase)
    {
        PdfPCell cell = new PdfPCell(phrase);
        cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //cell.PaddingBottom = 2f;
        cell.PaddingTop = 3f;
        //cell.BackgroundColor = new iTextSharp.text.BaseColor(60, 60, 60);
        //cell.HorizontalAlignment = 
        //cell.Width = Unit.Percentage(100);
        //cell.BorderWidth = PdfPCell.TOP_BORDER;
        cell.BorderWidthBottom = 0;
        cell.BorderWidthLeft = 0;
        cell.BorderWidthTop = 0;
        cell.BorderWidthRight = 0;
        cell.VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED;
        return cell;
    }
    private DataTable GetData(Int64 rId)
    {
        DataTable dt1 = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

        string sql = " SELECT (SELECT upper(name)  FROM MODULE WHERE ID = MODULE_ID) MOD_TYPE" +
                     ", substring ( CONVERT(VARCHAR,EXAM_DATE,113),4,8) EXAM_DATE  , NSQF_ROLL_NO , TOTAL_MARKS , 'PASS' RESULT " +
                     ", (SELECT ( CASE  WHEN  (SELECT MODULE_TYPE_ID  FROM MODULE WHERE ID = MODULE_ID) = 1 THEN THEORYPAPERMAXMARKS " +
                     " WHEN  (SELECT MODULE_TYPE_ID  FROM MODULE WHERE ID = MODULE_ID) = 3 THEN PRACTICALPAPERMAXMARKS" +
                     " WHEN  (SELECT MODULE_TYPE_ID  FROM MODULE WHERE ID = MODULE_ID) IN (4,7,8)  THEN PROJECT_PRESENTATION_ASSIGNMENTMAXMARKS " +
                     " WHEN  (SELECT MODULE_TYPE_ID  FROM MODULE WHERE ID = MODULE_ID) = 6 THEN INTERNALASSESSMENTMAXMARKS " +
                     " WHEN  (SELECT MODULE_TYPE_ID  FROM MODULE WHERE ID = MODULE_ID) IN (9,10) THEN  MAJORPROJECT_DISSERTATIONMAXMARKS END ) " +
                     "FROM NSQFTOTALPAPERS WHERE NSQFLEARNHOURTYPE =( SELECT NSQFLEARNHOURTYPE  FROM COURSEMAPPINGWITHNSQFCOURSECODE WHERE COURSEID =NSQF_MODULE_CANDIDATE_MARKS.COURSE_ID) ) MAX_MARKS " +
                     "FROM NSQF_MODULE_CANDIDATE_MARKS WHERE REGISTRATION_NO = '" + rId + "' AND RESULT ='PASS'  order by Module_type ";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt1);
                }
            }
        }

        return dt1;
    }
    private DataTable candidateInfo1(Int64 rId)
    {
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

        string sql = " SELECT DISTINCT upper(CO.NAME) as courseName, upper(p.name) as NAME, p.registration_no, upper(isnull(p.m_name,'xxxxxx')) as M_NAME, upper(isnull(F_NAME,'xxxxxx')) as F_NAME ,upper(isnull(G_NAME,'xxxxxx'))  as G_NAME " +
                     " FROM NSQF_CERT_PHASE_PRINT_DETAIL P, COURSEMAPPINGWITHNSQFCOURSECODE C ,COURSE CO WHERE C.COURSEID = CO.ID  AND P.COURSE_CODE= C.NSQFCOURSECODE AND  p.REGISTRATION_NO = '" + rId + "'";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
        }

        return dt;
    }
    public string get_qrcode(string encode_info, string file_name, string DirectoryName, Int64 dataDownloadedSequenceId, Int64 RegistrationId)
    {
        dataDownloadedSequenceId = Convert.ToInt32(ddlDataDownloadedSequence.SelectedValue);
        string barcode_path = Server.MapPath("~/Download/" + DirectoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId + "/" + RegistrationId + ".pdf");
        System.Drawing.Image image;
        var writer = new BarcodeWriter();
        writer.Format = BarcodeFormat.QR_CODE;
        var result = writer.Write(encode_info);
        image = new Bitmap(result);
        image.Save(Server.MapPath("~/Temp_Docs") + file_name, System.Drawing.Imaging.ImageFormat.Jpeg);
        return Server.MapPath("~/Temp_Docs") + file_name;
    }
    protected void CreateNSQFCertificate(long? RegistrationId, String DirectoryName)
    {
        try
        {
            Document doc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);

            // dataDownloadedSequenceId = 5;
            int dataDownloadedSequenceId = Convert.ToInt32(ddlDataDownloadedSequence.SelectedValue);
            PdfWriter writer =  PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Download/" + DirectoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId + "/" + RegistrationId + ".pdf"), FileMode.Create));
            //PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(@"\NSQF_Consolidated_Marksheets\" + DirectoryName + "\\DataDownloadedSequenceId_" + dataDownloadedSequenceId + "\\" + RegistrationId + ".pdf", FileMode.Create));          
            //writer.SetPdfVersion(PdfWriter.PDF_VERSION_1_5);
            writer.CompressionLevel = PdfStream.BEST_COMPRESSION;
            writer.SetFullCompression();

            doc.Open();

            if (dataDownloadedSequenceId != 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    var download = (from c in context.NSQFCertPhasePrintDetails
                                    where c.data_downloaded_sequence == dataDownloadedSequenceId && c.registration_no == RegistrationId
                                    select new
                                    {
                                        RegistrationNumber = c.registration_no,
                                        Name = c.Name,
                                        FatherName = c.f_name,
                                        MotherName = c.m_name,
                                        GuardianName = c.G_NAME,
                                        CourseCode = c.course_code,
                                        OverallGrade = c.grade_code,
                                        legends = c.grade_legends_desc

                                    }).FirstOrDefault();

                    if (download != null)
                    {

                        Int64 rId = Convert.ToInt64(download.RegistrationNumber);

                        string cName = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT DISTINCT upper(CO.NAME)  FROM NSQF_CERT_PHASE_PRINT_DETAIL P, COURSEMAPPINGWITHNSQFCOURSECODE C ,COURSE CO WHERE C.COURSEID = CO.ID  AND P.COURSE_CODE= C.NSQFCOURSECODE AND  p.REGISTRATION_NO = '" + rId + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));


                        Paragraph p5 = new Paragraph("* This is electronically generated Marksheet ,hence does not require signature. ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 7));
                        p5.Alignment = 1;
                        // p5.SetAlignment(doc.PageSize.Width - 375 - 0f);
                        // p5.Alignment = Element.ALIGN_TOP;
                        doc.Add(p5);


                        Paragraph sp = new Paragraph("\n", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                        sp.Alignment = Element.ALIGN_CENTER;
                        doc.Add(sp);

                        string imageURL = Server.MapPath("~/images/NielitLogoforPdf.jpg");
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageURL);

                        img.ScaleToFit(140f, 120f);
                        img.SpacingBefore = 100f;
                        img.Alignment = Element.ALIGN_CENTER;
                        doc.Add(img);

                        Paragraph para = new Paragraph(" NATIONAL INSTITUTE OF ELECTRONICS AND INFORMATION TECHNOLOGY ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13, iTextSharp.text.Font.BOLD));
                        para.Alignment = Element.ALIGN_CENTER;
                        doc.Add(para);

                        Paragraph para1 = new Paragraph(" (NIELIT Bhawan, Plot No 3, PSP Pocket, Institutional Area, Sector 8 Dwarka, South West Delhi, Delhi 110077) \n Website: nielit.gov.in ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.BOLDITALIC));
                        para1.Alignment = Element.ALIGN_CENTER;
                        doc.Add(para1);

                        Paragraph para2 = new Paragraph(" CONSOLIDATED MARKSHEET ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13, iTextSharp.text.Font.BOLD));
                        para2.Alignment = Element.ALIGN_CENTER;
                        doc.Add(para2);

                        doc.Add(Chunk.NEWLINE);

                        PdfPTable table1 = new PdfPTable(3);
                        table1.WidthPercentage = 60;
                        table1.HorizontalAlignment = Element.ALIGN_LEFT;
                        table1.WidthPercentage = 100;
                        table1.SetWidths(new float[] { 0.5F, 0.1F, 2f });

                        DataTable dtable1 = candidateInfo1(rId);
                        if (dtable1.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtable1.Rows)
                            {
                                table1.AddCell(GetCell1(new Phrase(new Chunk("Course Name", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk(":", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk(dr["courseName"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk("Registration Number", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk(":", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk(dr["REGISTRATION_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk("Candidate Name", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk(":", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk(dr["NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk("Father Name", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk(":", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk(dr["F_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk("Mother Name", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk(":", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk(dr["M_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk("Guardian Name", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk(":", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLD)))));
                                table1.AddCell(GetCell1(new Phrase(new Chunk(dr["G_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));

                            }
                        }
                        doc.Add(table1);
                        doc.Add(Chunk.NEWLINE);

                        PdfPTable table = new PdfPTable(5);
                        table.WidthPercentage = 100;
                        table.SetWidths(new float[] { 2, 0.8F, 0.6F, 0.6F, 0.6F });

                        table.AddCell("Subject");
                        table.AddCell("Roll No");
                        table.AddCell("Month / Year");
                        table.AddCell("Marks Obtained");
                        table.AddCell("Maximum Marks");

                        DataTable dt2 = GetData(rId);
                        if (dt2.Rows.Count > 0)
                        {
                            foreach (DataRow mod_row in dt2.Rows)
                            {
                                table.AddCell(new Phrase(new Chunk(mod_row["MOD_TYPE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                table.AddCell(new Phrase(new Chunk(mod_row["NSQF_ROLL_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                table.AddCell(new Phrase(new Chunk(mod_row["EXAM_DATE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                table.AddCell(new Phrase(new Chunk(mod_row["TOTAL_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                table.AddCell(new Phrase(new Chunk(mod_row["MAX_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            }

                        }

                        doc.Add(table);

                        doc.Add(Chunk.NEWLINE);
                        // doc.Add(Chunk.NEWLINE);

                        string marksObtained = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select sum(Total_Marks) Marks_Obtained from NSQF_Module_Candidate_marks  where Registration_no =  '" + rId + "'  and Result ='Pass' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                        string totalMarks = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT sum(( CASE WHEN MODULE_TYPE = 1 THEN THEORYPAPERMAXMARKS WHEN MODULE_TYPE = 3 THEN PRACTICALPAPERMAXMARKS WHEN MODULE_TYPE IN (4,7,8) THEN PROJECT_PRESENTATION_ASSIGNMENTMAXMARKS WHEN MODULE_TYPE = 6THEN INTERNALASSESSMENTMAXMARKS WHEN MODULE_TYPE IN (9,10) THEN  MAJORPROJECT_DISSERTATIONMAXMARKS END )) Total_Marks FROM NSQFTOTALPAPERS p,CourseMappingWithNSQFCoursecode t,NSQF_Module_Candidate_marks x where p.NSQFLearnHourType = t.NSQFLearnHourType and x.Course_id = t.CourseID   and  x.registration_no = '" + rId + "' and x.Result = 'Pass' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                        Paragraph p1 = new Paragraph(" (Total Marks Obtained) / (Total Maximum Marks) :  " + marksObtained + "/" + totalMarks + "                                                                                  Result : Pass ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11));
                        p1.Alignment = Element.ALIGN_LEFT;
                        doc.Add(p1);

                        string overallGrade = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select grade_code from NSQF_Cert_Phase_Print_Detail where registration_no =  '" + rId + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                        Paragraph p2 = new Paragraph(" Overall Grade  :   " + overallGrade + " \n ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11));
                        p2.Alignment = Element.ALIGN_LEFT;
                        doc.Add(p2);

                        iTextSharp.text.pdf.draw.LineSeparator l1 = new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 2);
                        doc.Add(l1);


                        Paragraph p4 = new Paragraph(" Grade Legends  :   " + download.legends + " \n ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11));
                        p4.Alignment = Element.ALIGN_LEFT;
                        doc.Add(p4);

                        string certificateCompletionDate = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller(" select  convert (varchar,Certificate_completion_date,113) From NSQF_Certificate_Phase_Master where Registration_no = '" + rId + "'", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                        Paragraph p3 = new Paragraph(" Place :  New Delhi                                                                                                                                                       \n  Date :  " + certificateCompletionDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
                        p3.Alignment = Element.ALIGN_LEFT;
                        doc.Add(p3);

                        StringBuilder hashdata = new StringBuilder();

                        iTextSharp.text.Image qrcode_jpg = iTextSharp.text.Image.GetInstance(
                                                           get_qrcode(
                                                           "Name :-" + download.Name + "\n" +
                                                           "RegistrationNumber :-" + download.RegistrationNumber + "\n" +
                                                           "CourseName :-" + cName + "\n" +
                                                           "FatherName :-" + download.FatherName + "\n" +
                                                           "MotherName :-" + download.MotherName + "\n" +
                                                           "GuardianName :-" + download.GuardianName + "\n" +
                                                           "OverallGrade :-" + download.OverallGrade + "\n" +
                                                           "Total :-" + marksObtained + "/" + totalMarks,
                                                           "BarCode.jpeg", "NSQFConsolidatedmarksheets", dataDownloadedSequenceId, rId));

                        qrcode_jpg.ScaleAbsolute(120f, 120f);
                        qrcode_jpg.Alignment = iTextSharp.text.Image.ALIGN_JUSTIFIED;
                        // qrcode_jpg.SetAbsolutePosition(70, 30);//(xPos, yPos)
                        qrcode_jpg.Alignment = Element.ALIGN_RIGHT;
                        qrcode_jpg.SpacingAfter = 0;
                        qrcode_jpg.SpacingBefore = 0;
                        doc.Add(qrcode_jpg);
                        hashdata.Clear();

                        table.FlushContent();

                    }

                    doc.Add(Chunk.NEWLINE);

                    doc.Close();
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void btnCertifcateDownload_Click(object sender, EventArgs e)
    {
        int CourseID = Convert.ToInt32(ddlCourseCategry.SelectedValue);//6
        int dataDownloadedSequenceId = Convert.ToInt32(ddlDataDownloadedSequence.SelectedValue);
        Int32 courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
        int phaseId = Convert.ToInt32(ddlPhaseNumber.SelectedValue);
        try
        {
           
            if (dataDownloadedSequenceId != 0 && courseId == 0 && phaseId == 0)
            {
                //ITA4STUP-WEBAP1\NSQF_Consolidated_Marksheets
                String path = Server.MapPath("~/Download/NSQFConsolidatedmarksheets/" + "DataDownloadedSequenceId_" + dataDownloadedSequenceId);
              
                if (Directory.Exists(path) == true)
                {
                    //ShowAlert("NSQF Certificate has already been downloaded.", true);
                }
                else
                {
                    using (EConnectContext context = new EConnectContext())
                    {
                        String directoryName = "NSQFConsolidatedmarksheets";

                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId));

                    var applications = (from c in context.NSQFCertPhasePrintDetails
                                            where c.data_downloaded_sequence == dataDownloadedSequenceId
                                            select new
                                            {
                                                RegistrationId = c.registration_no,
                                                ID = c.ID,
                                                CourseID = c.course_code
                                            }).ToList();

                        Lblerror.Text = "Total no of NSQF Certificate:- " + applications.Count().ToString();
                        if (applications.Count() <= 0)
                            return;

                        int applicationsCount = applications.Count();
                        //lblwait.Style.Add("Display","");
                        if (dataDownloadedSequenceId != 0)
                        {
                            for (int i = 0; i < applicationsCount; i++)
                            {
                                CreateNSQFCertificate(applications[i].RegistrationId, directoryName);
                            }
                        }
                        using (ZipFile zipFile = new ZipFile())
                        {
                            zipFile.AddDirectory(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId));
                            Response.Clear();
                            Response.ContentType = "application/zip";
                            Response.AddHeader("content-disposition", "filename=" + directoryName + "_DataDownloadedSequenceId_" + dataDownloadedSequenceId + ".zip");
                            zipFile.Save(Response.OutputStream);
                        }

                        System.IO.Directory.Delete(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId), true);
                    
                    }
                }
            }


            if (dataDownloadedSequenceId != 0 && courseId != 0 && phaseId == 0)
            {
                String path = Server.MapPath("~/Download/NSQFConsolidatedmarksheets/" + "DataDownloadedSequenceId_" + dataDownloadedSequenceId);
                //String path = @"\NSQF_Consolidated_Marksheets\NSQFConsolidatedmarksheets\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                if (Directory.Exists(path) == true)
                {
                    //ShowAlert("NSQF Certificate has already been downloaded.", true);
                }
                else
                {
                    using (EConnectContext context = new EConnectContext())
                    {

                        String directoryName = "NSQFConsolidatedmarksheets";

                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId));

                        var applications = (from c in context.NSQFCertPhasePrintDetails
                                            where c.data_downloaded_sequence == dataDownloadedSequenceId
                                            select new
                                            {
                                                RegistrationId = c.registration_no,
                                                ID = c.ID,
                                                CourseID = c.course_code
                                            }).ToList();

                        Lblerror.Text = "Total no of NSQF Certificate:- " + applications.Count().ToString();
                        if (applications.Count() <= 0)
                            return;

                        int applicationsCount = applications.Count();

                        if (dataDownloadedSequenceId != 0)
                        {
                            for (int i = 0; i < applicationsCount; i++)
                            {
                                CreateNSQFCertificate(applications[i].RegistrationId, directoryName);
                            }
                        }

                        using (ZipFile zipFile = new ZipFile())
                        {
                            zipFile.AddDirectory(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId));
                            Response.Clear();
                            Response.ContentType = "application/zip";
                            Response.AddHeader("content-disposition", "filename=" + directoryName + "_DataDownloadedSequenceId_" + dataDownloadedSequenceId + ".zip");
                            zipFile.Save(Response.OutputStream);
                        }
                        System.IO.Directory.Delete(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId), true);
               
                    }
                }
            }


            if (dataDownloadedSequenceId != 0 && courseId != 0 && phaseId != 0)
            {
                String path = Server.MapPath("~/Download/NSQFConsolidatedmarksheets/" + "DataDownloadedSequenceId_" + dataDownloadedSequenceId);
                //String path = @"\NSQF_Consolidated_Marksheets\\NSQFConsolidatedmarksheets\\DataDownloadedSequenceId_23" + dataDownloadedSequenceId;
                //String path = @"\NSQF_Consolidated_Marksheets\NSQFConsolidatedmarksheets\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                if (Directory.Exists(path) == true)
                {
                    //ShowAlert("NSQF Certificate has already been downloaded.", true);
                }
                else
                {
                    using (EConnectContext context = new EConnectContext())
                    {

                         String directoryName = "NSQFConsolidatedmarksheets";
                       
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId));
                        
                        var applications = (from c in context.NSQFCertPhasePrintDetails
                                            where c.data_downloaded_sequence == dataDownloadedSequenceId
                                            select new
                                            {
                                                RegistrationId = c.registration_no,
                                                ID = c.ID,
                                                CourseID = c.course_code
                                            }).ToList();

                        Lblerror.Text = "Total no of NSQF Certificate:- " + applications.Count().ToString();
                        if (applications.Count() <= 0)
                            return;

                        int applicationsCount = applications.Count();
                       
                 
                        if (dataDownloadedSequenceId != 0)
                        {
                            for (int i = 0; i < applicationsCount; i++)
                            {
                                CreateNSQFCertificate(applications[i].RegistrationId, directoryName);
                            }
                        }

                        using (ZipFile zipFile = new ZipFile())
                        {
                            zipFile.AddDirectory(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId));
                            Response.Clear();
                            Response.ContentType = "application/zip";
                            Response.AddHeader("content-disposition", "filename=" + directoryName + "_DataDownloadedSequenceId_" + dataDownloadedSequenceId + ".zip");
                            zipFile.Save(Response.OutputStream);
                        }
                       // ShowAlert("Downloading Finished!! Thank You");
                        System.IO.Directory.Delete(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId), true);
                
                    }
                }
            }
        }
        catch
        {
            //lblwait.Visible = false;
        }
    }

    // --start-- below this code added dated on 15102024 and developed by natasha mam
    protected void btnView_Click(object sender, EventArgs e)
    {
        int CourseID = Convert.ToInt32(ddlCourseCategry.SelectedValue);//6
        int dataDownloadedSequenceId = Convert.ToInt32(ddlDataDownloadedSequence.SelectedValue);
        Int32 courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
        int phaseId = Convert.ToInt32(ddlPhaseNumber.SelectedValue);
        Int32 Type = Convert.ToInt32(ddlType.SelectedValue);

        try
        {
            if (dataDownloadedSequenceId != 0 && courseId == 0 && phaseId == 0 && (Type == 0 || Type == 2))
            {
                //ITA4STUP-WEBAP1\NSQF_Consolidated_Marksheets
                //String path = Server.MapPath("~/Download/NSQFConsolidatedmarksheets/" + "DataDownloadedSequenceId_" + dataDownloadedSequenceId);
                String path = @"\NSQF_Consolidated_Marksheets\\NSQFConsolidatedmarksheets\\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                //String path = @"\NSQF_Consolidated_Marksheets\NSQFConsolidatedmarksheets\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                if (Directory.Exists(path) == true)
                {
                    //ShowAlert("NSQF Certificate has already been downloaded.", true);
                }
                else
                {
                    using (EConnectContext context = new EConnectContext())
                    {

                        String directoryName = "NSQFConsolidatedmarksheets";
                        //  System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId));
                        // var directorypath = @"\NSQF_Consolidated_Marksheets\" + directoryName + "\\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                        var directorypath = @"\NSQF_Consolidated_Marksheets\\" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId;


                        if (!Directory.Exists(directorypath))
                        {
                            Directory.CreateDirectory(directorypath);
                        }
                        var applications = (from c in context.NSQFCertPhasePrintDetails
                                            where c.data_downloaded_sequence == dataDownloadedSequenceId && c.Credits == null
                                            select new
                                            {
                                                RegistrationId = c.registration_no,
                                                ID = c.ID,
                                                CourseID = c.course_code
                                            }).ToList();

                        Lblerror.Text = "Total no of NSQF Certificate:- " + applications.Count().ToString();
                        if (applications.Count() <= 0)
                            return;

                        int applicationsCount = applications.Count();

                        if (dataDownloadedSequenceId != 0)
                        {
                            for (int i = 0; i < applicationsCount; i++)
                            {
                                CreateNSQFCertificate(applications[i].RegistrationId, directoryName);
                            }
                        }
                    }
                }

                Document doc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
                PdfWriter pdfWriter1 = PdfWriter.GetInstance(doc, Response.OutputStream);
                doc.Open();

                DataTable dt = new DataTable();
                string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                string sql = "SELECT DISTINCT COURSE_CODE,PHASE,CONVERT(VARCHAR,PHASE_DATE,103) PHASE_DATE FROM NSQF_CERT_PHASE_PRINT_DETAIL WHERE DATA_DOWNLOADED_SEQUENCE ='" + dataDownloadedSequenceId + "' GROUP BY PHASE,COURSE_CODE,PHASE_DATE ORDER BY PHASE";

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql))
                    {
                        cmd.Connection = conn;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }
                    }
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string phaseDate = Convert.ToString(dt.Rows[i]["PHASE_DATE"]);
                    Int64 pID = Convert.ToInt64(dt.Rows[i]["PHASE"]);
                    string courseCode = Convert.ToString(dt.Rows[i]["COURSE_CODE"]);
                    string cName = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT DISTINCT CO.NAME FROM NSQF_CERT_PHASE_PRINT_DETAIL P, COURSEMAPPINGWITHNSQFCOURSECODE C ,COURSE CO WHERE C.COURSEID = CO.ID  AND P.COURSE_CODE= C.NSQFCOURSECODE AND P.COURSE_CODE = '" + courseCode + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

                    Paragraph para = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                    para.Alignment = Element.ALIGN_CENTER;
                    doc.Add(para);
                    Paragraph para1 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + pID + "                 Course Name : " + courseCode + "  " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
                    para1.Alignment = Element.ALIGN_LEFT;
                    doc.Add(para1);
                    doc.Add(Chunk.NEWLINE);

                    PdfPTable table = new PdfPTable(14);
                    table.WidthPercentage = 90;
                    float[] width = { 0.8F, 0.8F, 0.8F, 0.8F, 0.8F, 1, 0.6F, 0.8F, 0.8F, 0.8F, 0.8F, 0.6F, 0.6F, 0.6F };
                    table.SetWidths(width);
                    //table.SetWidths(new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1 });
                    //table.AddCell("#");
                    table.AddCell("Reg No");
                    table.AddCell("Name");
                    table.AddCell("Father's Name");
                    table.AddCell("Mother's Name");
                    table.AddCell("Guardian Name");
                    table.AddCell("Institute Name");
                    table.AddCell("Gender");
                    table.AddCell("Module Code");
                    table.AddCell("Exam Date");
                    table.AddCell("Roll No");
                    table.AddCell("Total Marks");
                    table.AddCell("Max Marks");
                    table.AddCell("Grade");
                    table.AddCell("Avg Marks");
                    //int j = 0;
                    DataTable dt2 = GetDataPhase(dataDownloadedSequenceId, courseCode, pID);
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow mod_row in dt2.Rows)
                        {
                            //table.AddCell(GetCell(new Phrase(i + 1))); 
                            table.AddCell(GetCell(new Phrase(new Chunk(mod_row["REGISTRATION_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                            table.AddCell(new Phrase(new Chunk(mod_row["NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["F_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["M_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["G_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["inst_name"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["Gender"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));

                            DataTable dt_mod = GetDatamodule(Convert.ToInt64(mod_row["REGISTRATION_NO"]));
                            PdfPTable nested2 = new PdfPTable(1);
                            PdfPTable nested3 = new PdfPTable(1);
                            PdfPTable nested4 = new PdfPTable(1);
                            PdfPTable nested5 = new PdfPTable(1);
                            PdfPTable nested6 = new PdfPTable(1);

                            if (dt_mod.Rows.Count > 0)
                            {
                                foreach (DataRow module_row in dt_mod.Rows)
                                {
                                    nested2.AddCell(new Phrase(new Chunk(module_row["SHORT_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested3.AddCell(new Phrase(new Chunk(module_row["EXAM_DATE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested4.AddCell(new Phrase(new Chunk(module_row["NSQF_ROLL_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested5.AddCell(new Phrase(new Chunk(module_row["TOTAL_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested6.AddCell(new Phrase(new Chunk(module_row["MAX_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                }
                            }

                            table.AddCell(nested2);
                            table.AddCell(nested3);
                            table.AddCell(nested4);
                            table.AddCell(nested5);
                            table.AddCell(nested6);
                            table.AddCell(new Phrase(new Chunk(mod_row["GRADE_CODE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["PERCENTAGE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));

                        }

                        doc.Add(table);
                        table.FlushContent();
                        doc.NewPage();

                        Paragraph p1 = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                        p1.Alignment = Element.ALIGN_CENTER;
                        doc.Add(p1);
                        Paragraph p2 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + pID + "                 Course Name : " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
                        p2.Alignment = Element.ALIGN_LEFT;
                        doc.Add(p2);

                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);

                        PdfPTable tbl = new PdfPTable(1);
                        tbl.WidthPercentage = 60;
                        tbl.AddCell("                                                                Summary                                                       ");
                        
                        //DataTable dtable = candidatesSummary(pID, courseCode, dataDownloadedSequenceId);
                        DataTable dtable = candidatesSummary(pID, courseCode, dataDownloadedSequenceId);
                        if (dtable.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtable.Rows)
                            {
                                tbl.AddCell(GetCell(new Phrase(new Chunk("TQC(including this phase)                                         :                " + dr["TOTAL_QUALIFIED_CANDIDATES"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Total Candidates (NCVET + NON NCVET)                     :                " + dr["TOTAL_QUALIFIED_CANDIDATES_DOWNLOADSEQUENCE_WISE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Total Candidates in Phase (NON NCVET)                          :                " + dr["TOTAL_QULAIFIED_CANDIDATES_PHASEWISE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade A   (NON NCVET)                          :                " + dr["CANDIDATES_WITH_GRADE_A"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade B   (NON NCVET)                          :                " + dr["CANDIDATES_WITH_GRADE_B"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade C   (NON NCVET)                          :                " + dr["CANDIDATES_WITH_GRADE_C"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade D   (NON NCVET)                          :                " + dr["CANDIDATES_WITH_GRADE_D"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade S   (NON NCVET)                          :                " + dr["CANDIDATES_WITH_GRADE_S"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                            }
                        }

                        doc.Add(tbl);
                        tbl.FlushContent();

                        doc.NewPage();
                    }

                    else
                    {
                        Lblerror.Visible = true;
                        Lblerror.Text = "No record found in Non-NCVET format";
                        return;
                    }
                }
                doc.NewPage();

                doc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=PhaseDetailRegister.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(doc);
                Response.End();

            }

            if (dataDownloadedSequenceId != 0 && courseId == 0 && phaseId == 0 && Type == 1)
            {
                //ITA4STUP-WEBAP1\NSQF_Consolidated_Marksheets
                String path = Server.MapPath("~/Download/NSQFConsolidatedmarksheets/" + "DataDownloadedSequenceId_" + dataDownloadedSequenceId);
                //String path = @"\NSQF_Consolidated_Marksheets\\NSQFConsolidatedmarksheets\\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                //String path = @"\NSQF_Consolidated_Marksheets\NSQFConsolidatedmarksheets\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                if (Directory.Exists(path) == true)
                {
                    //ShowAlert("NSQF Certificate has already been downloaded.", true);
                }
                else
                {
                    using (EConnectContext context = new EConnectContext())
                    {

                        String directoryName = "NSQFConsolidatedmarksheets";
                        //  System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId));
                        // var directorypath = @"\NSQF_Consolidated_Marksheets\" + directoryName + "\\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                        var directorypath = @"\NSQF_Consolidated_Marksheets\\" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId;


                        if (!Directory.Exists(directorypath))
                        {
                            Directory.CreateDirectory(directorypath);
                        }
                        var applications = (from c in context.NSQFCertPhasePrintDetails
                                            where c.data_downloaded_sequence == dataDownloadedSequenceId && c.Credits != null
                                            select new
                                            {
                                                RegistrationId = c.registration_no,
                                                ID = c.ID,
                                                CourseID = c.course_code
                                            }).ToList();

                        Lblerror.Text = "Total no of NSQF Certificate:- " + applications.Count().ToString();
                        if (applications.Count() <= 0)
                            return;

                        int applicationsCount = applications.Count();

                        if (dataDownloadedSequenceId != 0)
                        {
                            for (int i = 0; i < applicationsCount; i++)
                            {
                                CreateNSQFCertificate(applications[i].RegistrationId, directoryName);
                            }
                        }
                    }
                }

                Document doc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
                PdfWriter pdfWriter1 = PdfWriter.GetInstance(doc, Response.OutputStream);
                doc.Open();

                DataTable dt = new DataTable();
                string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                string sql = "SELECT DISTINCT COURSE_CODE,PHASE,CONVERT(VARCHAR,PHASE_DATE,103) PHASE_DATE FROM NSQF_CERT_PHASE_PRINT_DETAIL WHERE DATA_DOWNLOADED_SEQUENCE ='" + dataDownloadedSequenceId + "' GROUP BY PHASE,COURSE_CODE,PHASE_DATE ORDER BY PHASE";

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql))
                    {
                        cmd.Connection = conn;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }
                    }
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string phaseDate = Convert.ToString(dt.Rows[i]["PHASE_DATE"]);
                    Int64 pID = Convert.ToInt64(dt.Rows[i]["PHASE"]);
                    string courseCode = Convert.ToString(dt.Rows[i]["COURSE_CODE"]);
                    string cName = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT DISTINCT CO.NAME FROM NSQF_CERT_PHASE_PRINT_DETAIL P, COURSEMAPPINGWITHNSQFCOURSECODE C ,COURSE CO WHERE C.COURSEID = CO.ID  AND P.COURSE_CODE= C.NSQFCOURSECODE AND P.COURSE_CODE = '" + courseCode + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

                    Paragraph para = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                    para.Alignment = Element.ALIGN_CENTER;
                    doc.Add(para);
                    Paragraph para1 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + pID + "                 Course Name : " + courseCode + "  " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
                    para1.Alignment = Element.ALIGN_LEFT;
                    doc.Add(para1);
                    doc.Add(Chunk.NEWLINE);

                    PdfPTable table = new PdfPTable(13);
                    table.WidthPercentage = 90;
                    float[] width = { 0.8F, 0.8F, 0.8F, 0.8F, 0.8F, 1, 0.6F, 0.8F, 0.8F, 0.8F, 0.8F, 0.6F, 0.6F };
                    table.SetWidths(width);
                    //table.SetWidths(new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1 });
                    //table.AddCell("#");
                    table.AddCell("Reg No");
                    table.AddCell("Name /Father Name /Mother Name/Guardian Name/ NCVET Qualcode/ NSQF Framework Level for Qualification/Course Duration(in Hrs)/ NCVET SrNo./Credits ");
                    //table.AddCell("Father's Name");
                    //table.AddCell("Mother's Name");
                    //table.AddCell("Guardian Name");
                    table.AddCell("Type(Module)");
                    table.AddCell("Module (Short Name)");
                    table.AddCell("Exam Date");
                    table.AddCell("Roll No");
                    table.AddCell("Total Marks");
                    table.AddCell("Grade");
                    table.AddCell("Sector Code");
                    table.AddCell("Final Grade");
                    table.AddCell("Overall Percentage");
                    table.AddCell("Centre Name/ District / State");
                    table.AddCell("Centre Address");

                    //int j = 0;
                    DataTable dt2 = GetCertificateDetailNCVET(dataDownloadedSequenceId);
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow mod_row in dt2.Rows)
                        {
                            //table.AddCell(GetCell(new Phrase(i + 1))); 
                            table.AddCell(GetCell(new Phrase(new Chunk(mod_row["REGISTRATION_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));

                            string fullName = mod_row["NAME"].ToString().Trim() + " / " + mod_row["F_NAME"].ToString().Trim() + " / " + mod_row["M_NAME"].ToString().Trim() + " / " + mod_row["G_NAME"].ToString().Trim()
                                              + " / " + mod_row["NCVETQUALCODE"].ToString().Trim() + " / " + mod_row["NSQF_FRAMWORK_LEVEL_FOR_QUALIFICATION"].ToString().Trim() + " / " + mod_row["COURSE_DURATION_FOR_QUALIFICATION_IN_HOURS"].ToString().Trim() + " / " + mod_row["NCVET_SR_NO"].ToString().Trim() + " / " + mod_row["Credits"].ToString().Trim();
                            table.AddCell(new Phrase(new Chunk(fullName, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));

                            DataTable dt_mod = GetNCVETCertificateModule(Convert.ToInt64(mod_row["REGISTRATION_NO"]));
                            PdfPTable nested2 = new PdfPTable(1);
                            PdfPTable nested3 = new PdfPTable(1);
                            PdfPTable nested4 = new PdfPTable(1);
                            PdfPTable nested5 = new PdfPTable(1);
                            PdfPTable nested6 = new PdfPTable(1);
                            PdfPTable nested7 = new PdfPTable(1);


                            if (dt_mod.Rows.Count > 0)
                            {
                                foreach (DataRow module_row in dt_mod.Rows)
                                {
                                    nested2.AddCell(new Phrase(new Chunk(module_row["Module_Type"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested3.AddCell(new Phrase(new Chunk(module_row["SHORT_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested4.AddCell(new Phrase(new Chunk(module_row["EXAM_DATE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested5.AddCell(new Phrase(new Chunk(module_row["NSQF_ROLL_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested6.AddCell(new Phrase(new Chunk(module_row["TOTAL_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested7.AddCell(new Phrase(new Chunk(module_row["OVERALL_GRADE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                }
                            }

                            table.AddCell(nested2);
                            table.AddCell(nested3);
                            table.AddCell(nested4);
                            table.AddCell(nested5);
                            table.AddCell(nested6);
                            table.AddCell(nested7);
                            table.AddCell(new Phrase(new Chunk(mod_row["SECTORSHORTCODE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["FINAL_GRADE_FOR_QUALIFICATION"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["OVERALL_PERCENTAGE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            string centreDetail = mod_row["INST_NAME"].ToString().Trim() + " / " + mod_row["TRANING_CENTER_NAME_DISTRICT"].ToString().Trim() + " / " + mod_row["TRANING_CENTER_NAME_STATE"].ToString().Trim();
                            table.AddCell(new Phrase(new Chunk(centreDetail, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["INST_ADDRESS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));

                        }

                        doc.Add(table);
                        table.FlushContent();
                        doc.NewPage();

                        Paragraph p1 = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                        p1.Alignment = Element.ALIGN_CENTER;
                        doc.Add(p1);
                        Paragraph p2 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + pID + "                 Course Name : " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
                        p2.Alignment = Element.ALIGN_LEFT;
                        doc.Add(p2);

                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);

                        PdfPTable tbl = new PdfPTable(1);
                        tbl.WidthPercentage = 60;
                        tbl.AddCell("                                                                Summary                                                       ");

                        DataTable dtable = candidatesSummaryNCVETD(pID, courseCode, dataDownloadedSequenceId);
                        if (dtable.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtable.Rows)
                            {
                                tbl.AddCell(GetCell(new Phrase(new Chunk("TQC(including this phase)                                     :                " + dr["TOTAL_QUALIFIED_CANDIDATES"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Total Candidates (NCVET + NON NCVET)                 :                " + dr["TOTAL_QUALIFIED_CANDIDATES_DOWNLOADSEQUENCE_WISE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Total Candidates in Phase  (NCVET)                          :                " + dr["TOTAL_QULAIFIED_CANDIDATES_PHASEWISE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade A    (NCVET)                          :                " + dr["CANDIDATES_WITH_GRADE_A"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade B    (NCVET)                          :                " + dr["CANDIDATES_WITH_GRADE_B"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade C    (NCVET)                          :                " + dr["CANDIDATES_WITH_GRADE_C"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade D    (NCVET)                          :                " + dr["CANDIDATES_WITH_GRADE_D"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade S    (NCVET)                          :                " + dr["CANDIDATES_WITH_GRADE_S"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                            }
                        }

                        doc.Add(tbl);
                        tbl.FlushContent();

                        doc.NewPage();
                    }

                    else
                    {
                        Lblerror.Visible = true;
                        Lblerror.Text = "No record found in NCVET format";                      
                    }
                }
                doc.NewPage();

                doc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=PhaseDetailRegister.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(doc);
                Response.End();

            }

            if (dataDownloadedSequenceId != 0 && courseId != 0 && phaseId == 0 && (Type == 0 || Type == 2))
            {

                String path = @"\NSQF_Consolidated_Marksheets\\NSQFConsolidatedmarksheets\\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                //String path = @"\NSQF_Consolidated_Marksheets\NSQFConsolidatedmarksheets\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                if (Directory.Exists(path) == true)
                {
                    //ShowAlert("NSQF Certificate has already been downloaded.", true);
                }
                else
                {
                    using (EConnectContext context = new EConnectContext())
                    {

                        String directoryName = "NSQFConsolidatedmarksheets";
                        //  System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId));
                        // var directorypath = @"\NSQF_Consolidated_Marksheets\" + directoryName + "\\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                        var directorypath = @"\NSQF_Consolidated_Marksheets\\" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId;

                        if (!Directory.Exists(directorypath))
                        {
                            Directory.CreateDirectory(directorypath);
                        }
                        var applications = (from c in context.NSQFCertPhasePrintDetails
                                            where c.data_downloaded_sequence == dataDownloadedSequenceId && c.Credits == null
                                            select new
                                            {
                                                RegistrationId = c.registration_no,
                                                ID = c.ID,
                                                CourseID = c.course_code
                                            }).ToList();

                        Lblerror.Text = "Total no of NSQF Certificate:- " + applications.Count().ToString();
                        if (applications.Count() <= 0)
                            return;

                        int applicationsCount = applications.Count();

                        if (dataDownloadedSequenceId != 0)
                        {
                            for (int i = 0; i < applicationsCount; i++)
                            {
                                CreateNSQFCertificate(applications[i].RegistrationId, directoryName);
                            }
                        }
                    }
                }


                Document doc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
                PdfWriter pdfWriter = PdfWriter.GetInstance(doc, Response.OutputStream);
                doc.Open();

                DataTable dt = new DataTable();
                string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

                string sql = "SELECT DISTINCT COURSE_CODE ,PD.PHASE,CONVERT(VARCHAR,PD.PHASE_DATE,103) PHASE_DATE FROM NSQF_CERT_PHASE_PRINT_DETAIL PD,COURSEMAPPINGWITHNSQFCOURSECODE MAP,COURSE C where pd.course_code  =  map.NSQFCourseCode  and map.CourseID  =  c.ID AND PD.DATA_DOWNLOADED_SEQUENCE = '" + dataDownloadedSequenceId + "' and c.ID =  '" + courseId + "'";

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql))
                    {
                        cmd.Connection = conn;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }
                    }
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string phaseDate = Convert.ToString(dt.Rows[i]["PHASE_DATE"]);
                    Int64 pID = Convert.ToInt64(dt.Rows[i]["PHASE"]);
                    string courseCode = Convert.ToString(dt.Rows[i]["COURSE_CODE"]);
                    string cName = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT DISTINCT CO.NAME FROM NSQF_CERT_PHASE_PRINT_DETAIL P, COURSEMAPPINGWITHNSQFCOURSECODE C ,COURSE CO WHERE C.COURSEID = CO.ID  AND P.COURSE_CODE= C.NSQFCOURSECODE AND P.COURSE_CODE = '" + courseCode + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

                    Paragraph para = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                    para.Alignment = Element.ALIGN_CENTER;
                    doc.Add(para);
                    Paragraph para1 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + pID + "                 Course Name :" + courseCode + "   " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
                    para1.Alignment = Element.ALIGN_LEFT;
                    doc.Add(para1);
                    doc.Add(Chunk.NEWLINE);

                    PdfPTable table = new PdfPTable(14);
                    table.WidthPercentage = 100;
                    float[] width = { 0.8F, 0.8F, 0.8F, 0.8F, 0.8F, 1, 0.6F, 0.8F, 0.8F, 0.8F, 0.8F, 0.6F, 0.6F, 0.6F };
                    //{ 0.8F, 0.8F, 1, 1, 1, 0.8F, 1, 1, 1.2F, 0.8F, 0.8F, 0.6F,0.6F, 0.7F };
                    table.SetWidths(width);
                    //table.SetWidths(new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1 });
                    table.AddCell("Reg No");
                    table.AddCell("Name");
                    table.AddCell("Father's Name");
                    table.AddCell("Mother's Name");
                    table.AddCell("Guardian Name");
                    table.AddCell("Institute Name");
                    table.AddCell("Gender");
                    table.AddCell("Module Code");
                    table.AddCell("Exam Date");
                    table.AddCell("Roll No");
                    table.AddCell("Total Marks");
                    table.AddCell("Max Marks");
                    table.AddCell("Grade");
                    table.AddCell("Avg Marks");

                    DataTable dt1 = GetDataPhase(dataDownloadedSequenceId, courseCode, pID);

                    if (dt1.Rows.Count > 0)
                    {
                        foreach (DataRow mod_row in dt1.Rows)
                        {
                            table.AddCell(GetCell(new Phrase(new Chunk(mod_row["REGISTRATION_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                            table.AddCell(new Phrase(new Chunk(mod_row["NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["F_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["M_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["G_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["inst_name"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["Gender"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));

                            DataTable dt_mod = GetDatamodule(Convert.ToInt64(mod_row["REGISTRATION_NO"]));
                            PdfPTable nested2 = new PdfPTable(1);
                            PdfPTable nested3 = new PdfPTable(1);
                            PdfPTable nested4 = new PdfPTable(1);
                            PdfPTable nested5 = new PdfPTable(1);
                            PdfPTable nested6 = new PdfPTable(1);

                            if (dt_mod.Rows.Count > 0)
                            {
                                foreach (DataRow module_row in dt_mod.Rows)
                                {
                                    nested2.AddCell(new Phrase(new Chunk(module_row["SHORT_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested3.AddCell(new Phrase(new Chunk(module_row["EXAM_DATE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested4.AddCell(new Phrase(new Chunk(module_row["NSQF_ROLL_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested5.AddCell(new Phrase(new Chunk(module_row["TOTAL_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested6.AddCell(new Phrase(new Chunk(module_row["MAX_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                }
                            }

                            table.AddCell(nested2);
                            table.AddCell(nested3);
                            table.AddCell(nested4);
                            table.AddCell(nested5);
                            table.AddCell(nested6);
                            table.AddCell(new Phrase(new Chunk(mod_row["GRADE_CODE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["PERCENTAGE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                        }

                        doc.Add(table);
                        table.FlushContent();

                        doc.NewPage();

                        Paragraph p1 = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                        p1.Alignment = Element.ALIGN_CENTER;
                        doc.Add(p1);
                        Paragraph p2 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + pID + "                 Course Name : " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
                        p2.Alignment = Element.ALIGN_LEFT;
                        doc.Add(p2);

                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);

                        PdfPTable tbl = new PdfPTable(1);
                        tbl.WidthPercentage = 60;
                        tbl.AddCell("                                                                Summary                                                       ");

                        //--DataTable dtable = candidatesSummary(pID, courseCode, dataDownloadedSequenceId);
                        DataTable dtable = candidatesSummaryNCVETD(pID, courseCode, dataDownloadedSequenceId);
                        if (dtable.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtable.Rows)
                            {
                                tbl.AddCell(GetCell(new Phrase(new Chunk("TQC(including this phase)                                    :                " + dr["TOTAL_QUALIFIED_CANDIDATES"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Total Candidates (NCVET + NON NCVET)               :                " + dr["TOTAL_QUALIFIED_CANDIDATES_DOWNLOADSEQUENCE_WISE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Total Candidates in Phase    (NON NCVET)                   :                " + dr["TOTAL_QULAIFIED_CANDIDATES_PHASEWISE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade A      (NON NCVET)                   :                " + dr["CANDIDATES_WITH_GRADE_A"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade B      (NON NCVET)                   :                " + dr["CANDIDATES_WITH_GRADE_B"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade C      (NON NCVET)                   :                " + dr["CANDIDATES_WITH_GRADE_C"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade D      (NON NCVET)                   :                " + dr["CANDIDATES_WITH_GRADE_D"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade S      (NON NCVET)                   :                " + dr["CANDIDATES_WITH_GRADE_S"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                            }
                        }

                        doc.Add(tbl);
                        tbl.FlushContent();

                        doc.NewPage();
                    }

                    else
                    {
                        Lblerror.Visible = true;
                        Lblerror.Text = "No record found in Non-NCVET format";
                        return;
                    }

                }

                doc.NewPage();

                doc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=PhaseDetailRegister.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(doc);
                Response.End();
            }


            if (dataDownloadedSequenceId != 0 && courseId != 0 && phaseId == 0 && Type == 1)
            {

                String path = @"\NSQF_Consolidated_Marksheets\\NSQFConsolidatedmarksheets\\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                //String path = @"\NSQF_Consolidated_Marksheets\NSQFConsolidatedmarksheets\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                if (Directory.Exists(path) == true)
                {
                    //ShowAlert("NSQF Certificate has already been downloaded.", true);
                }
                else
                {
                    using (EConnectContext context = new EConnectContext())
                    {

                        String directoryName = "NSQFConsolidatedmarksheets";
                        //  System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId));
                        // var directorypath = @"\NSQF_Consolidated_Marksheets\" + directoryName + "\\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                        var directorypath = @"\NSQF_Consolidated_Marksheets\\" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId;

                        if (!Directory.Exists(directorypath))
                        {
                            Directory.CreateDirectory(directorypath);
                        }
                        var applications = (from c in context.NSQFCertPhasePrintDetails
                                            where c.data_downloaded_sequence == dataDownloadedSequenceId && c.Credits != null
                                            select new
                                            {
                                                RegistrationId = c.registration_no,
                                                ID = c.ID,
                                                CourseID = c.course_code
                                            }).ToList();

                        Lblerror.Text = "Total no of NSQF Certificate:- " + applications.Count().ToString();
                        if (applications.Count() <= 0)
                            return;

                        int applicationsCount = applications.Count();

                        if (dataDownloadedSequenceId != 0)
                        {
                            for (int i = 0; i < applicationsCount; i++)
                            {
                                CreateNSQFCertificate(applications[i].RegistrationId, directoryName);
                            }
                        }
                    }
                }


                Document doc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
                PdfWriter pdfWriter = PdfWriter.GetInstance(doc, Response.OutputStream);
                doc.Open();

                DataTable dt = new DataTable();
                string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                string sql = "SELECT DISTINCT COURSE_CODE ,PD.PHASE,CONVERT(VARCHAR,PD.PHASE_DATE,103) PHASE_DATE FROM NSQF_CERT_PHASE_PRINT_DETAIL PD,COURSEMAPPINGWITHNSQFCOURSECODE MAP,COURSE C where pd.course_code  =  map.NSQFCourseCode  and map.CourseID  =  c.ID AND PD.DATA_DOWNLOADED_SEQUENCE = '" + dataDownloadedSequenceId + "' and c.ID =  '" + courseId + "'";
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql))
                    {
                        cmd.Connection = conn;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }
                    }
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string phaseDate = Convert.ToString(dt.Rows[i]["PHASE_DATE"]);
                    Int64 pID = Convert.ToInt64(dt.Rows[i]["PHASE"]);
                    string courseCode = Convert.ToString(dt.Rows[i]["COURSE_CODE"]);
                    string cName = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT DISTINCT CO.NAME FROM NSQF_CERT_PHASE_PRINT_DETAIL P, COURSEMAPPINGWITHNSQFCOURSECODE C ,COURSE CO WHERE C.COURSEID = CO.ID  AND P.COURSE_CODE= C.NSQFCOURSECODE AND P.COURSE_CODE = '" + courseCode + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

                    Paragraph para = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                    para.Alignment = Element.ALIGN_CENTER;
                    doc.Add(para);
                    Paragraph para1 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + pID + "                 Course Name :" + courseCode + "   " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
                    para1.Alignment = Element.ALIGN_LEFT;
                    doc.Add(para1);
                    doc.Add(Chunk.NEWLINE);

                    PdfPTable table = new PdfPTable(13);
                    table.WidthPercentage = 90;
                    float[] width = { 0.8F, 0.8F, 0.8F, 0.8F, 0.8F, 1, 0.6F, 0.8F, 0.8F, 0.8F, 0.8F, 0.6F, 0.6F };
                    table.SetWidths(width);
                    //table.SetWidths(new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1 });
                    //table.AddCell("#");
                    table.AddCell("Reg No");
                    table.AddCell("Name /Father Name /Mother Name/Guardian Name/ NCVET Qualcode/ NSQF Framework Level for Qualification /Course Duration(in  Hrs)/ NCVET SrNo./ Credits");
                    //table.AddCell("Father's Name");
                    //table.AddCell("Mother's Name");
                    //table.AddCell("Guardian Name");

                    table.AddCell("Type(Module)");
                    table.AddCell("Module (Short Name)");
                    table.AddCell("Exam Date");
                    table.AddCell("Roll No");
                    table.AddCell("Total Marks");
                    table.AddCell("Grade");
                    table.AddCell("Sector Code");
                    table.AddCell("Final Grade");
                    table.AddCell("Overall Percentage");
                    table.AddCell("Centre Name/ District / State");
                    table.AddCell("Centre Address");

                    //int j = 0;
                    DataTable dt2 = GetCertificateDetailNCVETC(dataDownloadedSequenceId, courseCode);
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow mod_row in dt2.Rows)
                        {
                            //table.AddCell(GetCell(new Phrase(i + 1))); 
                            table.AddCell(GetCell(new Phrase(new Chunk(mod_row["REGISTRATION_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                            string fullName = mod_row["NAME"].ToString().Trim() + " / " + mod_row["F_NAME"].ToString().Trim() + " / " + mod_row["M_NAME"].ToString().Trim() + " / " + mod_row["G_NAME"].ToString().Trim()
                                             + " / " + mod_row["NCVETQUALCODE"].ToString().Trim() + " / " + mod_row["NSQF_FRAMWORK_LEVEL_FOR_QUALIFICATION"].ToString().Trim() + " / " + mod_row["COURSE_DURATION_FOR_QUALIFICATION_IN_HOURS"].ToString().Trim() + " / " + mod_row["NCVET_SR_NO"].ToString().Trim() + " / " + mod_row["Credits"].ToString().Trim();
                            table.AddCell(new Phrase(new Chunk(fullName, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));

                            DataTable dt_mod = GetNCVETCertificateModule(Convert.ToInt64(mod_row["REGISTRATION_NO"]));
                            PdfPTable nested2 = new PdfPTable(1);
                            PdfPTable nested3 = new PdfPTable(1);
                            PdfPTable nested4 = new PdfPTable(1);
                            PdfPTable nested5 = new PdfPTable(1);
                            PdfPTable nested6 = new PdfPTable(1);
                            PdfPTable nested7 = new PdfPTable(1);


                            if (dt_mod.Rows.Count > 0)
                            {
                                foreach (DataRow module_row in dt_mod.Rows)
                                {
                                    nested2.AddCell(new Phrase(new Chunk(module_row["Module_Type"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested3.AddCell(new Phrase(new Chunk(module_row["SHORT_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested4.AddCell(new Phrase(new Chunk(module_row["EXAM_DATE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested5.AddCell(new Phrase(new Chunk(module_row["NSQF_ROLL_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested6.AddCell(new Phrase(new Chunk(module_row["TOTAL_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested7.AddCell(new Phrase(new Chunk(module_row["OVERALL_GRADE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                }
                            }

                            table.AddCell(nested2);
                            table.AddCell(nested3);
                            table.AddCell(nested4);
                            table.AddCell(nested5);
                            table.AddCell(nested6);
                            table.AddCell(nested7);
                            table.AddCell(new Phrase(new Chunk(mod_row["SECTORSHORTCODE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["FINAL_GRADE_FOR_QUALIFICATION"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["OVERALL_PERCENTAGE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            string centreDetail = mod_row["INST_NAME"].ToString().Trim() + " / " + mod_row["TRANING_CENTER_NAME_DISTRICT"].ToString().Trim() + " / " + mod_row["TRANING_CENTER_NAME_STATE"].ToString().Trim();
                            table.AddCell(new Phrase(new Chunk(centreDetail, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));

                            table.AddCell(new Phrase(new Chunk(mod_row["INST_ADDRESS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));

                            // table.AddCell(new Phrase(new Chunk(mod_row["OVERALL_PERCENTAGE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                        }

                        doc.Add(table);
                        table.FlushContent();

                        doc.NewPage();

                        Paragraph p1 = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                        p1.Alignment = Element.ALIGN_CENTER;
                        doc.Add(p1);
                        Paragraph p2 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + pID + "                 Course Name : " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
                        p2.Alignment = Element.ALIGN_LEFT;
                        doc.Add(p2);

                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);

                        PdfPTable tbl = new PdfPTable(1);
                        tbl.WidthPercentage = 60;
                        tbl.AddCell("                                                                Summary                                                       ");

                        DataTable dtable = candidatesSummaryNCVETD(pID, courseCode, dataDownloadedSequenceId);
                        if (dtable.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtable.Rows)
                            {
                                tbl.AddCell(GetCell(new Phrase(new Chunk("TQC(including this phase)                                        :                " + dr["TOTAL_QUALIFIED_CANDIDATES"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Total Candidates (NCVET + NON NCVET)                    :                " + dr["TOTAL_QUALIFIED_CANDIDATES_DOWNLOADSEQUENCE_WISE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Total Candidates in Phase  (NCVET)                             :                " + dr["TOTAL_QULAIFIED_CANDIDATES_PHASEWISE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade A    (NCVET)                             :                " + dr["CANDIDATES_WITH_GRADE_A"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade B    (NCVET)                             :                " + dr["CANDIDATES_WITH_GRADE_B"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade C    (NCVET)                             :                " + dr["CANDIDATES_WITH_GRADE_C"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade D    (NCVET)                             :                " + dr["CANDIDATES_WITH_GRADE_D"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade S    (NCVET)                             :                " + dr["CANDIDATES_WITH_GRADE_S"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                            }
                        }

                        doc.Add(tbl);
                        tbl.FlushContent();

                        doc.NewPage();
                    }

                    else
                    {
                        Lblerror.Visible = true;
                        Lblerror.Text = "No record found in NCVET format";
                    }

                }

                doc.NewPage();

                doc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=PhaseDetailRegister.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(doc);
                Response.End();
            }

            if (dataDownloadedSequenceId != 0 && courseId != 0 && phaseId != 0 && (Type == 0 || Type == 2))
            {
                String path = @"\NSQF_Consolidated_Marksheets\\NSQFConsolidatedmarksheets\\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                //String path = @"\NSQF_Consolidated_Marksheets\NSQFConsolidatedmarksheets\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                if (Directory.Exists(path) == true)
                {
                    //ShowAlert("NSQF Certificate has already been downloaded.", true);
                }
                else
                {
                    using (EConnectContext context = new EConnectContext())
                    {

                        String directoryName = "NSQFConsolidatedmarksheets";
                        //  System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId));
                        // var directorypath = @"\NSQF_Consolidated_Marksheets\" + directoryName + "\\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                        var directorypath = @"\NSQF_Consolidated_Marksheets\\" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId;


                        if (!Directory.Exists(directorypath))
                        {
                            Directory.CreateDirectory(directorypath);
                        }
                        var applications = (from c in context.NSQFCertPhasePrintDetails
                                            where c.data_downloaded_sequence == dataDownloadedSequenceId && c.Credits == null
                                            select new
                                            {
                                                RegistrationId = c.registration_no,
                                                ID = c.ID,
                                                CourseID = c.course_code
                                            }).ToList();

                        Lblerror.Text = "Total no of NSQF Certificate:- " + applications.Count().ToString();
                        if (applications.Count() <= 0)
                            return;

                        int applicationsCount = applications.Count();

                        if (dataDownloadedSequenceId != 0)
                        {
                            for (int i = 0; i < applicationsCount; i++)
                            {
                                CreateNSQFCertificate(applications[i].RegistrationId, directoryName);
                            }
                        }
                    }
                }

                Document doc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
                PdfWriter pdfWriter = PdfWriter.GetInstance(doc, Response.OutputStream);
                doc.Open();

                DataTable dt1 = new DataTable();
                string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

                string sql = " SELECT DISTINCT COURSE_CODE ,PD.PHASE,CONVERT(VARCHAR,PD.PHASE_DATE,103) PHASE_DATE FROM NSQF_CERT_PHASE_PRINT_DETAIL PD,COURSEMAPPINGWITHNSQFCOURSECODE MAP,COURSE C where pd.course_code  =  map.NSQFCourseCode  and map.CourseID  =  c.ID AND PD.DATA_DOWNLOADED_SEQUENCE = '" + dataDownloadedSequenceId + "'  and c.ID = '" + courseId + "'  and phase =  '" + phaseId + "'";

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql))
                    {
                        cmd.Connection = conn;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt1);
                        }
                    }
                }
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    string phaseDate = Convert.ToString(dt1.Rows[i]["PHASE_DATE"]);
                    string courseCode = Convert.ToString(dt1.Rows[i]["COURSE_CODE"]);
                    string cName = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT DISTINCT CO.NAME FROM NSQF_CERT_PHASE_PRINT_DETAIL P, COURSEMAPPINGWITHNSQFCOURSECODE C ,COURSE CO WHERE C.COURSEID = CO.ID  AND P.COURSE_CODE= C.NSQFCOURSECODE AND P.COURSE_CODE = '" + courseCode + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

                    Paragraph para = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                    para.Alignment = Element.ALIGN_CENTER;
                    doc.Add(para);
                    Paragraph para1 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + phaseId + "                 Course Name :" + courseCode + "   " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
                    para1.Alignment = Element.ALIGN_LEFT;
                    doc.Add(para1);
                    doc.Add(Chunk.NEWLINE);

                    PdfPTable table = new PdfPTable(14);
                    table.WidthPercentage = 100;
                    float[] width = { 0.8F, 0.8F, 0.8F, 0.8F, 0.8F, 1, 0.6F, 0.8F, 0.8F, 0.8F, 0.8F, 0.6F, 0.6F, 0.6F };
                    //{ 0.8F, 0.8F, 1, 1, 1, 0.8F, 1, 1, 1.2F, 0.8F, 0.8F, 0.6F,0.6F, 0.7F };
                    table.SetWidths(width);
                    //table.SetWidths(new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1 });
                    table.AddCell("Reg No");
                    table.AddCell("Name");
                    table.AddCell("Father's Name");
                    table.AddCell("Mother's Name");
                    table.AddCell("Guardian Name");
                    table.AddCell("Institute Name");
                    table.AddCell("Gender");
                    table.AddCell("Module Code");
                    table.AddCell("Exam Date");
                    table.AddCell("Roll No");
                    table.AddCell("Total Marks");
                    table.AddCell("Max Marks");
                    table.AddCell("Grade");
                    table.AddCell("Avg Marks");

                    DataTable dt = GetDataPhase(dataDownloadedSequenceId, courseCode, phaseId);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow mod_row in dt.Rows)
                        {
                            table.AddCell(GetCell(new Phrase(new Chunk(mod_row["REGISTRATION_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                            table.AddCell(new Phrase(new Chunk(mod_row["NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["F_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["M_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["G_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["inst_name"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["Gender"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));

                            DataTable dt_mod = GetDatamodule(Convert.ToInt64(mod_row["registration_no"]));
                            PdfPTable nested2 = new PdfPTable(1);
                            PdfPTable nested3 = new PdfPTable(1);
                            PdfPTable nested4 = new PdfPTable(1);
                            PdfPTable nested5 = new PdfPTable(1);
                            PdfPTable nested6 = new PdfPTable(1);

                            if (dt_mod.Rows.Count > 0)
                            {
                                foreach (DataRow module_row in dt_mod.Rows)
                                {
                                    nested2.AddCell(new Phrase(new Chunk(module_row["SHORT_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested3.AddCell(new Phrase(new Chunk(module_row["EXAM_DATE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested4.AddCell(new Phrase(new Chunk(module_row["NSQF_ROLL_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested5.AddCell(new Phrase(new Chunk(module_row["TOTAL_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested6.AddCell(new Phrase(new Chunk(module_row["MAX_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                }
                            }

                            table.AddCell(nested2);
                            table.AddCell(nested3);
                            table.AddCell(nested4);
                            table.AddCell(nested5);
                            table.AddCell(nested6);
                            table.AddCell(new Phrase(new Chunk(mod_row["GRADE_CODE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["PERCENTAGE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                        }

                        doc.Add(table);
                        table.FlushContent();

                        doc.NewPage();

                        Paragraph p1 = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                        p1.Alignment = Element.ALIGN_CENTER;
                        doc.Add(p1);
                        Paragraph p2 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + phaseId + "                 Course Name : " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
                        p2.Alignment = Element.ALIGN_LEFT;
                        doc.Add(p2);

                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);

                        PdfPTable tbl = new PdfPTable(1);
                        tbl.WidthPercentage = 60;
                        tbl.AddCell("                                                                Summary                                                       ");

                       //-- DataTable dtable = candidatesSummary(phaseId, courseCode, dataDownloadedSequenceId);
                        DataTable dtable = candidatesSummaryNCVETD(phaseId, courseCode, dataDownloadedSequenceId);
                        if (dtable.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtable.Rows)
                            {
                                tbl.AddCell(GetCell(new Phrase(new Chunk("TQC(including this phase)                                        :                " + dr["TOTAL_QUALIFIED_CANDIDATES"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Total Candidates (NCVET + NON NCVET)                    :                " + dr["TOTAL_QUALIFIED_CANDIDATES_DOWNLOADSEQUENCE_WISE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Total Candidates in Phase    (NON NCVET)                       :                " + dr["TOTAL_QULAIFIED_CANDIDATES_PHASEWISE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade A      (NON NCVET)                       :                " + dr["CANDIDATES_WITH_GRADE_A"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade B      (NON NCVET)                       :                " + dr["CANDIDATES_WITH_GRADE_B"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade C      (NON NCVET)                       :                " + dr["CANDIDATES_WITH_GRADE_C"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade D      (NON NCVET)                       :                " + dr["CANDIDATES_WITH_GRADE_D"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade S      (NON NCVET)                       :                " + dr["CANDIDATES_WITH_GRADE_S"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                            }
                        }

                        doc.Add(tbl);
                        tbl.FlushContent();

                        doc.NewPage();
                    }

                    else
                    {
                        Lblerror.Visible = true;
                        Lblerror.Text = "No record found in Non-NCVET format";
                        return;
                    }
                }

                doc.NewPage();

                doc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=PhaseDetailRegister.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(doc);
                Response.End();
            }

            if (dataDownloadedSequenceId != 0 && courseId != 0 && phaseId != 0 && Type == 1)
            {
                String path = @"\NSQF_Consolidated_Marksheets\\NSQFConsolidatedmarksheets\\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                //String path = @"\NSQF_Consolidated_Marksheets\NSQFConsolidatedmarksheets\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                if (Directory.Exists(path) == true)
                {
                    //ShowAlert("NSQF Certificate has already been downloaded.", true);
                }
                else
                {
                    using (EConnectContext context = new EConnectContext())
                    {

                        String directoryName = "NSQFConsolidatedmarksheets";
                        //  System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId));
                        // var directorypath = @"\NSQF_Consolidated_Marksheets\" + directoryName + "\\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
                        var directorypath = @"\NSQF_Consolidated_Marksheets\\" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId;

                        if (!Directory.Exists(directorypath))
                        {
                            Directory.CreateDirectory(directorypath);
                        }
                        var applications = (from c in context.NSQFCertPhasePrintDetails
                                            where c.data_downloaded_sequence == dataDownloadedSequenceId && c.Credits != null
                                            select new
                                            {
                                                RegistrationId = c.registration_no,
                                                ID = c.ID,
                                                CourseID = c.course_code
                                            }).ToList();

                        Lblerror.Text = "Total no of NSQF Certificate:- " + applications.Count().ToString();
                        if (applications.Count() <= 0)
                            return;

                        int applicationsCount = applications.Count();

                        if (dataDownloadedSequenceId != 0)
                        {
                            for (int i = 0; i < applicationsCount; i++)
                            {
                                CreateNSQFCertificate(applications[i].RegistrationId, directoryName);
                            }
                        }
                    }
                }

                Document doc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
                PdfWriter pdfWriter = PdfWriter.GetInstance(doc, Response.OutputStream);
                doc.Open();

                DataTable dt1 = new DataTable();
                string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                string sql = " SELECT DISTINCT COURSE_CODE ,PD.PHASE,CONVERT(VARCHAR,PD.PHASE_DATE,103) PHASE_DATE FROM NSQF_CERT_PHASE_PRINT_DETAIL PD,COURSEMAPPINGWITHNSQFCOURSECODE MAP,COURSE C where pd.course_code  =  map.NSQFCourseCode  and map.CourseID  =  c.ID AND PD.DATA_DOWNLOADED_SEQUENCE = '" + dataDownloadedSequenceId + "'  and c.ID = '" + courseId + "'  and phase =  '" + phaseId + "'";

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(sql))
                    {
                        cmd.Connection = conn;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt1);
                        }
                    }
                }
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    Int64 pID = Convert.ToInt64(dt1.Rows[i]["PHASE"]);
                    string phaseDate = Convert.ToString(dt1.Rows[i]["PHASE_DATE"]);
                    string courseCode = Convert.ToString(dt1.Rows[i]["COURSE_CODE"]);
                    string cName = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT DISTINCT CO.NAME FROM NSQF_CERT_PHASE_PRINT_DETAIL P, COURSEMAPPINGWITHNSQFCOURSECODE C ,COURSE CO WHERE C.COURSEID = CO.ID  AND P.COURSE_CODE= C.NSQFCOURSECODE AND P.COURSE_CODE = '" + courseCode + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

                    Paragraph para = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                    para.Alignment = Element.ALIGN_CENTER;
                    doc.Add(para);
                    Paragraph para1 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + phaseId + "                 Course Name :" + courseCode + "   " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
                    para1.Alignment = Element.ALIGN_LEFT;
                    doc.Add(para1);
                    doc.Add(Chunk.NEWLINE);

                    PdfPTable table = new PdfPTable(13);
                    table.WidthPercentage = 90;
                    float[] width = { 0.8F, 0.8F, 0.8F, 0.8F, 0.8F, 1, 0.6F, 0.8F, 0.8F, 0.8F, 0.8F, 0.6F, 0.6F };
                    table.SetWidths(width);
                    //table.SetWidths(new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1 });
                    //table.AddCell("#");
                    table.AddCell("Reg No");
                    table.AddCell("Name /Father Name /Mother Name/Guardian Name/ NCVET Qualcode/ NSQF Framework Level for Qualification / Course Duration (in  Hrs)/NCVET SrNo. / Credits");
                    table.AddCell("Type(Module)");
                    table.AddCell("Module (Short Name)");
                    table.AddCell("Exam Date");
                    table.AddCell("Roll No");
                    table.AddCell("Total Marks");
                    table.AddCell("Grade");
                    table.AddCell("Sector Code");
                    table.AddCell("Final Grade");
                    table.AddCell("Overall Percentage");
                    table.AddCell("Centre Name/ District / State");
                    table.AddCell("Centre Address");

                    //int j = 0;
                    DataTable dt2 = GetCertificateDetailNCVETP(dataDownloadedSequenceId, courseCode, pID);
                    if (dt2.Rows.Count > 0)
                    {
                        foreach (DataRow mod_row in dt2.Rows)
                        {
                            //table.AddCell(GetCell(new Phrase(i + 1))); 
                            table.AddCell(GetCell(new Phrase(new Chunk(mod_row["REGISTRATION_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));


                            string fullName = mod_row["NAME"].ToString().Trim() + " / " + mod_row["F_NAME"].ToString().Trim() + " / " + mod_row["M_NAME"].ToString().Trim() + " / " + mod_row["G_NAME"].ToString().Trim()
                              + " / " + mod_row["NCVETQUALCODE"].ToString().Trim() + " / " + mod_row["NSQF_FRAMWORK_LEVEL_FOR_QUALIFICATION"].ToString().Trim() + " / " + mod_row["COURSE_DURATION_FOR_QUALIFICATION_IN_HOURS"].ToString().Trim() + " / " + mod_row["NCVET_SR_NO"].ToString().Trim() + " / " + mod_row["Credits"].ToString().Trim();
                            table.AddCell(new Phrase(new Chunk(fullName, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));

                            DataTable dt_mod = GetNCVETCertificateModule(Convert.ToInt64(mod_row["REGISTRATION_NO"]));
                            PdfPTable nested2 = new PdfPTable(1);
                            PdfPTable nested3 = new PdfPTable(1);
                            PdfPTable nested4 = new PdfPTable(1);
                            PdfPTable nested5 = new PdfPTable(1);
                            PdfPTable nested6 = new PdfPTable(1);
                            PdfPTable nested7 = new PdfPTable(1);


                            if (dt_mod.Rows.Count > 0)
                            {
                                foreach (DataRow module_row in dt_mod.Rows)
                                {
                                    nested2.AddCell(new Phrase(new Chunk(module_row["Module_Type"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested3.AddCell(new Phrase(new Chunk(module_row["SHORT_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested4.AddCell(new Phrase(new Chunk(module_row["EXAM_DATE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested5.AddCell(new Phrase(new Chunk(module_row["NSQF_ROLL_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested6.AddCell(new Phrase(new Chunk(module_row["TOTAL_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                    nested7.AddCell(new Phrase(new Chunk(module_row["OVERALL_GRADE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                                }
                            }

                            table.AddCell(nested2);
                            table.AddCell(nested3);
                            table.AddCell(nested4);
                            table.AddCell(nested5);
                            table.AddCell(nested6);
                            table.AddCell(nested7);
                            table.AddCell(new Phrase(new Chunk(mod_row["SECTORSHORTCODE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["FINAL_GRADE_FOR_QUALIFICATION"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["OVERALL_PERCENTAGE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            string centreDetail = mod_row["INST_NAME"].ToString().Trim() + " / " + mod_row["TRANING_CENTER_NAME_DISTRICT"].ToString().Trim() + " / " + mod_row["TRANING_CENTER_NAME_STATE"].ToString().Trim();
                            table.AddCell(new Phrase(new Chunk(centreDetail, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                            table.AddCell(new Phrase(new Chunk(mod_row["INST_ADDRESS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));

                        }

                        doc.Add(table);
                        table.FlushContent();

                        doc.NewPage();

                        Paragraph p1 = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                        p1.Alignment = Element.ALIGN_CENTER;
                        doc.Add(p1);
                        Paragraph p2 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + phaseId + "                 Course Name : " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
                        p2.Alignment = Element.ALIGN_LEFT;
                        doc.Add(p2);

                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);
                        doc.Add(Chunk.NEWLINE);

                        PdfPTable tbl = new PdfPTable(1);
                        tbl.WidthPercentage = 60;
                        tbl.AddCell("                                                                Summary                                                       ");

                        DataTable dtable = candidatesSummaryNCVETD(phaseId, courseCode, dataDownloadedSequenceId);
                        if (dtable.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtable.Rows)
                            {
                                tbl.AddCell(GetCell(new Phrase(new Chunk("TQC(including this phase)                                      :                " + dr["TOTAL_QUALIFIED_CANDIDATES"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Total Candidates (NCVET + NON NCVET)                      :                " + dr["TOTAL_QUALIFIED_CANDIDATES_DOWNLOADSEQUENCE_WISE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Total Candidates in Phase            (NCVET)                   :                " + dr["TOTAL_QULAIFIED_CANDIDATES_PHASEWISE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade A              (NCVET)                   :                " + dr["CANDIDATES_WITH_GRADE_A"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade B              (NCVET)                   :                " + dr["CANDIDATES_WITH_GRADE_B"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade C              (NCVET)                   :                " + dr["CANDIDATES_WITH_GRADE_C"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade D              (NCVET)                   :                " + dr["CANDIDATES_WITH_GRADE_D"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                                tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade S              (NCVET)                   :                " + dr["CANDIDATES_WITH_GRADE_S"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
                            }
                        }

                        doc.Add(tbl);
                        tbl.FlushContent();

                        doc.NewPage();
                    }

                    else
                    {
                        Lblerror.Visible = true;
                        Lblerror.Text = "No record found in NCVET format";
                        return;
                    }

                }

                doc.NewPage();

                doc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=PhaseDetailRegister.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Write(doc);
                Response.End();
            }
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    private DataTable GetCertificateDetailNCVET(Int64 dataDownloadedSequenceId)
    {
        DataTable dt2 = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        //string sql = "SELECT DISTINCT CM.REGISTRATION_NO,C.NAME AS COURSE_NAME,PD.NAME,PD.F_NAME,PD.M_NAME,PD.G_NAME  AS G_NAME, PD.inst_name as inst_name ,PD.PHASE,PD.GRADE_CODE, PD.PERCENTAGE,PD.PHASE_DATE,ISNULL(can.Gender,'na') AS Gender from  NSQF_CERT_PHASE_PRINT_DETAIL PD,NSQF_MODULE_CANDIDATE_MARKS CM,COURSE C ,MODULE M,candidate can where " +
        //             "PD.REGISTRATION_NO = CM.REGISTRATION_NO AND C.ID = M.COURSE_ID AND  CM.COURSE_ID = M.COURSE_ID AND M.COURSE_CATEGORY_ID = 6 and can.ID = cm.Candidate_id AND M.COURSE_ID > 102 AND CM.RESULT = 'PASS' AND PD.DATA_DOWNLOADED_SEQUENCE = '" + dataDownloadedSequenceId + "' AND PD.COURSE_CODE = '" + courseCode + "' AND PD.PHASE = '" + pId + "' ORDER BY REGISTRATION_NO";

        string sql = "SELECT  P.REGISTRATION_NO REGISTRATION_NO,P.NAME,F_NAME, M_NAME, G_NAME G_NAME, p.NCVETQualCode,p.NSQF_Framwork_Level_for_qualification   [NSQF_FRAMWORK_LEVEL_FOR_QUALIFICATION], " +
                        "p.course_duration_for_qualification_in_hours  COURSE_DURATION_FOR_QUALIFICATION_IN_HOURS ,  NCVET_SR_NO, " +
                        "sectorShortCode SECTORSHORTCODE,P.GRADE_CODE FINAL_GRADE_FOR_QUALIFICATION,PERCENTAGE OVERALL_PERCENTAGE,INST_NAME, [TRANING_CENTER_NAME_DISTRICT] , TRANING_CENTER_NAME_STATE ," +
                        "ADDRESS1 + ' ' + ADDRESS2 + ' ' + ADDRESS3 + ' ' + ADDRESS_CITY + ' ' + ADDRESS_STATE     INST_ADDRESS , Credits  " +
                        "FROM NSQF_CERT_PHASE_PRINT_DETAIL P  WHERE  P.PHASE > 10  AND P.DATA_DOWNLOADED_SEQUENCE ='" + dataDownloadedSequenceId + "'  AND P.GRADE_CODE IN ('S', 'A', 'B', 'C', 'D')  and p.Credits is not  null  ";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt2);
                }
            }
        }

        return dt2;
    }
    private DataTable GetCertificateDetailNCVETC(Int64 dataDownloadedSequenceId, string courseCode)
    {
        DataTable dt2 = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        //string sql = "SELECT DISTINCT CM.REGISTRATION_NO,C.NAME AS COURSE_NAME,PD.NAME,PD.F_NAME,PD.M_NAME,PD.G_NAME  AS G_NAME, PD.inst_name as inst_name ,PD.PHASE,PD.GRADE_CODE, PD.PERCENTAGE,PD.PHASE_DATE,ISNULL(can.Gender,'na') AS Gender from  NSQF_CERT_PHASE_PRINT_DETAIL PD,NSQF_MODULE_CANDIDATE_MARKS CM,COURSE C ,MODULE M,candidate can where " +
        //             "PD.REGISTRATION_NO = CM.REGISTRATION_NO AND C.ID = M.COURSE_ID AND  CM.COURSE_ID = M.COURSE_ID AND M.COURSE_CATEGORY_ID = 6 and can.ID = cm.Candidate_id AND M.COURSE_ID > 102 AND CM.RESULT = 'PASS' AND PD.DATA_DOWNLOADED_SEQUENCE = '" + dataDownloadedSequenceId + "' AND PD.COURSE_CODE = '" + courseCode + "' AND PD.PHASE = '" + pId + "' ORDER BY REGISTRATION_NO";


        string sql = "SELECT  P.REGISTRATION_NO REGISTRATION_NO,P.NAME,F_NAME, M_NAME, G_NAME G_NAME, p.NCVETQualCode,p.NSQF_Framwork_Level_for_qualification   [NSQF_FRAMWORK_LEVEL_FOR_QUALIFICATION], " +
                        "p.course_duration_for_qualification_in_hours  COURSE_DURATION_FOR_QUALIFICATION_IN_HOURS ,  NCVET_SR_NO, " +
                        "sectorShortCode SECTORSHORTCODE,P.GRADE_CODE [FINAL_GRADE_FOR_QUALIFICATION],PERCENTAGE OVERALL_PERCENTAGE,INST_NAME, [TRANING_CENTER_NAME_DISTRICT] , TRANING_CENTER_NAME_STATE," +
                        "ADDRESS1 + ' ' + ADDRESS2 + ' ' + ADDRESS3 + ' ' + ADDRESS_CITY + ' ' + ADDRESS_STATE     INST_ADDRESS , Credits  " +
                        "FROM NSQF_CERT_PHASE_PRINT_DETAIL P  WHERE  P.PHASE > 10  AND P.DATA_DOWNLOADED_SEQUENCE ='" + dataDownloadedSequenceId + "'  and p.course_code = '" + courseCode + "'    AND P.GRADE_CODE IN ('S', 'A', 'B', 'C', 'D')  and p.Credits is not  null  ";
        //  " FROM NSQF_CERT_PHASE_PRINT_DETAIL P  WHERE  P.PHASE > 10  AND P.DATA_DOWNLOADED_SEQUENCE = '" + dataDownloadedSequenceId + "'  and p.course_code = '" + courseCode + "'  AND P.GRADE_CODE IN ('S', 'A', 'B', 'C', 'D')  ";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt2);
                }
            }
        }

        return dt2;
    }
    private DataTable GetCertificateDetailNCVETP(Int64 dataDownloadedSequenceId, string courseCode, Int64 pId)
    {
        DataTable dt2 = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        //string sql = "SELECT DISTINCT CM.REGISTRATION_NO,C.NAME AS COURSE_NAME,PD.NAME,PD.F_NAME,PD.M_NAME,PD.G_NAME  AS G_NAME, PD.inst_name as inst_name ,PD.PHASE,PD.GRADE_CODE, PD.PERCENTAGE,PD.PHASE_DATE,ISNULL(can.Gender,'na') AS Gender from  NSQF_CERT_PHASE_PRINT_DETAIL PD,NSQF_MODULE_CANDIDATE_MARKS CM,COURSE C ,MODULE M,candidate can where " +
        //             "PD.REGISTRATION_NO = CM.REGISTRATION_NO AND C.ID = M.COURSE_ID AND  CM.COURSE_ID = M.COURSE_ID AND M.COURSE_CATEGORY_ID = 6 and can.ID = cm.Candidate_id AND M.COURSE_ID > 102 AND CM.RESULT = 'PASS' AND PD.DATA_DOWNLOADED_SEQUENCE = '" + dataDownloadedSequenceId + "' AND PD.COURSE_CODE = '" + courseCode + "' AND PD.PHASE = '" + pId + "' ORDER BY REGISTRATION_NO";

        string sql = "SELECT  P.REGISTRATION_NO REGISTRATION_NO,P.NAME,F_NAME, M_NAME, G_NAME G_NAME, p.NCVETQualCode,p.NSQF_Framwork_Level_for_qualification   [NSQF_FRAMWORK_LEVEL_FOR_QUALIFICATION], " +
                     "p.course_duration_for_qualification_in_hours  COURSE_DURATION_FOR_QUALIFICATION_IN_HOURS ,  NCVET_SR_NO, " +
                     "sectorShortCode SECTORSHORTCODE,P.GRADE_CODE [FINAL_GRADE_FOR_QUALIFICATION],PERCENTAGE OVERALL_PERCENTAGE,INST_NAME, [TRANING_CENTER_NAME_DISTRICT] , TRANING_CENTER_NAME_STATE ," +
                     "ADDRESS1 + ' ' + ADDRESS2 + ' ' + ADDRESS3 + ' ' + ADDRESS_CITY + ' ' + ADDRESS_STATE     INST_ADDRESS , Credits  " +
                     "FROM NSQF_CERT_PHASE_PRINT_DETAIL P  WHERE  P.PHASE > 10  AND P.DATA_DOWNLOADED_SEQUENCE ='" + dataDownloadedSequenceId + "'  and p.course_code = '" + courseCode + "' and p.phase = '" + pId + "'    AND P.GRADE_CODE IN ('S', 'A', 'B', 'C', 'D')  and p.Credits is not  null  ";
        //  " FROM NSQF_CERT_PHASE_PRINT_DETAIL P  WHERE  P.PHASE > 10  AND P.DATA_DOWNLOADED_SEQUENCE = '" + dataDownloadedSequenceId + "'  and p.course_code = '" + courseCode + "'  and p.phase = '" + pId  + "'  AND P.GRADE_CODE IN ('S', 'A', 'B', 'C', 'D')  ";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt2);
                }
            }
        }

        return dt2;
    }
    private DataTable candidatesSummaryNCVETD(Int64 pId, String courseCode, Int64 dataDownloadedSequenceId)
    {
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        string sql = " SELECT (SELECT COUNT(*) FROM NSQF_CERT_PHASE_PRINT_DETAIL WHERE COURSE_CODE = '" + courseCode + "'  AND PHASE <=  '" + pId + "' AND (DATA_DOWNLOADED_SEQUENCE <= '" + dataDownloadedSequenceId + "' OR " +
                     " DATA_DOWNLOADED_SEQUENCE IS NULL) ) AS TOTAL_QUALIFIED_CANDIDATES,(SELECT COUNT(*) FROM NSQF_CERT_PHASE_PRINT_DETAIL WHERE DATA_DOWNLOADED_SEQUENCE ='" + dataDownloadedSequenceId + "'  ) AS TOTAL_QUALIFIED_CANDIDATES_DOWNLOADSEQUENCE_WISE," +
                     "(SELECT COUNT(*) FROM NSQF_CERT_PHASE_PRINT_DETAIL WHERE DATA_DOWNLOADED_SEQUENCE = 105  AND PHASE ='" + pId + "') TOTAL_QULAIFIED_CANDIDATES_PHASEWISE, " +
                     " SUM(CASE  GRADE_CODE  WHEN 'A' THEN 1 ELSE 0 END) CANDIDATES_WITH_GRADE_A,SUM(CASE GRADE_CODE WHEN 'B' THEN 1 ELSE 0 END) CANDIDATES_WITH_GRADE_B, " +
                     " SUM(CASE  GRADE_CODE  WHEN 'C' THEN 1 ELSE 0 END) CANDIDATES_WITH_GRADE_C,SUM(CASE GRADE_CODE WHEN 'D' THEN 1 ELSE 0 END) CANDIDATES_WITH_GRADE_D, " +
                     " SUM(CASE  GRADE_CODE  WHEN 'S' THEN 1 ELSE 0 END) CANDIDATES_WITH_GRADE_S FROM NSQF_CERT_PHASE_PRINT_DETAIL WHERE " +
                     " DATA_DOWNLOADED_SEQUENCE = '" + dataDownloadedSequenceId + "' and Credits is not null";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
        }

        return dt;
    }
    private DataTable GetNCVETCertificateModule(Int64 regno)
    {
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        string sql = " SELECT   MT.Code Module_Type  ,M.SHORT_NAME,CONVERT(VARCHAR,CM.EXAM_DATE,113) EXAM_DATE,CM.NSQF_ROLL_NO,CM.TOTAL_MARKS,PD.GRADE_CODE AS OVERALL_GRADE" +
                      "  FROM MODULE M,NSQF_CERT_PHASE_PRINT_DETAIL PD,COURSEMAPPINGWITHNSQFCOURSECODE MAP,MODULE_TYPE MT,NSQF_MODULE_CANDIDATE_MARKS CM WHERE  m.ID = cm.Module_id and  m.Code= cm.Module_code and  M.MODULE_TYPE_ID = MT.ID " +
                      "  AND M.COURSE_CATEGORY_ID = 6 AND M.COURSE_ID > 102 AND CM.COURSE_ID = M.COURSE_ID AND CM.COURSE_ID = MAP.COURSEID AND MAP.NSQFCOURSECODE = PD.COURSE_CODE AND PD.REGISTRATION_NO = CM.REGISTRATION_NO " +
                      "  AND MT.ID = CM.MODULE_TYPE AND CM.RESULT = 'PASS' AND CM.REGISTRATION_NO = '" + regno + "' ORDER BY PD.PHASE";

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql))
            {
                cmd.Connection = conn;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
        }

        return dt;
    }

    // --End--

    // below code commented by natasha on dated 15102024

    //protected void btnView_Click(object sender, EventArgs e)
    //{

    //    btnView.Visible = false;
    //    int CourseID = Convert.ToInt32(ddlCourseCategry.SelectedValue);//6
    //    int dataDownloadedSequenceId = Convert.ToInt32(ddlDataDownloadedSequence.SelectedValue);
    //    Int32 courseId = Convert.ToInt32(ddlCourseName.SelectedValue);
    //    int phaseId = Convert.ToInt32(ddlPhaseNumber.SelectedValue);

    //    try
    //    {
    //        if (dataDownloadedSequenceId != 0 && courseId == 0 && phaseId == 0)
    //        {
               

    //            Document doc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
    //            PdfWriter pdfWriter1 = PdfWriter.GetInstance(doc, Response.OutputStream);
    //            doc.Open();

    //            DataTable dt = new DataTable();
    //            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    //            string sql = "SELECT DISTINCT COURSE_CODE,PHASE,CONVERT(VARCHAR,PHASE_DATE,103) PHASE_DATE FROM NSQF_CERT_PHASE_PRINT_DETAIL WHERE DATA_DOWNLOADED_SEQUENCE ='" + dataDownloadedSequenceId + "' GROUP BY PHASE,COURSE_CODE,PHASE_DATE ORDER BY PHASE";

    //            using (SqlConnection conn = new SqlConnection(constr))
    //            {
    //                using (SqlCommand cmd = new SqlCommand(sql))
    //                {
    //                    cmd.Connection = conn;
    //                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
    //                    {
    //                        sda.Fill(dt);
    //                    }
    //                }
    //            }

    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                string phaseDate = Convert.ToString(dt.Rows[i]["PHASE_DATE"]);
    //                Int64 pID = Convert.ToInt64(dt.Rows[i]["PHASE"]);
    //                string courseCode = Convert.ToString(dt.Rows[i]["COURSE_CODE"]);
    //                string cName = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT DISTINCT CO.NAME FROM NSQF_CERT_PHASE_PRINT_DETAIL P, COURSEMAPPINGWITHNSQFCOURSECODE C ,COURSE CO WHERE C.COURSEID = CO.ID  AND P.COURSE_CODE= C.NSQFCOURSECODE AND P.COURSE_CODE = '" + courseCode + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

    //                Paragraph para = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
    //                para.Alignment = Element.ALIGN_CENTER;
    //                doc.Add(para);
    //                Paragraph para1 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + pID + "                 Course Name : " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
    //                para1.Alignment = Element.ALIGN_LEFT;
    //                doc.Add(para1);
    //                doc.Add(Chunk.NEWLINE);

    //                PdfPTable table = new PdfPTable(12);
    //                table.WidthPercentage = 100;
    //                float[] width = { 1, 1, 1, 1, 0.8F, 1, 1, 2, 1, 1, 0.8F, 0.8F };
    //                table.SetWidths(width);
    //                //table.SetWidths(new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1 });
    //                //table.AddCell("#");
    //                table.AddCell("Reg No");
    //                table.AddCell("Name");
    //                table.AddCell("Father's Name");
    //                table.AddCell("Mother's Name");
    //                table.AddCell("Gender");
    //                table.AddCell("Module Code");
    //                table.AddCell("Exam Date");
    //                table.AddCell("Roll No");
    //                table.AddCell("Total Marks");
    //                table.AddCell("Max Marks");
    //                table.AddCell("Grade");
    //                table.AddCell("Avg Marks");
    //                //int j = 0;
    //                DataTable dt2 = GetDataPhase(dataDownloadedSequenceId, courseCode, pID);
    //                if (dt2.Rows.Count > 0)
    //                {
    //                    foreach (DataRow mod_row in dt2.Rows)
    //                    {
    //                        //table.AddCell(GetCell(new Phrase(i + 1))); 
    //                        table.AddCell(GetCell(new Phrase(new Chunk(mod_row["REGISTRATION_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
    //                        table.AddCell(new Phrase(new Chunk(mod_row["NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                        table.AddCell(new Phrase(new Chunk(mod_row["F_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                        table.AddCell(new Phrase(new Chunk(mod_row["M_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                        table.AddCell(new Phrase(new Chunk(mod_row["Gender"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));

    //                        DataTable dt_mod = GetDatamodule(Convert.ToInt64(mod_row["REGISTRATION_NO"]));
    //                        PdfPTable nested2 = new PdfPTable(1);
    //                        PdfPTable nested3 = new PdfPTable(1);
    //                        PdfPTable nested4 = new PdfPTable(1);
    //                        PdfPTable nested5 = new PdfPTable(1);
    //                        PdfPTable nested6 = new PdfPTable(1);

    //                        if (dt_mod.Rows.Count > 0)
    //                        {
    //                            foreach (DataRow module_row in dt_mod.Rows)
    //                            {
    //                                nested2.AddCell(new Phrase(new Chunk(module_row["SHORT_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                                nested3.AddCell(new Phrase(new Chunk(module_row["EXAM_DATE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                                nested4.AddCell(new Phrase(new Chunk(module_row["NSQF_ROLL_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                                nested5.AddCell(new Phrase(new Chunk(module_row["TOTAL_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                                nested6.AddCell(new Phrase(new Chunk(module_row["MAX_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                            }
    //                        }

    //                        table.AddCell(nested2);
    //                        table.AddCell(nested3);
    //                        table.AddCell(nested4);
    //                        table.AddCell(nested5);
    //                        table.AddCell(nested6);
    //                        table.AddCell(new Phrase(new Chunk(mod_row["GRADE_CODE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                        table.AddCell(new Phrase(new Chunk(mod_row["PERCENTAGE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));

    //                    }

    //                    doc.Add(table);
    //                    table.FlushContent();
    //                    doc.NewPage();

    //                    Paragraph p1 = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
    //                    p1.Alignment = Element.ALIGN_CENTER;
    //                    doc.Add(p1);
    //                    Paragraph p2 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + pID + "                 Course Name : " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
    //                    p2.Alignment = Element.ALIGN_LEFT;
    //                    doc.Add(p2);

    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);

    //                    PdfPTable tbl = new PdfPTable(1);
    //                    tbl.WidthPercentage = 60;
    //                    tbl.AddCell("                                                                Summary                                                       ");

    //                    DataTable dtable = candidatesSummary(pID, courseCode, dataDownloadedSequenceId);
    //                    if (dtable.Rows.Count > 0)
    //                    {
    //                        foreach (DataRow dr in dtable.Rows)
    //                        {
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("TQC(including this phase)                          :                " + dr["TOTAL_QUALIFIED_CANDIDATES"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Total Candidates in Phase                          :                " + dr["TOTAL_QULAIFIED_CANDIDATES_PHASEWISE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade A                            :                " + dr["CANDIDATES_WITH_GRADE_A"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade B                            :                " + dr["CANDIDATES_WITH_GRADE_B"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade C                            :                " + dr["CANDIDATES_WITH_GRADE_C"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade D                            :                " + dr["CANDIDATES_WITH_GRADE_D"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade S                            :                " + dr["CANDIDATES_WITH_GRADE_S"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                        }
    //                    }

    //                    doc.Add(tbl);
    //                    tbl.FlushContent();

    //                    doc.NewPage();
    //                }
    //            }
    //            doc.NewPage();

    //            doc.Close();
    //            Response.Buffer = true;
    //            Response.ContentType = "application/pdf";
    //            Response.AddHeader("content-disposition", "attachment;filename=PhaseDetailRegister.pdf");
    //            Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //            Response.Write(doc);
    //            Response.End();
    //            //lblwait.Visible = false;
    //        }

    //        if (dataDownloadedSequenceId != 0 && courseId != 0 && phaseId == 0)
    //        {

               


    //            Document doc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
    //            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, Response.OutputStream);
    //            doc.Open();

    //            DataTable dt = new DataTable();
    //            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    //            string sql = " SELECT DISTINCT COURSE_CODE ,PD.PHASE,CONVERT(VARCHAR,PD.PHASE_DATE,103) PHASE_DATE FROM NSQF_CERT_PHASE_PRINT_DETAIL PD,COURSEMAPPINGWITHNSQFCOURSECODE MAP,COURSE C,NSQF_MODULE_CANDIDATE_MARKS CM," +
    //                         " MODULE M,MODULE_TYPE MT WHERE M.MODULE_TYPE_ID = MT.ID AND M.COURSE_CATEGORY_ID = 6 AND M.COURSE_ID > 102 AND C.ID = M.COURSE_ID AND CM.COURSE_ID = M.COURSE_ID " +
    //                         " AND CM.COURSE_ID = MAP.COURSEID AND MAP.NSQFCOURSECODE = PD.COURSE_CODE AND PD.REGISTRATION_NO = CM.REGISTRATION_NO AND MT.ID = CM.MODULE_TYPE " +
    //                         " AND PD.DATA_DOWNLOADED_SEQUENCE = '" + dataDownloadedSequenceId + "' AND CM.RESULT = 'PASS' AND CM.MODULE_ID = M.ID AND M.COURSE_ID = '" + courseId + "' ORDER BY PHASE ";

    //            using (SqlConnection conn = new SqlConnection(constr))
    //            {
    //                using (SqlCommand cmd = new SqlCommand(sql))
    //                {
    //                    cmd.Connection = conn;
    //                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
    //                    {
    //                        sda.Fill(dt);
    //                    }
    //                }
    //            }

    //            for (int i = 0; i < dt.Rows.Count; i++)
    //            {
    //                string phaseDate = Convert.ToString(dt.Rows[i]["PHASE_DATE"]);
    //                Int64 pID = Convert.ToInt64(dt.Rows[i]["PHASE"]);
    //                string courseCode = Convert.ToString(dt.Rows[i]["COURSE_CODE"]);
    //                string cName = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT DISTINCT CO.NAME FROM NSQF_CERT_PHASE_PRINT_DETAIL P, COURSEMAPPINGWITHNSQFCOURSECODE C ,COURSE CO WHERE C.COURSEID = CO.ID  AND P.COURSE_CODE= C.NSQFCOURSECODE AND P.COURSE_CODE = '" + courseCode + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

    //                Paragraph para = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
    //                para.Alignment = Element.ALIGN_CENTER;
    //                doc.Add(para);
    //                Paragraph para1 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + pID + "                 Course Name : " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
    //                para1.Alignment = Element.ALIGN_LEFT;
    //                doc.Add(para1);
    //                doc.Add(Chunk.NEWLINE);

    //                PdfPTable table = new PdfPTable(12);
    //                table.WidthPercentage = 100;
    //                float[] width = { 1, 1, 1, 1, 0.8F, 1, 1, 2, 1, 1, 0.8F, 0.8F };
    //                table.SetWidths(width);
    //                //table.SetWidths(new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1 });
    //                table.AddCell("Reg No");
    //                table.AddCell("Name");
    //                table.AddCell("Father's Name");
    //                table.AddCell("Mother's Name");
    //                table.AddCell("Gender");
    //                table.AddCell("Module Code");
    //                table.AddCell("Exam Date");
    //                table.AddCell("Roll No");
    //                table.AddCell("Total Marks");
    //                table.AddCell("Max Marks");
    //                table.AddCell("Grade");
    //                table.AddCell("Avg Marks");

    //                DataTable dt1 = GetDataPhase(dataDownloadedSequenceId, courseCode, pID);

    //                if (dt1.Rows.Count > 0)
    //                {
    //                    foreach (DataRow mod_row in dt1.Rows)
    //                    {
    //                        table.AddCell(GetCell(new Phrase(new Chunk(mod_row["REGISTRATION_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
    //                        table.AddCell(new Phrase(new Chunk(mod_row["NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                        table.AddCell(new Phrase(new Chunk(mod_row["F_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                        table.AddCell(new Phrase(new Chunk(mod_row["M_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                        table.AddCell(new Phrase(new Chunk(mod_row["Gender"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));

    //                        DataTable dt_mod = GetDatamodule(Convert.ToInt64(mod_row["REGISTRATION_NO"]));
    //                        PdfPTable nested2 = new PdfPTable(1);
    //                        PdfPTable nested3 = new PdfPTable(1);
    //                        PdfPTable nested4 = new PdfPTable(1);
    //                        PdfPTable nested5 = new PdfPTable(1);
    //                        PdfPTable nested6 = new PdfPTable(1);

    //                        if (dt_mod.Rows.Count > 0)
    //                        {
    //                            foreach (DataRow module_row in dt_mod.Rows)
    //                            {
    //                                nested2.AddCell(new Phrase(new Chunk(module_row["SHORT_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                                nested3.AddCell(new Phrase(new Chunk(module_row["EXAM_DATE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                                nested4.AddCell(new Phrase(new Chunk(module_row["NSQF_ROLL_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                                nested5.AddCell(new Phrase(new Chunk(module_row["TOTAL_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                                nested6.AddCell(new Phrase(new Chunk(module_row["MAX_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                            }
    //                        }

    //                        table.AddCell(nested2);
    //                        table.AddCell(nested3);
    //                        table.AddCell(nested4);
    //                        table.AddCell(nested5);
    //                        table.AddCell(nested6);
    //                        table.AddCell(new Phrase(new Chunk(mod_row["GRADE_CODE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                        table.AddCell(new Phrase(new Chunk(mod_row["PERCENTAGE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                    }

    //                    doc.Add(table);
    //                    table.FlushContent();

    //                    doc.NewPage();

    //                    Paragraph p1 = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
    //                    p1.Alignment = Element.ALIGN_CENTER;
    //                    doc.Add(p1);
    //                    Paragraph p2 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + pID + "                 Course Name : " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
    //                    p2.Alignment = Element.ALIGN_LEFT;
    //                    doc.Add(p2);

    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);

    //                    PdfPTable tbl = new PdfPTable(1);
    //                    tbl.WidthPercentage = 60;
    //                    tbl.AddCell("                                                                Summary                                                       ");

    //                    DataTable dtable = candidatesSummary(pID, courseCode, dataDownloadedSequenceId);
    //                    if (dtable.Rows.Count > 0)
    //                    {
    //                        foreach (DataRow dr in dtable.Rows)
    //                        {
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("TQC(including this phase)                       :                " + dr["TOTAL_QUALIFIED_CANDIDATES"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Total Candidates in Phase                       :                " + dr["TOTAL_QULAIFIED_CANDIDATES_PHASEWISE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade A                         :                " + dr["CANDIDATES_WITH_GRADE_A"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade B                         :                " + dr["CANDIDATES_WITH_GRADE_B"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade C                         :                " + dr["CANDIDATES_WITH_GRADE_C"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade D                         :                " + dr["CANDIDATES_WITH_GRADE_D"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade S                         :                " + dr["CANDIDATES_WITH_GRADE_S"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                        }
    //                    }

    //                    doc.Add(tbl);
    //                    tbl.FlushContent();

    //                    doc.NewPage();
    //                }

    //            }

    //            doc.NewPage();

    //            doc.Close();
    //            Response.Buffer = true;
    //            Response.ContentType = "application/pdf";
    //            Response.AddHeader("content-disposition", "attachment;filename=PhaseDetailRegister.pdf");
    //            Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //            Response.Write(doc);
    //            Response.End();
    //           // lblwait.Visible = false;
    //        }

    //        if (dataDownloadedSequenceId != 0 && courseId != 0 && phaseId != 0)
    //        {
    //            // String path = Server.MapPath("~/Download/NSQFConsolidatedmarksheets/" + "DataDownloadedSequenceId_" + dataDownloadedSequenceId);
    //            ////String path = @"\NSQF_Consolidated_Marksheets\\NSQFConsolidatedmarksheets\\DataDownloadedSequenceId_23" + dataDownloadedSequenceId;
    //            ////String path = @"\NSQF_Consolidated_Marksheets\NSQFConsolidatedmarksheets\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
    //            //if (Directory.Exists(path) == true)
    //            //{
    //            //    //ShowAlert("NSQF Certificate has already been downloaded.", true);
    //            //}
    //            //else
    //            //{
    //            //    using (EConnectContext context = new EConnectContext())
    //            //    {

    //            //        String directoryName = "NSQFConsolidatedmarksheets";
    //            //        //  System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId));
    //            //        // var directorypath = @"\NSQF_Consolidated_Marksheets\" + directoryName + "\\DataDownloadedSequenceId_" + dataDownloadedSequenceId;
    //            //        var directorypath = @"\Download\\" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId;

    //            //       // System.IO.Directory.Delete(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId), true);

    //            //       // if (!Directory.Exists(directorypath))
    //            //        //{
    //            //            //Directory.CreateDirectory(directorypath);
    //            //            System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId));
    //            //       // }
    //            //        var applications = (from c in context.NSQFCertPhasePrintDetails
    //            //                            where c.data_downloaded_sequence == dataDownloadedSequenceId
    //            //                            select new
    //            //                            {
    //            //                                RegistrationId = c.registration_no,
    //            //                                ID = c.ID,
    //            //                                CourseID = c.course_code
    //            //                            }).ToList();

    //            //        Lblerror.Text = "Total no of NSQF Certificate:- " + applications.Count().ToString();
    //            //        if (applications.Count() <= 0)
    //            //            return;

    //            //        int applicationsCount = applications.Count();

    //            //        if (dataDownloadedSequenceId != 0)
    //            //        {
    //            //            for (int i = 0; i < applicationsCount; i++)
    //            //            {
    //            //                CreateNSQFCertificate(applications[i].RegistrationId, directoryName);
    //            //            }
    //            //        }

    //            //        using (ZipFile zipFile = new ZipFile())
    //            //        {
    //            //            zipFile.AddDirectory(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId));
    //            //            Response.Clear();
    //            //            Response.ContentType = "application/zip";
    //            //            Response.AddHeader("content-disposition", "filename=" + directoryName + "_DataDownloadedSequenceId_" +dataDownloadedSequenceId+ ".zip");
    //            //            zipFile.Save(Response.OutputStream);
    //            //        }
    //            //        System.IO.Directory.Delete(Server.MapPath("~/Download/" + directoryName + "/DataDownloadedSequenceId_" + dataDownloadedSequenceId), true);
    //            //    }
    //            //}

    //            Document doc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
    //            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, Response.OutputStream);
    //            doc.Open();

    //            DataTable dt1 = new DataTable();
    //            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    //            string sql = " SELECT DISTINCT COURSE_CODE ,PD.PHASE,CONVERT(VARCHAR,PD.PHASE_DATE,103) PHASE_DATE FROM NSQF_CERT_PHASE_PRINT_DETAIL PD,COURSEMAPPINGWITHNSQFCOURSECODE MAP,COURSE C,NSQF_MODULE_CANDIDATE_MARKS CM," +
    //                         " MODULE M,MODULE_TYPE MT WHERE M.MODULE_TYPE_ID = MT.ID AND M.COURSE_CATEGORY_ID = 6 AND M.COURSE_ID > 102 AND C.ID = M.COURSE_ID AND CM.COURSE_ID = M.COURSE_ID " +
    //                         " AND CM.COURSE_ID = MAP.COURSEID AND MAP.NSQFCOURSECODE = PD.COURSE_CODE AND PD.REGISTRATION_NO = CM.REGISTRATION_NO AND MT.ID = CM.MODULE_TYPE" +
    //                         " AND PD.DATA_DOWNLOADED_SEQUENCE = '" + dataDownloadedSequenceId + "' AND CM.RESULT = 'PASS' AND CM.MODULE_ID = M.ID AND M.COURSE_ID = '" + courseId + "' AND PD.PHASE = '" + phaseId + "' ORDER BY PHASE";

    //            using (SqlConnection conn = new SqlConnection(constr))
    //            {
    //                using (SqlCommand cmd = new SqlCommand(sql))
    //                {
    //                    cmd.Connection = conn;
    //                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
    //                    {
    //                        sda.Fill(dt1);
    //                    }
    //                }
    //            }
    //            for (int i = 0; i < dt1.Rows.Count; i++)
    //            {
    //                string phaseDate = Convert.ToString(dt1.Rows[i]["PHASE_DATE"]);
    //                string courseCode = Convert.ToString(dt1.Rows[i]["COURSE_CODE"]);
    //                string cName = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT DISTINCT CO.NAME FROM NSQF_CERT_PHASE_PRINT_DETAIL P, COURSEMAPPINGWITHNSQFCOURSECODE C ,COURSE CO WHERE C.COURSEID = CO.ID  AND P.COURSE_CODE= C.NSQFCOURSECODE AND P.COURSE_CODE = '" + courseCode + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

    //                Paragraph para = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
    //                para.Alignment = Element.ALIGN_CENTER;
    //                doc.Add(para);
    //                Paragraph para1 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + phaseId + "                 Course Name : " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
    //                para1.Alignment = Element.ALIGN_LEFT;
    //                doc.Add(para1);
    //                doc.Add(Chunk.NEWLINE);

    //                PdfPTable table = new PdfPTable(12);
    //                table.WidthPercentage = 100;
    //                float[] width = { 1, 1, 1, 1, 0.8F, 1, 1, 2, 1, 1, 0.8F, 0.8F };
    //                table.SetWidths(width);
    //                //table.SetWidths(new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1 });
    //                table.AddCell("Reg No");
    //                table.AddCell("Name");
    //                table.AddCell("Father's Name");
    //                table.AddCell("Mother's Name");
    //                table.AddCell("Gender");
    //                table.AddCell("Module Code");
    //                table.AddCell("Exam Date");
    //                table.AddCell("Roll No");
    //                table.AddCell("Total Marks");
    //                table.AddCell("Max Marks");
    //                table.AddCell("Grade");
    //                table.AddCell("Avg Marks");

    //                DataTable dt = GetDataPhase(dataDownloadedSequenceId, courseCode, phaseId);
    //                if (dt.Rows.Count > 0)
    //                {
    //                    foreach (DataRow mod_row in dt.Rows)
    //                    {
    //                        table.AddCell(GetCell(new Phrase(new Chunk(mod_row["REGISTRATION_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
    //                        table.AddCell(new Phrase(new Chunk(mod_row["NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                        table.AddCell(new Phrase(new Chunk(mod_row["F_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                        table.AddCell(new Phrase(new Chunk(mod_row["M_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                        table.AddCell(new Phrase(new Chunk(mod_row["Gender"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));

    //                        DataTable dt_mod = GetDatamodule(Convert.ToInt64(mod_row["registration_no"]));
    //                        PdfPTable nested2 = new PdfPTable(1);
    //                        PdfPTable nested3 = new PdfPTable(1);
    //                        PdfPTable nested4 = new PdfPTable(1);
    //                        PdfPTable nested5 = new PdfPTable(1);
    //                        PdfPTable nested6 = new PdfPTable(1);

    //                        if (dt_mod.Rows.Count > 0)
    //                        {
    //                            foreach (DataRow module_row in dt_mod.Rows)
    //                            {
    //                                nested2.AddCell(new Phrase(new Chunk(module_row["SHORT_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                                nested3.AddCell(new Phrase(new Chunk(module_row["EXAM_DATE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                                nested4.AddCell(new Phrase(new Chunk(module_row["NSQF_ROLL_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                                nested5.AddCell(new Phrase(new Chunk(module_row["TOTAL_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                                nested6.AddCell(new Phrase(new Chunk(module_row["MAX_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                            }
    //                        }

    //                        table.AddCell(nested2);
    //                        table.AddCell(nested3);
    //                        table.AddCell(nested4);
    //                        table.AddCell(nested5);
    //                        table.AddCell(nested6);
    //                        table.AddCell(new Phrase(new Chunk(mod_row["GRADE_CODE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                        table.AddCell(new Phrase(new Chunk(mod_row["PERCENTAGE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
    //                    }

    //                    doc.Add(table);
    //                    table.FlushContent();

    //                    doc.NewPage();

    //                    Paragraph p1 = new Paragraph("NIELIT \n CERTIFICATE PRINT DETAIL REGISTER ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
    //                    p1.Alignment = Element.ALIGN_CENTER;
    //                    doc.Add(p1);
    //                    Paragraph p2 = new Paragraph("Data Downloaded Sequence No : " + dataDownloadedSequenceId + "                 Phase No : " + phaseId + "                 Course Name : " + cName + "                                     Phase Date : " + phaseDate, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10));
    //                    p2.Alignment = Element.ALIGN_LEFT;
    //                    doc.Add(p2);

    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);
    //                    doc.Add(Chunk.NEWLINE);

    //                    PdfPTable tbl = new PdfPTable(1);
    //                    tbl.WidthPercentage = 60;
    //                    tbl.AddCell("                                                                Summary                                                       ");

    //                    DataTable dtable = candidatesSummary(phaseId, courseCode, dataDownloadedSequenceId);
    //                    if (dtable.Rows.Count > 0)
    //                    {
    //                        foreach (DataRow dr in dtable.Rows)
    //                        {
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("TQC(including this phase)                       :                " + dr["TOTAL_QUALIFIED_CANDIDATES"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Total Candidates in Phase                       :                " + dr["TOTAL_QULAIFIED_CANDIDATES_PHASEWISE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade A                         :                " + dr["CANDIDATES_WITH_GRADE_A"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade B                         :                " + dr["CANDIDATES_WITH_GRADE_B"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade C                         :                " + dr["CANDIDATES_WITH_GRADE_C"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade D                         :                " + dr["CANDIDATES_WITH_GRADE_D"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                            tbl.AddCell(GetCell(new Phrase(new Chunk("Candidates with Grade S                         :                " + dr["CANDIDATES_WITH_GRADE_S"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 11, iTextSharp.text.Font.NORMAL)))));
    //                        }
    //                    }

    //                    doc.Add(tbl);
    //                    tbl.FlushContent();

    //                    doc.NewPage();
    //                }
    //            }

    //            doc.NewPage();

    //            doc.Close();
    //            Response.Buffer = true;
    //            Response.ContentType = "application/pdf";
    //            Response.AddHeader("content-disposition", "attachment;filename=PhaseDetailRegister.pdf");
    //            Response.Cache.SetCacheability(HttpCacheability.NoCache);
    //            Response.Write(doc);
    //            Response.End();
    //            //lblwait.Visible = false;
    //        }

    //    }

    //    catch (Exception ex)
    //    {
    //        btnView.Visible = true;
    //        ShowAlert(ex.Message);
    //    }
    //    btnView.Visible = true;
    //}

}