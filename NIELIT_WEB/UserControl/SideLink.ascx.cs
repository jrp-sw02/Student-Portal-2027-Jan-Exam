using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;

public partial class UserControl_SideLink : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (ViewState["SideLinkType"] == null)
            {
                ViewState["SideLinkType"] = Convert.ToInt32(SideLinkItem.SideLinkType.Hyperlink);
            }
        }
    }
    #region Private Members
    private List<SideLinkItem> _items = new List<SideLinkItem>();
    #endregion
    #region Public Properties
    /// <summary>
    /// gets or sets the  items list of BreadCrumb object..
    /// </summary>
    public List<SideLinkItem> Items
    {
        set { _items = value; }
        get { return _items; }
    }
    #endregion
    public SideLinkItem.SideLinkType SideLinkType
    {
        get { return (SideLinkItem.SideLinkType)ViewState["SideLinkType"]; }
        set { ViewState["SideLinkType"] = (Convert.ToInt32(value)); }
    }
    public void Render()
    {
        try
        {
            divLinks.InnerHtml = "";
            StringBuilder sb = new StringBuilder("");
            if (this.Items.Count > 0)
            {
                if (this.SideLinkType == SideLinkItem.SideLinkType.ButtonWithImageLink)
                {
                    sb.Append("<table cellpadding='2'>");
                    foreach (SideLinkItem bs in this.Items)
                    {
                        
                        sb.Append("<tr> <td>");
                        if (bs.Href.Trim() != "")
                        {
                            if (bs.Target == "_self" || bs.Target == "")
                            {
                                sb.Append("<input onclick=window.location.href='" + EConnect.Utils.Security.QuertStringModule.Encrypt(bs.Href) + "' class='btnNormal'  type='button' Style='border-style:None;height:54px;width:212px;  background-image:url(" + bs.ImageSrc + "); padding: 0 0 13px 50px; text-align: left;' value='" + bs.Text + "' />");
                            }
                            else
                            {
                                if (Context.Request.Browser.Type.ToString().ToUpper().StartsWith("IE"))
                                    sb.Append("<input onclick=window.open('" + EConnect.Utils.Security.QuertStringModule.Encrypt(bs.Href) + "') class='btnNormal'  type='button' Style='border-style:None;height:54px;width:212px;  background-image:url(" + bs.ImageSrc + "); padding: 0 0 13px 50px; text-align: left;' value='" + bs.Text + "' />");
                                else
                                    sb.Append("<a Style='text-decoration:none;' target='" + bs.Target + "' href = '" + EConnect.Utils.Security.QuertStringModule.Encrypt(bs.Href) + "'><input onclick=window.location.href='" + EConnect.Utils.Security.QuertStringModule.Encrypt(bs.Href) + "' class='btnNormal'  type='button' Style='border-style:None;height:54px;width:212px;  background-image:url(" + bs.ImageSrc + "); padding: 0 0 13px 50px; text-align: left;' value='" + bs.Text + "' /></a>");
                            }
                        }
                        else 
                        {
                            if (Context.Request.Browser.Type.ToString().ToUpper().StartsWith("IE"))
                                sb.Append("<input class='btnNormal'  type='button' Style='border-style:None;height:54px;width:212px;  background-image:url(" + bs.ImageSrc + "); padding: 0 0 13px 50px; text-align: left;' value='" + bs.Text + "' />");
                            else
                                sb.Append("<input class='btnNormal'  type='button' Style='border-style:None;height:54px;width:212px;  background-image:url(" + bs.ImageSrc + "); padding: 0 0 13px 50px; text-align: left;' value='" + bs.Text + "' />");
                        }
                        sb.Append("</td></tr>");
                    }
                    sb.Append("</table>");
                }
                else if (this.SideLinkType == SideLinkItem.SideLinkType.DownloadLink)
                {
                    sb.Append("<table align='center' class='download' cellspacing='0' cellpadding='0' id='tblNavLinks' width='97%'>");
                    foreach (SideLinkItem bs in this.Items)
                    {
                        sb.Append("<tr> <td>");
                        sb.Append("<a target='" + bs.Target + "' href='" + EConnect.Utils.Security.QuertStringModule.Encrypt(bs.Href) + "'<span></span>" + bs.Text + "</a>");
                        sb.Append("</td></tr>");
                    }
                    sb.Append("</table>");
                }
                else if (this.SideLinkType == SideLinkItem.SideLinkType.Hyperlink)
                {
                    sb.Append("<table align='center' class='nav' cellspacing='0' cellpadding='0' id='tblNavLinks' width='97%'>");
                    foreach (SideLinkItem bs in this.Items)
                    {
                     sb.Append("<tr> <td style= 'border: 1px solid #2c5070; background-color: #FFFFFF;' width='100%'>");
                     sb.Append("<a target='" + bs.Target + "' href='" + EConnect.Utils.Security.QuertStringModule.Encrypt(bs.Href) + "'<span></span>" + bs.Text + "</a>");
                     sb.Append("</td></tr>");
                    }
                    sb.Append("</table>");
                }
            }
            divLinks.InnerHtml = sb.ToString();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}