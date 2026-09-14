using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.DataVisualization.Charting;

public partial class Dashboard_NDLMDashboardStatewise : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            RepositoryData(null);
            GetChartTypes();
        }

    }

    private void RepositoryData(string SortExpression)
    {
        string CS = ConfigurationManager.ConnectionStrings["DashboardContext"].ConnectionString;
        using (SqlConnection con = new SqlConnection(CS))
        {
            //string str = StateListDDL.Text;
            string sql = "Select project_id, StudentData_Admitted 'Admitted', StudentData_Appeared 'Appeared', StudentData_Passed 'Passed', DigitalCertificateIssued 'Certificates Issued' from ProjectStudentMaster order by Project_Id DESC";
            SqlCommand cmd = new SqlCommand(sql, con);
            con.Open();
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //DataSet ds = new DataSet();
            //da.Fill(ds);
            SqlDataReader rdr = cmd.ExecuteReader();
            Chart1.DataBindTable(rdr, "project_id");
            //Chart1.Series["Services"].XValueMember = "Subject_Name";
            //Chart1.Series["Services"].YValueMembers = "Subject_Status_Count";
            //Chart1.Series["Services"]["PieLabelStyle"] = "Outside";
            //Chart1.Series[0]["DrawingStyle"] = "Cylinder";
            //Chart1.Series[1]["DrawingStyle"] = "Cylinder";
            //Chart1.Series[2]["DrawingStyle"] = "Cylinder";
            //Chart1.Series[3]["DrawingStyle"] = "Cylinder";

            Chart1.Series[0].ChartType = SeriesChartType.Bar;
            Chart1.Series[1].ChartType = SeriesChartType.Bar;
            Chart1.Series[2].ChartType = SeriesChartType.Bar;
            Chart1.Series[3].ChartType = SeriesChartType.Bar;

            Chart1.Series[0].IsValueShownAsLabel = true;
            Chart1.Series[1].IsValueShownAsLabel = true;
            Chart1.Series[2].IsValueShownAsLabel = true;
            Chart1.Series[3].IsValueShownAsLabel = true;
            //Chart1.Series["Services"]["CustomProperties"] = "Exploded=true";
            //Chart1.Series["Services"].ToolTip ="X value = #VALX{d}\nY value = #VALY";
            Chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.Enabled = false;
            //Chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.Enabled = false;

            //Chart1.ChartAreas["ChartArea1"].Appearance.Dimensions.Margins.Left = new Unit(10, UnitType.Pixel);
            //Chart1.Legends["Default"].Title = "Star(*) indicated services are last updated on 26/03/2015. Hash(#) indicated Services are updated on daily basis \n And all the counts are taken from last two years";
            //DataView dv = ds.Tables[0].DefaultView;
            //dv.Sort = string.IsNullOrEmpty(SortExpression) ? "Subject_Status_Count DESC" : SortExpression;
            //Chart1.DataSource = dv;
            //Chart1.DataBind();
            con.Close();
        }
    }
    private void GetChartTypes()
    {
        foreach (int chartType in Enum.GetValues(typeof(SeriesChartType)))
        {
            ListItem ChartList = new ListItem(Enum.GetName(typeof(SeriesChartType), chartType), chartType.ToString());
            ddlChartList.Items.Add(ChartList);
        }
    }

    protected void ddlChartList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ThreeDChkBox.Checked == true)
        {
            Chart1.ChartAreas[0].Area3DStyle.Enable3D = true;
            Chart1.Series[0].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartList.SelectedValue);
            Chart1.Series[1].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartList.SelectedValue);
            Chart1.Series[2].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartList.SelectedValue);
            Chart1.Series[3].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartList.SelectedValue);
            RepositoryData(ddlSortBy.SelectedValue + " " + ddlSortDirection.SelectedValue);
        }
        else
        {
            Chart1.ChartAreas[0].Area3DStyle.Enable3D = false;
            Chart1.Series[0].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartList.SelectedValue);
            Chart1.Series[1].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartList.SelectedValue);
            Chart1.Series[2].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartList.SelectedValue);
            Chart1.Series[3].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartList.SelectedValue);
            RepositoryData(ddlSortBy.SelectedValue + " " + ddlSortDirection.SelectedValue);
        }
    }

    protected void ThreeDChkBox_CheckedChanged(object sender, EventArgs e)
    {
        if (ThreeDChkBox.Checked == true)
        {
            Chart1.ChartAreas[0].Area3DStyle.Enable3D = true;
            //Chart1.Series["Services"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartList.SelectedValue);
            //RepositoryData(ddlSortBy.SelectedValue + " " + ddlSortDirection.SelectedValue);
        }
        else
        {
            Chart1.ChartAreas[0].Area3DStyle.Enable3D = false;
            //Chart1.Series["Services"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChartList.SelectedValue);
            //RepositoryData(ddlSortBy.SelectedValue + " " + ddlSortDirection.SelectedValue);
        }
    }
    protected void ddlSortBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        RepositoryData(ddlSortBy.SelectedValue + " " + ddlSortDirection.SelectedValue);
    }
    protected void ddlSortDirection_SelectedIndexChanged(object sender, EventArgs e)
    {
        RepositoryData(ddlSortBy.SelectedValue + " " + ddlSortDirection.SelectedValue);
    }
}