using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.OleDb;
using System.IO;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using EConnect.URM;



public partial class HO_Rpt_NSQFCoursesGrantedtoInstitute : BasePage
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
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
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
    protected void btnShowDetails_Click(object sender, EventArgs e)
    {

        if (ddlCourse.SelectedValue == "0")
        {
            ShowAlert("Please select the course level!!");
            return;
        }

        if (txtAccrNo.Text == "")
        {
            ShowAlert("Please Enter Accrediation Number!!");
            return;
        }  
        Int32 courseId =Convert.ToInt32(ddlCourse.SelectedValue);
        string accrediationNumber = txtAccrNo.Text;

        string cs = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        DataTable dt = new DataTable();
        SqlConnection con = new SqlConnection(cs);
        SqlDataAdapter adapt = new SqlDataAdapter(" select  i.Name InstituteName , (i.Address1 + ' ' +  i.Address2 + ' ' + i.Address3 ) as InstituteAddress  , "+
                                                  " convert(varchar ,ia.Effective_To_Date, 106) AccrediationValidity , case   when ia.Accreditation_Status_ID = 1 then  'Full Extended'    when ia.Accreditation_Status_ID = 2 then  'Full'  when ia.Accreditation_Status_ID = 3 then  'Extended'" +
                                                  " when ia.Accreditation_Status_ID = 4 then  'Provisional'   when ia.Accreditation_Status_ID = 5 then  'Withdrawal'" +
                                                  " when ia.Accreditation_Status_ID = 6 then  'Acknowledged' when ia.Accreditation_Status_ID = 7 then  'Rejected' "+
                                                  " when ia.Accreditation_Status_ID = 8 then  'Deferred' else 'Full Provisional'  end as   InstituteStatus  from  Institute i  inner  join   Intitute_Accreditation_Detail ia on i.ID = ia.Institute_ID   where " +
                                                  " ia.course_Category_ID = 1    and ia.Course_ID = '" + courseId + "' and ia.Accreditation_Number ='" + accrediationNumber + "'  order by ia.ID desc ", con);
        con.Open();
        adapt.Fill(dt);
        con.Close();
        if (dt.Rows.Count > 0)
        {
            grdInstituteDetails.DataSource = dt;
            grdInstituteDetails.DataBind();
            btnViewReport.Visible = true;
        }                       
    }
    protected void btnViewReport_Click(object sender, EventArgs e)
    {
        tbl.CssClass = "sample3";
        tbl.CellPadding = 2;
        tbl.CellSpacing = 1;
        tbl.Width = Unit.Percentage(100);
        imgPDF.Visible = true;
        BindGridView();
    }

    public DataTable FillGridViewRecord()
    {
        string accrediationNumber = txtAccrNo.Text;
        Int32 courseId = Convert.ToInt32(ddlCourse.SelectedValue) ;

        using (var context = new EConnectContext())
        {
            var searchInstitute = (from i in context.AccreditationDetails
                                   where i.AccreditationNumber == accrediationNumber
                                   select new { Id = i.InstituteID }).FirstOrDefault();
            Int64 InstituteId = Convert.ToInt64(searchInstitute.Id);


            string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
            DataTable myDt = new DataTable();
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("getNSQFFreeCoursesGranted_rep", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@instituteId", SqlDbType.Int));
                    cmd.Parameters["@instituteId"].Value = InstituteId;
                    cmd.Parameters.Add(new SqlParameter("@courseId", SqlDbType.Int));
                    cmd.Parameters["@courseId"].Value = courseId;

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
    //protected void gvMain_Sorting(object sender, GridViewSortEventArgs e)
    //{
    //    try
    //    {
    //        ViewState["SortField"] = e.SortExpression;
    //        if (ViewState["SortOrder"].ToString() == "DESC")
    //            ViewState["SortOrder"] = "ASC";
    //        else
    //            ViewState["SortOrder"] = "DESC";
    //        BindGridView();
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message, true);
    //    }
    //}
    //protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {

    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            //Encryption url of hypelink field
    //            HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
    //            string href = hl.NavigateUrl;
    //            if (!String.IsNullOrEmpty(Request.QueryString["ID"]))
    //            {
    //                href += "&ID=" + Request.QueryString["ID"].ToString();
    //            }
    //            e.Row.Cells[0].Text = ((e.Row.RowIndex + 1) + (PagingBar1.CurrentPageSize * PagingBar1.CurrentPageIndex)).ToString();

    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
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
    protected void ShowTableHeader()
    {
       
        try
        {
           
            TableHeaderRow th1 = new TableHeaderRow();
            th1.CssClass = "head1";

            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);
            tc1.ColumnSpan = 6;
            tc1.Text = "<font size = '4'><b>National Institute of Electronics and Information Technology (NIELIT)</b></font><br/>" + "<font size = '2'><b>NSQF Free Courses Granted </b></font><p style='text-align:right;'>" + "Date: " + System.DateTime.Now.ToLongDateString() + "</p>";
            tc1.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tc1);
            tbl.Rows.Add(th1);

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";
         
            TableHeaderCell thc = new TableHeaderCell();
            thc.Width = Unit.Percentage(3);
            thc.HorizontalAlign = HorizontalAlign.Center;
            thc.Text = "#";
            th.Cells.Add(thc);

            TableHeaderCell thc1 = new TableHeaderCell();
            thc1.Width = Unit.Percentage(3);
            thc1.HorizontalAlign = HorizontalAlign.Center;
            thc1.Text = "Course Name";
            th.Cells.Add(thc1);

            TableHeaderCell thc2 = new TableHeaderCell();
            thc2.Width = Unit.Percentage(3);
            thc2.HorizontalAlign = HorizontalAlign.Center;
            thc2.Text = "Grant Date";
            th.Cells.Add(thc2);

            TableHeaderCell thc3 = new TableHeaderCell();
            thc3.Width = Unit.Percentage(3);
            thc3.HorizontalAlign = HorizontalAlign.Center;
            thc3.Text = "Active";
            th.Cells.Add(thc3);

            TableHeaderCell thc4 = new TableHeaderCell();
            thc4.Width = Unit.Percentage(3);
            thc4.HorizontalAlign = HorizontalAlign.Center;
            thc4.Text = "eFileNo";
            th.Cells.Add(thc4);

            TableHeaderCell thc6 = new TableHeaderCell();
            thc6.Width = Unit.Percentage(3);
            thc6.HorizontalAlign = HorizontalAlign.Center;
            thc6.Text = "Approval Date";
            th.Cells.Add(thc6);

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
          
            string accrediationNumber = txtAccrNo.Text;
            Int32 courseId = Convert.ToInt32(ddlCourse.SelectedValue);

            using (var context = new EConnectContext())
            {
                var searchInstitute = (from i in context.AccreditationDetails
                                       where i.AccreditationNumber == accrediationNumber
                                       select new { Id = i.InstituteID }).FirstOrDefault();
                Int64 InstituteId = Convert.ToInt64(searchInstitute.Id);


                string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                SqlCommand scCommand = new SqlCommand("getNSQFFreeCoursesGranted_rep", new SqlConnection(con.ConnectionString));
                scCommand.CommandType = CommandType.StoredProcedure;

                scCommand.Parameters.Add(new SqlParameter("@instituteId", SqlDbType.Int));
                scCommand.Parameters["@instituteId"].Value = InstituteId;
                scCommand.Parameters.Add(new SqlParameter("@courseId", SqlDbType.Int));
                scCommand.Parameters["@courseId"].Value = courseId;
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
                        tdRow.Width = Unit.Percentage(1);
                        tdRow.Text = (i + 1).ToString();
                        tdRow.HorizontalAlign = HorizontalAlign.Center;
                        tr.Cells.Add(tdRow);

                        TableCell tdRow2 = new TableCell();
                        tdRow2.Width = Unit.Percentage(4);
                        tdRow2.Text = ds.Tables[0].Rows[i]["ID"].ToString();
                        tdRow2.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow2);

                        TableCell tdRow2a = new TableCell();
                        tdRow2a.Width = Unit.Percentage(4);
                        tdRow2a.Text = ds.Tables[0].Rows[i]["grantDate"].ToString();
                        tdRow2a.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow2a);

                        TableCell tdRow3 = new TableCell();
                        tdRow3.Width = Unit.Percentage(5);
                        tdRow3.Text = ds.Tables[0].Rows[i]["isActive"].ToString();
                        tdRow3.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow3);

                        TableCell tdRow16 = new TableCell();
                        tdRow16.Width = Unit.Percentage(5);
                        tdRow16.Text = ds.Tables[0].Rows[i]["eFileNo"].ToString();
                        tdRow16.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow16);

                        TableCell tdRow4 = new TableCell();
                        tdRow4.Width = Unit.Percentage(25);
                        tdRow4.Text = ds.Tables[0].Rows[i]["approvalDate"].ToString();
                        tdRow4.HorizontalAlign = HorizontalAlign.Left;

                        tr.Cells.Add(tdRow4);

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
            Response.AddHeader("content-disposition", "attachment;filename=NSQFFreeCoursesGrantedReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
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
                                               ID = p.Field<String>("ID"),
                                               grantDate = p.Field<String>("grantDate"),
                                               isActive = p.Field<String>("isActive"),
                                               eFileNo = p.Field<String>("eFileNo"),
                                               approvalDate = p.Field<String>("approvalDate")
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
}