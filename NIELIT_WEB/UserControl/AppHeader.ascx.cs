using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_AppHeader : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            hlHome.HRef = "~/MainPage.aspx";
            FillModules();
            
        }
    }
    protected void FillModules()
    {
        ListItem lst = new ListItem();
        lst.Selected = true;
        lst.Value = "1";
        lst.Text ="My Information";
        lst.Attributes.Add("Title","Images/icon-ok.png");
        ddlModules.Items.Add(lst);

        ListItem lst1 = new ListItem();
        
        lst1.Value = "2";
        lst1.Text = "Human Resource Management";
        lst1.Attributes.Add("Title", "Images/icon-ok.png");
        ddlModules.Items.Add(lst1);

        ListItem lst2 = new ListItem();
         
        lst2.Value = "3";
        lst2.Text = "Administrative Module";
        lst2.Attributes.Add("Title", "Images/icon-ok.png");
        ddlModules.Items.Add(lst2);

    }
    protected void ddlModules_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FillModules();
        //Response.Write(ddlModules.SelectedItem.Text);
    }
}