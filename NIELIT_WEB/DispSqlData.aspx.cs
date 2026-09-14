using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using EConnect.Connections;
using EConnect.Utils.Data;

public partial class DispSqlData : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        lblErrMsg.Text = "";
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdBtnSelection.SelectedIndex == 0)
            {
                if (txtQuery.Text.Contains(';') || Regex.Matches(txtQuery.Text.ToLower(), "select").Count > 1)
                {
                    lblErrMsg.Text = "Please enter one select query at a time.";
                    return;
                }
                DataTable dt = DbUtility.GetDataTable(txtQuery.Text.Trim(), new SqlCon(), null, CommandType.Text, false);
                dtgView.DataSource = dt;
                dtgView.DataBind();
                divData.Visible = true;
                lblCount.Text = "Total Records Found: " + dt.Rows.Count.ToString();
            }
            else
            {
                divData.Visible = false;
                DbUtility.ExecuteNonQuery(txtQuery.Text, new SqlCon(), null, CommandType.Text, false);
                lblCount.Text = "Query executed successfully.";
            }
        }
        catch (Exception ex)
        {
            lblErrMsg.Text = "Please check the query parameters. " + ex.Message;
        }
    }
    protected void btnResetSelect_Click(object sender, EventArgs e)
    {
        Response.Redirect("DispSqlData.aspx");
    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        if (txtPassword.Text.Trim() == System.Web.Configuration.WebConfigurationManager.AppSettings["AdminPwd"].ToString())
        {
            trLogin.Visible = false;
            trQuery.Visible = true;
        }
        else
        {
            lblErrMsg.Text = "Incorrect password.";
        }

    }
}