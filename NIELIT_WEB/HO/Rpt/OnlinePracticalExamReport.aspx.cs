using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HO_Rpt_OnlinePracticalExamReport : BasePage
{
    Table tbl = new Table();
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (IsSessionAlive() == false)
        //    Response.Redirect("../Index.aspx");

        try
        {
            if (!IsPostBack)
            {
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

    private DataTable GetData()
    {

        DataTable dt = new DataTable();
        string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

        string sql = " SELECT [Center_Code], convert(varchar,Exam_date,106) Exam_date ,count(*) Total_sheduled ," +
                     " sum(case when Examiner_marks_40 is not null then 1 else 0  end )  Examiner_marks_40_Entered_range_minus_1_to_40," +
                     " sum(case when Examiner_marks_40 =0 then 1 else 0 end )  Examiner_marks_40_Entered_0," +
                     " sum(case when Examiner_marks_40 =-1 then 1 else 0 end ) Examiner_marks_40_Entered_minus_1," +
                     " sum(case when Observer_marks_40 is not null then 1 else 0 end ) Observer_marks_40_Entered_range_minus_1_to_40, " +
                     " sum(case when Observer_marks_40 =0 then 1 else 0 end )  Observer_marks_40_Entered_0, " +
                     " sum(case when Observer_marks_40=-1 then 1 else 0 end )  Observer_marks_40_Entered_minus_1," +
                     " sum(case when Observer_marks_20 is not null then 1 else 0 end ) Observer_marks_20_Entered_range_minus_1_to_20," +
                     " sum(case when Observer_marks_20 =0 then 1 else 0 end ) Observer_marks_20_Entered_0," +
                     " sum(case when Observer_marks_20 =-1 then 1 else 0 end ) Observer_marks_20_Entered_Entered_minus_1" +
                     " FROM [NIELIT].[dbo].[Practical_candidate_marks] where  Exam_Month = 1  and Exam_Year =  2023" +
        //Examiner_marks_40 is not null or Observer_marks_40 is not null or Observer_marks_20 is not null" +
                     " group by [Center_Code], Exam_date order by  Exam_date ,[Center_Code]";

        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.CommandType = CommandType.Text;

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
            TableHeaderRow th = new TableHeaderRow();
            th.CssClass = "head1";

            TableHeaderCell thc = new TableHeaderCell();
            thc.Width = Unit.Percentage(1);
            thc.HorizontalAlign = HorizontalAlign.Center;
            thc.Text = "#";
            th.Cells.Add(thc);

            TableHeaderCell thc1 = new TableHeaderCell();
            thc1.Width = Unit.Percentage(3);
            thc1.HorizontalAlign = HorizontalAlign.Center;
            thc1.Text = "Centre Code";
            th.Cells.Add(thc1);

            TableHeaderCell thc2 = new TableHeaderCell();
            thc2.Width = Unit.Percentage(3);
            thc2.HorizontalAlign = HorizontalAlign.Center;
            thc2.Text = "Exam Date";
            th.Cells.Add(thc2);

            TableHeaderCell thc3 = new TableHeaderCell();
            thc3.Width = Unit.Percentage(3);
            thc3.HorizontalAlign = HorizontalAlign.Center;
            thc3.Text = "Total Scheduled";
            th.Cells.Add(thc3);

            TableHeaderCell thc4 = new TableHeaderCell();
            thc4.Width = Unit.Percentage(3);
            thc4.HorizontalAlign = HorizontalAlign.Center;
            thc4.Text = "Examiner Marks Entered";
            th.Cells.Add(thc4);

            //TableHeaderCell thc6 = new TableHeaderCell();
            //thc6.Width = Unit.Percentage(3);
            //thc6.HorizontalAlign = HorizontalAlign.Center;
            //thc6.Text = "Examiner Marks Entered as 0";
            //th.Cells.Add(thc6);

            TableHeaderCell thc7 = new TableHeaderCell();
            thc7.Width = Unit.Percentage(3);
            thc7.HorizontalAlign = HorizontalAlign.Center;
            thc7.Text = "Examiner Marks Entered as (-1)";
            th.Cells.Add(thc7);

            //TableHeaderCell thc14 = new TableHeaderCell();
            //thc14.Width = Unit.Percentage(3);
            //thc14.HorizontalAlign = HorizontalAlign.Center;
            //thc14.Text = "Total Examiner Marks Entered";
            //th.Cells.Add(thc14);

            TableHeaderCell thc8 = new TableHeaderCell();
            thc8.Width = Unit.Percentage(3);
            thc8.HorizontalAlign = HorizontalAlign.Center;
            thc8.Text = "Total Observer Marks Entered";
            th.Cells.Add(thc8);

            //TableHeaderCell thc9 = new TableHeaderCell();
            //thc9.Width = Unit.Percentage(3);
            //thc9.HorizontalAlign = HorizontalAlign.Center;
            //thc9.Text = "Observer Marks Entered as 0";
            //th.Cells.Add(thc9);

            TableHeaderCell thc10 = new TableHeaderCell();
            thc10.Width = Unit.Percentage(3);
            thc10.HorizontalAlign = HorizontalAlign.Center;
            thc10.Text = "Observer Marks Entered as (-1)";
            th.Cells.Add(thc10);

            TableHeaderCell thc11 = new TableHeaderCell();
            thc11.Width = Unit.Percentage(3);
            thc11.HorizontalAlign = HorizontalAlign.Center;
            thc11.Text = "Total Viva Marks Entered";
            th.Cells.Add(thc11);

            //TableHeaderCell thc12 = new TableHeaderCell();
            //thc12.Width = Unit.Percentage(3);
            //thc12.HorizontalAlign = HorizontalAlign.Center;
            //thc12.Text = "Viva Marks Entered as 0";
            //th.Cells.Add(thc12);

            TableHeaderCell thc13 = new TableHeaderCell();
            thc13.Width = Unit.Percentage(3);
            thc13.HorizontalAlign = HorizontalAlign.Center;
            thc13.Text = "Viva Marks Entered as (-1)";
            th.Cells.Add(thc13);

            //TableHeaderCell thc14 = new TableHeaderCell();
            //thc14.Width = Unit.Percentage(3);
            //thc14.HorizontalAlign = HorizontalAlign.Center;
            //thc14.Text = "Result Upload Date";
            //th.Cells.Add(thc14);

            //TableHeaderCell thc15 = new TableHeaderCell();
            //thc15.Width = Unit.Percentage(3);
            //thc15.HorizontalAlign = HorizontalAlign.Center;
            //thc15.Text = "Result Declaration Date";
            //th.Cells.Add(thc15);

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

        int i = 0;

        DataTable dt1 = GetData();

        if (dt1.Rows.Count > 0)
        {
            foreach (DataRow dtrow in dt1.Rows)
            {
                TableRow tr = new TableRow();
                if (i % 2 == 0)
                    tr.CssClass = "gdalternate1";
                else
                    tr.CssClass = "gdrow1";
                i++;

                TableHeaderCell thc = new TableHeaderCell();
                thc.Width = Unit.Percentage(1);
                thc.HorizontalAlign = HorizontalAlign.Center;
                thc.Text = i.ToString();
                tr.Cells.Add(thc);

                TableCell thc1 = new TableCell();
                thc1.Width = Unit.Percentage(3);
                thc1.HorizontalAlign = HorizontalAlign.Center;
                thc1.Text = dtrow["Center_Code"].ToString();
                tr.Cells.Add(thc1);

                TableCell thc2 = new TableCell();
                thc2.Width = Unit.Percentage(3);
                thc2.HorizontalAlign = HorizontalAlign.Center;
                thc2.Text = dtrow["Exam_date"].ToString();
                tr.Cells.Add(thc2);

                TableCell thc3 = new TableCell();
                thc3.Width = Unit.Percentage(3);
                thc3.HorizontalAlign = HorizontalAlign.Center;
                thc3.Text = dtrow["Total_sheduled"].ToString();
                tr.Cells.Add(thc3);

                TableCell thc4 = new TableCell();
                thc4.Width = Unit.Percentage(3);
                thc4.HorizontalAlign = HorizontalAlign.Center;
                thc4.Text = dtrow["Examiner_marks_40_Entered_range_minus_1_to_40"].ToString();
                if (thc4.Text == thc3.Text)
                {
                    thc4.BackColor = System.Drawing.Color.Green;
                    thc4.ForeColor = System.Drawing.Color.Yellow;
                }
                if (thc4.Text == "0")
                {
                    thc4.BackColor = System.Drawing.Color.Red;
                    thc4.ForeColor = System.Drawing.Color.Yellow;
                }
                if (Convert.ToInt32(thc4.Text) > 0 && thc4.Text != thc3.Text)
                {
                    thc4.BackColor = System.Drawing.Color.Orange;
                    //  thc4.ForeColor = System.Drawing.Color.White;
                }
                tr.Cells.Add(thc4);

                //TableCell thc5 = new TableCell();
                //thc5.Width = Unit.Percentage(3);
                //thc5.HorizontalAlign = HorizontalAlign.Center;
                //thc5.Text = dtrow["Examiner_marks_40_Entered_0"].ToString();
                //tr.Cells.Add(thc5);

                TableCell thc6 = new TableCell();
                thc6.Width = Unit.Percentage(3);
                thc6.HorizontalAlign = HorizontalAlign.Center;
                thc6.Text = dtrow["Examiner_marks_40_Entered_minus_1"].ToString();
                tr.Cells.Add(thc6);

                //TableCell thc13 = new TableCell();
                //thc13.Width = Unit.Percentage(3);
                //thc13.HorizontalAlign = HorizontalAlign.Center;
                //thc13.Text = (Convert.ToInt32(dtrow["Examiner_marks_40_Entered_range_minus_1_to_40"]) + Convert.ToInt32(dtrow["Examiner_marks_40_Entered_0"]) + Convert.ToInt32(dtrow["Examiner_marks_40_Entered_minus_1"])).ToString();
                //  tr.Cells.Add(thc13);

                TableCell thc7 = new TableCell();
                thc7.Width = Unit.Percentage(3);
                thc7.HorizontalAlign = HorizontalAlign.Center;
                thc7.Text = dtrow["Observer_marks_40_Entered_range_minus_1_to_40"].ToString();
                if (thc7.Text == thc3.Text)
                {
                    thc7.BackColor = System.Drawing.Color.Green;
                    thc7.ForeColor = System.Drawing.Color.Yellow;
                }
                if (thc7.Text == "0")
                {
                    thc7.BackColor = System.Drawing.Color.Red;
                    thc7.ForeColor = System.Drawing.Color.Yellow;
                }
                if (Convert.ToInt32(thc7.Text) > 0 && thc7.Text != thc3.Text)
                {
                    thc7.BackColor = System.Drawing.Color.Orange;
                }
                tr.Cells.Add(thc7);

                //TableCell thc8 = new TableCell();
                //thc8.Width = Unit.Percentage(3);
                //thc8.HorizontalAlign = HorizontalAlign.Center;
                //thc8.Text = dtrow["Observer_marks_40_Entered_0"].ToString();
                //tr.Cells.Add(thc8);

                TableCell thc9 = new TableCell();
                thc9.Width = Unit.Percentage(3);
                thc9.HorizontalAlign = HorizontalAlign.Center;
                thc9.Text = dtrow["Observer_marks_40_Entered_minus_1"].ToString();
                tr.Cells.Add(thc9);

                TableCell thc10 = new TableCell();
                thc10.Width = Unit.Percentage(3);
                thc10.HorizontalAlign = HorizontalAlign.Center;
                thc10.Text = dtrow["Observer_marks_20_Entered_range_minus_1_to_20"].ToString();
                if (thc10.Text == thc3.Text)
                {
                    thc10.BackColor = System.Drawing.Color.Green;
                    thc10.ForeColor = System.Drawing.Color.Yellow;
                }
                if (thc10.Text == "0")
                {
                    thc10.BackColor = System.Drawing.Color.Red;
                    thc10.ForeColor = System.Drawing.Color.Yellow;
                }
                if (Convert.ToInt32(thc10.Text) > 0 && thc10.Text != thc3.Text)
                {
                    thc10.BackColor = System.Drawing.Color.Orange;
                }
                tr.Cells.Add(thc10);

                //TableCell thc11 = new TableCell();
                //thc11.Width = Unit.Percentage(3);
                //thc11.HorizontalAlign = HorizontalAlign.Center;
                //thc11.Text = dtrow["Observer_marks_20_Entered_0"].ToString();
                //tr.Cells.Add(thc11);

                TableCell thc12 = new TableCell();
                thc12.Width = Unit.Percentage(3);
                thc12.HorizontalAlign = HorizontalAlign.Center;
                thc12.Text = dtrow["Observer_marks_20_Entered_Entered_minus_1"].ToString();
                tr.Cells.Add(thc12);

                //TableCell thc14 = new TableCell();
                //thc14.Width = Unit.Percentage(3);
                //thc14.HorizontalAlign = HorizontalAlign.Center;
                //thc14.Text = dtrow["Result_Declaration_Date"].ToString();
                //tr.Cells.Add(thc14);

                tbl.Rows.Add(tr);
                divReportData.Controls.Add(tbl);
            }
        }
        else
        {
            lblError.Visible = true;
            lblError.Text = "No Record Found!";
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        ShowTableHeader();
        ShowData();
    }
}