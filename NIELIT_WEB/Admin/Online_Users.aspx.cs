using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using System.Web.SessionState;
public partial class ReportPgae : BasePage
{
    Table tbl = new Table();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblerror.Text = "";
        lblerror.Visible = false;
        try
        {
            //if (!IsSessionAlive())
            //{
            //    Response.Redirect("~/index.aspx");
            //}
            if (!Page.IsPostBack)
            {

                using (EConnectContext context = new EConnectContext())
                {
                    var userTypes = (from p in context.UserTypes
                                     orderby p.Name
                                     select new { ValueField = p.ID, TextField = p.Name });
                    ListItem lst = new ListItem("--All--", "0");
                    EConnect.Utils.Common.ControlUtility.BindListObject(ddlUserType, userTypes, lst);
                };
                ShowTableHeader();
                ShowTableData();
                divReportData.Controls.Add(tbl);
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
        }
    }
    protected void ShowTableHeader()
    {
        tbl.CssClass = "sample3";
        tbl.CellPadding = 2;
        tbl.CellSpacing = 0;
        tbl.Width = Unit.Percentage(100);

        TableHeaderRow th = new TableHeaderRow();
        th.CssClass = "head1";
        th.HorizontalAlign = HorizontalAlign.Left;

        TableHeaderCell tcCol1 = new TableHeaderCell();
        tcCol1.Width = Unit.Percentage(5);
        tcCol1.Text = "#";
        th.Cells.Add(tcCol1);

        TableHeaderCell tcCol2 = new TableHeaderCell();
        tcCol2.Width = Unit.Percentage(25);
        tcCol2.Text = "User Name";
        th.Cells.Add(tcCol2);

        TableHeaderCell tcCol3 = new TableHeaderCell();
        tcCol3.Width = Unit.Percentage(19);
        tcCol3.Text = "User Type";
        th.Cells.Add(tcCol3);

        TableHeaderCell tcCol4 = new TableHeaderCell();
        tcCol4.Width = Unit.Percentage(16);
        tcCol4.Text = "Login Time";
        th.Cells.Add(tcCol4);

        TableHeaderCell tcCol5 = new TableHeaderCell();
        tcCol5.Width = Unit.Percentage(35);
        tcCol5.Text = "Details (Browser, Client IP, Source IPD)";
        th.Cells.Add(tcCol5);

        tbl.Rows.Add(th);
        
    }
    protected void ShowTableData()
    {
        try
        {
            //Active Sessions
            Dictionary<String, HttpSessionState> sessionData = (Dictionary<String, HttpSessionState>)Application["s"];
            lblSessionCount.Text = "&nbsp;&nbsp;&nbsp;Number of Active Sessions : " + sessionData.Count.ToString();

            Dictionary<Int32, Int32> LoginUsersList = new Dictionary<Int32, Int32>();
            LoginUsersList = (Dictionary<Int32, Int32>)Application["LoginUsersList"];
            //LoginUsersList.Add(loginUser.UserID, Convert.ToInt32(Session["LogID"]));
            Int32 rowNumber = 0;
            using (EConnectContext context = new EConnectContext())
            {
                Int32 userTypeID = Convert.ToInt32(ddlUserType.SelectedValue);
                foreach (KeyValuePair<Int32, Int32> item in LoginUsersList)
                {

                    TableRow tr = new TableRow();
                    if (rowNumber % 2 == 0)
                        tr.CssClass = "gdalternate1";
                    else
                        tr.CssClass = "gdrow1";
                    var loginUser = (from u in context.Users where u.UserID == item.Key select new { UserName = u.UserName, UserType = u.UserType.Name, userTypeID = u.UserTypeID }).ToList();
                    if (userTypeID > 0)
                        loginUser = loginUser.Where(m => m.userTypeID == userTypeID).ToList();
                    var logUser = loginUser.FirstOrDefault();
                    if (logUser != null)
                    {
                        EConnect.URM.LoginLog log = context.LoginLogs.Find(item.Value);
                        rowNumber += 1;
                        TableCell tdRow1 = new TableCell();
                        tdRow1.Text = rowNumber.ToString();
                        tr.Cells.Add(tdRow1);

                        TableCell tdRow2 = new TableCell();
                        tdRow2.Text = GetInitCap(logUser.UserName);
                        tr.Cells.Add(tdRow2);

                        TableCell tdRow3 = new TableCell();
                        tdRow3.Text = logUser.UserType;
                        tr.Cells.Add(tdRow3);

                        TableCell tdRow4 = new TableCell();
                        tdRow4.Text = log.LoginTime.ToString("dd-MMM-yyyy hh:mm:ss tt");
                        tr.Cells.Add(tdRow4);

                        TableCell tdRow5 = new TableCell();
                        tdRow5.Text = log.BrowserName + ", " + log.ClientIP + ", " + log.SourceIP.ToString();
                        tr.Cells.Add(tdRow5);

                        tbl.Rows.Add(tr);
                    }
                }
            }
            if (rowNumber == 0)
            {
                lblerror.Text = "No user found logged in at this time.";
                lblerror.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ibExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            System.IO.StringWriter StringWrite = new System.IO.StringWriter();
            Html32TextWriter htmlWrite;
            divReportData.Visible = true;
            ShowTableHeader();
            ShowTableData();
            divReportData.Controls.Add(tbl);
            divReportData.Controls.Add(tbl);
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=LoginUsrsOn" + DateTime.Now.ToString() + ".xls");
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
    protected void ddlUserType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ShowTableHeader();
            ShowTableData();
            divReportData.Controls.Add(tbl);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
        }
    }
    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            ShowTableHeader();
            ShowTableData();
            divReportData.Controls.Add(tbl);
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
            lblerror.Visible = true;
        }
    }
}