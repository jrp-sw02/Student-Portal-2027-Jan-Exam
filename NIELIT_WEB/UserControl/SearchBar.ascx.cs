using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SearchBar : System.Web.UI.UserControl
{
    // Delegate declaration
    public delegate void OnButtonClick(object sender, EventArgs e);
    // Event declaration
    public event OnButtonClick LnkBtnGO;
    public event OnButtonClick LnkBtnResetSearch;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            ViewState["ErrMsg"] = "search text";
    }
    public string SearchText
    {
        get { return txtSearch.Text; }
        set { txtSearch.Text = value; }
    }
    public string SearchTextToolTip
    {
        get { return txtSearch.ToolTip; }
        set { txtSearch.ToolTip = value; }
    }
    public string SearchTextValidationMessage
    {
        get { return ViewState["ErrMsg"].ToString(); }
        set { ViewState["ErrMsg"] = value; }
    }
    public string AutoCompleteServiceMethod
    {
        get { return aceSearch.ServiceMethod.ToString(); }
        set { aceSearch.ServiceMethod = value; }
    }
    public string AutoCompleteServicePath
    {
        get { return aceSearch.ServicePath.ToString(); }
        set { aceSearch.ServicePath = value; }
    }
    public Boolean AutoCompleteFirstRowSelected
    {
        get { return aceSearch.FirstRowSelected; }
        set { aceSearch.FirstRowSelected = value; }
    }
    public Int32 AutoCompleteMinimumPrefixLength
    {
        get { return aceSearch.MinimumPrefixLength; }
        set { aceSearch.MinimumPrefixLength = value; }
    }
    public Int32 AutoCompleteCompletionSetCount
    {
        get { return aceSearch.CompletionSetCount; }
        set { aceSearch.CompletionSetCount = value; }
    }

    public String AutoCompleteContextKey
    {
        get { return aceSearch.ContextKey; }

        set
        {
            if (value == "")
            {
                aceSearch.ContextKey ="";
                aceSearch.UseContextKey = false;
            }
            else
            {
                aceSearch.ContextKey = value;
                aceSearch.UseContextKey = true;
            }
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (LnkBtnGO != null)
            LnkBtnGO(sender, e);
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtSearch.Text = "";
        searchbar_upnlSearch.Update();
        if (LnkBtnResetSearch != null)
            LnkBtnResetSearch(sender, e);
    }
}