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
using System.Data .SqlClient ;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web;
using System.Configuration;
using System.Collections.Generic;

using System.Transactions;
using EConnect.Utils.Common;
using System.Drawing;

public partial class NSQFAccrGrantedReplacementRpt : BasePage
{
    Table tbl = new Table();
    
    EConnectContext context;
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 applicantTypeID = 0, currentRoleId = 0, UserTypeId=0;
    Int64 centreID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

        if (!IsSessionAlive())
            {
            Response.Redirect("~/index.aspx");
            }

        currentRoleId = Convert.ToInt32(Session["RoleID"]);
        ShowTableHeader();
        if (!UserManager.HasRight(currentRoleId, enmRight.View, "HO/Rpt/NSQFAccrGrantedReplacementRpt.aspx"))
            {
            //for testing by vishal
            Response.Write("Sorry! You don't have rights  to view this page");
            Response.End();
            }

        loginUserType = (UserType)Session["UserType"];
        entityID = Convert.ToInt64(Session["EntityID"]);
        UserTypeId = Convert.ToInt32(Session["UserTypeId"]);

        if (UserTypeId != 6)  // For Non Ho Users
            {
            Response.Write("Sorry! You don't have rights  to view this page");
            Response.End();
            }

            if (!Page.IsPostBack)
            {
            BindNSQFCourse();

                //FillCategories();
                //ddlcoursecategory.SelectedValue = "1";
                //FillCourses();
                //BindNSQFCourse();

                //tbl.CssClass = "sample3";
                //tbl.BorderStyle  = BorderStyle .Solid ;  
                //tbl.CellPadding = 2;
                //tbl.CellSpacing = 1;
                //tbl.Width = Unit.Percentage(100);
                //divReportData.Controls.Add(tbl);                
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }        
    
    #region vCode
    protected void BindNSQFCourse()
        {
        try
            {

            int courseCatId = 6;  // only for NSQF Courses 
            using (var context = new EConnectContext())
                {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");


                var courses = from s in context.Courses
                              join m in context.NSQFFreeCourseMapping on s.ID equals m.mappedCourseID
                              where s.CourseCategoryID == courseCatId && m.isReplacement == true
                              select new { ValueField = s.ID, TextField = s.Name + " (" + s.Code + ")" };
                courses = courses.Distinct();
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCourseName, courses, lst);
                };

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
            int ColumnSpanNo = 9;
            TableHeaderRow th1 = new TableHeaderRow();
            th1.CssClass = "head1";

            TableHeaderCell tc1 = new TableHeaderCell();
            tc1.Width = Unit.Percentage(100);
            tc1.ColumnSpan = ColumnSpanNo;

            string courseName = "";
            courseName = ddlCourseName.SelectedItem.ToString();
            tc1.Text = "NSQF Accreditation Granted Replacement Report For - " + courseName;


            tc1.HorizontalAlign = HorizontalAlign.Center;
            th1.Cells.Add(tc1);

            tbl.Rows.Add(th1);


            TableHeaderRow th2 = new TableHeaderRow();
            th2.CssClass = "head1";

            TableHeaderCell tc2 = new TableHeaderCell();
            tc2.Width = Unit.Percentage(100);
            tc2.ColumnSpan = ColumnSpanNo;

            tc2.ColumnSpan = ColumnSpanNo;
            tc2.Text = "Report Generated on: " + System.DateTime.Now.ToLongDateString();
            tc2.HorizontalAlign = HorizontalAlign.Right;
            th2.Cells.Add(tc2);

            tbl.Rows.Add(th2);


            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.Text = "#";
            tcCol.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(12);
            tcCol2.Text = "Accr No.";
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(15);
            tcCol6.Text = "Institute Name";
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol6);

            TableHeaderCell tcCol66 = new TableHeaderCell();
            tcCol66.Width = Unit.Percentage(20);
            tcCol66.Text = "Address";
            tcCol66.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol66);

            TableCell tdRow113 = new TableCell();
            tdRow113.Width = Unit.Percentage(10);
            tdRow113.Text = "Grant Date";
            tdRow113.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tdRow113);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(10);
            tcCol3.Text = "Effective To Date ";
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(10);
            tcCol5.Text = "E-File No";
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(10);
            tcCol7.Text = "Approval Date";
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol7);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(10);
            tcCol4.Text = "Remarks";
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol4);

            tbl.Rows.Add(th);
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }
    protected void ShowData()
        {
        try
            {
            if (ddlCourseName.SelectedValue.ToString() == "0")
                {
                ShowAlert("Please, Select Course Name");
                return;
                }
            //Int64 gtot = 0;
            using (EConnectContext context = new EConnectContext())
                {
                Int64 courseId = Convert.ToInt64(ddlCourseName.SelectedValue);
                var InstDetails = (from m in context.NSQFFreeCourseMapping
                                   join g in context.NSQFFreeCourseGrants on m.mappedCourseID equals g.mappedCourseID
                                   join i in context.AccreditationDetails on g.mappedCourseID equals i.CourseID
                                   join ii in context.Institutes on i.InstituteID equals ii.ID
                                   join l in context.Locations on ii.StateID equals l.ID
                                   where m.mappedCourseID == g.mappedCourseID && m.isReplacement == true && m.CourseID == g.accrCourseID
                                   && m.mappedCourseID == courseId
                                   && g.isActive == true && g.InstituteID == i.InstituteID
                                   && i.CourseID == g.mappedCourseID && i.CourseCategoryID == 6 // only for NSQF Courses
                                   && (i.AccreditationStatusID != 5 || i.AccreditationStatusID != 6 || i.AccreditationStatusID != 7 || i.AccreditationStatusID != 8)
                                   select new
                                   {
                                       AccrNo = i.AccreditationNumber,
                                       InstName = ii.Name,
                                       InstAdd = ii.AddressLine1 + " " + ii.AddressLine2 + " " + ii.AddressLine3 + " " + ii.CityName + " (" + l.Name + ") " + ii.PinCode,
                                       GrantDate = g.grantDate,
                                       EffToDate = i.EffectiveToDate,
                                       eFileNo = g.eFileNo,
                                       approvalDate = g.approvalDate,
                                       remarks = g.Remarks
                                   }).ToList();

                if (InstDetails.Count() > 0)
                    {
                    lblError.Visible = false;
                    lblError.Text = "";
                    int i = 0;
                    imgPDF.Visible = true;
                    ibExport.Visible = true;
                    imPrint.Visible = true;

                    //gtot = Convert.ToInt64(InstDetails.Count());
                    for (i = 0; i < InstDetails.Count(); i++)
                        {
                        string accrNoForReplacement = "", accrNo3rdChar = "";                        
                        accrNoForReplacement = InstDetails[i].AccrNo.ToString();
                        accrNo3rdChar = accrNoForReplacement.Substring(2, 1);

                        if (accrNo3rdChar == "R")
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
                            tdRow2.Width = Unit.Percentage(12);
                            tdRow2.Text = InstDetails[i].AccrNo.ToString();
                            tdRow2.HorizontalAlign = HorizontalAlign.Left;
                            tr.Cells.Add(tdRow2);

                            TableCell tdRow3 = new TableCell();
                            tdRow3.Width = Unit.Percentage(15);
                            tdRow3.Text = InstDetails[i].InstName.ToString();
                            tdRow3.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow3);

                            TableCell tdRow33 = new TableCell();
                            tdRow33.Width = Unit.Percentage(20);
                            string s = InstDetails[i].InstAdd.ToString();
                            int index = s.LastIndexOf(',');
			if(index>0)
                            tdRow33.Text = s.Remove(index, 1);
			else
				tdRow33.Text=s;
                            tdRow33.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow33);

                            TableCell tdRow16 = new TableCell();
                            tdRow16.Width = Unit.Percentage(10);
                            DateTime grantDate = Convert.ToDateTime(InstDetails[i].GrantDate);
                            tdRow16.Text = grantDate.ToString("dd/MMM/yyyy");
                            tdRow16.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow16);

                            TableCell tdRow4 = new TableCell();
                            tdRow4.Width = Unit.Percentage(10);
                            DateTime effTodate = Convert.ToDateTime(InstDetails[i].EffToDate);
                            tdRow4.Text = effTodate.ToString("dd/MMM/yyyy");
                            tdRow4.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow4);

                            TableCell tdRow7 = new TableCell();
                            tdRow7.Width = Unit.Percentage(5);
                            tdRow7.Text = InstDetails[i].eFileNo.ToString();
                            tdRow7.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow7);

                            TableCell tdRow8 = new TableCell();
                            tdRow8.Width = Unit.Percentage(10);
                            DateTime appDate = Convert.ToDateTime(InstDetails[i].approvalDate);
                            tdRow8.Text = appDate.ToString("dd/MMM/yyyy");
                            tdRow8.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow8);

                            TableCell tdRow5 = new TableCell();
                            tdRow5.Width = Unit.Percentage(10);
                            tdRow5.Text = InstDetails[i].remarks.ToString();
                            tdRow5.HorizontalAlign = HorizontalAlign.Center;
                            tr.Cells.Add(tdRow5);

                            tbl.Rows.Add(tr);


                            }
                        }

                    // For Total Records
                    //TableHeaderRow th1 = new TableHeaderRow();
                    //th1.CssClass = "head1";

                    //TableHeaderCell tc1 = new TableHeaderCell();
                    //tc1.Width = Unit.Percentage(100);
                    //tc1.ColumnSpan = 9;

                    //string courseName = "";
                    //courseName = ddlCourseName.SelectedItem.ToString();
                    //tc1.Text = "Total Record - " + gtot.ToString().Trim();

                    //tc1.HorizontalAlign = HorizontalAlign.Left;
                    //th1.Cells.Add(tc1);

                    //tbl.Rows.Add(th1);
                    }
                else
                    {
                    lblError.Visible = true;
                    lblError.Text = "No Record Found !";
                    lblheading.Visible = true;
                    lblheadingCandDetails.Visible = false;
                    imgPDF.Visible = false;
                    ibExport.Visible = false;
                    imPrint.Visible = true;
                    }
                };
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }       
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
        {
        if (ddlCourseName.SelectedValue.ToString() == "0")
            {
            ShowAlert("Please, Select Course Name");
            return;
            }

        try
            {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;

            divReportData.Visible = true;
            ShowData();
            divReportData.Controls.Add(tbl);
            Response.Clear();

            Response.AddHeader("content-disposition", "attachment;filename=NSQFAccrGrantedReplacementRpt.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.xls";
            htmlWrite = new Html32TextWriter(StringWrite);
            htmlWrite.AddAttribute("border", "2");
            divReportData.RenderControl(htmlWrite);
            Response.Write(StringWrite.ToString());
            Response.End();
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }

        }
    protected void imgPDF_Click(object sender, ImageClickEventArgs e)
        {
        if (ddlCourseName.SelectedValue.ToString() == "0")
            {
            ShowAlert("Please, Select Course Name");
            return;
            }

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
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 5f, 5f, 5f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=NSQFAccrGrantedReplacementRpt.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Write(pdfDoc);
            Response.End();
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message);
            }
        }
    protected void btnReportChoice_Click(object sender, EventArgs e)
        {

        if (ddlCourseName.SelectedValue.ToString() == "0")
            {
            ShowAlert("Please, Select Course Name");
            return;
            }

        lblheading.Visible = true;
        tbl.CssClass = "sample3";
        tbl.CellPadding = 2;
        tbl.CellSpacing = 1;
        tbl.Width = Unit.Percentage(100);
        ShowData();
        divReportData.Controls.Add(tbl);
        divReportData.Visible = true;
        } 
    protected void btnReset_Click(object sender, EventArgs e)
        {
        Response.Redirect("~/HO/Rpt/NSQFAccrGrantedReplacementRpt.aspx");
        }
    #endregion


    #region Others
    protected void RdoNSQFCourseRptChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
        try
            {
            //lblheading.Visible = false;
            //lblError.Visible = false;
            //if (RdoNSQFCourseRptChoice.SelectedValue == "4")
            //    {
            //    ddlCourseName.Enabled = true;
            //    // ddlcourse.SelectedValue = "0";
            //    txtPaymentFromDate.Enabled = false;
            //    txPaymentToDate.Enabled = false;
            //    txtPaymentFromDate.Text = "";
            //    txPaymentToDate.Text = "";
            //    }
            //else if (RdoNSQFCourseRptChoice.SelectedValue == "5")
            //    {
            //    ddlCourseName.Enabled = false;
            //    BindNSQFCourse();
            //    txtPaymentFromDate.Enabled = true;
            //    txPaymentToDate.Enabled = true;
            //    }
            //else
            //    {
            //    ddlCourseName.Enabled = false;
            //    BindNSQFCourse();
            //    txtPaymentFromDate.Enabled = false;
            //    txPaymentToDate.Enabled = false;
            //    txtPaymentFromDate.Text = "";
            //    txPaymentToDate.Text = "";
            //    }
            }
        catch (Exception ex)
            {
            ShowAlert(ex.Message.ToString() + ex.Source.ToString());
            }
        }
    protected void FillCategories()
        {
        try
            {
            //using (EConnectContext context = new EConnectContext())
            //    {
            //    Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationCourse);
            //    System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");

            //    var Category = (from s in context.CourseCategories
            //                    join c in context.Courses on s.ID equals c.CourseCategoryID
            //                    where c.CourseTypeID == CourseType && s.ID == 1
            //                    orderby (s.Name)
            //                    select new { ValueField = s.ID, TextField = s.Name }).Distinct();
            //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlcoursecategory, Category, lst);
            //    };
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }
    protected void FillCourses()
        {
        try
            {
            //using (EConnectContext context = new EConnectContext())
            //{
            //    Int32 CourseType = Convert.ToInt32(enmCourseType.CertificationExam);
            //    System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--Select One--", "0");
            //    int id = Convert.ToInt32(ddlcoursecategory.SelectedValue);
            //    var CourseList = from p in context.Courses
            //                     where p.CourseCategoryID == id                                 
            //                     select new { ValueField = p.ID, TextField = p.Name };
            //    EConnect.Utils.Common.ControlUtility.BindListObject(ddlcourse, CourseList, lst);
            //};
            }
        catch (Exception ex)
            {
            throw ex;
            }
        }
    protected void ddlcoursecategory_SelectedIndexChanged(object sender, EventArgs e)
        {
        //ddlcourse.Items.Clear();

        //FillCourses();
        //divReportData.Visible = false;       
        }
    protected void ddlcourse_SelectedIndexChanged(object sender, EventArgs e)
        {

        //divReportData.Visible = false;
        //lblheading.Visible = false;
        //lblheadingCandDetails.Visible = false;
        //lblError.Text = "";
        //lblError.Visible = false;
        }
    #endregion
    }