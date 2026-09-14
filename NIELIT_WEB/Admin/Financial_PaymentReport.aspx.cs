using EConnect;
using EConnect.DAL;
using EConnect.URM;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Financial_PaymentReport : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        UserType loginUserType;
        Int64 entityID = 0;
        //Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
        Int32 currentRoleId = 0;
        Int32 loginUserNo = 0;

        //if (!IsPostBack)
        //{

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
                FillGateways();

            }
            //}
            //else
            //{                 
            //    lblError.Text = "You can not download NSQF Course GST Report";
            //    lblError.Visible = true;
            //}

        }
        catch (Exception ex)
        {
            //lblError.Text = ex.Message;
            //lblError.Visible = true;
            ShowAlert(ex.Message);
        }
        // }

    }


    // added by amit start
    protected void FillGateways()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("-- ALL Gateways --", "0");
                var gateways = from p in context.Paymentgateways
                               orderby (p.Description)
                               select new { ValueField = p.ID, TextField = p.Description };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlgateway, gateways, lst);
            }
            ;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    // added by amit end

    private DataTable financialPayemntOnline(DateTime settledDate_From, DateTime settledDate_To)
    {
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("FinancialPayment_OnlineAshutosh"))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@Settled_On_from_date", SqlDbType.DateTime));
                cmd.Parameters["@Settled_On_from_date"].Value = settledDate_From;

                cmd.Parameters.Add(new SqlParameter("@Settled_On_to_date", SqlDbType.DateTime));
                cmd.Parameters["@Settled_On_to_date"].Value = settledDate_To;

                cmd.Parameters.Add(new SqlParameter("@gateway", SqlDbType.Int));
                cmd.Parameters["@gateway"].Value = ddlgateway.SelectedValue;


                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
        }

        return dt;
    }

    private DataTable financialPayemntNEFT(DateTime settledDate_From, DateTime settledDate_To)
    {
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("FinancialPayment_NEFT"))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@neft_Settled_On_from_date", SqlDbType.DateTime));
                cmd.Parameters["@neft_Settled_On_from_date"].Value = settledDate_From;

                cmd.Parameters.Add(new SqlParameter("@neft_Settled_On_to_date", SqlDbType.DateTime));
                cmd.Parameters["@neft_Settled_On_to_date"].Value = settledDate_To;

                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(dt);
                }
            }
        }

        return dt;
    }


    private DataTable financialPaymentCSC(DateTime settledDate_From, DateTime settledDate_To)
    {
        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);

        using (SqlConnection conn = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("FinancialPayment_CSC"))
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@csc_Settled_On_from_date", SqlDbType.DateTime));
                cmd.Parameters["@csc_Settled_On_from_date"].Value = settledDate_From;

                cmd.Parameters.Add(new SqlParameter("@csc_Settled_On_to_date", SqlDbType.DateTime));
                cmd.Parameters["@csc_Settled_On_to_date"].Value = settledDate_To;

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

   

    protected void btn_exptopdf_Click(object sender, EventArgs e)
    {
        //using (StringWriter sw = new StringWriter())
        //{
        //    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
        //    {
        //        //To Export all pages
        //        gdview.AllowPaging = false;
        //        this.BindGridView();

        //        gdview.RenderControl(hw);
        //        StringReader sr = new StringReader(sw.ToString());
        //        Document pdfDoc = new Document(PageSize.A2, 10f, 10f, 10f, 0f);
        //        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //        PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //        pdfDoc.Open();
        //        htmlparser.Parse(sr);
        //        pdfDoc.Close();

        //        Response.ContentType = "application/pdf";
        //        Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.pdf");
        //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        Response.Write(pdfDoc);
        //        Response.End();
        //    }

        try
        {

            Document doc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, Response.OutputStream);

            doc.Open();

            DateTime settledDate_From = Convert.ToDateTime(txtDateFrom.Text);
            DateTime settledDate_To = Convert.ToDateTime(txtDateTo.Text);


            if (Convert.ToInt32(ddlpaymentmode.SelectedValue) == 1)
            {

                Paragraph para = new Paragraph("NIELIT \n NSQF Course GST Report ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                para.Alignment = Element.ALIGN_CENTER;
                doc.Add(para);

                doc.Add(Chunk.NEWLINE);

                PdfPTable table = new PdfPTable(12);
                table.WidthPercentage = 100;
                float[] width = { 1, 1, 1, 1, 0.8F, 1, 1, 2, 1, 1, 0.8F, 1 };
                table.SetWidths(width);
                //table.SetWidths(new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1 });
                table.AddCell("Online Settled On Date ");
                table.AddCell("Exam Name");
                table.AddCell("Course Name");
                table.AddCell("Regional Centre Name");
                table.AddCell("Exam State Name");
                table.AddCell("Total Exam Fees All Paper");
                table.AddCell("Total Exam Fees Theory Paper");
                table.AddCell("Total Exam Fees Other Than Theory Paper Centre Share 100 Per");
                table.AddCell("total Exam Fees Theory Paper Center Share 75 Per");
                table.AddCell("Total Exam Fees Theory Paper HQ Share 25 Per");
                table.AddCell("Total Exam Fees Center Share Total With 75 Per And 100 Per");
                table.AddCell("Payment Gateway Used");

                DataTable DT = financialPayemntOnline(settledDate_From, settledDate_To);

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow dr in DT.Rows)
                    {
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["ONLINE_SETTLED_ON"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));

                        table.AddCell(GetCell(new Phrase(new Chunk(dr["EXAM_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["COURSE_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["REGIONALCENTRE_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["EXAM_STATE_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESALLPAPER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESTHPAPER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESOTHERTHENTHPAPERCENTERSHARE100PER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESTHPAPERCENTERSHARE75PER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESTHPAPERHQSHARE25PER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESCENTERSHARETOTALWITH75PERAND100PER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["PAYMENTGATEWAY"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL))))); // ADDED BY AMIT
                    }

                    doc.Add(table);
                }

                doc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=FinancialPayment_Online.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //Response.Write(doc);
                Response.Write(HttpUtility.HtmlEncode(doc));
                Response.End();
            }

            else if (Convert.ToInt32(ddlpaymentmode.SelectedValue) == 2)
            {
                Paragraph para = new Paragraph("NIELIT \n NSQF Course GST Report ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                para.Alignment = Element.ALIGN_CENTER;
                doc.Add(para);

                doc.Add(Chunk.NEWLINE);

                PdfPTable table = new PdfPTable(11);
                table.WidthPercentage = 100;
                float[] width = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                table.SetWidths(width);
                //table.SetWidths(new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1 });
                table.AddCell("NEFT Settled On Date ");
                table.AddCell("Exam Name");
                table.AddCell("Course Name");
                table.AddCell("Regional Centre Name");
                table.AddCell("Exam State Name");
                table.AddCell("Total Exam Fees All Paper");
                table.AddCell("Total Exam Fees Theory Paper");
                table.AddCell("Total Exam Fees Other Than Theory Paper Centre Share 100 Per");
                table.AddCell("total Exam Fees Theory Paper Center Share 75 Per");
                table.AddCell("Total Exam Fees Theory Paper HQ Share 25 Per");
                table.AddCell("Total Exam Fees Center Share Total With 75 Per And 100 Per");


                DataTable DT = financialPayemntNEFT(settledDate_From, settledDate_To);

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow dr in DT.Rows)
                    {
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["NEFT_SETTLED_ON"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["EXAM_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["COURSE_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["REGIONALCENTRE_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["EXAM_STATE_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESALLPAPER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESTHPAPER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESOTHERTHENTHPAPERCENTERSHARE100PER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESTHPAPERCENTERSHARE75PER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESTHPAPERHQSHARE25PER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESCENTERSHARETOTALWITH75PERAND100PER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                    }

                    doc.Add(table);
                }

                doc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=FinancialPayment_NEFT.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //                Response.Write(doc);
                Response.Write(HttpUtility.HtmlEncode(doc));
                Response.End();

            }

            // added by amit start 25-02-26
            else if (Convert.ToInt32(ddlpaymentmode.SelectedValue) == 3)
            {
                Paragraph para = new Paragraph("NIELIT \n NSQF Course GST Report ", new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 13));
                para.Alignment = Element.ALIGN_CENTER;
                doc.Add(para);

                doc.Add(Chunk.NEWLINE);

                PdfPTable table = new PdfPTable(11);
                table.WidthPercentage = 100;
                float[] width = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1};
                table.SetWidths(width);
                //table.SetWidths(new int[] { 1, 1, 1, 1, 1, 1, 1, 2, 1, 1, 1, 1 });
                table.AddCell("CSC Settled On Date ");
                table.AddCell("Exam Name");
                table.AddCell("Course Name");
                table.AddCell("Regional Centre Name");
                table.AddCell("Exam State Name");
                table.AddCell("Total Exam Fees All Paper");
                table.AddCell("Total Exam Fees Theory Paper");
                table.AddCell("Total Exam Fees Other Than Theory Paper Centre Share 100 Per");
                table.AddCell("total Exam Fees Theory Paper Center Share 75 Per");
                table.AddCell("Total Exam Fees Theory Paper HQ Share 25 Per");
                table.AddCell("Total Exam Fees Center Share Total With 75 Per And 100 Per");


                DataTable DT = financialPaymentCSC(settledDate_From, settledDate_To);

                if (DT.Rows.Count > 0)
                {
                    foreach (DataRow dr in DT.Rows)
                    {
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["CSC_SETTLED_ON"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["EXAM_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["COURSE_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["REGIONALCENTRE_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["EXAM_STATE_NAME"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESALLPAPER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESTHPAPER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESOTHERTHENTHPAPERCENTERSHARE100PER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESTHPAPERCENTERSHARE75PER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESTHPAPERHQSHARE25PER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                        table.AddCell(GetCell(new Phrase(new Chunk(dr["TOTALEXAMFEESCENTERSHARETOTALWITH75PERAND100PER"].ToString().Trim(), new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 9, iTextSharp.text.Font.NORMAL)))));
                    }

                    doc.Add(table);
                }

                doc.Close();
                Response.Buffer = true;
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-disposition", "attachment;filename=FinancialPayment_NEFT.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                //                Response.Write(doc);
                Response.Write(HttpUtility.HtmlEncode(doc));
                Response.End();

            }
            // added by amit end 25-02-26
            else
            {
                lblError.Visible = true;
                lblError.Text = " Please select the Payment Mode.";
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


    protected void ddlpaymentmode_SelectedIndexChanged(object sender, EventArgs e)
    {
        // added by amit start
        if (ddlpaymentmode.SelectedValue == "1")
        {
            // added by amit start
            ddlgateway.Visible = true;
            // added by amit end
        }
        else
        {
            //added by amit start
            ddlgateway.Visible = false;
            ddlgateway.ClearSelection();
            //added by amit end
        }
        // added by amit end
    }
}