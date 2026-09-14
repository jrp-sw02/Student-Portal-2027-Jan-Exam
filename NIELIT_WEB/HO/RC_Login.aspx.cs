using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class RC_Login : System.Web.UI.Page
{
    private string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string uname = txtUsername.Text.Trim();
        string pwd = txtPassword.Text.Trim();

        if (uname == "" || pwd == "")
        {
            lblError.Text = "Please enter both username and password.";
            lblError.Visible = true;
            return;
        }

        string rcCode = "";
        string rcName = "";

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql = "SELECT rc_code, rc_name FROM tblRCMaster WHERE login_username = @uname AND login_password = @pwd AND is_active = 1";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@uname", uname);
                cmd.Parameters.AddWithValue("@pwd", pwd);
                conn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        rcCode = rdr["rc_code"].ToString();
                        rcName = rdr["rc_name"].ToString();
                    }
                }
            }
        }

        if (rcCode == "")
        {
            lblError.Text = "Invalid username or password.";
            lblError.Visible = true;
            return;
        }

        Session["rc_code"] = rcCode;
        Session["rc_name"] = rcName;

        Response.Redirect("~/StudentPortal/RC_ExamCentreDashboard.aspx");
    }
}