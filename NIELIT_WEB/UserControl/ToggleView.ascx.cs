using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ToggleView : System.Web.UI.UserControl
{
    // Delegate declaration
    public delegate void OnButtonClick(object sender, EventArgs e);
    // Event declaration
    public event OnButtonClick BtnMode_Click;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
             
        }
    }
    public enum Mode
    {
        New=0,
        List =1
    }

    public Mode ViewMode
    {
        get { return (Mode) Convert.ToInt16(ViewState["Mode"]); }
        set 
        { 
            ViewState["Mode"] = Convert.ToInt16(value).ToString();
            if (value == ToggleView.Mode.New)
            {
                btnNew.CssClass = "btnToggleNew";
                btnNew.ToolTip = "New";
            }
            else if (value == ToggleView.Mode.List)
            {
                btnNew.CssClass = "btnToggleList";
                btnNew.ToolTip = "List";
            }
        }
    }

    public String ToolTipText
    {
        get { return btnNew.ToolTip ; }
        set { btnNew.ToolTip = value; }
    }
    protected void btnNew_Click(object sender, EventArgs e)
    {
        if (BtnMode_Click != null)
            BtnMode_Click(sender, e);
    }
}