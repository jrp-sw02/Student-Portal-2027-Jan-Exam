using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;

public partial class Rpt_StudentExamData : BasePage
{
    Table tbl = new Table();
    public static int j;    
    Int32 currentRoleId = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblError.Visible = false;
        try
        {
            if (!IsSessionAlive())
            {
                Response.Redirect("~/index.aspx");
            }
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/BulkCertificateExamData.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }

            if (!Page.IsPostBack)
            {
                ShowData();
                tbl.CssClass = "sample3";
                tbl.CellPadding = 2;
                tbl.CellSpacing = 1;
                tbl.Width = Unit.Percentage(100);
                divReportData.Controls.Add(tbl);
            }
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
            DataTable dt;
            int CourseCatId = Convert.ToInt32(Request.QueryString["CourseCatId"]);
            int CourseId = Convert.ToInt32(Request.QueryString["CourseId"]);
            int ExamId = Convert.ToInt32(Request.QueryString["ExamId"]);
            int RcId = Convert.ToInt32(Request.QueryString["RcId"]);
            DateTime ExamDate = Convert.ToDateTime(Request.QueryString["ExamDate"]);
            int ExamBatch = Convert.ToInt32(Request.QueryString["ExamBatch"]);
            string ExamCenter = Request.QueryString["ExamCenter"];
            int i = 0;
            string strheader = "";
            strheader += "</br><b>Regional Center :</b> " + EConnect.Utils.Data.DbUtility.ExecuteScaller("select Name from Regional_Center where id=" + RcId.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false).ToString();
            strheader += "</br><b>Exam Center :</b> " + ExamCenter;
            strheader += "</br><b>Course Name :</b> " + EConnect.Utils.Data.DbUtility.ExecuteScaller("select code from course where id=" + CourseId.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false).ToString();
            strheader += "</br><b>Exam Date :</b> " + ExamDate.ToString("dd-MMM-yyyy") + "</br><b>Batch Number :</b> " + ExamBatch;
            LblRptSubHeader.Text = strheader;

            string query = "select Roll_Number, Name, Father_Name, Mother_Name, Guardian_Name, Photo, [Signature], Left_Thumb, Exam_Centre_Name, Date_of_Exam, Exam_Batch_Number from Certificate_Exam_Application where Course_Category_ID = '" + CourseCatId + "' and Course_ID = '" + CourseId + "' and Exam_ID = '" + ExamId + "' and Regional_Center_ID = '" + RcId + "' and Date_of_Exam = '" + ExamDate.ToString("dd-MMM-yyyy") + "' and Exam_Batch_Number = '" + ExamBatch + "' and Exam_Centre_Name = '" + ExamCenter + "'  order by Roll_Number ";
            dt = EConnect.Utils.Data.DbUtility.GetDataTable(query.ToString(), new EConnect.Connections.SqlCon(), null, CommandType.Text, false);           
            if (dt.Rows.Count > 0)
            {
                ShowTableHeader();
                foreach (DataRow dtRow in dt.Rows)
                {
                    TableRow tr = new TableRow();
                    if (i % 2 == 0)
                        tr.CssClass = "gdalternate1";
                    else
                        tr.CssClass = "gdrow1";
                    i++;

                    TableHeaderCell tcCol1 = new TableHeaderCell();
                    tcCol1.Width = Unit.Percentage(3);
                    tcCol1.HorizontalAlign = HorizontalAlign.Center;
                    tcCol1.Text = i.ToString();
                    tr.Cells.Add(tcCol1);

                    TableCell tcCol8 = new TableCell();
                    tcCol8.Width = Unit.Percentage(5);
                    tcCol8.HorizontalAlign = HorizontalAlign.Left;
                    tcCol8.Text = dtRow["Exam_Centre_Name"].ToString();
                    tr.Cells.Add(tcCol8);

                    TableCell tcCol3 = new TableCell();
                    tcCol3.Width = Unit.Percentage(8);
                    tcCol3.HorizontalAlign = HorizontalAlign.Left;
                    tcCol3.Text = dtRow["Roll_Number"].ToString();
                    tr.Cells.Add(tcCol3);

                    TableCell tcCol4 = new TableCell();
                    tcCol4.Width = Unit.Percentage(10);
                    tcCol4.HorizontalAlign = HorizontalAlign.Left;
                    tcCol4.Text = dtRow["Name"].ToString();
                    tr.Cells.Add(tcCol4);

                    TableCell tcCol5 = new TableCell();
                    tcCol5.Width = Unit.Percentage(8);
                    Image Pimage = new Image();
                    Pimage.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])dtRow["Photo"]);
                    Pimage.Width = 80;
                    Pimage.Height = 70;
                    tcCol5.Controls.Add(Pimage);
                    tcCol5.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tcCol5);

                    TableCell tcCol6 = new TableCell();
                    tcCol5.Width = Unit.Percentage(10);
                    Image Simage = new Image();
                    Simage.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])dtRow["Signature"]);
                    Simage.Width = 150;
                    Simage.Height = 60;
                    tcCol6.Controls.Add(Simage);
                    tcCol6.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tcCol6);

                    TableCell tcCol7 = new TableCell();
                    tcCol5.Width = Unit.Percentage(8);
                    Image Timage = new Image();
                    Timage.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String((byte[])dtRow["Left_Thumb"]);
                    Timage.Width = 100;
                    Timage.Height = 60;
                    tcCol7.Controls.Add(Timage);
                    tcCol7.HorizontalAlign = HorizontalAlign.Left;
                    tr.Cells.Add(tcCol7);

                    tbl.Rows.Add(tr);
                }
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "No Record Found !";
            }
           
        }
        catch (Exception ex) { ShowAlert(ex.Message.ToString()); }
    }
    protected void ShowTableHeader()
    {
        try
        {         
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell tcCol1 = new TableHeaderCell();
            tcCol1.Width = Unit.Percentage(3);
            tcCol1.HorizontalAlign = HorizontalAlign.Center;
            tcCol1.Text = "#";
            th.Cells.Add(tcCol1);

            TableHeaderCell tcCol2 = new TableHeaderCell();
            tcCol2.Width = Unit.Percentage(5);
            tcCol2.HorizontalAlign = HorizontalAlign.Center;
            tcCol2.Text = "Center_Code";
            th.Cells.Add(tcCol2);

            TableHeaderCell tcCol3 = new TableHeaderCell();
            tcCol3.Width = Unit.Percentage(8);
            tcCol3.HorizontalAlign = HorizontalAlign.Center;
            tcCol3.Text = "Roll NUmber";
            th.Cells.Add(tcCol3);

            TableHeaderCell tcCol4 = new TableHeaderCell();
            tcCol4.Width = Unit.Percentage(10);
            tcCol4.HorizontalAlign = HorizontalAlign.Center;
            tcCol4.Text = "Name";
            th.Cells.Add(tcCol4);

            TableHeaderCell tcCol5 = new TableHeaderCell();
            tcCol5.Width = Unit.Percentage(8);
            tcCol5.HorizontalAlign = HorizontalAlign.Center;
            tcCol5.Text = "Photo";
            th.Cells.Add(tcCol5);

            TableHeaderCell tcCol6 = new TableHeaderCell();
            tcCol6.Width = Unit.Percentage(10);
            tcCol6.HorizontalAlign = HorizontalAlign.Center;
            tcCol6.Text = "Signature";
            th.Cells.Add(tcCol6);

            TableHeaderCell tcCol7 = new TableHeaderCell();
            tcCol7.Width = Unit.Percentage(8);
            tcCol7.HorizontalAlign = HorizontalAlign.Center;
            tcCol7.Text = "Thumb";
            th.Cells.Add(tcCol7);

            tbl.Rows.Add(th);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }

   
}