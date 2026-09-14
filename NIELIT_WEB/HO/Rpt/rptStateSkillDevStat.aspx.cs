using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class rptStateSkillDevStat : BasePage
{
    UserType loginUserType;
    Int64 entityID = 0;
    Int32 courseTypeCertificateExam = Convert.ToInt32(enmCourseType.CertificationExam);
    Int32 currentRoleId = 0;
    Int32 loginUserNo = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        lblerror.Text = "";
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            loginUserNo = Convert.ToInt32(Session["UserID"]);
            if (!UserManager.HasRight(currentRoleId, enmRight.View))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            loginUserType = (UserType)Session["UserType"];
            entityID = Convert.ToInt64(Session["EntityID"]);
            if (!IsPostBack)
            {
               
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("State Wise Skill Development Report", "#", ""));
            }
            BreadCrumb1.Render();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.Message;
        }

    }

   

   
    protected void btnView_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt=new DataTable ();
            
            DateTime FromDate = Convert.ToDateTime(txtDateFrom.Text.Trim());
            DateTime ToDate = Convert.ToDateTime(txtDateto.Text.Trim());

            SqlConnection conn = new SqlConnection();
             string constr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

             using (SqlConnection con = new SqlConnection(constr))
             {
                 SqlCommand cmd = new SqlCommand();
                 cmd.Connection = con;
                 con.Open();
                 cmd.CommandText = "getStateSkillDevStat";
                 cmd.CommandType = CommandType.StoredProcedure;
                 cmd.Parameters .Add ("@pDateFrom",SqlDbType.Date );
                 cmd.Parameters["@pDateFrom"].Value = Convert.ToDateTime(txtDateFrom.Text );
                 cmd.Parameters.Add("@pDateTo", SqlDbType.Date);
                 cmd.Parameters["@pDateTo"].Value = Convert.ToDateTime(txtDateto.Text);

                 SqlDataAdapter da = new SqlDataAdapter();
                 da.SelectCommand = cmd;

                 da.Fill(dt);

                 string sheetname = "StateCategoryWiseData_" + System.DateTime.Now.ToString();
                 if (dt.Rows.Count > 0)
                 {
                     GridView GridView1 = new GridView();
                     GridView1.Caption = "<b>NATIONAL INSTITUTE OF ELECTRONICS AND INFORMATION TECHNOLOGY (NIELIT) <br/>State Wise Skill Development Report From " + txtDateFrom.Text + " To " + txtDateto.Text+"</b>";
                     GridView1.AllowPaging = false;
                     GridView1.DataSource = dt;
                     GridView1.DataBind();
                     Response.Clear();
                     Response.Buffer = true;
                     Response.AddHeader("content-disposition", "attachment;filename=" + sheetname + ".xls");
                     Response.Charset = "";
                     Response.ContentType = "application/vnd.ms-excel";
                     StringWriter sw = new StringWriter();
                     HtmlTextWriter hw = new HtmlTextWriter(sw);
                     for (int i = 0; i < GridView1.Rows.Count; i++)
                     {
                         //Apply text style to each Row
                         GridView1.Rows[i].Attributes.Add("class", "textmode");
                     }
                     GridView1.RenderControl(hw);
                     //style to format numbers to string
                     string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                     Response.Write(style);
                     Response.Output.Write(sw.ToString());
                     Response.Flush();
                     Response.End();
                 }
                 else
                 {
                     ShowAlert("No record found.");
                 }
             }
        }

        catch (Exception ex)
        {
            ShowAlert(ex.Message.ToString());
        }

    }

}


