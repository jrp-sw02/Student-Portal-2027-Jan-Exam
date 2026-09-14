using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AdminModuleDetail : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (string.IsNullOrWhiteSpace(Convert.ToString(Session["level"])))
            {
                licourse.Visible = false;
                liLevel.Visible = false;
                //Divregstud.Visible = true;
                //Divcourse.Visible = false;

            }
            else
            {
                liLevel.Visible = true;
                licourse.Visible = true;
                Label2.Text = Convert.ToString(Session["level"]);
                //Divcourse.Visible = true;
                //Divregstud.Visible = false;
                //spn1.InnerText = "Exam details Of  " + Convert.ToString(Session["level"]);
            }
        }
          
    }
}