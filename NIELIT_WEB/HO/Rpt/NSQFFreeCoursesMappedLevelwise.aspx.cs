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
using EConnect.DAL;
using EConnect.URM;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class HO_Rpt_NSQFFreeCoursesMappedLevelwise : BasePage
{

    Table tbl = new Table();
    UserType loginUserType;
    Int64 entityID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsSessionAlive())
        {
            Response.Redirect("~/index.aspx");
        }
        loginUserType = (UserType)Session["UserType"];
        entityID = Convert.ToInt64(Session["EntityID"]);

        if (!IsPostBack)
        {
            BindCourse();
        }
    }
    protected void BindCourse()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("-- All --", "0");
              
                var CourseList = from p in context.Courses
                                 where p.CourseCategoryID == 1
                                 orderby (p.Name)
                                 select new { ValueField = p.ID, TextField = p.Name };
                CourseList = CourseList.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourse, CourseList, lst);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

    }

    public DataTable FillGridViewRecord()
    {
        
        Int32 courseId = Convert.ToInt32(ddlCourse.SelectedValue);

        using (var context = new EConnectContext())
        {
          
            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            DataTable myDt = new DataTable();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("NSQFFreeCoursesMappedLevelwise", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@courseID", SqlDbType.Int));
                    cmd.Parameters["@courseID"].Value = courseId;

                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        sda.Fill(myDt);
                    }
                }
            }

            return myDt;
        }
    }

    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
        //if (ddlCourse.SelectedValue == "0")
        //{
        //    ShowAlert("Please select the course level!!");
        //    return;
        //}

        tbl.CssClass = "sample3";
        tbl.CellPadding = 2;
        tbl.CellSpacing = 1;
        tbl.Width = Unit.Percentage(100);
        imgPDF.Visible = true;
        BindGridView();

    }
    protected void PageIndexChanged(Int32 NewPageIndex)
    {
        try
        {
            gvMain.PageIndex = PagingBar1.CurrentPageIndex;
            BindGridView();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }

    protected void BindGridView()
    {
        try
        {
            using (DataTable dt = FillGridViewRecord())
            {
                if (dt.Rows.Count > 0)
                {
                    
                var ApplicationData = (from p in dt.AsEnumerable()
                                           select new
                                           {
                                              // ID = p.Field<Int64>("ID"),
                                               courseName = p.Field<String>("courseName"),
                                               mappedCourse = p.Field<String>("mappedCourse"),
                                               effectiveFrom = p.Field<String>("effectiveFrom"),
                                               effectiveTo = p.Field<String>("effectiveTo"),
                                               mappingApprovalDate = p.Field<String>("mappingApprovalDate"),
                                               eFileNo=p.Field <string >("eFileNo"),
                                               isReplacement = p.Field<Boolean>("isReplacement"),
                                               replacedCourse = p.Field<String>("replacedCourse")
                                           });

                     PagingBar1.Bind(ApplicationData, ref gvMain);
                     uPnlGrid.Update();
                     uPnlNavigation.Update();
                     lblError.Visible = false;
                     lblheading.Visible = true;
                     gvMain.Visible = true;
                     if (gvMain.Rows.Count <= 0)
                     {
                         lblError.Text = "No record found.";
                         lblError.Visible = true;
                         gvMain.Visible = false;
                         lblheading.Visible = true;
                     }
                }
                else
                 {
                     lblError.Text = "No record found.";
                     lblError.Visible = true;
                     gvMain.Visible = false;
                     lblheading.Visible = true;
                 }
            
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void ShowTableHeader()
    {

        try
        {

            TableHeaderRow th1 = new TableHeaderRow();
            th1.CssClass = "head1";

            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);
            tc1.ColumnSpan = 8;
            tc1.Text = "<font size = '4'><b>National Institute of Electronics and Information Technology (NIELIT)</b></font><br/>" + "<font size = '2'><b>NSQF Free Courses Mapped Level-Wise </b></font><p style='text-align:right;'>" + "Date: " + System.DateTime.Now.ToLongDateString() + "</p>";
            tc1.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tc1);
            tbl.Rows.Add(th1);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell thc = new TableHeaderCell();
            //thc.Width = Unit.Percentage(0);
            thc.HorizontalAlign = HorizontalAlign.Center;
            thc.Text = "#";
            th.Cells.Add(thc);


            TableHeaderCell thc0 = new TableHeaderCell();
            thc0.Width = Unit.Percentage(5);
            thc0.HorizontalAlign = HorizontalAlign.Center;
            thc0.Text = " Course Name";
            th.Cells.Add(thc0);

            TableHeaderCell thc1 = new TableHeaderCell();
            thc1.Width = Unit.Percentage(5);
            thc1.HorizontalAlign = HorizontalAlign.Center;
            thc1.Text = " Mapped Course Name";
            th.Cells.Add(thc1);

            TableHeaderCell thc2 = new TableHeaderCell();
            thc2.Width = Unit.Percentage(3);
            thc2.HorizontalAlign = HorizontalAlign.Center;
            thc2.Text = "Effective From Date";
            th.Cells.Add(thc2);

            TableHeaderCell thc3 = new TableHeaderCell();
            thc3.Width = Unit.Percentage(3);
            thc3.HorizontalAlign = HorizontalAlign.Center;
            thc3.Text = "Effective To Date";
            th.Cells.Add(thc3);

            TableHeaderCell thc4 = new TableHeaderCell();
            thc4.Width = Unit.Percentage(3);
            thc4.HorizontalAlign = HorizontalAlign.Center;
            thc4.Text = "Mapping Approval Date";
            th.Cells.Add(thc4);
            
            TableHeaderCell thc5 = new TableHeaderCell();
            thc5.Width = Unit.Percentage(3);
            thc5.HorizontalAlign = HorizontalAlign.Center;
            thc5.Text = "e File No.";
            th.Cells.Add(thc5);


            TableHeaderCell thc6 = new TableHeaderCell();
            thc6.Width = Unit.Percentage(3);
            thc6.HorizontalAlign = HorizontalAlign.Center;
            thc6.Text = "Whether Replaced";
            th.Cells.Add(thc6);

            TableHeaderCell thc7 = new TableHeaderCell();
            thc7.Width = Unit.Percentage(3);
            thc7.HorizontalAlign = HorizontalAlign.Center;
            thc7.Text = "Replaced Course Name";
            th.Cells.Add(thc7);

            tbl.Rows.Add(th);

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }


    protected void getData()
    {
        try
        {

            ShowTableHeader();           
            Int32 courseId = Convert.ToInt32(ddlCourse.SelectedValue);

            using (var context = new EConnectContext())
            {               
                string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                SqlCommand scCommand = new SqlCommand("NSQFFreeCoursesMappedLevelwise", new SqlConnection(con.ConnectionString));
                scCommand.CommandType = CommandType.StoredProcedure;

                scCommand.Parameters.Add(new SqlParameter("@courseID", SqlDbType.Int));
                scCommand.Parameters["@courseID"].Value = courseId;
                scCommand.CommandTimeout = 50000;
                if (scCommand.Connection.State == ConnectionState.Closed)
                {
                    scCommand.Connection.Open();
                }

                SqlDataAdapter da = new SqlDataAdapter(scCommand);
                DataSet ds = new DataSet();

                da.Fill(ds);
                int TotalRecord = 0;
                TotalRecord = Convert.ToInt32(ds.Tables[0].Rows.Count);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int i;
                    for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        TableRow tr = new TableRow();
                        if (i % 2 == 0)
                            tr.CssClass = "gdalternate1";
                        else
                            tr.CssClass = "gdrow1";

                        TableCell tdRow = new TableCell();
                       // tdRow.Width = Unit.Percentage(0);
                        tdRow.Text = (i + 1).ToString();
                        tdRow.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow);


                        TableCell tdRow1 = new TableCell();
                        tdRow1.Width = Unit.Percentage(3);
                        tdRow1.Text = ds.Tables[0].Rows[i]["courseName"].ToString();
                        tdRow1.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow1);

                        TableCell tdRow2 = new TableCell();
                        tdRow2.Width = Unit.Percentage(5);
                        tdRow2.Text = ds.Tables[0].Rows[i]["mappedCourse"].ToString();
                        tdRow2.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow2);

                        TableCell tdRow2a = new TableCell();
                        tdRow2a.Width = Unit.Percentage(3);
                        tdRow2a.Text = ds.Tables[0].Rows[i]["effectiveFrom"].ToString();
                        tdRow2a.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow2a);

                        TableCell tdRow3 = new TableCell();
                        tdRow3.Width = Unit.Percentage(3);
                        tdRow3.Text = ds.Tables[0].Rows[i]["effectiveTo"].ToString();
                        tdRow3.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow3);

                        TableCell tdRow16 = new TableCell();
                        tdRow16.Width = Unit.Percentage(3);
                        tdRow16.Text = ds.Tables[0].Rows[i]["mappingApprovalDate"].ToString();
                        tdRow16.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow16);

                        TableHeaderCell thc15 = new TableHeaderCell();
                        thc15.Width = Unit.Percentage(3);
                        thc15.HorizontalAlign = HorizontalAlign.Center;
                        thc15.Text = ds.Tables[0].Rows[i]["eFileNo"].ToString();
                        tr.Cells.Add(thc15);

                        TableCell tdRow4 = new TableCell();
                        tdRow4.Width = Unit.Percentage(3);
                        tdRow4.Text = ds.Tables[0].Rows[i]["isReplacement"].ToString();
                        tdRow4.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow4);


                        TableCell tdRow5 = new TableCell();
                        tdRow5.Width = Unit.Percentage(3);
                        tdRow5.Text = ds.Tables[0].Rows[i]["replacedCourse"].ToString();
                        tdRow5.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow5);

                        tbl.Rows.Add(tr);
                    }

                    TableRow trNew = new TableRow();
                    if (i % 2 == 0)
                        trNew.CssClass = "gdalternate1";
                    else
                        trNew.CssClass = "gdrow1";

                    TableCell tdNewRow2 = new TableCell();
                    tdNewRow2.Width = Unit.Percentage(8);
                    tdNewRow2.Height = Unit.Percentage(15);
                    tdNewRow2.Text = "<font size = '2'> <b>Total : </b>" + TotalRecord.ToString() + "</font>";
                    tdNewRow2.ColumnSpan = 5;
                    tdNewRow2.HorizontalAlign = HorizontalAlign.Left;
                    tdNewRow2.BorderWidth = 1;
                    tdNewRow2.BorderColor = System.Drawing.Color.White;
                    trNew.Cells.Add(tdNewRow2);

                    tbl.Rows.Add(trNew);

                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

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
            getData();
            divReportData.Controls.Add(tbl);
            divReportData.RenderControl(hw);
            hw.RenderEndTag();
            Response.Clear();
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 5f, 5f, 5f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=NSQFFreeMappedCoursesLevelWise.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}