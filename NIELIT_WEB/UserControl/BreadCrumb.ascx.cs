using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI.HtmlControls;
/// <summary>
///  this class contains the items collection of BreadCrumbItem class.
/// </summary>
public partial class BreadCrumb : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
     ///<summary>
        ///steps to access the BreadCrumbItem class.
     ///</summary>
        //BreadCrumbItem breadCrumbItem = new BreadCrumbItem("Home","#","");
        //BreadCrumbItem breadCrumbItem1 = new BreadCrumbItem("Courses", "#", "");
        //BreadCrumbItem breadCrumbItem2 = new BreadCrumbItem("O Level", "#", "");
        //BreadCrumbItem breadCrumbItem3 = new BreadCrumbItem("Exams", "#", "");
        //List<BreadCrumbItem> lstItems = new List<BreadCrumbItem>();
        //lstItems.Add(breadCrumbItem);
        //lstItems.Add(breadCrumbItem1);
        //lstItems.Add(breadCrumbItem2);
        //lstItems.Add(breadCrumbItem3);
        //BreadCrumb1.Items = lstItems;
        //BreadCrumb1.Render();
    }

 #region Private Members
    private List<BreadCrumbItem> _items = new List<BreadCrumbItem>();
#endregion
//#region Public Properties
//    /// <summary>
//    /// gets or sets the  items list of BreadCrumb object..
//    /// </summary>
//    public List<BreadCrumbItem> Items
//    {
//        get { return _items; }
//    }
//    /// <summary>
//    /// gets or sets the collection of BreadCrumb items in JSON (Serialized String) format
//    /// </summary>
//    public String SerializedItems
//    {
//        set
//        {
//            StringBuilder deSerializedItems = new StringBuilder(value);
//            JavaScriptSerializer js = new JavaScriptSerializer();
//            List<BreadCrumbItem> lst = (List<BreadCrumbItem>)js.Deserialize(deSerializedItems.ToString().Replace("@", "&").Replace("^", "{").Replace("$", "[").Replace("!", "}").Replace("#", "}"), typeof(List<BreadCrumbItem>));
//            _items = lst;
//        }
//        get
//        {
//            StringBuilder serializedItems = new StringBuilder();
//            JavaScriptSerializer js = new JavaScriptSerializer();
//            js.Serialize(_items, serializedItems);
//            return serializedItems.ToString().Replace("&", "@").Replace("{", "^").Replace("[", "$").Replace("}", "!").Replace("}", "#");
//        }
//    }
// #endregion
   

 /// <summary>
 /// this function shows the navigation menu which shows user's current position in 
/// relation to the site's hierarchy.
/// </summary>
    public void Render()
    {
        try
        {
            _items = (List<BreadCrumbItem>)Session["Bread"];
            ulScrumb.InnerHtml = "";
            StringBuilder sb = new StringBuilder("");
            if (_items.Count > 0)
            {
                Int32 itemCount = _items.Count;
                Int32 maxLimit = 4;
                //sb.Append("<ul class='crumbs'>");
                Int16 zindex = 9;
                foreach (BreadCrumbItem bs in _items)
                {
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    HyperLink hl = new HyperLink();
                    li.Attributes.Add("class", "first");
                    hl.Target = bs.Target;
                    hl.ToolTip = bs.Text;
                    if(bs.Href !="" && bs.Href !="#")
                        hl.NavigateUrl = EConnect.Utils.Security.QuertStringModule.Encrypt("~/" + bs.Href);
                    else
                        hl.NavigateUrl = "";
                    hl.Style.Add("z-index", zindex.ToString());
                    if (zindex == 9)
                    {
                        if (itemCount > maxLimit)
                        {
                            hl.Text = "<span></span>" + (bs.Text.Length > 5 ? bs.Text.Substring(0, 5) + ".." : bs.Text);
                            //sb.Append("<li class='first'><a target='" + bs.Target + "' title='" + bs.Text + "' href='" + EConnect.Utils.Security.QuertStringModule.Encrypt(bs.Href) + "' style='z-index:" + zindex.ToString() + ";'><span></span>" + (bs.Text.Length > 5 ? bs.Text.Substring(0, 5) + ".." : bs.Text) + "</a></li>");
                        }
                        else
                        {
                            hl.Text = "<span></span>" +  bs.Text;
                            //sb.Append("<li class='first'><a target='" + bs.Target + "' title='" + bs.Text + "' href='" + EConnect.Utils.Security.QuertStringModule.Encrypt(bs.Href) + "' style='z-index:" + zindex.ToString() + ";'><span></span>" + bs.Text + "</a></li>");
                        }
                       
                        //sb.Append("<li class='first'><a target='" + bs.Target + "' title='" + bs.Text + "' href='" + bs.Href + "' style='z-index:" + zindex.ToString() + ";'><span></span>" + bs.Text + "</a></li>");
                    }
                    else
                    {
                        if (itemCount > maxLimit && _items.IndexOf(bs) < maxLimit)
                        {
                            hl.Text = (bs.Text.Length > 5 ? bs.Text.Substring(0, 5) + ".." : bs.Text);
                            //sb.Append("<li ><a target='" + bs.Target + "' title='" + bs.Text + "' href='" + EConnect.Utils.Security.QuertStringModule.Encrypt(bs.Href) + "' style='z-index:" + zindex.ToString() + ";'>" + (bs.Text.Length > 5 ? bs.Text.Substring(0, 5) + ".." : bs.Text) + "</a></li>");
                        }
                        else
                        {
                            hl.Text =  bs.Text;
                            //sb.Append("<li ><a target='" + bs.Target + "' title='" + bs.Text + "' href='" + EConnect.Utils.Security.QuertStringModule.Encrypt(bs.Href) + "' style='z-index:" + zindex.ToString() + ";'>" + bs.Text + "</a></li>");
                        }
                        //sb.Append("<li ><a target='" + bs.Target + "' title='" + bs.Text + "' href='" + bs.Href + "' style='z-index:" + zindex.ToString() + ";'>" + bs.Text + "</a></li>");
                    }
                    zindex--;
                    li.Controls.Add(hl);
                    ulScrumb.Controls.Add(li);
                }
                //sb.Append("</ul>");
            }
        }
        catch (Exception ex)
        {
            //throw ex;
        }
    }
    public void AddNewBreadCrumbItem(BreadCrumbItem newBreadCrumbItem)
    {
        try
        {
            if (Context.Request.UrlReferrer != null)
            {
                if (Context.Request.UrlReferrer.ToString().ToUpper().Contains("MAINPAGE.ASPX") || Context.Request.UrlReferrer.ToString().ToUpper().Contains("INDEX.ASPX"))
                    Session["Bread"] = null;
            }
            else
                Session["Bread"] = null;
            if (Session["Bread"] == null)
                _items.Add(newBreadCrumbItem);
            else
                _items = (List<BreadCrumbItem>)Session["Bread"];

            List<BreadCrumbItem> lstITem = new List<BreadCrumbItem>();
            foreach (BreadCrumbItem bc in _items)
            {
                if (!(bc.Href == newBreadCrumbItem.Href && bc.Text == newBreadCrumbItem.Text && bc.Target == newBreadCrumbItem.Target))
                    lstITem.Add(bc);
                else
                    break;
            }
            lstITem.Add(newBreadCrumbItem);
            _items = lstITem;
            Session["Bread"] = _items;
            this.Render();
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    public void UpdateLastBreadCrumbItem(BreadCrumbItem newBreadCrumbItem)
    {
        try
        {
            if (Context.Request.UrlReferrer.ToString().ToUpper().Contains("MAINPAGE.ASPX") || Context.Request.UrlReferrer.ToString().ToUpper().Contains("INDEX.ASPX"))
                Session["Bread"] = null;
            if (Session["Bread"] == null)
                return;
            else
                _items = (List<BreadCrumbItem>)Session["Bread"];
            _items[_items.Count - 1] = newBreadCrumbItem;
            Session["Bread"] = _items;
            this.Render();
        }
        catch (Exception ex)
        {
            //throw ex;
        }
    }
    public void RemoveLastBreadCrumbItem()
    {
        try
        {
            if (Context.Request.UrlReferrer.ToString().ToUpper().Contains("MAINPAGE.ASPX") || Context.Request.UrlReferrer.ToString().ToUpper().Contains("INDEX.ASPX"))
                Session["Bread"] = null;
            if (Session["Bread"] == null)
                return;
            else
                _items = (List<BreadCrumbItem>)Session["Bread"];
            _items.RemoveAt(_items.Count - 1);
            Session["Bread"] = _items;
        }
        catch (Exception ex)
        {
            //throw ex;
        }
    }
    //public void RestoreSerializedItems(String serializedItems = null )
    //{
    //    try 
    //    {
    //        if (string.IsNullOrEmpty(this.renderedItems.Value))
    //            throw new Exception("Serialized items not set or are null");
    //        StringBuilder deSerializedItems = new StringBuilder(this.renderedItems.Value);
    //        JavaScriptSerializer js = new JavaScriptSerializer();
    //        List<BreadCrumbItem> lst = (List<BreadCrumbItem>)js.Deserialize(deSerializedItems.ToString(), typeof(List<BreadCrumbItem>));
    //        _items = lst;
    //    }
    //    catch(Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
}

