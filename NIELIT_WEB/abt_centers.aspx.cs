using System;
using System.Linq;
using System.Web.UI.WebControls;
using EConnect.DAL;

public partial class abt_centers : BasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        showdata();
        //showsidelink();
        if (Request.UrlReferrer.ToString().ToLower().Contains("frmaccredetedcentre.aspx"))
        {
            BreadCrumb1.RemoveLastBreadCrumbItem();
        }
        BreadCrumb1.AddNewBreadCrumbItem(new BreadCrumbItem("Regional Centres", "abt_centers.aspx", ""));
    }
    protected void showdata()
    {
        try
        {
            using (EConnectContext context = new EConnectContext())
            {
                var Regcentre = (from p in context.RegionalCenters
								where p.isActive==true
                                 select new
                                 {
                                     ID = p.ID,
                                     organization = "NIELIT Centre " + p.Name,
                                     address = p.Address,
                                     website = p.Website,
                                     contactno = p.ContactNumbers,
                                     email = p.EmailAddresses
                                 }).ToList();
                gvMain.AllowPaging = false;
                gvMain.DataSource = Regcentre;
                gvMain.DataBind();
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvMain_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Encryption url of hypelink field
                HyperLink hl1 = (HyperLink)e.Row.Cells[3].Controls[0];
                e.Row.Cells[0].Text = (e.Row.RowIndex + 1).ToString(); 
				//Added for email display 3 feb 2021
				 Label l1 =(Label) e.Row.FindControl("chEmail");
                if (l1 != null)
                    l1.Text = CommonFunctions.ChangeEmailDisplay(e.Row.Cells[5].Text);
            }
			 e.Row .Cells [5].Visible =false;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    //protected void showsidelink()
    //{
    //    try
    //    {
    //        Sidelink.Items.Add(new SideLinkItem("Apply Online", "./WEB/allCourses.aspx?query=apply", "images/Apply_Online.jpg"));
    //        Sidelink.Items.Add(new SideLinkItem("Certifications/Courses", "./WEB/allCourses.aspx", "images/Get_Filled_Form.jpg"));
    //        Sidelink.SideLinkType = SideLinkItem.SideLinkType.ButtonWithImageLink;
    //        Sidelink.Render();
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //}
}