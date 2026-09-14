using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class DashBoard1_NIELITCentres : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        refreshdata();
    }

    public void refreshdata()
    {
        string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

        // Count of NIELIT Centres
        parameters vPara = new parameters();
        vPara.count = 1;
        vPara.pLinkedToCentre = 0;
        DataSet ds = new DataSet();
        ds = utility.executeProcedure("getStateNIELITCentres", vPara);
        

       // using (SqlConnection con = new SqlConnection(CS))
       // {
            //string sqlquery = " select distinct  st.ID, st.Name, count(st.Name)Centre FROM [NIELIT].[dbo].[Location] st,[NIELIT].[dbo].[NIELITCentreBatch] nb," +
            //                 "[NIELIT].[dbo].[Institute] inst  where Parent_ID=1 and nb.InstituteID=inst.ID and inst.State_ID=st.ID group by st.ID, st.Name Order by 1,3";

            //string sqlquery = "  select distinct st.Name, nc.instituteID, upper(nc.Name) as instname from [NIELIT].[dbo].[NIELITCentres] nc,[NIELIT].[dbo].[Location] st where st.ID=nc.State_ID " +
            //                 "and linkedToCentre is not null group by st.Name,nc.instituteID, nc.Name";
            //string sqlquery = "  select distinct st.Name, nc.instituteID, nc.Name as instname from [NIELIT].[dbo].[NIELITCentres] nc,[NIELIT].[dbo].[Location] st where st.ID=nc.State_ID and st.Parent_ID=1" +
            //                 "and nc.instituteid in (select centreid from NIELIT.dbo.centrewisecourses) group by st.Name,nc.instituteID, nc.Name";   
            //using (SqlCommand cmd = new SqlCommand(sqlquery, con))
            //{
            //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
            //    DataSet ds = new DataSet();
               // sda.Fill(ds);
                GridView1.DataSource = ds;
                GridView1.DataBind();
            //}
       // }
        btnBack.Visible = false;
    }
    protected void gvChildGrid_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "CentreDet")
        {
            string ID = e.CommandArgument.ToString();
            //Get the data value and bind into textbox..
            if (ID != null)
            {
                //string querystring = EncryptDecryptD.Encrypt("RegionalCentreDetails.aspx?pInstituteID=" + ID);
                //Response.Redirect("RegionalCentreDetails.aspx?pInstituteID=" + ID);
                //Response.Redirect(querystring);

                Response.Redirect("RegionalCentreDetails.aspx?pInstituteID=" + ID);
            }
            

        }

    }

    protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Select$" + e.Row.RowIndex);
            //e.Row.ToolTip = "Click to select this row.";

            parameters vPara = new parameters();
            vPara.count = 0;
            

            Label lblinst = (Label)e.Row.FindControl("lblInstID");
            if (lblinst != null)
            {
                vPara.pLinkedToCentre = Convert.ToInt64(lblinst.Text);
                vPara.count = 1;
            }



                GridView gv = (GridView) e.Row.FindControl("gvChildGrid");  
                string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;

                    // Count of NIELIT Centres
        
                 DataSet ds = new DataSet();
                 ds = utility.executeProcedure("getStateNIELITCentres", vPara);
        
                gv.DataSource = ds;
                gv.DataBind();



   
        }         




    }

    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        string querystring = EncryptDecryptD.Encrypt("RegionalCentreDetails.aspx?pInstituteID=" + GridView1.SelectedValue);
        Response.Redirect(querystring);
        //Response.Redirect("RegionalCentreDetails.aspx?pInstituteID=" + GridView1.SelectedValue);
        //int index = GridView1.SelectedRow.RowIndex;
        // int Id = Convert.ToInt32(GridView1.DataKeys[index].Values[0]);
        //string name = GridView1.SelectedRow.Cells[1].Text;
        //string country = GridView1.SelectedRow.Cells[2].Text;
        //string message = "Row Index: " + index + "\\nName: " + name + "\\nCountry: " + country;
        ////ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
      
        //string CS = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
        //using (SqlConnection con = new SqlConnection(CS))
        //{
        //    //string sqlquery = " select distinct st.ID, st.Name,  inst.Name as inst from [NIELIT].[dbo].[NIELITCentreBatch] nb,[NIELIT].[dbo].[Institute] inst," +
        //    //                 "[NIELIT].[dbo].[Location] st where nb.InstituteID=inst.ID  and st.ID=inst.State_ID and st.Parent_ID=1  and st.ID = " + Id + " Order by st.Name";

        //    string sqlquery = "select distinct st.Name, nc.instituteID, nc.Name as instname,cc.Name as CourseName from [NIELIT].[dbo].[NIELITCentres] nc,[NIELIT].[dbo].[Location] st ,[NIELIT].[dbo].[centrewisecourses] ncc,[NIELIT].[dbo].[NIELITCourse] cc " +
        //                     " where st.ID=nc.State_ID and st.Parent_ID=1 and cc.ID=ncc.CourseId and nc.instituteid= ncc.CentreID and nc.instituteID=" + Id + " and ncc.CourseId in ( Select id  FROM [NIELIT].[dbo].[Course]) group by st.Name,nc.instituteID, nc.Name,cc.Name";

        //    using (SqlCommand cmd = new SqlCommand(sqlquery, con))
        //    {
        //        SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //        DataSet ds = new DataSet();
        //        sda.Fill(ds);
        //        gvpartnerlist.DataSource = ds;
        //        gvpartnerlist.DataBind();
        //    }
        //    GridView1.Visible = false;
        //    btnBack.Visible = true;
        //    lblstatenamelist.Text = name.ToUpper()+ " " +" NIELIT REGIONAL CENTER COURSE LIST";
        //}      
       
    }
    protected void gvpartnerlist_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    if (e.Row.RowState == DataControlRowState.Alternate)
        //    {
        //        e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#F5F5DC ';");
        //        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");
        //    }
        //    else
        //    {
        //        e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#F5F5DC';");
        //        e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='White';");
        //    }
        //}
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("NIELITRegionalCentre.aspx");
    }
   //protected void btnBackNRC_Click(object sender, EventArgs e)
   //{
   //    Response.Redirect("NIELITRegionalCentre.aspx");
   //}
}