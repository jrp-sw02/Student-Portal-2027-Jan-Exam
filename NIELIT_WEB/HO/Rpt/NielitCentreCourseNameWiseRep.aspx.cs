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
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data.Objects;
using System.Data.OleDb;
using EConnect.Utils.Data;
using System.Security.Cryptography;

public partial class NielitCentreCourseNameWiseRep : BasePage
    {
    Table tbl = new Table();
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 currentRoleId = 0, UserTypeId;
    Int64 centreID = 0;
    DataSet ds = new DataSet();
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }

            currentRoleId = Convert.ToInt32(Session["RoleID"]);

            if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/Rpt/NielitCentreCourseNameWiseFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            loginUserType = (UserType)Session["UserType"]; 
            entityID = Convert.ToInt64(Session["EntityID"]);
            UserTypeId = Convert.ToInt32(Session["UserTypeId"]);            

            if (UserTypeId != 6)  // For Non Ho Users
            {
                centreID = Convert.ToInt64(Request.QueryString["centreID"]);
                if (entityID != centreID)
                {
                    Response.Write("Sorry! You don't have rights  to view this page");
                    Response.End();
                }
            }

            if (!Page.IsPostBack)
            {
                    tbl.CssClass = "sample3";
                    tbl.CellPadding = 2;
                    tbl.CellSpacing = 1;
                    tbl.Width = Unit.Percentage(100);
                    ShowData();
                    divReportData.Controls.Add(tbl);
            }
        }
        catch (Exception ex)
        {

        }
    }
    
    #region vCode

    protected void ShowTableHeader()
    {
        try
        {
            Int64 centreID, courseCat = 0, courseID = 0;
            DateTime FromDate, ToDate;
            string centreName = "", courseCatName = "", courseName = "";

            centreID = Convert.ToInt64(Request.QueryString["centreID"]);
            courseCat = Convert.ToInt64(Request.QueryString["courseCat"]);
            courseID = Convert.ToInt64(Request.QueryString["courseID"]);
            FromDate = Convert.ToDateTime(Request.QueryString["startDate"]);
            ToDate = Convert.ToDateTime(Request.QueryString["endDate"]);

            
                if (centreID != 0)
                {
                using (NIELITMISContext context = new NIELITMISContext())
                    {
                    centreName = (from s in context.NielitCentres
                                  where s.ID == centreID
                                  select new { CentreName = s.Name }).FirstOrDefault().CentreName;                    
                    }
                }
           
            if (courseCat != 0)
                {

                if (courseCat.ToString().Length > 2)
                    {
                    using (NIELITMISContext context = new NIELITMISContext())
                        {
                        courseCatName = (from k in context.NielitCentreCourseCategorys
                                         where k.ID == courseCat
                                         select new { courseCat1 = k.Name }).FirstOrDefault().courseCat1;

                        courseName = (from p in context.NielitCentreCourses
                                      join a in context.NielitCourseDurations on p.ID equals a.courseID
                                      where p.CourseCategoryID == courseCat && a.ID == courseID
                                      select new { cName = p.Name }).FirstOrDefault().cName;
                        }
                    }
                else
                    {
                    using (EConnectContext vContext = new EConnectContext())
                        {
                        courseCatName = (from c in vContext.CourseCategories
                                   where c.ID == courseCat
                                   select new { CatName = c.Name }).FirstOrDefault().CatName;

                        courseName = (from j in vContext.Courses
                                      where j.CourseCategoryID == courseCat && j.ID == courseID
                                      select new { cName = j.Name }).FirstOrDefault().cName;
                        }
                    }
                }

            int colSpanNo = 6;

            TableHeaderRow th11 = new TableHeaderRow();
            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(18);
            tcCol1.Text = "Centre Name : " + centreName;
            tcCol1.ColumnSpan = colSpanNo;
            tcCol1.HorizontalAlign = HorizontalAlign.Left;
            th11.Cells.Add(tcCol1);
            tbl.Rows.Add(th11);

            TableHeaderRow th02n = new TableHeaderRow();
            TableHeaderCell tcCol03n = new TableHeaderCell();
            tcCol03n.Width = Unit.Percentage(18);
            tcCol03n.Text = "Course Category : " + courseCatName;
            tcCol03n.HorizontalAlign = HorizontalAlign.Left;
            tcCol03n.ColumnSpan = colSpanNo;
            th02n.Cells.Add(tcCol03n);
            tbl.Rows.Add(th02n);

            TableHeaderRow th02c = new TableHeaderRow();
            TableHeaderCell tcCol03c = new TableHeaderCell();
            tcCol03c.Width = Unit.Percentage(18);
            tcCol03c.Text = "Course : " + courseName;
            tcCol03c.HorizontalAlign = HorizontalAlign.Left;
            tcCol03c.ColumnSpan = colSpanNo;
            th02c.Cells.Add(tcCol03c);
            tbl.Rows.Add(th02c);

            TableHeaderRow th01 = new TableHeaderRow();
            TableHeaderCell tcCol02 = new TableHeaderCell();
            tcCol02.Width = Unit.Percentage(18);
            tcCol02.Text = "Batch From Date : " + FromDate.ToString("dd-MMM-yyyy");
            tcCol02.ColumnSpan = colSpanNo;
            tcCol02.HorizontalAlign = HorizontalAlign.Left;
            th01.Cells.Add(tcCol02);
            tbl.Rows.Add(th01);

            TableHeaderRow th02 = new TableHeaderRow();
            TableHeaderCell tcCol03 = new TableHeaderCell();
            tcCol03.Width = Unit.Percentage(18);
            tcCol03.Text = "Batch To Date : " + ToDate.ToString("dd-MMM-yyyy") + "<br/>";
            tcCol03.ColumnSpan = colSpanNo;
            tcCol03.HorizontalAlign = HorizontalAlign.Left;
            th02.Cells.Add(tcCol03);
            tbl.Rows.Add(th02);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

                TableHeaderCell tcCol = new TableHeaderCell();
                tcCol.Width = Unit.Percentage(1);
                tcCol.Text = "#";
                tcCol.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol);

                TableHeaderCell tcCol2 = new TableHeaderCell();
                tcCol2.Width = Unit.Percentage(20);
                tcCol2.HorizontalAlign = HorizontalAlign.Center;
                tcCol2.Text = " Course ";
                th.Cells.Add(tcCol2);

                TableHeaderCell tcCol3 = new TableHeaderCell();
                tcCol3.Width = Unit.Percentage(8);
                tcCol3.HorizontalAlign = HorizontalAlign.Center;
                tcCol3.Text = "Course Type";
                th.Cells.Add(tcCol3);

                TableHeaderCell tcCol4 = new TableHeaderCell();
                tcCol4.Width = Unit.Percentage(8);
                tcCol4.Text = "Cast Category";
                tcCol4.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol4);

                TableHeaderCell tcCol5 = new TableHeaderCell();
                tcCol5.Width = Unit.Percentage(6);
                tcCol5.Text = "Gender";
                tcCol5.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol5);

                TableHeaderCell tcCol6 = new TableHeaderCell();
                tcCol6.Width = Unit.Percentage(8);
                tcCol6.Text = "Total Trained";
                tcCol6.HorizontalAlign = HorizontalAlign.Center;
                th.Cells.Add(tcCol6);

            tbl.Rows.Add(th);

        }
        catch (Exception ex)
        {
            //ShowAlert(ex.Message);
        }
    }

    protected void ShowData()
    {
        try
        {
            lblError.Visible = false;
            DateTime FromDate, ToDate;
            Int64 centreID, courseCat = 0, courseID = 0, noofTotalCandidates=0;

            centreID = Convert.ToInt64(Request.QueryString["centreID"]);
            courseCat = Convert.ToInt64(Request.QueryString["courseCat"]);
            courseID = Convert.ToInt64(Request.QueryString["courseID"]);
            FromDate = Convert.ToDateTime(Request.QueryString["startDate"]);
            ToDate = Convert.ToDateTime(Request.QueryString["endDate"]);

            using (ds = CourseNameWiseStudent(centreID, FromDate, ToDate, courseCat, courseID))
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                int j = 0;
                    ShowTableHeader();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        TableRow tr = new TableRow();
                        if (i % 2 == 0)
                            tr.CssClass = "gdalternate1";
                        else
                            tr.CssClass = "gdrow1";
                        j = i;
                        ds.Tables[0].NewRow();
                        TableCell tdRow = new TableCell();
                        tdRow.Width = Unit.Percentage(1);
                        tdRow.Text = (i + 1).ToString();
                        tdRow.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow);

                            TableCell tdRow1 = new TableCell();
                            tdRow1.Width = Unit.Percentage(20);
                            tdRow1.Text = ds.Tables[0].Rows[i]["CourseName"].ToString();
                            tdRow1.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow1);

                            TableCell tdRow2 = new TableCell();
                            tdRow2.Width = Unit.Percentage(10);
                            tdRow2.Text = ds.Tables[0].Rows[i]["CourseType"].ToString();
                            tdRow2.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow2);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(12);
                            tdRow3.Text = ds.Tables[0].Rows[i]["CastCategoty"].ToString();
                            tdRow3.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow3);

                            TableCell tdRow4 = new TableCell();
                            tdRow4.Width = Unit.Percentage(6);
                            tdRow4.Text = ds.Tables[0].Rows[i]["Gender"].ToString();
                            tdRow4.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow4);

                            TableCell tdRow5 = new TableCell();
                            tdRow5.Width = Unit.Percentage(10);
                            tdRow5.Text = ds.Tables[0].Rows[i]["TotalTrained"].ToString();
                            noofTotalCandidates += Convert.ToInt64(ds.Tables[0].Rows[i]["TotalTrained"]);
                            tdRow5.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow5);

                        tbl.Rows.Add(tr);
                    }

                    TableRow trTotal = new TableRow();
                    if (j % 2 == 0)
                        trTotal.CssClass = "gdrow1";
                    else
                        trTotal.CssClass = "gdalternate1";

                    TableCell tdNewRow1 = new TableCell();
                    tdNewRow1.Width = Unit.Percentage(1);
                    tdNewRow1.Text = "<b>Total</b>";
                    tdNewRow1.ColumnSpan = 5;
                    tdNewRow1.HorizontalAlign = HorizontalAlign.Center;
                    trTotal.Cells.Add(tdNewRow1);

                    TableCell tdNewRow5 = new TableCell();
                    tdNewRow5.Width = Unit.Percentage(1);
                    tdNewRow5.Text = "<b>" + noofTotalCandidates.ToString() + "</b>";
                    tdNewRow5.HorizontalAlign = HorizontalAlign.Center;
                    trTotal.Cells.Add(tdNewRow5);

                    tbl.Rows.Add(trTotal);
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
            // ShowAlert(ex.Message);
        }
        finally
        {
        }
    }

    public DataSet CourseNameWiseStudent(Int64 centreID, DateTime FromDate, DateTime ToDate, Int64 CourseCatId, Int64 CourseId)
    {
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
        using (SqlCommand cmd = new SqlCommand("RepCourseNameWiseStudentCount", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@centreId", SqlDbType.BigInt));
                cmd.Parameters["@centreId"].Value = centreID;
                cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.Date));
                cmd.Parameters["@FromDate"].Value = FromDate;
                cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.Date));
                cmd.Parameters["@ToDate"].Value = ToDate;
                cmd.Parameters.Add(new SqlParameter("@courseCatId", SqlDbType.BigInt));
                cmd.Parameters["@courseCatId"].Value = CourseCatId;
                cmd.Parameters.Add(new SqlParameter("@courseId", SqlDbType.BigInt));
                cmd.Parameters["@courseId"].Value = CourseId;

                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    sda.Fill(ds);
                }
            }
        }
        return ds;
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

            divpdf.Visible = true;
            tbl.BorderStyle = BorderStyle.Solid;
            tbl.CssClass = "sample3";
            tbl.CellPadding = 2;
            tbl.CellSpacing = 1;
            tbl.Width = Unit.Percentage(200);

            ShowData();
            divpdf.Controls.Add(tbl);

            divpdf.RenderControl(hw);
            hw.RenderEndTag();
            Response.Clear();
            StringReader sr = new StringReader(sw.ToString());
            // Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);

            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=NielitCentreCourseNameWiseReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            //  ShowAlert(ex.Message);
        }
    }
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
            divReportData.Visible = true;
            ShowData();
            divReportData.Controls.Add(tbl);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=NielitCentreCourseNameWiseReport.xls");
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

    #endregion
}