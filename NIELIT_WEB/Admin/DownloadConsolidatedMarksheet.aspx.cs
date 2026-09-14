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
using System.Drawing.Imaging;
using Ionic.Zip;

public partial class ReportPgae : BasePage
{
    //public Int64 courseId;
    //public Int64 dataDownloadedSequenceId;
    //public Int64 phaseId;
    //Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //if (!IsSessionAlive())
            //{
            //    Response.Redirect("~/index.aspx");
            //}
            //currentRoleId = Convert.ToInt32(Session["RoleID"]);
            //if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/DownloadPhaseDetailRegister.aspx"))
            //{
            //    Response.Write("Sorry! You don't have rights  to view this page");
            //    Response.End();
            //GetData();
            //}
            //if (Request.UrlReferrer == null)
            //{
            //    Response.Write(GeInvalidRequestMessage("Goto Home Page", "../MainPage.aspx"));
            //    Response.End();
            //    return;
            //}

            //dataDownloadedSequenceId = Convert.ToInt32(Request.QueryString["dataDownloadedSequenceId"]);
            //courseId = Convert.ToInt32(Request.QueryString["courseId"]);
            //phaseId = Convert.ToInt32(Request.QueryString["phaseId"]);
            //dataDownloadedSequenceId = 1212534;
            //courseId = 0; //115
            //phaseId = 0; //13

            //if (!Page.IsPostBack)
            //{
            //    tbl.CssClass = "sample3";
            //    tbl.CellPadding = 2;
            //    tbl.CellSpacing = 1;
            //    tbl.Width = Unit.Percentage(100);

            //    ShowData();
            //    divReportData.Controls.Add(tbl);
            //}
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    private DataTable GetData(Int64 rId)
    {
        DataTable dt1 = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

        string sql = " SELECT  PD.REGISTRATION_NO,PD.NAME,M.NAME AS MOD_TYPE,CONVERT(VARCHAR,CM.EXAM_DATE,113) EXAM_DATE,CM.NSQF_ROLL_NO,CM.TOTAL_MARKS,CM.RESULT,PD.GRADE_CODE AS OVERALL_GRADE,PD.PERCENTAGE," +
                     " (SELECT ( CASE WHEN MODULE_TYPE = 1 THEN THEORYPAPERMAXMARKS WHEN MODULE_TYPE = 3 THEN PRACTICALPAPERMAXMARKS WHEN MODULE_TYPE IN (4,7,8) THEN PROJECT_PRESENTATION_ASSIGNMENTMAXMARKS WHEN MODULE_TYPE = 6" +
                     " THEN INTERNALASSESSMENTMAXMARKS WHEN MODULE_TYPE IN (9,10) THEN  MAJORPROJECT_DISSERTATIONMAXMARKS END ) FROM NSQFTOTALPAPERS WHERE NSQFLEARNHOURTYPE = MAP.NSQFLEARNHOURTYPE ) MAX_MARKS " +
                     " FROM MODULE M,NSQF_CERT_PHASE_PRINT_DETAIL PD,COURSEMAPPINGWITHNSQFCOURSECODE MAP,MODULE_TYPE MT,NSQF_MODULE_CANDIDATE_MARKS CM WHERE M.MODULE_TYPE_ID = MT.ID " +
                     " AND M.COURSE_CATEGORY_ID = 6 AND M.COURSE_ID > 102 AND CM.COURSE_ID = M.COURSE_ID AND CM.COURSE_ID = MAP.COURSEID AND MAP.NSQFCOURSECODE = PD.COURSE_CODE AND PD.REGISTRATION_NO = CM.REGISTRATION_NO " +
                     " AND MT.ID = CM.MODULE_TYPE AND CM.RESULT = 'PASS' AND PD.REGISTRATION_NO = '" + rId + "' ORDER BY REGISTRATION_NO";

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
    private PdfPCell GetCell(Phrase phrase)
    {
        PdfPCell cell = new PdfPCell(phrase);
        cell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
        //cell.PaddingBottom = 2f;
        cell.PaddingTop = 3f;
        //cell.BackgroundColor = new iTextSharp.text.BaseColor(60, 60, 60);
        //cell.HorizontalAlignment = 
        //cell.Width = Unit.Percentage(100);
        //cell.BorderWidth = PdfPCell.TOP_BORDER;
        //cell.BorderWidthBottom= 0 ;
        //cell.BorderWidthLeft=0 ;
        //cell.BorderWidthTop=0 ;
        //cell.BorderWidthRight =  0 ;
        cell.Width = 2f;
        cell.VerticalAlignment = PdfPCell.ALIGN_JUSTIFIED;
        return cell;
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
    protected void chkBtn_Click(object sender, EventArgs e)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                Int64 rId = Convert.ToInt64(regisId.Text.ToString());                
                var result = (from m in context.Modules
                              join cm in context.NSQFModuleCandidateMarks on m.CourseID equals cm.CourseId
                              join pd in context.NSQFCertPhasePrintDetails on cm.RegistrationNo equals pd.registration_no
                              join map in context.CourseMappingWithNSQFCoursecodes on pd.course_code equals map.NSQFCourseCode
                              join mt in context.ModuleTypes on cm.ModuleType equals mt.ID
                              where cm.RegistrationNo == rId                           
                              select new { name = cm.RegistrationNo, }).FirstOrDefault();

                if (result == null)
                {
                    Lblerror.Text = " Registration No does not exist";
                    Lblerror.Visible = true;
                }
                else
                {                   
                    Lblerror.Visible = false;
                }
            }
        }
        catch(Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }    
    protected void imgPDF_Click(object sender, ImageClickEventArgs e)
    {      
        try
        {        
            Document doc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);
            Int64 rId = Convert.ToInt64(regisId.Text.ToString());
            
            string courseCode = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT c.NSQFCourseCode FROM NSQF_CERT_PHASE_PRINT_DETAIL P, COURSEMAPPINGWITHNSQFCOURSECODE C ,COURSE CO WHERE C.COURSEID = CO.ID  AND P.COURSE_CODE= C.NSQFCOURSECODE AND p.registration_no = '" + rId + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
            string phaseYear = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select year(phase_date) Phase_Year from NSQF_Cert_Phase_Print_Detail where registration_no = '" + rId + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));

            string corseCodeDirectoryName = "";
            corseCodeDirectoryName = courseCode;
            string phaseYearDirectoryName = "";
            phaseYearDirectoryName = phaseYear;

            string filename = "ConsoliadtedMarksheet" + "_" + courseCode + "_" + phaseYear + "_" + rId +".pdf";

            System.IO.Directory.CreateDirectory(Server.MapPath("~/Download/NSQFConsolidatedMarksheet/" + corseCodeDirectoryName + "/" + phaseYearDirectoryName));
            string path = Server.MapPath("~/Download/NSQFConsolidatedMarksheet/" + corseCodeDirectoryName + "/" + phaseYearDirectoryName  + "/" + filename);

            if (System.IO.File.Exists(path))
                System.IO.File.Delete(path);           
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None));
                               
            doc.Open();
         
            Paragraph sp = new Paragraph("\n", new Font(Font.FontFamily.TIMES_ROMAN, 13));
            sp.Alignment = Element.ALIGN_CENTER;
            doc.Add(sp);
           
            string imageURL = Server.MapPath("~/images/NIELIT.jpg");
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imageURL);
            img.ScaleToFit(140f, 120f);
            img.SpacingBefore = 100f;
            img.Alignment = Element.ALIGN_CENTER;
            doc.Add(img);

                    
            string cName = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT DISTINCT CO.NAME FROM NSQF_CERT_PHASE_PRINT_DETAIL P, COURSEMAPPINGWITHNSQFCOURSECODE C ,COURSE CO WHERE C.COURSEID = CO.ID  AND P.COURSE_CODE= C.NSQFCOURSECODE AND P.registration_no = '" + rId + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));          

            Paragraph para = new Paragraph(" NATIONAL INSTITUTE OF ELECTRONICS AND INFORMATION TECHNOLOGY  \n CONSOLIDATED MARKSHEET ", new Font(Font.FontFamily.TIMES_ROMAN, 13));
            para.Alignment = Element.ALIGN_CENTER;
            doc.Add(para);
     
            Paragraph para1 = new Paragraph("\n  Course Name             :        " + cName, new Font(Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC));
            para1.Alignment = Element.ALIGN_LEFT;
            doc.Add(para1);

            PdfPTable tbl = new PdfPTable(1);
            tbl.WidthPercentage = 60;
            tbl.HorizontalAlignment = Element.ALIGN_LEFT;

            DataTable dtable = candidateInfo(rId);
            if (dtable.Rows.Count > 0)
            {
                foreach (DataRow dr in dtable.Rows)
                {
                    tbl.AddCell(GetCell1(new Phrase(new Chunk("Registration No         :      " + dr["REGISTRATION_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));
                    tbl.AddCell(GetCell1(new Phrase(new Chunk("Candidate Name       :      " + dr["NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));
                    tbl.AddCell(GetCell1(new Phrase(new Chunk("Father Name           :      " + dr["F_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));
                    tbl.AddCell(GetCell1(new Phrase(new Chunk("Mother Name          :    " + dr["M_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));
                    tbl.AddCell(GetCell1(new Phrase(new Chunk("Guardian Name      :      " + dr["G_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, iTextSharp.text.Font.BOLDITALIC)))));

                }
            }

            doc.Add(tbl);                 
            doc.Add(Chunk.NEWLINE);

            PdfPTable table = new PdfPTable(5);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 2, 0.8F, 0.6F, 0.6F, 0.6F });

            table.AddCell("Subject");
            table.AddCell("Roll No");
            table.AddCell("Exam Date");
            table.AddCell("Max Marks");
            table.AddCell("Marks Obtained");         

            DataTable dt2 = GetData(rId);
            if (dt2.Rows.Count > 0)
            {
                foreach (DataRow mod_row in dt2.Rows)
                {
                    table.AddCell(new Phrase(new Chunk(mod_row["MOD_TYPE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                    table.AddCell(new Phrase(new Chunk(mod_row["NSQF_ROLL_NO"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                    table.AddCell(new Phrase(new Chunk(mod_row["EXAM_DATE"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                    table.AddCell(new Phrase(new Chunk(mod_row["MAX_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                    table.AddCell(new Phrase(new Chunk(mod_row["TOTAL_MARKS"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))));
                }

                doc.Add(table);             

                string marksObtained = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select sum(Total_Marks) Marks_Obtained from NSQF_Module_Candidate_marks  where Registration_no =  '" + rId + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                string totalMarks = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("SELECT sum(( CASE WHEN MODULE_TYPE = 1 THEN THEORYPAPERMAXMARKS WHEN MODULE_TYPE = 3 THEN PRACTICALPAPERMAXMARKS WHEN MODULE_TYPE IN (4,7,8) THEN PROJECT_PRESENTATION_ASSIGNMENTMAXMARKS WHEN MODULE_TYPE = 6THEN INTERNALASSESSMENTMAXMARKS WHEN MODULE_TYPE IN (9,10) THEN  MAJORPROJECT_DISSERTATIONMAXMARKS END )) Total_Marks FROM NSQFTOTALPAPERS p,CourseMappingWithNSQFCoursecode t,NSQF_Module_Candidate_marks x where p.NSQFLearnHourType = t.NSQFLearnHourType and x.Course_id = t.CourseID   and  x.registration_no = '" + rId + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                Paragraph p1 = new Paragraph(" Total                :  "+ marksObtained + "/" + totalMarks  + "                                                                                                                                             Result : Pass ", new Font(Font.FontFamily.TIMES_ROMAN, 11));
                p1.Alignment = Element.ALIGN_LEFT;
                doc.Add(p1);

                string overallGrade = Convert.ToString(EConnect.Utils.Data.DbUtility.ExecuteScaller("select grade_code from NSQF_Cert_Phase_Print_Detail where registration_no =  '" + rId + "' ", new EConnect.Connections.SqlCon(), null, CommandType.Text, false));
                Paragraph p2 = new Paragraph("\n Overall Grade  :   " + overallGrade + " \n ", new Font(Font.FontFamily.TIMES_ROMAN, 11));
                p2.Alignment = Element.ALIGN_LEFT;
                doc.Add(p2);
                table.FlushContent();

                iTextSharp.text.pdf.draw.LineSeparator l1 = new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, BaseColor.BLACK, Element.ALIGN_LEFT, 2);
                doc.Add(l1);

                Paragraph p3 = new Paragraph(" Place :                                                                                                                                                                 QR Code :  \n  Date :", new Font(Font.FontFamily.TIMES_ROMAN, 10));
                p3.Alignment = Element.ALIGN_LEFT;
                doc.Add(p3);
                table.FlushContent();         
                doc.NewPage();

                doc.Close();

                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=" + filename);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.WriteFile(path);
                Response.Write(doc);
                Response.End();
                Response.Close();

            }
         
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }


}


