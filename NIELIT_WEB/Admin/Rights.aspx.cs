using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;

public partial class Admin_Rights :BasePage
{
    int moduleID = 0;
    Boolean bshow = true;
    String sBGColor = "#FFFFFF";
    Int32 currentRoleId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.CacheControl = "no-cache";
        Response.AddHeader("Progra", "no-cache");
        Response.Expires = -1500;
        try
        {
            if (IsSessionAlive() == false)
                Response.Redirect("../Index.aspx");
            currentRoleId = Convert.ToInt32(Session["RoleID"]);
            if (!Page.IsPostBack)
            {
                ViewState["SortField"] = "";
                ViewState["SortOrder"] = "";
                FillRole();
                ddlRole.Focus();
                btnCancel.Visible = false;
                btnSave.Visible = false;
                BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Rights", "Admin/Rights.aspx", ""));
            }
            if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                ShowAlert(Request.QueryString["msg"].ToString());
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }
    }
    protected void FillRole()
    {
        try
        {
            using (var context = new EConnectContext())
            {
                ListItem lst = new ListItem("--Select One--", "0");
                var roles = from s in context.Roles
                              select new { ValueField = s.ID, TextField = s.Name };
                EConnect.Utils.Common.ControlUtility.BindListObject(ddlRole, roles, lst);

            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void BindMenuBar()
    {
       
        try
        {
            LiteralControl ctlLiteral = new LiteralControl();
            ctlLiteral.Text = "";
            using (EConnectContext context = new EConnectContext())
            {
                var menuObjects = from m in context.MenuObjects
                                  where (m.MenuObjectTypeID == (int)enmMenuObjectType.Menu && m.OrganizationID == 1 && m.ParentMenuObjectID == moduleID)
                                  orderby m.DisplayOrder
                                  select m;
                foreach (var menu in menuObjects)
                {
                    if (menuObjects.Count() > 0)
                    {
                        ctlLiteral.Text += "<Br><table border='0' cellpadding='0' cellspacing='0' width='100%'>" + Environment.NewLine;
                        ctlLiteral.Text += "<tr><td  Style='cursor:pointer;text-align:center;background-color:#31597C;color:#ffffff;' onClick='javascript:ShowHideobject(\"" + "M_" + menu.ID.ToString() + "\")'><b>" + menu.Name.ToString() + "</b></td></tr>" + Environment.NewLine;
                        ctlLiteral.Text += "<tr><td>" + Environment.NewLine;
                        if (bshow == true)
                        {
                            ctlLiteral.Text += "<input Type=hidden id='selectedElmnt' value='" + "M_" + menu.ID.ToString() + "' >" + System.Environment.NewLine;
                            ctlLiteral.Text += "<table id='" + "M_" + menu.ID.ToString() + "' border='0'  cellpadding='0' cellspacing='0' style='display: block' width='100%'>" + Environment.NewLine;
                            bshow = false;
                        }
                        else
                        {
                            ctlLiteral.Text += "<table id='" + "M_" + menu.ID.ToString() + "' border='0' cellpadding='0' cellspacing='0' style='display: none' width='100%'>" + Environment.NewLine;
                        }
                        ctlLiteral.Text += "<tr>" + System.Environment.NewLine;
                        ctlLiteral.Text += "<td>" + System.Environment.NewLine;
                        ctlLiteral.Text += MenuOptions(Convert.ToInt32(menu.ID)) + System.Environment.NewLine;
                        ctlLiteral.Text += "</td>" + System.Environment.NewLine;
                        ctlLiteral.Text += "</tr>" + System.Environment.NewLine;
                        ctlLiteral.Text += "</table>" + System.Environment.NewLine;
                        ctlLiteral.Text += "</td></tr>" + System.Environment.NewLine;
                        ctlLiteral.Text += "</table>" + System.Environment.NewLine;
                    }
                }
		    };
		    pnlObjects.Controls.Add(ctlLiteral);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    private string MenuOptions(int GroupId)
    {
        try
        {
            string tempMenuOptions = null;
            Int32 counter = 1;
            Int32 roleID = Convert.ToInt32(ddlRole.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                var submenu = (from c in context.MenuObjects
                               join r in context.UserRights on new { c.ID, roleID } equals new { ID = r.MenuObjectID, roleID = r.RoleID } into rt
                               from r in rt.DefaultIfEmpty()
                               where c.ParentMenuObjectID == GroupId
                               orderby c.DisplayOrder
                               select new
                               {
                                   isedit = r.HasEdit == null ? "" : r.HasEdit == false ? "" : "checked",
                                   isnew = r.HasNew == null ? "" : r.HasNew == false ? "" : "checked",
                                   isview = r.HasView == null ? "" : r.HasView == false ? "" : "checked",
                                   isdelete = r.HasDelete == null ? "" : r.HasDelete == false ? "" : "checked",
                                   isfullcontrol = r.HasFullControl == null ? "" : r.HasFullControl == false ? "" : "checked",
                                   Name = c.Name,
                                   ID = c.ID,
                               });
                //submenu = submenu.Where(d => d.roleID == roleID);
                tempMenuOptions = "<table border='0' cellpadding='0' cellspacing='0' width='100%'>" + Environment.NewLine;
                tempMenuOptions += "<tr>" + Environment.NewLine;
                tempMenuOptions += "<td  width='15px' Align=center>#</td>" + Environment.NewLine;
                tempMenuOptions += "<td  width='410px' Align=center>Object Name</td>" + Environment.NewLine;
                tempMenuOptions += "<td  width='50px' Align=center>Add</td>" + Environment.NewLine;
                tempMenuOptions += "<td  width='50px' Align=center>Edit</td>" + Environment.NewLine;
                tempMenuOptions += "<td  width='50px' Align=center>Delete</td>" + Environment.NewLine;
                tempMenuOptions += "<td  width='50px' Align=center>View</td>" + Environment.NewLine;
                tempMenuOptions += "<td  width='80px' Align=center>Full Control</td>" + Environment.NewLine;
                tempMenuOptions += "</tr>" + Environment.NewLine;
                sBGColor = "#FFFFFF";
                foreach (var menu in submenu)
                {
                    tempMenuOptions += "<tr>" + Environment.NewLine;
                    tempMenuOptions += "<td BgColor=" + sBGColor + " Align=Right >" + counter + "</td>" + Environment.NewLine;
                    tempMenuOptions += "<td BgColor=" + sBGColor + " Align=left>" + menu.Name.ToString() + "</td>" + Environment.NewLine;
                    tempMenuOptions += "<td BgColor=" + sBGColor + " Align=center><input  " + menu.isnew.ToString().ToLower() + " onclick='checkUncheck()' id='" + GroupId.ToString() + "_" + menu.ID.ToString() + "_chkAdd' type='checkbox' name='" + GroupId.ToString() + "$" + menu.ID.ToString() + "$chkAdd" + "' /></td>" + Environment.NewLine; //need to change
                    tempMenuOptions += "<td BgColor=" + sBGColor + " Align=center><input  " + menu.isedit.ToString().ToLower() + " onclick='checkUncheck()' id='" + GroupId.ToString() + "_" + menu.ID.ToString() + "_chkEdit' type='checkbox' name='" + GroupId.ToString() + "$" + menu.ID.ToString() + "$chkEdit" + "' /></td>" + Environment.NewLine;
                    tempMenuOptions += "<td BgColor=" + sBGColor + " Align=center><input  " + menu.isdelete.ToString().ToLower() + " onclick='checkUncheck()' id='" + GroupId.ToString() + "_" + menu.ID.ToString() + "_chkDelete' type='checkbox' name='" + GroupId.ToString() + "$" + menu.ID.ToString() + "$chkDelete" + "' /></td>" + Environment.NewLine;
                    tempMenuOptions += "<td BgColor=" + sBGColor + " Align=center><input  " + menu.isview.ToString().ToLower() + " onclick='checkUncheck()' id='" + GroupId.ToString() + "_" + menu.ID.ToString() + "_chkView' type='checkbox' name='" + GroupId.ToString() + "$" + menu.ID.ToString() + "$chkView" + "' /></td>" + Environment.NewLine;
                    tempMenuOptions += "<td BgColor=" + sBGColor + " Align=center><input  " + menu.isfullcontrol.ToString().ToLower() + " onclick='checkUncheck()' id='" + GroupId.ToString() + "_" + menu.ID.ToString() + "_chkFull' type='checkbox' name='" + GroupId.ToString() + "$" + menu.ID.ToString() + "$chkFull" + "'/></td>" + Environment.NewLine;
                    tempMenuOptions += "</tr>" + Environment.NewLine;
                    if (sBGColor == "#FFFFFF")
                    {
                        sBGColor = "#EEEEEE";
                    }
                    else
                    {
                        sBGColor = "#FFFFFF";
                    }
                    counter = counter + 1;
                    //tempMenuOptions += ProcessOptions(Convert.ToInt32(menu.ID));
                }
            };
            tempMenuOptions += "</table>" + Environment.NewLine;
            return tempMenuOptions;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private string ProcessOptions(int ObjectId)
    {
        try
        {
            string tempProcessOptions = null;
            Int32 counter = 1;
            Int32 roleID = Convert.ToInt32(ddlRole.SelectedValue);
            using (EConnectContext context = new EConnectContext())
            {
                var submenu1 = (from c in context.MenuObjects
                                join r in context.UserRights
                                    on c.ID equals r.MenuObjectID into t
                                from r in t.DefaultIfEmpty()
                                where c.ParentMenuObjectID == ObjectId && r.RoleID == roleID && c.MenuObjectTypeID == (int)enmMenuObjectType.Menu
                                orderby c.DisplayOrder 
                                select new
                                {
                                    isedit = r.HasEdit,
                                    isnew = r.HasNew,
                                    isview = r.HasView,
                                    isdelete = r.HasDelete,
                                    isfullcontrol = r.HasFullControl,
                                    Name = c.Name,
                                    ID = c.ID
                                });

                foreach (var submenu in submenu1)
                {
                    if (submenu != null)
                    {
                        tempProcessOptions = "";
                        tempProcessOptions += "<tr>" + Environment.NewLine;
                        tempProcessOptions += "<td BgColor=" + sBGColor + " Align=Right >" + counter.ToString() + "</td>" + Environment.NewLine;
                        tempProcessOptions += "<td BgColor=" + sBGColor + " Align=left>" + submenu.Name.ToString() + "</td>" + Environment.NewLine;
                        tempProcessOptions += "<td BgColor=" + sBGColor + " Align=center><input onclick='checkUncheck()' id='" + ObjectId.ToString() + "_" + submenu.ID.ToString() + "_chkAdd' type='checkbox' name='" + ObjectId.ToString() + "$" + submenu.ID.ToString() + "$chkAdd' " + submenu.isnew.ToString() + " /></td>" + Environment.NewLine;
                        tempProcessOptions += "<td BgColor=" + sBGColor + " Align=center><input onclick='checkUncheck()' id='" + ObjectId.ToString() + "_" + submenu.ID.ToString() + "_chkEdit' type='checkbox' name='" + ObjectId.ToString() + "$" + submenu.ID.ToString() + "$chkEdit' " + submenu.isedit.ToString() + " /></td>" + Environment.NewLine;
                        tempProcessOptions += "<td BgColor=" + sBGColor + " Align=center><input onclick='checkUncheck()' id='" + ObjectId.ToString() + "_" + submenu.ID.ToString() + "_chkDelete' type='checkbox' name='" + ObjectId.ToString() + "$" + submenu.ID.ToString() + "$chkDelete' " + submenu.isdelete.ToString() + " /></td>" + Environment.NewLine;
                        tempProcessOptions += "<td BgColor=" + sBGColor + " Align=center><input onclick='checkUncheck()' id='" + ObjectId.ToString() + "_" + submenu.ID.ToString() + "_chkView' type='checkbox' name='" + ObjectId.ToString() + "$" + submenu.ID.ToString() + "$chkView' " + submenu.isview.ToString() + " /></td>" + Environment.NewLine;
                        tempProcessOptions += "<td BgColor=" + sBGColor + " Align=center><input onclick='checkUncheck()' id='" + ObjectId.ToString() + "_" + submenu.ID.ToString() + "_chkFull' type='checkbox' name='" + ObjectId.ToString() + "$" + submenu.ID.ToString() + "$chkFull' " + submenu.isfullcontrol.ToString() + " /></td>" + Environment.NewLine;
                        tempProcessOptions += "</tr>" + Environment.NewLine;
                        if (sBGColor == "#FFFFFF")
                        {
                            sBGColor = "#EEEEEE";
                        }
                        else
                        {
                            sBGColor = "#FFFFFF";
                        }
                    }
                    else
                    {
                        tempProcessOptions = "";
                    }
                    counter = counter + 1;
                }
            };
            return tempProcessOptions;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Int32 roleID = Convert.ToInt32(ddlRole.SelectedValue);
            if (roleID != 0)
            {
                using (EConnectContext context = new EConnectContext())
                {
                    moduleID = (from c in context.Roles
                                where c.ID == roleID
                                select new
                                {
                                    moduleid = c.Usertype.ModuleID
                                }).FirstOrDefault().moduleid;
                };
                BindMenuBar();
            }
            if (ddlRole.SelectedValue == "0")
            {
                btnCancel.Visible = false;
                btnSave.Visible = false;
            }
            else
            {
                btnCancel.Visible = true;
                btnSave.Visible = true;
            }
      }
      catch(Exception ex)
      {
          ShowAlert(ex.Message);
      }
   }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            Response.Redirect("Rights.aspx");
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            LiteralControl ctlLiteral = new LiteralControl();
            string chkAdd = "";
            string chkEdit = "";
            string chkDelete = "";
            string chkView = "";
            string chkFull = "";
            Int32 roleID = Convert.ToInt32(ddlRole.SelectedValue);
            Int32 counter = 1;
            ctlLiteral.Text = "";
            using (EConnectContext context = new EConnectContext())
            {
                context.Database.ExecuteSqlCommand("delete from URM_RIGHT where Role_ID = " + roleID);
                Int32 moduleID = (from c in context.Roles
                            where c.ID == roleID
                            select new
                            {
                                moduleid = c.Usertype.ModuleID
                            }).FirstOrDefault().moduleid;

                var grouplist = (from m in context.MenuObjects
                                where m.OrganizationID == 1 && m.ParentMenuObjectID == moduleID
                                orderby m.DisplayOrder
                                select m.ID).ToArray();

                for (int i = 0; i < grouplist.Length; i++)
                {
                    Int32 groupid = Convert.ToInt32(grouplist[i]);
                    var submenu1 = from m in context.MenuObjects
                                    where m.ParentMenuObjectID == groupid
                                    orderby m.DisplayOrder
                                    select new
                                    {
                                        ID = m.ID
                                    };
                    foreach (var submenu in submenu1)
                    {
                        EConnect.URM.Right newRights = new EConnect.URM.Right();
                        chkAdd = Request[groupid.ToString() + "$" + submenu.ID.ToString() + "$chkAdd"];
                        chkEdit = Request[groupid.ToString() + "$" + submenu.ID.ToString() + "$chkEdit"];
                        chkDelete = Request[groupid.ToString() + "$" + submenu.ID.ToString() + "$chkDelete"];
                        chkView = Request[groupid.ToString() + "$" + submenu.ID.ToString() + "$chkView"];
                        chkFull = Request[groupid.ToString() + "$" + submenu.ID.ToString() + "$chkFull"];
                        if (string.IsNullOrEmpty(chkAdd))
                        {
                            newRights.HasNew = false;
                        }
                        else
                        {
                            newRights.HasNew = true;
                        }

                        if (string.IsNullOrEmpty(chkEdit))
                        {
                            newRights.HasEdit = false;
                        }
                        else
                        {
                            newRights.HasEdit = true;
                        }
                        if (string.IsNullOrEmpty(chkDelete))
                        {
                            newRights.HasDelete = false;
                        }
                        else
                        {
                            newRights.HasDelete = true;
                        }
                        if (string.IsNullOrEmpty(chkView))
                        {
                            newRights.HasView = false;
                        }
                        else
                        {
                            newRights.HasView = true;
                        }
                        if (string.IsNullOrEmpty(chkFull))
                        {
                            newRights.HasFullControl = false;
                        }
                        else
                        {
                            newRights.HasFullControl = true;
                        }
                        newRights.RoleID = roleID;
                        newRights.MenuObjectID = submenu.ID;
                        newRights.CreatedBy = Convert.ToInt32(Session["UserID"]);
                        newRights.CreatedOn = DateTime.Now;
                        context.UserRights.Add(newRights);
                        counter = counter + 1;
                    }
                }
                context.SaveChanges();
            };
            ddlRole_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}