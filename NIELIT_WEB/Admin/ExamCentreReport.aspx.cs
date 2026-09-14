using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;

public partial class Admin_ExamCentreReport : BasePage
{
    Table tbl = new Table();
    //EConnectContext context = new EConnectContext();
    Int32 currentRoleId = 0;
    UserType loginUserType;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserType = (UserType)Session["UserType"];
            if (loginUserType == EConnect.URM.UserType.Admin)
            {
                if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/CertificateCourse.aspx"))
                {
                    Response.Write("Sorry! You don't have rights  to view this page");
                    Response.End();
                }
            }
            if (String.IsNullOrEmpty(Request.QueryString["ExamId"]) && String.IsNullOrEmpty(Request.QueryString["CourseId"]))
                GeInvalidRequestMessage("Go to home page", "../mainpage.aspx");
            if (!Page.IsPostBack)
            {
                tbl.CssClass = "sample3";
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(98);
                tbl.HorizontalAlign = HorizontalAlign.Center;
                ShowData();
                divReportData.Controls.Add(tbl);
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void ShowTableHeader()
    {
        TableHeaderRow th = new TableHeaderRow();
        th.CssClass = "head1";
        TableHeaderCell tcCol1 = new TableHeaderCell();
        tcCol1.Width = Unit.Percentage(1);
        tcCol1.Text = "#";
        tcCol1.HorizontalAlign = HorizontalAlign.Center;
        th.Cells.Add(tcCol1);


        TableHeaderCell tcCol2 = new TableHeaderCell();
        tcCol2.Width = Unit.Percentage(20);
        tcCol2.Text = "State";
        tcCol2.HorizontalAlign = HorizontalAlign.Center;
        th.Cells.Add(tcCol2);

        TableHeaderCell tcCol3 = new TableHeaderCell();
        tcCol3.Width = Unit.Percentage(15);
        tcCol3.HorizontalAlign = HorizontalAlign.Center;
        tcCol3.Text = "Centre Name";
        th.Cells.Add(tcCol3);

        TableHeaderCell tcCol4 = new TableHeaderCell();
        tcCol4.Width = Unit.Percentage(5);
        tcCol4.Text = "Code";
        tcCol4.HorizontalAlign = HorizontalAlign.Center;
        th.Cells.Add(tcCol4);
        tbl.Rows.Add(th);

        TableHeaderCell tcCol5 = new TableHeaderCell();
        tcCol5.Width = Unit.Percentage(25);
        tcCol5.Text = "Exam Centre Type";
        tcCol5.HorizontalAlign = HorizontalAlign.Center;
        th.Cells.Add(tcCol5);
        tbl.Rows.Add(th);

    }
    protected void ShowData()
    {
        try
        {
            Int32 ExamId = Convert.ToInt32(Request.QueryString["ExamId"]);
           
            Int32 CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            if (ExamId != 0 && CourseId != 0)
            {
                    using (EConnectContext context = new EConnectContext())
                    {
                        Exam currentexam = context.Exams.Find(ExamId);
                        Course currentcourse = context.Courses.Find(CourseId);
                        if (currentexam != null && currentcourse != null)
                        {
                            lblCourse.Text = currentcourse.Name + " ( " + currentexam.Name + " )" + " Exam ";
                        }
                        ShowTableHeader();
                        var examcentrlist = (from a in context.ExamWiseExamCenters
                                           where a.ExamID == ExamId
                                             orderby a.ExamCenter.State.Name, a.ExamCenter.Name 
                                           select a).ToList();

                         int i = 1;
                         foreach (var examlist in examcentrlist)
                         {

                             TableRow tr = new TableRow();
                             if (i % 2 == 0)
                                 tr.CssClass = "gdalternate1";
                             else
                                 tr.CssClass = "gdrow1";

                             TableHeaderCell tcCol1 = new TableHeaderCell();
                             tcCol1.Width = Unit.Percentage(2);
                             tcCol1.HorizontalAlign = HorizontalAlign.Left;
                             tcCol1.Text = i.ToString();
                             tr.Cells.Add(tcCol1);

                             TableCell tcCol = new TableCell();
                             tcCol.Width = Unit.Percentage(10);
                             tcCol.HorizontalAlign = HorizontalAlign.Left;
                             tcCol.Text = examlist.ExamCenter.State.Name;
                             tr.Cells.Add(tcCol);

                             TableCell tcCol2 = new TableCell();
                             tcCol2.Width = Unit.Percentage(15);
                             tcCol2.HorizontalAlign = HorizontalAlign.Left;
                             tcCol2.Text = examlist.ExamCenter.Name;
                             tr.Cells.Add(tcCol2);

                           
                            TableCell tcCol3 = new TableCell();
                            tcCol3.HorizontalAlign = HorizontalAlign.Left;
                            tcCol3.Width = Unit.Percentage(10);
                            tcCol3.Text = examlist.ExamCenter.Code;
                            tr.Cells.Add(tcCol3);

                            TableCell tcCol4 = new TableCell();
                            tcCol4.HorizontalAlign = HorizontalAlign.Left;
                            tcCol4.Width = Unit.Percentage(25);
                            tcCol4.Text = EConnect.Utils.Common.EnumUtility.GetDescription((enmExamCenterType)(examlist.ExamCentreTypeID)); 
                            tr.Cells.Add(tcCol4);

                            tbl.Rows.Add(tr);
                            i++;
                         }
                   };
             }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        System.IO.StringWriter StringWrite = new System.IO.StringWriter();
        Html32TextWriter htmlWrite;
        divReportData.Visible = true;
        ShowData();
        divReportData.Controls.Add(tbl);
        Response.Clear();
        Response.AddHeader("content-disposition", "attachment;filename=Applications.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.xls";
        htmlWrite = new Html32TextWriter(StringWrite);
        divReportData.RenderControl(htmlWrite);
        Response.Write(StringWrite.ToString());
        Response.End();

    }
}