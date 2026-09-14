using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class aadharFormMantra : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.UrlReferrer == null)
        {
            Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
            Response.End();
            return;
        }

        if (Request.QueryString["para1"] != null)
            txtAppName.Text = Request.QueryString["para1"].ToString();
        else
        {
            Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
            Response.End();
            return;
        }

        if (Request.QueryString["para2"] != null)
            txtDob.Text = Request.QueryString["para2"].ToString();
        else
        {
            Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
            Response.End();
            return;
        }
        string x;
        if (Request.QueryString["para3"] != null)
        {
            x = Request.QueryString["para3"].ToString();
            rdbtnlstgender.SelectedValue = x;
        }
        else
        {
            Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
            Response.End();
            return;
        }

        if (Request.QueryString["para4"] != null)
        {
            txtaadhar.Text = Request.QueryString["para4"].ToString();
            string aadhaar = EConnect.Utils.Security.Decryption.Decrypt (txtaadhar.Text);
            txtaadhar.Text = aadhaar;
        }
        else
        {
            Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
            Response.End();
            return;
        }

        if (Request.QueryString["para5"] != null)
        {
            HStudentID.Value  = Request.QueryString["para5"].ToString();
            
        }
        else
        {
            Response.Write(GeInvalidRequestMessage("Goto Home Page", ("../Index.aspx")));
            Response.End();
            return;
        }

        ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:Capture(); ", true);
        
    }
    protected String GeInvalidRequestMessage(String redirectText, String redirectPage)
    {
        return "<B>Invalid Request Parameters</B><br><a href='" + redirectPage + "'>" + redirectText + "</a>";
    }
}