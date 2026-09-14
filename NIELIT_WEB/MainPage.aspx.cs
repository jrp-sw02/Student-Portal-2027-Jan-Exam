using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.DAL;
using EConnect.URM;

public partial class MainPage : BasePage, ICallbackEventHandler
{
    String defaultIconPath = "";
    String result = null;
    EConnectContext context;
    int moduleID = 3;
    int roleID = 3;
    UserType loginUserType;
    #region ICallbackEventHandler Members
    public void RaiseCallbackEvent(string eventArgument)
    {
        if (eventArgument == "CheckLog")
        {
            Dictionary<Int32, Int32> LoginUsersList = new Dictionary<Int32, Int32>();
            LoginUsersList = (Dictionary<Int32, Int32>)Application["LoginUsersList"];
            if (!LoginUsersList.ContainsValue(Convert.ToInt32(Session["LogID"])))
                result = "You have been logged out. Please login again.\nReason: Either you are logged in from another location or your session has been expired due to some reason.";
            else
                result = "";
        }

    }
    public string GetCallbackResult()
    {
        return result;
    }
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false && Request.RequestType == "GET")
                Response.Redirect("Index.aspx");
            loginUserType = (UserType)Session["UserType"];
         
            if (!Page.IsPostBack)
            {
                Session["OrgID"] = "1";
                showheader();            
                moduleID = Convert.ToInt32(Session["ModuleId"]);
                roleID  = Convert.ToInt32(Session["RoleID"]);
                BindMenuBar();
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "<script language='javascript'>function CheckLog(){ var args = 'CheckLog';" + ClientScript.GetCallbackEventReference(this, "args", "ShowAlert", null) + "}</script>");
            }
        }
        catch (Exception ex)
        {
            this.ShowAlert(ex.Message);
        }
    }
    protected void showheader()
    {
        using (EConnectContext ctx = new EConnectContext())
        {
            Int32 orgID = Convert.ToInt32(Session["OrgID"]);
            var users1 = (from u in ctx.Organizations
                          where u.ID == orgID
                          select u).Single();
            tdHeaderBig.InnerText = users1.Name;
            string m = users1.MainHeading;
            string s = users1.SubHeading;
            tdHeaderSmall.InnerText = string.Concat(m, s);
        }
    }
    protected void BindMenuBar()
    {
        try
        {
            Menu1.Items.Clear();
            using (EConnectContext context = new EConnectContext())
            {//
                var menuObjects = (from m in context.MenuObjects
                                   join r in context.UserRights on m.ID equals r.MenuObject.ParentMenuObjectID
                                   where (m.MenuObjectTypeID == (int)enmMenuObjectType.Menu && m.OrganizationID == 1 &&
                                   m.ParentMenuObjectID == moduleID && 
                                   (r.HasDelete == true || r.HasEdit == true || r.HasFullControl == true || r.HasNew == true || r.HasView == true)
                                   && r.MenuObject.ParentMenuObjectID == m.ID && r.RoleID == roleID)
                                   select m).Distinct().OrderBy(d=>d.DisplayOrder);
                MenuItem mnItem;
                foreach (MenuObject menuObject in menuObjects)
                {
                    mnItem = new MenuItem(menuObject.Name, menuObject.ID.ToString());
                    Menu1.Items.Add(mnItem);
                    if (menuObject.IsParent == false)
                    {
                        mnItem.Selectable = true;
                        mnItem.NavigateUrl = menuObject.FormURL;
                        if (menuObject.OpenInNewWindow == true)
                        {
                            mnItem.Target = "_blank";
                        }
                        else
                        {
                            mnItem.Target = "ifHome";
                        }
                    }
                    else
                        mnItem.Selectable = false;
                    BindMenuForms(context, ref mnItem, menuObject.ID);
                }
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void BindMenuForms(EConnectContext context, ref MenuItem ParentMenuItem, Int32 parentID)
    {
        try
        {// && r.HasView == true 
            MenuItem mnItem;
            var menuObjects = (from m in context.MenuObjects join r in context.UserRights on m.ID equals r.MenuObjectID 
                              where m.ParentMenuObjectID == parentID && r.RoleID == roleID && 
                              (r.HasDelete ==  true || r.HasEdit ==  true || r.HasFullControl == true || r.HasNew == true || r.HasView == true)
                              orderby m.DisplayOrder
                              select m).Distinct();
            foreach (MenuObject menuObject in menuObjects)
            {
                mnItem = new MenuItem(menuObject.Name, menuObject.ID.ToString());
                ParentMenuItem.ChildItems.Add(mnItem);
                if (menuObject.IsParent == false)
                {
                    mnItem.Selectable = true;
                    mnItem.NavigateUrl = menuObject.FormURL;
                    if (menuObject.OpenInNewWindow == true)
                    {
                        mnItem.Target = "_blank";
                    }
                    else
                    {
                        mnItem.Target = "ifHome";
                    }
                }
                else
                    mnItem.Selectable = false;
                BindMenuForms(context,ref mnItem, menuObject.ID);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void lbLogOut_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
            Response.Redirect("Index.aspx", true);
        }
        catch (Exception ex)
        {
            this.ShowAlert(ex.Message);
        }
    }
    protected String GetUserName()
    {
        try
        {            
            String UserName = "Guest";
            if (Session["UserID"] != null)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    User loginUser = context.Users.Find(Convert.ToInt32(Session["UserID"]));
                    UserName = loginUser.UserName.ToLower() ;
                    if (UserName.Length > 15)
                        UserName = UserName.Substring(0, 10) + "...";                     
                }
            }
            return UserName;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected String GetDefaultPath(Int16 MenuObjectType, String ImageType)
    {
        try
        {
            context = new EConnectContext();
            if (defaultIconPath == "")
            {
                if (ImageType.ToUpper() == "ICON")
                    return context.MenuObjectTypes.Find(2).DefaultIconPath ;
                else
                    return context.MenuObjectTypes.Find(2).DefaultThumbNailPath;
            }
            else
                return defaultIconPath;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally{ context.Dispose(); }
    }

 
}