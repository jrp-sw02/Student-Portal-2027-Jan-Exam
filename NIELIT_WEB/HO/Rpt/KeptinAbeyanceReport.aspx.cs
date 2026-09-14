using System;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;

public partial class HO_Rpt_KeptinAbeyanceReport : BasePage
{
    Table tbl = new Table();
    public Int32 CourseId;
    public DateTime DateFrom;
    public DateTime DateTo;
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;
    EConnectContext context;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/KeptinAbeyanceFilter.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
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
            ShowAlert(ex.Message);
        }
    }
    protected void ShowTableHeader()
    {
        try
        {

            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(1);
            tcCol1.HorizontalAlign = HorizontalAlign.Left;
            tcCol1.Text = "#";
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol = new TableHeaderCell();
            tcCol.Width = Unit.Percentage(1);
            tcCol.HorizontalAlign = HorizontalAlign.Left;
            tcCol.Text = "Application No.";
            th.Cells.Add(tcCol);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(2);
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            tcCol2.Text = "Level";
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Width = Unit.Percentage(10);
            tcCol3.Text = "Candidate Name";
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            tcCol4.Width = Unit.Percentage(20);
            tcCol4.Text = "Father / Mother Name";
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(12);
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            tcCol5.Text = "Date of Birth";
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(4);
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            tcCol6.Text = "Sex";
            th.Cells.Add(tcCol6);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(25);
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            tcCol7.Text = "Defficiency Details";
            th.Cells.Add(tcCol7);

            TableHeaderCell tcCol8 = new TableHeaderCell();
            tcCol8.Width = Unit.Percentage(5);
            tcCol8.HorizontalAlign = HorizontalAlign.Center;
            tcCol8.Text = "Entry Date";
            th.Cells.Add(tcCol8);

            TableHeaderCell tcCol9 = new TableHeaderCell();
            tcCol9.Width = Unit.Percentage(5);
            tcCol9.HorizontalAlign = HorizontalAlign.Center;
            tcCol9.Text = "Batch Number";
            th.Cells.Add(tcCol9);

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
            context = new EConnectContext();
            Int32 appstatusid = Convert.ToInt32(enmCourseApplicationStatus.KeptInAbeyance);


            CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            DateFrom = Convert.ToDateTime(Request.QueryString["DateFrom"]);
            DateTo = Convert.ToDateTime(Request.QueryString["DateTo"]);
            string strHead = "";
            if (CourseId != 0)
            {
                var courses = context.Courses.Find(CourseId);
                if (courses != null)
                {
                    strHead += "<br/><b>Course Name :</b>" + courses.Name;
                }
            }
            else
            {
                strHead += "<br/><b>Course Name :</b> All";
            }
            StringBuilder mySql = new StringBuilder();
            StringBuilder mySql1 = new StringBuilder();
            strHead += "</br><b> Application Status :</b>" + EConnect.Utils.Common.EnumUtility.GetDescription((enmCourseApplicationStatus)(appstatusid));
            strHead += "</br><b> Application Date From :</b>" + DateFrom.ToString("dd-MMM-yyyy") + " to " + DateTo.ToString("dd-MMM-yyyy");


            var application = (from c in context.CourseRegistrationApplications
                               join b in context.BatchItems
                                   on c.BatchItemID equals b.ID
                               where c.ApplicationStatusID == appstatusid && c.FinalSubmitted == true
                               orderby c.ApplicationDate
                               select new
                               {

                                   Gender = c.Gender,
                                   Name = c.Name,
                                   ApplicationDate = System.Data.Entity.DbFunctions.TruncateTime(c.ApplicationDate),
                                   Level = c.Course.Code,
                                   Dob = c.DateOfBirth,
                                   fathername = c.FatherName,
                                   mothername = c.MotherName,
                                   guardianname = c.GuardianName,
                                   ApplicationStatus = c.ApplicationStatusID,
                                   Number = c.Number,
                                   couID = c.CourseID,
                                   batchitemid = c.BatchItemID,
                                   ID = c.ID
                               }).ToList();

            if (CourseId != 0)
            {
                application = application.Where(s => s.couID == CourseId).ToList();
            }
            application = application.Where(s => s.ApplicationDate >= DateFrom && s.ApplicationDate <= DateTo).ToList();

            if (currentRoleId == Convert.ToInt32(enmRole.CourseWiseAdmin))
            {
                var roleCourses = context.CourseWiseUserMappings.Where(a => a.UserNo == loginUserNo).Select(k => k.CourseID).Distinct();
                application = application.Where(a => roleCourses.Contains(a.couID)).ToList();
            }
            if (application.Count() > 0)
            {

                ShowTableHeader();
                int i = 1;
                foreach (var app in application)
                {
                    Int32 count = 0;
                    StringBuilder defficiencydetails = new StringBuilder();
                    TableRow tr = new TableRow();
                    if (i % 2 == 0)
                        tr.CssClass = "gdalternate1";
                    else
                        tr.CssClass = "gdrow1";

                    TableCell tdRow = new TableCell();
                    tdRow.Width = Unit.Percentage(1);
                    tdRow.Text = i.ToString();
                    tdRow.HorizontalAlign = HorizontalAlign.Right;
                    tr.Cells.Add(tdRow);

                    HyperLink link = new HyperLink();
                    link.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/CAND/FrmPreview.aspx?ID=" + app.couID + "&Appid=" + app.ID + "&Dob=" + app.Dob + "&Type=Print");
                    link.Target = "_blank";
                    link.Style.Add("text-decoration", "none");
                    link.ForeColor = System.Drawing.Color.Black;
                    TableCell tdRow1 = new TableCell();
                    tdRow1.Width = Unit.Percentage(2);
                    link.Text = app.Number.ToString();
                    tdRow1.HorizontalAlign = HorizontalAlign.Center;
                    tdRow1.Controls.Add(link);
                    tr.Cells.Add(tdRow1);

                    TableCell tdRow5 = new TableCell();
                    tdRow5.Width = Unit.Percentage(3);
                    if (app.Level != null)
                        tdRow5.Text = app.Level.ToString().ToUpper();
                    tdRow5.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow5);


                    TableCell tdRow6 = new TableCell();
                    tdRow6.Width = Unit.Percentage(24);
                    if (app.Name != null)
                        tdRow6.Text = GetInitCap(app.Name.ToString());
                    tdRow6.HorizontalAlign = HorizontalAlign.Left;
                    tdRow6.Wrap = false;
                    tr.Cells.Add(tdRow6);


                    if (string.IsNullOrEmpty(app.guardianname) == true && string.IsNullOrWhiteSpace(app.guardianname) == true)
                    {
                        TableCell tdRow4 = new TableCell();
                        tdRow4.Width = Unit.Percentage(15);
                        if (app.fathername != null && app.mothername != null)
                            tdRow4.Text = GetInitCap(app.fathername.ToString()) + " / " + GetInitCap(app.mothername.ToString());
                        tdRow4.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(tdRow4);
                    }
                    else
                    {
                        TableCell tdRow4 = new TableCell();
                        tdRow4.Width = Unit.Percentage(15);
                        if (app.guardianname != null)
                            tdRow4.Text = GetInitCap(app.guardianname.ToString());
                        tdRow4.HorizontalAlign = HorizontalAlign.Left;
                        tr.Cells.Add(tdRow4);

                    }

                    TableCell tdRow7 = new TableCell();
                    tdRow7.Width = Unit.Percentage(10);
                    if (app.Dob != null)
                        tdRow7.Text = app.Dob.ToString("dd-MMM-yyyy");
                    tdRow7.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow7);

                    TableCell tdRow8 = new TableCell();
                    tdRow8.Width = Unit.Percentage(3);
                    if (app.Gender != null)
                        tdRow8.Text = app.Gender.ToString().ToUpper();
                    tdRow8.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow8);

                    var defficiency = (from a in context.BatchItemDeficiencyDetails
                                       join d in context.DeficiencyCodes
                                           on a.DeficiencyID equals d.ID
                                       where a.BatchItemID == app.batchitemid && a.IsFullFilled == false
                                       select new
                                       {
                                           ID = a.DeficiencyID,
                                           Batch_Item_ID = a.BatchItemID
                                       }).Distinct();
                    if (defficiency.Count() > 0)
                    {
                        foreach (var deffdesc in defficiency)
                        {
                            count = count + 1;
                            defficiencydetails.Append(count + ". " + context.DeficiencyCodes.Where(a => a.ID == deffdesc.ID).FirstOrDefault().Description + "<br/>");
                        }
                    }

                    TableCell tdRow10 = new TableCell();
                    tdRow10.Width = Unit.Percentage(15);
                    if (!string.IsNullOrEmpty(defficiencydetails.ToString()))
                        tdRow10.Text = defficiencydetails.ToString();
                    else
                        tdRow10.Text = "NA";
                    tdRow10.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow10);

                    TableCell tdRow9 = new TableCell();
                    tdRow9.Width = Unit.Percentage(12);
                    if (defficiency.Count() > 0)
                        tdRow9.Text = context.BatchItemDeficiencyDetails.Where(a => a.DeficiencyID == defficiency.FirstOrDefault().ID).FirstOrDefault().CreatedOn.ToString("dd-MMM-yyyy");
                    else
                        tdRow9.Text = "NA";
                    tdRow9.HorizontalAlign = HorizontalAlign.Center;
                    tr.Cells.Add(tdRow9);

                    TableCell tdRow12 = new TableCell();
                    tdRow12.Width = Unit.Percentage(9);
                    var batch_no = context.BatchItems.Find(app.batchitemid).Batch.Number;
                    if (batch_no != null)
                        tdRow12.Text = batch_no;
                    else
                        tdRow12.Text = "NA";
                    tdRow12.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tdRow12);

                    tbl.Rows.Add(tr);
                    i++;
                }
                lblCount.Visible = true;
                lblCount.Text = "Total Records : " + application.Count().ToString();
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "No Record Found";
            }
            LblRptSubHeader.Text = strHead;
        }
        catch (Exception ex)
        { ShowAlert(ex.Message); }
        finally { context.Dispose(); }
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
            Response.AddHeader("content-disposition", "attachment;filename=Applications.xls");
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
}