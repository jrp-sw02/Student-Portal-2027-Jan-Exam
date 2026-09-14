using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;
using System.Data.Objects;

public partial class HO_Rpt_TransferCandidateReport : BasePage
{
    Table tbl = new Table();
    public Int32 lowerCourseId;
    public Int32 upperCourseId;
    public DateTime DateFrom;
    public DateTime DateTo;
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Common/TransferRecordsFilter.aspx"))
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

            TableHeaderCell tcCol0 = new TableHeaderCell();
            tcCol0.Width = Unit.Percentage(3);
            tcCol0.Text = "#";
            tcCol0.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol0);

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(10);
            tcCol1.Text = "Level Transfer";
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(10);
            tcCol3.Text = "Total Transfer Candidates";
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            th.Cells.Add(tcCol3);
            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    public void getAllParameters()
    {
        try
        {
            lowerCourseId = Convert.ToInt32(Request.QueryString["lowerCourseId"]);
            DateFrom = Convert.ToDateTime(Request.QueryString["DateFrom"]);
            DateTo = Convert.ToDateTime(Request.QueryString["DateTo"]);
            upperCourseId = Convert.ToInt32(Request.QueryString["upperCourseId"]);
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
            int i = 0;
            Int32 regtypeID = Convert.ToInt32(enmRegistrationType.Transfer);
            //String strSql = "";
            //String Sql = "";
            string strHead = "";
            string strhead1 = "";
            getAllParameters();
            List<Int32> listcourseid = new List<Int32>();
            //String strExcel = "";
            strHead += " <b>Date From  :</b> " + DateFrom.ToString("dd-MMM-yyyy") + "  and  <b> Date To : </b>" + DateTo.ToString("dd-MMM-yyyy");
            //Sql = " select Course_ID from Course where Course_Category_ID = 1 order by Display_Order ";
            //DataTable dr2 = EConnect.Utils.Data.DbUtility.GetDataTable(Sql, new EConnect.Connections.SqlCon(), null, CommandType.Text, true);
            //Int32 Course_ID = 0;
            //for (int j = 0; j <= dr2.Rows.Count - 1; j++)
            //{
            //    Course_ID = Convert.ToInt32(dr2.Rows[i]["Course_ID"]);
            //    strExcel += Course_ID.ToString() + ",";
            //}
            using (EConnectContext context = new EConnectContext())
            {
                if (lowerCourseId != 0 && upperCourseId != 0)
                {
                    var course = context.Courses.Find(lowerCourseId);
                    var course1 = context.Courses.Find(upperCourseId);
                    strhead1 = " From " + course.Name + " to " + course1.Name;
                    //strExcel.Remove(lowerCourseId);
                    //strExcel.Remove(upperCourseId);
                }
                //strSql += "select CR.Display_Order, CR.Name as CourseName,  COUNT(*) as Count" +
                //            " from Registration_Detail RD, Course CR " +
                //            " Where RD.Course_ID = CR.ID   and CAST(Registration_Date as DATE) >= '" + DateFrom + "' and CAST(Registration_Date as DATE) <= '" + DateTo + "' and RD.Reg_Type_ID = '" + regtypeID +"' ";

                //if (upperCourseId != 0)
                //    strSql += " and RD.Course_ID = " + upperCourseId + " and RD.Registration_No not in ( select Registration_No from Registration_Detail RD1 where and RD1.Course_ID  IN (" + strExcel.Trim().Trim(',') + "))";

                //strSql += " group by CR.Display_Order,CR.Name" +
                //            " Order by 1,3 ";

                var students = context.RegistrationDetails.Where(a => a.CourseID == upperCourseId && a.RegistrationTypeID == regtypeID && System.Data.Entity.DbFunctions.TruncateTime(a.RegistrationDate) >= System.Data.Entity.DbFunctions.TruncateTime(DateFrom) && System.Data.Entity.DbFunctions.TruncateTime(a.RegistrationDate) <= System.Data.Entity.DbFunctions.TruncateTime(DateTo)).Select(k => k.RegistrationNo);
                var students1 = context.RegistrationDetails.Where(s => s.CourseID == lowerCourseId && students.Contains(s.RegistrationNo));
                for (i = lowerCourseId + 1; i < upperCourseId; i++)
                {
                    var students2 = context.RegistrationDetails.Where(t => t.CourseID == i && !students.Contains(t.RegistrationNo));
                    students1 = students1.Except(students2);
                }

                ShowTableHeader();
                int j = 1;
                TableHeaderRow tr = new TableHeaderRow();
                tr.CssClass = "gdalternate1";

                TableHeaderCell tcCol1 = new TableHeaderCell();
                tcCol1.Width = Unit.Percentage(2);
                tcCol1.HorizontalAlign = HorizontalAlign.Center;
                tcCol1.Text = j.ToString();
                tr.Cells.Add(tcCol1);

                TableCell tcCol = new TableCell();
                tcCol.Width = Unit.Percentage(15);
                tcCol.HorizontalAlign = HorizontalAlign.Center;
                tcCol.Text = strhead1.ToString();
                tr.Cells.Add(tcCol);

                TableCell tcCol2 = new TableCell();
                tcCol2.Width = Unit.Percentage(7);
                tcCol2.HorizontalAlign = HorizontalAlign.Center;
                tcCol2.Text = students1.Count().ToString();
                tr.Cells.Add(tcCol2);

                tbl.Rows.Add(tr);
            };
            LblRptSubHeader.Text = strHead;
            //DataTable dt = DbUtility.GetDataTable(strSql, new EConnect.Connections.SqlCon(), null, CommandType.Text, false);
            //if (dt.Rows.Count > 0)
            //{
            //    lberror.Visible = false;
            //    lberror.Text = "";
            //    ShowTableHeader();
            //    foreach (DataRow dtRow in dt.Rows)
            //    {
            //        TableRow tr = new TableRow();
            //        if (i % 2 == 0)
            //            tr.CssClass = "gdalternate1";
            //        else
            //            tr.CssClass = "gdrow1";
            //        i++;
            //        TableHeaderCell tcCol1 = new TableHeaderCell();
            //        tcCol1.Width = Unit.Percentage(2);
            //        tcCol1.HorizontalAlign = HorizontalAlign.Center;
            //        tcCol1.Text = i.ToString();
            //        tr.Cells.Add(tcCol1);

            //        TableCell tcCol = new TableCell();
            //        tcCol.Width = Unit.Percentage(3);
            //        tcCol.HorizontalAlign = HorizontalAlign.Center;
            //        tcCol.Text = dtRow["CourseName"].ToString();
            //        tr.Cells.Add(tcCol);

            //        TableCell tcCol2 = new TableCell();
            //        tcCol2.Width = Unit.Percentage(7);
            //        tcCol2.HorizontalAlign = HorizontalAlign.Center;
            //        tcCol2.Text = Convert.ToInt64(dtRow["Count"]).ToString();
            //        tr.Cells.Add(tcCol2);

            //        tbl.Rows.Add(tr);
            //    }
            //}
            //else
            //{
            //    lberror.Visible = true;
            //    lberror.Text = "No record found";
            //}
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