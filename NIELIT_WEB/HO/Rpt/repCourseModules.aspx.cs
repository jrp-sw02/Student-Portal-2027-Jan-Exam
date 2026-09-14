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
using DocumentFormat.OpenXml.Wordprocessing;
using EConnect.DAL;
using EConnect.URM;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class HO_Rpt_repCourseModules : BasePage
{

    string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    System.Web.UI.WebControls.Table tbl = new System.Web.UI.WebControls.Table(); 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("~/index.aspx");
            if (!Page.IsPostBack)
                FillCourseCategory();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    private DataTable GetCourseModulesReport()
    {
        DataTable dt = new DataTable();
        Int64 courseCategory = Convert.ToInt64(ddlCourseCategoryName.SelectedValue);
        Int64 course;
        if (courseCategory == -99)
            course = -99;
        else
            course = Convert.ToInt64(ddlCourses.SelectedValue);
        

        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("repCourseModules", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pCourseCatID", courseCategory);
                cmd.Parameters.AddWithValue("@pCourseID", course);

                con.Open();

                using (SqlDataAdapter adpt = new SqlDataAdapter(cmd))
                {
                    adpt.Fill(dt);
                }
            }
        }

        return dt;
    }
    protected void ShowTableHeader()
    {
        try
        {

            System.Web.UI.WebControls.TableHeaderCell tc1 = new System.Web.UI.WebControls.TableHeaderCell();
            tc1.Width = Unit.Percentage(100);


            System.Web.UI.WebControls.TableHeaderRow th1 = new System.Web.UI.WebControls.TableHeaderRow();
            TableHeaderCell tcCol11 = new TableHeaderCell();
            tcCol11.ColumnSpan = 15;
            tcCol11.Width = Unit.Percentage(18);
            tcCol11.Text = "Course Module Report ";
            tcCol11.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tcCol11);
            tbl.Rows.Add(th1);


            //TableHeaderRow t1 = new TableHeaderRow();
            //TableHeaderCell tcCol1 = new TableHeaderCell();
            //tcCol1.ColumnSpan = 11;
            //tcCol1.Width = Unit.Percentage(18);
            //tcCol1.Text = "Exam Session: ";
            //tcCol1.HorizontalAlign = HorizontalAlign.Center;
            //t1.Cells.Add(tcCol1);
            //tbl.Rows.Add(t1);

            System.Web.UI.WebControls.TableHeaderRow th = new System.Web.UI.WebControls.TableHeaderRow();
            th.CssClass = "head1";

            System.Web.UI.WebControls.TableHeaderCell thc = new System.Web.UI.WebControls.TableHeaderCell();
            thc.ColumnSpan = 1;
            thc.Width = Unit.Pixel(10);
            thc.HorizontalAlign = HorizontalAlign.Center;
            thc.Text = "SrNo.";
            th.Cells.Add(thc);

            System.Web.UI.WebControls.TableHeaderCell thc1 = new System.Web.UI.WebControls.TableHeaderCell();
            thc1.ColumnSpan = 4;
            thc1.Width = Unit.Percentage(3);
            thc1.HorizontalAlign = HorizontalAlign.Center;
            thc1.Text = "Name of Course";
            th.Cells.Add(thc1);

            System.Web.UI.WebControls.TableHeaderCell thc2 = new System.Web.UI.WebControls.TableHeaderCell();
            thc2.ColumnSpan = 6;
            thc2.Width = Unit.Percentage(3);
            thc2.HorizontalAlign = HorizontalAlign.Center;
            thc2.Text = "Module Name";
            th.Cells.Add(thc2);


            System.Web.UI.WebControls.TableHeaderCell thc3 = new System.Web.UI.WebControls.TableHeaderCell();
            thc3.ColumnSpan = 4;
            thc3.Width = Unit.Percentage(3);
            thc3.HorizontalAlign = HorizontalAlign.Center;
            thc3.Text = "Module Type";
            th.Cells.Add(thc3);

            //TableHeaderCell thc18 = new TableHeaderCell();
            //thc18.Width = Unit.Percentage(3);
            //thc18.HorizontalAlign = HorizontalAlign.Center;
            //thc18.Text = "BSB Code";
            //th.Cells.Add(thc18);

          
            tbl.Rows.Add(th);
            divReportData.Controls.Add(tbl);
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowData()
    {
        // int i = 0;

        // DataTable dt1 = GetCompiledResult();
        try
        {
            using (DataTable dt1 = GetCourseModulesReport())
                {
                    if (dt1.Rows.Count > 0)
                    {
                        var application = (from a in dt1.AsEnumerable()
                                           select new
                                           {
                                               Name_of_Course = a.Field<string>("Name of Course") ?? "",
                                               Module_Name = a.Field<string>("Module Name"),
                                               Module_Type = a.Field<string>("Module Type") ?? ""
                                                                                             
                                           }).ToList();

                        if (application.Count() > 0)
                        {

                            ShowTableHeader();

                            int i = 0;

                            //  if (dt1.Rows.Count > 0)
                            // {
                            foreach (var app in application)
                            {
                            System.Web.UI.WebControls.TableRow tr = new System.Web.UI.WebControls.TableRow();
                                if (i % 2 == 0)
                                    tr.CssClass = "gdalternate1";
                                else
                                    tr.CssClass = "gdrow1";
                                i++;

                                TableHeaderCell thc = new TableHeaderCell();
                                thc.ColumnSpan = 1;
                                thc.Width = Unit.Percentage(3);
                                thc.HorizontalAlign = HorizontalAlign.Center;
                                thc.Text = i.ToString();
                                tr.Cells.Add(thc);

                            System.Web.UI.WebControls.TableCell thc1 = new System.Web.UI.WebControls.TableCell();
                                thc1.ColumnSpan = 4;
                                thc1.Width = Unit.Percentage(3);
                                thc1.HorizontalAlign = HorizontalAlign.Center;
                                thc1.Text = app.Name_of_Course.ToString();
                                tr.Cells.Add(thc1);

                            System.Web.UI.WebControls.TableCell thc2 = new System.Web.UI.WebControls.TableCell();
                                thc2.ColumnSpan = 6;
                                thc2.Width = Unit.Percentage(3);
                                thc2.HorizontalAlign = HorizontalAlign.Center;
                                thc2.Text = app.Module_Name.ToString();
                                tr.Cells.Add(thc2);

                            System.Web.UI.WebControls.TableCell thc3 = new System.Web.UI.WebControls.TableCell();
                                thc3.ColumnSpan = 4;
                                thc3.Width = Unit.Percentage(3);
                                thc3.HorizontalAlign = HorizontalAlign.Center;
                                thc3.Text = app.Module_Type.ToString();
                                tr.Cells.Add(thc3);

                                tbl.Rows.Add(tr);
                                divReportData.Controls.Add(tbl);
                                 lblError.Visible = false;
                            }
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "No Record Found";
                        }
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No Record Found";
                    }
                }

            }

     
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }
    protected void btnView_Click(object sender, EventArgs e)
    {
        ShowData();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        ddlCourseCategoryName.SelectedValue = "-99";
        ddlCourses.SelectedValue = "-99";
    }
    protected void imgPDF_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            hw.AddAttribute("border", "1");
            hw.RenderBeginTag(HtmlTextWriterTag.Font);
            hw.AddStyleAttribute(HtmlTextWriterStyle.FontSize, "9");


            divReportData.Visible = true;
            tbl.BorderStyle = BorderStyle.Solid;
            tbl.CssClass = "sample3";
            tbl.CellPadding = 2;
            tbl.CellSpacing = 1;
            tbl.Width = Unit.Percentage(200);

            ShowData();
            divReportData.Controls.Add(tbl);

            divReportData.RenderControl(hw);
            hw.RenderEndTag();
            Response.Clear();
            StringReader sr = new StringReader(sw.ToString());
            iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
           
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=CourseModulesReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                     
            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
            divReportData.Visible = true;
            //ShowTableHeader();
            ShowData();
            divReportData.Controls.Add(tbl);
            Response.Clear();         
            Response.AddHeader("content-disposition", "attachment;filename=CourseModulesReport.xls");         
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            htmlWrite = new Html32TextWriter(StringWrite);
            divReportData.RenderControl(htmlWrite);
            Response.Write(StringWrite.ToString());
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }


    protected void FillCourseCategory()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "-99");
                var CourseList = from p in context.CourseCategories
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseCategoryName, CourseList, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }



    protected void ddlCourseCategoryName_SelectedIndexChanged(object sender, EventArgs e)
    {
        int id2 = 0;
        id2 = Convert.ToInt32(ddlCourseCategoryName.SelectedValue);
        ddlCourses.Items.Clear();
        FillCourses(id2);
    }

    protected void FillCourses(int catID)
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select--", "-99");
                if (catID != null || catID != 0)
                {
                        var CourseName = from p in context.Courses
                                         where p.CourseCategoryID == catID
                                         orderby (p.Name)
                                         select new { ValueField = p.ID, TextField = p.Name };
                        EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourses, CourseName, lst);
                }
            };
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}