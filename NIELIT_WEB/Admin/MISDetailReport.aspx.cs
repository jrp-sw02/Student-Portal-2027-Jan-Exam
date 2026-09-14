using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect.URM;
using EConnect.DAL;
using EConnect.NIELIT;

using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;
using System.IO.Compression;

using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Data.SqlTypes;
using System.IO;
using System.Drawing.Imaging;

public partial class Admin_MISDetailReport : BasePage
{
    SqlConnection conn = new SqlConnection();
    SqlCommand cmd = new SqlCommand();
    System.Web.UI.WebControls.Table tbl = new System.Web.UI.WebControls.Table();

    Table tblhead = new Table();
    string strHTML = "";
    String strMessage = string.Empty;
    EConnectContext context;
    Int32 currentRoleId = 0;
    String currentRoleName = string.Empty;

    DataTable dt = new DataTable();
    DataTable dt1 = new DataTable();
    DataTable dt2 = new DataTable();
    // Dim copyDataTable As DataTable
    DataTable dtNF = new DataTable();
    DataTable dtGrandTotal = new DataTable();
    DataTable DTFormal = new DataTable();
    DataTable DTNFormal = new DataTable();
    DataTable DTShortTerm = new DataTable();
    DataTable DTTotal = new DataTable();

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
            currentRoleName = (string)Session["RoleName"];
            if (!UserManager.HasRight(currentRoleId, enmRight.View, "Admin/MISDetailReport.aspx"))
            {
                Response.Write("Sorry! You don't have rights  to view this page");
                Response.End();
            }
            if (!Page.IsPostBack)
            {
                if (!Page.IsPostBack)
                {

                    FillProjects();
                    FillNIELITCenter();
                }
                if (!string.IsNullOrWhiteSpace(Request.QueryString["msg"]))
                    ShowAlert(Request.QueryString["msg"].ToString());
				
				if (Session["RoleID"].ToString() == "28")
                {
                    ddlProject.SelectedValue = "2";
                    ddlProject.Enabled = false;
                }
                else
                {
                    ddlProject.SelectedValue = "0";
                    ddlProject.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message, true);
        }

        
    }

    protected void BindDataTable()
    {
        DTFormal.Columns.Add("SNo");
        DTFormal.Columns.Add("Center");
        DTFormal.Columns.Add("Courses");

        DTFormal.Columns.Add("AnnualTarget", typeof(int));
        DTFormal.Columns.Add("CTrain", typeof(int));
        DTFormal.Columns.Add("CCert", typeof(int));
        DTFormal.Columns.Add("CPlaced", typeof(int));


        DTFormal.Columns.Add("TrainW", typeof(int));
        DTFormal.Columns.Add("CertW", typeof(int));
        DTFormal.Columns.Add("PlacedW", typeof(int));
        DTFormal.Columns.Add("TrainSC", typeof(int));
        DTFormal.Columns.Add("CertSC", typeof(int));
        DTFormal.Columns.Add("PlacedSC", typeof(int));
        DTFormal.Columns.Add("TrainST", typeof(int));
        DTFormal.Columns.Add("CertST", typeof(int));
        DTFormal.Columns.Add("PlacedST", typeof(int));
        DTFormal.Columns.Add("TrainOBC", typeof(int));
        DTFormal.Columns.Add("CertOBC", typeof(int));
        DTFormal.Columns.Add("PlacedOBC", typeof(int));
        DTFormal.Columns.Add("TrainPWD", typeof(int));
        DTFormal.Columns.Add("CertPWD", typeof(int));
        DTFormal.Columns.Add("PlacedPWD", typeof(int));

        DTNFormal.Columns.Add("Center");
        DTNFormal.Columns.Add("Courses");

        DTNFormal.Columns.Add("AnnualTarget", typeof(int));
        DTNFormal.Columns.Add("CTrain", typeof(int));
        DTNFormal.Columns.Add("CCert", typeof(int));
        DTNFormal.Columns.Add("CPlaced", typeof(int));

        DTNFormal.Columns.Add("TrainW", typeof(int));
        DTNFormal.Columns.Add("CertW", typeof(int));
        DTNFormal.Columns.Add("PlacedW", typeof(int));
        DTNFormal.Columns.Add("TrainSC", typeof(int));
        DTNFormal.Columns.Add("CertSC", typeof(int));
        DTNFormal.Columns.Add("PlacedSC", typeof(int));
        DTNFormal.Columns.Add("TrainST", typeof(int));
        DTNFormal.Columns.Add("CertST", typeof(int));
        DTNFormal.Columns.Add("PlacedST", typeof(int));
        DTNFormal.Columns.Add("TrainOBC", typeof(int));
        DTNFormal.Columns.Add("CertOBC", typeof(int));
        DTNFormal.Columns.Add("PlacedOBC", typeof(int));
        DTNFormal.Columns.Add("TrainPWD", typeof(int));
        DTNFormal.Columns.Add("CertPWD", typeof(int));
        DTNFormal.Columns.Add("PlacedPWD", typeof(int));

        DTShortTerm.Columns.Add("Center");
        DTShortTerm.Columns.Add("Courses");

        DTShortTerm.Columns.Add("AnnualTarget", typeof(int));
        DTShortTerm.Columns.Add("CTrain", typeof(int));
        DTShortTerm.Columns.Add("CCert", typeof(int));
        DTShortTerm.Columns.Add("CPlaced", typeof(int));

        DTShortTerm.Columns.Add("TrainW", typeof(int));
        DTShortTerm.Columns.Add("CertW", typeof(int));
        DTShortTerm.Columns.Add("PlacedW", typeof(int));
        DTShortTerm.Columns.Add("TrainSC", typeof(int));
        DTShortTerm.Columns.Add("CertSC", typeof(int));
        DTShortTerm.Columns.Add("PlacedSC", typeof(int));
        DTShortTerm.Columns.Add("TrainST", typeof(int));
        DTShortTerm.Columns.Add("CertST", typeof(int));
        DTShortTerm.Columns.Add("PlacedST", typeof(int));
        DTShortTerm.Columns.Add("TrainOBC", typeof(int));
        DTShortTerm.Columns.Add("CertOBC", typeof(int));
        DTShortTerm.Columns.Add("PlacedOBC", typeof(int));
        DTShortTerm.Columns.Add("TrainPWD", typeof(int));
        DTShortTerm.Columns.Add("CertPWD", typeof(int));
        DTShortTerm.Columns.Add("PlacedPWD", typeof(int));

        //Total Count 

        DTTotal.Columns.Add("Courses");
        DTTotal.Columns.Add("AnnualTarget", typeof(int));
        DTTotal.Columns.Add("CTrain", typeof(int));
        DTTotal.Columns.Add("CCert", typeof(int));
        DTTotal.Columns.Add("CPlaced", typeof(int));
        DTTotal.Columns.Add("TrainW", typeof(int));
        DTTotal.Columns.Add("CertW", typeof(int));
        DTTotal.Columns.Add("PlacedW", typeof(int));
        DTTotal.Columns.Add("TrainSC", typeof(int));
        DTTotal.Columns.Add("CertSC", typeof(int));
        DTTotal.Columns.Add("PlacedSC", typeof(int));
        DTTotal.Columns.Add("TrainST", typeof(int));
        DTTotal.Columns.Add("CertST", typeof(int));
        DTTotal.Columns.Add("PlacedST", typeof(int));
        DTTotal.Columns.Add("TrainOBC", typeof(int));
        DTTotal.Columns.Add("CertOBC", typeof(int));
        DTTotal.Columns.Add("PlacedOBC", typeof(int));
        DTTotal.Columns.Add("TrainPWD", typeof(int));
        DTTotal.Columns.Add("CertPWD", typeof(int));
        DTTotal.Columns.Add("PlacedPWD", typeof(int));
        
    }

    protected void FillNIELITCenter()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--All--", "-1");
                var Center = from p in context.NielitCentres

                               select new { ValueField = p.ID, TextField = p.Name };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlCenter, Center, lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void FillProjects()
    {
        try
        {
            using (NIELITMISContext context = new NIELITMISContext())
            {
                System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem("--All--", "-1");
                var projList = from p in context.NielitProjectss
                               //where System.DateTime.Today >= p.projectFromDate
                               //&& System.DateTime.Today <= p.projectTodate
                               select new { ValueField = p.ID, TextField = p.ProjectName };

                EConnect.Utils.Common.ControlUtility.BindListObject(ddlProject, projList.Distinct(), lst);
            };
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        try
        {
            BreadCrumb1.Render();
            ResetAll();

        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);

        }

    }

    protected void ResetAll()
    {
        try
        {
            lblError.Visible = false;
            ddlCenter.SelectedValue = "0";
            Div_GridPnl.Visible = false;
            txtDateFrom.Text = "";
            txtToDate.Text = "";
            ddlProject.SelectedValue = "0";
            txtDateFrom.Text = "";
            txtToDate.Text = "";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }

    }


    //Print and Create PDF 
    protected void btnView_Click(object sender, EventArgs e)
    {

       // string str = Session["UserName"].ToString();
        GenResult();
        //AddPageNumber();

    }

    protected void GenResult()
    {
        //ShowTableHeader();
        NIELITMISContext context = new NIELITMISContext(); 
        string StrBunch = "";
        string StrImpExp = "";
        string Query = "";
        string Query1 = "";

        string pathHead = "";
        string Path = "";
        string imgPath = "";
        string path1 = "";
        string path2 = "";
        string path3 = "";

        DateTime pDateFrom = Convert.ToDateTime("1/1/1990");
        DateTime pDateFromTo = Convert.ToDateTime("1/1/1990");
        string imgpath=Server.MapPath ("~/images");

        //FileStream fsHead = new FileStream(imgpath, FileMode.Open, FileAccess.Read, FileShare.Read);
        //FileStream fs = new FileStream(imgpath, FileMode.Open, FileAccess.Read, FileShare.Read);
        //FileStream fsLogo = new FileStream(imgpath, FileMode.Open, FileAccess.Read, FileShare.Read);
        //FileStream fsGD;
        FileStream fsHead; 
        FileStream fs;
        FileStream fsLogo; 
        FileStream fsGD;

        BindDataTable();
        string constr = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        try
        {

            con.Open();

            Int64 centerID = Convert.ToInt64(ddlCenter.SelectedValue);
            Int64 projectID = Convert.ToInt64(ddlProject.SelectedValue);
            pDateFrom = Convert.ToDateTime(txtDateFrom.Text);
            pDateFromTo = Convert.ToDateTime(txtToDate.Text);
            int i = 1;

            //NonAffiliated CenterWise 
            using (SqlCommand Cmm = new SqlCommand("NFReportCenterwiseSummary", con))
            {
                Cmm.CommandType = CommandType.StoredProcedure;

                Cmm.Parameters.Add(new SqlParameter("@pCentreID", centerID));
                Cmm.Parameters.Add(new SqlParameter("@pProjectID", projectID));
                Cmm.Parameters.Add(new SqlParameter("@pDateFrom", pDateFrom));
                Cmm.Parameters.Add(new SqlParameter("@pDateTo", pDateFromTo));

                SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                Sda.Fill(dtNF);
            }

            if (centerID == -1)
            {
                var Center = from p in context.NielitCentres
                             select new
                             {
                                 p.ID,
                                 p.Name
                             };



                foreach (var CenterID in Center.Distinct())
                {
                    centerID = Convert.ToInt64(CenterID.ID);


                    using (SqlCommand Cmm = new SqlCommand("ReportCenterwiseSummary", con))
                    {
                        Cmm.CommandType = CommandType.StoredProcedure;

                        Cmm.CommandType = CommandType.StoredProcedure;
                        Cmm.Parameters.Add(new SqlParameter("@pCentreID", centerID));
                        Cmm.Parameters.Add(new SqlParameter("@pProjectID", projectID));
                        Cmm.Parameters.Add(new SqlParameter("@pDateFrom", pDateFrom));
                        Cmm.Parameters.Add(new SqlParameter("@pDateTo", pDateFromTo));

                        SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                        Sda.Fill(dt1);
                    }


                    foreach (DataRow sourcerow in dt1.Rows)
                    {

                        DataRow destRow = DTFormal.NewRow();

                        destRow["Sno"] = i.ToString();
                        destRow["Center"] = CenterID.Name.ToString();
                        destRow["Courses"] = "Formal Courses";

                        destRow["AnnualTarget"] = sourcerow["FAnnualTarget"];
                        destRow["CTrain"] = sourcerow["CFTrain"];
                        destRow["CCert"] = sourcerow["CFCert"];
                        destRow["CPlaced"] = sourcerow["CFPlaced"];


                        destRow["TrainW"] = sourcerow["FTrainW"];
                        destRow["CertW"] = sourcerow["FCertW"];
                        destRow["PlacedW"] = sourcerow["FPlacedW"];

                        destRow["TrainSC"] = sourcerow["FTrainSC"];
                        destRow["CertSC"] = sourcerow["FCertSC"];
                        destRow["PlacedSC"] = sourcerow["FPlacedSC"];

                        destRow["TrainST"] = sourcerow["FTrainST"];
                        destRow["CertST"] = sourcerow["FCertST"];
                        destRow["PlacedST"] = sourcerow["FPlacedST"];

                        destRow["TrainOBC"] = sourcerow["FTrainOBC"];
                        destRow["CertOBC"] = sourcerow["FCertOBC"];
                        destRow["PlacedOBC"] = sourcerow["FPlacedOBC"];

                        destRow["TrainPWD"] = sourcerow["FTrainPWD"];
                        destRow["CertPWD"] = sourcerow["FCertPWD"];
                        destRow["PlacedPWD"] = sourcerow["FPlacedPWD"];

                        DTFormal.Rows.Add(destRow);
                        i = i + 1;

                    }
                    //i = 1;
                    foreach (DataRow sourcerow in dt1.Rows)
                    {
                        DataRow destRow = DTNFormal.NewRow();
                        destRow["Center"] = "";
                        destRow["Courses"] = "Non-Formal Courses";

                        destRow["AnnualTarget"] = sourcerow["NFAnnualTarget"];
                        destRow["CTrain"] = sourcerow["CNFTrain"];
                        destRow["CCert"] = sourcerow["CNFCert"];
                        destRow["CPlaced"] = sourcerow["CNFPlaced"];

                        destRow["TrainW"] = sourcerow["NFTrainW"];
                        destRow["CertW"] = sourcerow["NFCertW"];
                        destRow["PlacedW"] = sourcerow["NFPlacedW"];

                        destRow["TrainSC"] = sourcerow["NFTrainSC"];
                        destRow["CertSC"] = sourcerow["NFCertSC"];
                        destRow["PlacedSC"] = sourcerow["NFPlacedSC"];

                        destRow["TrainST"] = sourcerow["NFTrainST"];
                        destRow["CertST"] = sourcerow["NFCertST"];
                        destRow["PlacedST"] = sourcerow["NFPlacedST"];



                        destRow["TrainOBC"] = sourcerow["NFTrainOBC"];
                        destRow["CertOBC"] = sourcerow["NFCertOBC"];
                        destRow["PlacedOBC"] = sourcerow["NFPlacedOBC"];

                        destRow["TrainPWD"] = sourcerow["NFTrainPWD"];
                        destRow["CertPWD"] = sourcerow["NFCertPWD"];
                        destRow["PlacedPWD"] = sourcerow["NFPlacedPWD"];

                        DTNFormal.Rows.Add(destRow);
                    }

                    foreach (DataRow sourcerow in dt1.Rows)
                    {
                        DataRow destRow = DTShortTerm.NewRow();
                        destRow["Center"] = "";
                        destRow["Courses"] = "Short Term Courses";

                        destRow["AnnualTarget"] = sourcerow["SAnnualTarget"];
                        destRow["CTrain"] = sourcerow["CShortTrain"];
                        destRow["CCert"] = sourcerow["CShortCert"];
                        destRow["CPlaced"] = sourcerow["CShortPlaced"];

                        destRow["TrainW"] = sourcerow["ShortTrainW"];
                        destRow["CertW"] = sourcerow["ShortCertW"];
                        destRow["PlacedW"] = sourcerow["ShortPlacedW"];

                        destRow["TrainSC"] = sourcerow["ShortTrainSC"];
                        destRow["CertSC"] = sourcerow["ShortCertSC"];
                        destRow["PlacedSC"] = sourcerow["ShortPlacedSC"];

                        destRow["TrainST"] = sourcerow["ShortTrainST"];
                        destRow["CertST"] = sourcerow["ShortCertST"];
                        destRow["PlacedST"] = sourcerow["ShortPlacedST"];

                        destRow["TrainOBC"] = sourcerow["ShortTrainOBC"];
                        destRow["CertOBC"] = sourcerow["ShortCertOBC"];
                        destRow["PlacedOBC"] = sourcerow["ShortPlacedOBC"];

                        destRow["TrainPWD"] = sourcerow["ShortTrainPWD"];
                        destRow["CertPWD"] = sourcerow["ShortCertPWD"];
                        destRow["PlacedPWD"] = sourcerow["ShortPlacedPWD"];

                        DTShortTerm.Rows.Add(destRow);

                        DTFormal.Merge(DTNFormal);
                        DTFormal.Merge(DTShortTerm);

                        DTNFormal.Rows.Clear();
                        DTShortTerm.Rows.Clear();



                    }
                    dt1.Rows.Clear();
                }
            }
            else
            {
                using (SqlCommand Cmm = new SqlCommand("ReportCenterwiseSummary", con))
                {
                    Cmm.CommandType = CommandType.StoredProcedure;

                    Cmm.CommandType = CommandType.StoredProcedure;
                    Cmm.Parameters.Add(new SqlParameter("@pCentreID", centerID));
                    Cmm.Parameters.Add(new SqlParameter("@pProjectID", projectID));
                    Cmm.Parameters.Add(new SqlParameter("@pDateFrom", pDateFrom));
                    Cmm.Parameters.Add(new SqlParameter("@pDateTo", pDateFromTo));

                    SqlDataAdapter Sda = new SqlDataAdapter(Cmm);

                    Sda.Fill(dt1);
                }


                foreach (DataRow sourcerow in dt1.Rows)
                {
                    DataRow destRow = DTFormal.NewRow();

                    destRow["Sno"] = "1";
                    destRow["Center"] = ddlCenter.SelectedItem.ToString();
                    destRow["Courses"] = "Formal Courses";

                    destRow["AnnualTarget"] = sourcerow["FAnnualTarget"];
                    destRow["CTrain"] = sourcerow["CFTrain"];
                    destRow["CCert"] = sourcerow["CFCert"];
                    destRow["CPlaced"] = sourcerow["CFPlaced"];


                    destRow["TrainW"] = sourcerow["FTrainW"];
                    destRow["CertW"] = sourcerow["FCertW"];
                    destRow["PlacedW"] = sourcerow["FPlacedW"];

                    destRow["TrainSC"] = sourcerow["FTrainSC"];
                    destRow["CertSC"] = sourcerow["FCertSC"];
                    destRow["PlacedSC"] = sourcerow["FPlacedSC"];

                    destRow["TrainST"] = sourcerow["FTrainST"];
                    destRow["CertST"] = sourcerow["FCertST"];
                    destRow["PlacedST"] = sourcerow["FPlacedST"];

                    destRow["TrainOBC"] = sourcerow["FTrainOBC"];
                    destRow["CertOBC"] = sourcerow["FCertOBC"];
                    destRow["PlacedOBC"] = sourcerow["FPlacedOBC"];

                    destRow["TrainPWD"] = sourcerow["FTrainPWD"];
                    destRow["CertPWD"] = sourcerow["FCertPWD"];
                    destRow["PlacedPWD"] = sourcerow["FPlacedPWD"];

                    DTFormal.Rows.Add(destRow);
                }

                foreach (DataRow sourcerow in dt1.Rows)
                {
                    DataRow destRow = DTNFormal.NewRow();
                    destRow["Center"] = "";
                    destRow["Courses"] = "Non-Formal Courses";

                    destRow["AnnualTarget"] = sourcerow["NFAnnualTarget"];
                    destRow["CTrain"] = sourcerow["CNFTrain"];
                    destRow["CCert"] = sourcerow["CNFCert"];
                    destRow["CPlaced"] = sourcerow["CNFPlaced"];

                    destRow["TrainW"] = sourcerow["NFTrainW"];
                    destRow["CertW"] = sourcerow["NFCertW"];
                    destRow["PlacedW"] = sourcerow["NFPlacedW"];

                    destRow["TrainSC"] = sourcerow["NFTrainSC"];
                    destRow["CertSC"] = sourcerow["NFCertSC"];
                    destRow["PlacedSC"] = sourcerow["NFPlacedSC"];

                    destRow["TrainST"] = sourcerow["NFTrainST"];
                    destRow["CertST"] = sourcerow["NFCertST"];
                    destRow["PlacedST"] = sourcerow["NFPlacedST"];

                    destRow["TrainOBC"] = sourcerow["NFTrainOBC"];
                    destRow["CertOBC"] = sourcerow["NFCertOBC"];
                    destRow["PlacedOBC"] = sourcerow["NFPlacedOBC"];

                    destRow["TrainPWD"] = sourcerow["NFTrainPWD"];
                    destRow["CertPWD"] = sourcerow["NFCertPWD"];
                    destRow["PlacedPWD"] = sourcerow["NFPlacedPWD"];

                    DTNFormal.Rows.Add(destRow);
                }

                foreach (DataRow sourcerow in dt1.Rows)
                {
                    DataRow destRow = DTShortTerm.NewRow();
                    destRow["Center"] = "";
                    destRow["Courses"] = "Short Term Courses";

                    destRow["AnnualTarget"] = sourcerow["SAnnualTarget"];
                    destRow["CTrain"] = sourcerow["CShortTrain"];
                    destRow["CCert"] = sourcerow["CShortCert"];
                    destRow["CPlaced"] = sourcerow["CShortPlaced"];

                    destRow["TrainW"] = sourcerow["ShortTrainW"];
                    destRow["CertW"] = sourcerow["ShortCertW"];
                    destRow["PlacedW"] = sourcerow["ShortPlacedW"];

                    destRow["TrainSC"] = sourcerow["ShortTrainSC"];
                    destRow["CertSC"] = sourcerow["ShortCertSC"];
                    destRow["PlacedSC"] = sourcerow["ShortPlacedSC"];

                    destRow["TrainST"] = sourcerow["ShortTrainST"];
                    destRow["CertST"] = sourcerow["ShortCertST"];
                    destRow["PlacedST"] = sourcerow["ShortPlacedST"];

                    destRow["TrainOBC"] = sourcerow["ShortTrainOBC"];
                    destRow["CertOBC"] = sourcerow["ShortCertOBC"];
                    destRow["PlacedOBC"] = sourcerow["ShortPlacedOBC"];

                    destRow["TrainPWD"] = sourcerow["ShortTrainPWD"];
                    destRow["CertPWD"] = sourcerow["ShortCertPWD"];
                    destRow["PlacedPWD"] = sourcerow["ShortPlacedPWD"];

                    DTShortTerm.Rows.Add(destRow);

                    DTFormal.Merge(DTNFormal);
                    DTFormal.Merge(DTShortTerm);


                }
            }

            if (DTFormal.Rows.Count > 0)
            {
                DataRow destRow1 = DTTotal.NewRow();
                //destRow1["Center"] = "";
                destRow1["Courses"] = "Total";

                destRow1["AnnualTarget"] = DTFormal.Compute("SUM(AnnualTarget)", string.Empty);
                destRow1["CTrain"] = DTFormal.Compute("SUM(CTrain)", string.Empty);
                destRow1["CCert"] = DTFormal.Compute("SUM(CCert)", string.Empty);
                destRow1["CPlaced"] = DTFormal.Compute("SUM(CPlaced)", string.Empty);

                destRow1["TrainW"] = DTFormal.Compute("SUM(TrainW)", string.Empty);
                destRow1["CertW"] = DTFormal.Compute("SUM(CertW)", string.Empty);
                destRow1["PlacedW"] = DTFormal.Compute("SUM(PlacedW)", string.Empty);

                destRow1["TrainSC"] = DTFormal.Compute("SUM(TrainSC)", string.Empty);
                destRow1["CertSC"] = DTFormal.Compute("SUM(CertSC)", string.Empty);
                destRow1["PlacedSC"] = DTFormal.Compute("SUM(PlacedSC)", string.Empty);

                destRow1["TrainST"] = DTFormal.Compute("SUM(TrainST)", string.Empty);
                destRow1["CertST"] = DTFormal.Compute("SUM(CertST)", string.Empty);
                destRow1["PlacedST"] = DTFormal.Compute("SUM(PlacedST)", string.Empty);

                destRow1["TrainOBC"] = DTFormal.Compute("SUM(TrainOBC)", string.Empty);
                destRow1["CertOBC"] = DTFormal.Compute("SUM(CertOBC)", string.Empty);
                destRow1["PlacedOBC"] = DTFormal.Compute("SUM(PlacedOBC)", string.Empty);

                destRow1["TrainPWD"] = DTFormal.Compute("SUM(TrainPWD)", string.Empty);
                destRow1["CertPWD"] = DTFormal.Compute("SUM(CertPWD)", string.Empty);
                destRow1["PlacedPWD"] = DTFormal.Compute("SUM(PlacedPWD)", string.Empty);

                DTTotal.Rows.Add(destRow1);

                DTFormal.Merge(DTTotal);
            }


            object sumObject = DTFormal.Compute("SUM(CTrain)", string.Empty);

            DateTime Efrm = Convert.ToDateTime(txtDateFrom.Text.ToString());
            DateTime ETo = Convert.ToDateTime(txtToDate.Text.ToString());
            if (true)
            {

                StrBunch = StrBunch + "<div style='page-break-after:always'>&nbsp;</div><h4 align='right' style='padding-right: 150px;'>Report Generated on: " + DateTime.Now.ToString("dd-MMM-yyyy");
                StrBunch = StrBunch + "<div style='page-break-after:always'>&nbsp;</div><br /><h4 align='center'> <b> Centre-wise Summary ";// +dt.Rows(0)("roll_no").ToString() + " </b> </h4>";
                //StrBunch = StrBunch + "<div style='page-break-after:always'>&nbsp;</div><h4 align='left'> ";// +dt.Rows(0)("registration_no").ToString() + " <br /> Candidate's Name: " + dt.Rows(0)("candidate_name").ToString();
                StrBunch = StrBunch + "<div style='page-break-after:always'>&nbsp;</div><h4 align='center'> <b> Reporting Period - " + Efrm.ToString("dd-MMM-yyyy") + " TO " + ETo.ToString("dd-MMM-yyyy");
                StrBunch = StrBunch + "<div style='page-break-after:always'>&nbsp;</div><br /><h4 align='center'> ";
                //StrBunch = StrBunch + ShowTableHeader();
                iTextSharp.text.Font font5 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 8);
                iTextSharp.text.Font font14 = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 14);

                PdfPTable tblFormal = new PdfPTable(DTFormal.Columns.Count);
                PdfPTable tblNF = new PdfPTable(DTFormal.Columns.Count);
                PdfPTable tblGrandTotals = new PdfPTable(DTFormal.Columns.Count);

                float[] widthss = { 0.19F, 0.33F, 0.58F, 0.29F ,
                                          
                                          0.33F, 0.20F, 0.19F, 0.30F,
                                          0.11F, 0.14F, 0.32F, 0.14F,
                                          0.13F, 0.30F, 0.12F, 0.14F,
                                          0.31F, 0.13F, 0.13F, 0.33F,
                                          0.13F, 0.10F};

                tblFormal.SetWidths(widthss);
                //.tblNF.SetWidths(widthss);
                foreach (DataRow r in DTFormal.Rows)
                {
                    if (DTFormal.Rows.Count > 0)
                    {
                        tblFormal.AddCell(new Phrase(r[0].ToString(), font5));
                        tblFormal.AddCell(new Phrase(r[1].ToString(), font5));
                        tblFormal.AddCell(new Phrase(r[2].ToString(), font5));
                        tblFormal.AddCell(new Phrase(r[3].ToString(), font5));

                        tblFormal.AddCell(new Phrase(r[4].ToString(), font5));
                        tblFormal.AddCell(new Phrase(r[5].ToString(), font5));
                        tblFormal.AddCell(new Phrase(r[6].ToString(), font5));
                        tblFormal.AddCell(new Phrase(r[7].ToString(), font5));

                        tblFormal.AddCell(new Phrase(r[8].ToString(), font5));
                        tblFormal.AddCell(new Phrase(r[9].ToString(), font5));
                        tblFormal.AddCell(new Phrase(r[10].ToString(), font5));
                        tblFormal.AddCell(new Phrase(r[11].ToString(), font5));

                        tblFormal.AddCell(new Phrase(r[12].ToString(), font5));
                        tblFormal.AddCell(new Phrase(r[13].ToString(), font5));
                        tblFormal.AddCell(new Phrase(r[14].ToString(), font5));
                        tblFormal.AddCell(new Phrase(r[15].ToString(), font5));

                        tblFormal.AddCell(new Phrase(r[16].ToString(), font5));
                        tblFormal.AddCell(new Phrase(r[17].ToString(), font5));

                        tblFormal.AddCell(new Phrase(r[18].ToString(), font5));
                        tblFormal.AddCell(new Phrase(r[19].ToString(), font5));
                        tblFormal.AddCell(new Phrase(r[20].ToString(), font5));
                        tblFormal.AddCell(new Phrase(r[21].ToString(), font5));

                    }
                }

                //Non-Affiliated 
                //if (ddlCenter.SelectedValue == "-1" && ddlProject.SelectedValue == "-1")
                //{
                dtGrandTotal = DTTotal.Copy();
                DTFormal.Rows.Clear();
                DTTotal.Rows.Clear();

                int j = 1;
                if (ddlCenter.SelectedValue == "-1")
                {
                    foreach (DataRow sourcerow in dtNF.Rows)
                    {
                        Int32 id = Convert.ToInt32(sourcerow["ID"]);
                        var Center = from p in context.NonAffInstitutes
                                     join c in context.NielitCentres on p.linkedToCentre equals c.ID
                                     where p.ID == id
                                     select new
                                     {
                                         ID = p.ID,
                                         Name = p.Name,
                                         lNKName = c.Name
                                     };

                        foreach (var CenterName in Center.Distinct())
                        {
                            string cName = CenterName.Name.ToString() + " (" + CenterName.lNKName.ToString() + ")";
                            DataRow destRow = DTFormal.NewRow();

                            destRow["Sno"] = j.ToString();
                            destRow["Center"] = cName;
                            destRow["Courses"] = "Formal Courses";

                            destRow["AnnualTarget"] = sourcerow["FAnnualTarget"];
                            destRow["CTrain"] = sourcerow["CFTrain"];
                            destRow["CCert"] = sourcerow["CFCert"];
                            destRow["CPlaced"] = sourcerow["CFPlaced"];


                            destRow["TrainW"] = sourcerow["FTrainW"];
                            destRow["CertW"] = sourcerow["FCertW"];
                            destRow["PlacedW"] = sourcerow["FPlacedW"];

                            destRow["TrainSC"] = sourcerow["FTrainSC"];
                            destRow["CertSC"] = sourcerow["FCertSC"];
                            destRow["PlacedSC"] = sourcerow["FPlacedSC"];

                            destRow["TrainST"] = sourcerow["FTrainST"];
                            destRow["CertST"] = sourcerow["FCertST"];
                            destRow["PlacedST"] = sourcerow["FPlacedST"];

                            destRow["TrainOBC"] = sourcerow["FTrainOBC"];
                            destRow["CertOBC"] = sourcerow["FCertOBC"];
                            destRow["PlacedOBC"] = sourcerow["FPlacedOBC"];

                            destRow["TrainPWD"] = sourcerow["FTrainPWD"];
                            destRow["CertPWD"] = sourcerow["FCertPWD"];
                            destRow["PlacedPWD"] = sourcerow["FPlacedPWD"];

                            DTFormal.Rows.Add(destRow);
                            j = j + 1;
                        }


                        //foreach (DataRow sourcerow in dtNF.Rows)
                        //{
                        DataRow destRowNF = DTNFormal.NewRow();
                        destRowNF["Center"] = "";
                        destRowNF["Courses"] = "Non-Formal Courses";

                        destRowNF["AnnualTarget"] = sourcerow["NFAnnualTarget"];
                        destRowNF["CTrain"] = sourcerow["CNFTrain"];
                        destRowNF["CCert"] = sourcerow["CNFCert"];
                        destRowNF["CPlaced"] = sourcerow["CNFPlaced"];

                        destRowNF["TrainW"] = sourcerow["NFTrainW"];
                        destRowNF["CertW"] = sourcerow["NFCertW"];
                        destRowNF["PlacedW"] = sourcerow["NFPlacedW"];

                        destRowNF["TrainSC"] = sourcerow["NFTrainSC"];
                        destRowNF["CertSC"] = sourcerow["NFCertSC"];
                        destRowNF["PlacedSC"] = sourcerow["NFPlacedSC"];

                        destRowNF["TrainST"] = sourcerow["NFTrainST"];
                        destRowNF["CertST"] = sourcerow["NFCertST"];
                        destRowNF["PlacedST"] = sourcerow["NFPlacedST"];

                        destRowNF["TrainOBC"] = sourcerow["NFTrainOBC"];
                        destRowNF["CertOBC"] = sourcerow["NFCertOBC"];
                        destRowNF["PlacedOBC"] = sourcerow["NFPlacedOBC"];

                        destRowNF["TrainPWD"] = sourcerow["NFTrainPWD"];
                        destRowNF["CertPWD"] = sourcerow["NFCertPWD"];
                        destRowNF["PlacedPWD"] = sourcerow["NFPlacedPWD"];

                        DTNFormal.Rows.Add(destRowNF);
                        //}

                        //foreach (DataRow sourcerow in dtNF.Rows)
                        //{
                        DataRow destRow2 = DTShortTerm.NewRow();
                        destRow2["Center"] = "";
                        destRow2["Courses"] = "Short Term Courses";

                        destRow2["AnnualTarget"] = sourcerow["SAnnualTarget"];
                        destRow2["CTrain"] = sourcerow["CShortTrain"];
                        destRow2["CCert"] = sourcerow["CShortCert"];
                        destRow2["CPlaced"] = sourcerow["CShortPlaced"];

                        destRow2["TrainW"] = sourcerow["ShortTrainW"];
                        destRow2["CertW"] = sourcerow["ShortCertW"];
                        destRow2["PlacedW"] = sourcerow["ShortPlacedW"];

                        destRow2["TrainSC"] = sourcerow["ShortTrainSC"];
                        destRow2["CertSC"] = sourcerow["ShortCertSC"];
                        destRow2["PlacedSC"] = sourcerow["ShortPlacedSC"];

                        destRow2["TrainST"] = sourcerow["ShortTrainST"];
                        destRow2["CertST"] = sourcerow["ShortCertST"];
                        destRow2["PlacedST"] = sourcerow["ShortPlacedST"];

                        destRow2["TrainOBC"] = sourcerow["ShortTrainOBC"];
                        destRow2["CertOBC"] = sourcerow["ShortCertOBC"];
                        destRow2["PlacedOBC"] = sourcerow["ShortPlacedOBC"];

                        destRow2["TrainPWD"] = sourcerow["ShortTrainPWD"];
                        destRow2["CertPWD"] = sourcerow["ShortCertPWD"];
                        destRow2["PlacedPWD"] = sourcerow["ShortPlacedPWD"];

                        DTShortTerm.Rows.Add(destRow2);

                        DTFormal.Merge(DTNFormal);
                        DTFormal.Merge(DTShortTerm);

                        DTNFormal.Rows.Clear();
                        DTShortTerm.Rows.Clear();

                        //}
                    }


                    DataRow destRowT = DTTotal.NewRow();
                    //destRow1["Center"] = "";
                    destRowT["Courses"] = "Total";

                    destRowT["AnnualTarget"] = DTFormal.Compute("SUM(AnnualTarget)", string.Empty);
                    destRowT["CTrain"] = DTFormal.Compute("SUM(CTrain)", string.Empty);
                    destRowT["CCert"] = DTFormal.Compute("SUM(CCert)", string.Empty);
                    destRowT["CPlaced"] = DTFormal.Compute("SUM(CPlaced)", string.Empty);

                    destRowT["TrainW"] = DTFormal.Compute("SUM(TrainW)", string.Empty);
                    destRowT["CertW"] = DTFormal.Compute("SUM(CertW)", string.Empty);
                    destRowT["PlacedW"] = DTFormal.Compute("SUM(PlacedW)", string.Empty);

                    destRowT["TrainSC"] = DTFormal.Compute("SUM(TrainSC)", string.Empty);
                    destRowT["CertSC"] = DTFormal.Compute("SUM(CertSC)", string.Empty);
                    destRowT["PlacedSC"] = DTFormal.Compute("SUM(PlacedSC)", string.Empty);

                    destRowT["TrainST"] = DTFormal.Compute("SUM(TrainST)", string.Empty);
                    destRowT["CertST"] = DTFormal.Compute("SUM(CertST)", string.Empty);
                    destRowT["PlacedST"] = DTFormal.Compute("SUM(PlacedST)", string.Empty);

                    destRowT["TrainOBC"] = DTFormal.Compute("SUM(TrainOBC)", string.Empty);
                    destRowT["CertOBC"] = DTFormal.Compute("SUM(CertOBC)", string.Empty);
                    destRowT["PlacedOBC"] = DTFormal.Compute("SUM(PlacedOBC)", string.Empty);

                    destRowT["TrainPWD"] = DTFormal.Compute("SUM(TrainPWD)", string.Empty);
                    destRowT["CertPWD"] = DTFormal.Compute("SUM(CertPWD)", string.Empty);
                    destRowT["PlacedPWD"] = DTFormal.Compute("SUM(PlacedPWD)", string.Empty);

                    DTTotal.Rows.Add(destRowT);

                    DTFormal.Merge(DTTotal);

                    dtGrandTotal.Merge(DTTotal);
                    DataTable dtGrandTotals = new DataTable();
                    dtGrandTotals = DTFormal.Clone();
                    DataRow destRow3 = dtGrandTotals.NewRow();

                    destRow3["Sno"] = " ";
                    destRow3["Center"] = " ";
                    destRow3["Courses"] = "Grand Total";

                    destRow3["AnnualTarget"] = dtGrandTotal.Compute("SUM(AnnualTarget)", string.Empty);
                    destRow3["CTrain"] = dtGrandTotal.Compute("SUM(CTrain)", string.Empty);
                    destRow3["CCert"] = dtGrandTotal.Compute("SUM(CCert)", string.Empty);
                    destRow3["CPlaced"] = dtGrandTotal.Compute("SUM(CPlaced)", string.Empty);

                    destRow3["TrainW"] = dtGrandTotal.Compute("SUM(TrainW)", string.Empty);
                    destRow3["CertW"] = dtGrandTotal.Compute("SUM(CertW)", string.Empty);
                    destRow3["PlacedW"] = dtGrandTotal.Compute("SUM(PlacedW)", string.Empty);

                    destRow3["TrainSC"] = dtGrandTotal.Compute("SUM(TrainSC)", string.Empty);
                    destRow3["CertSC"] = dtGrandTotal.Compute("SUM(CertSC)", string.Empty);
                    destRow3["PlacedSC"] = dtGrandTotal.Compute("SUM(PlacedSC)", string.Empty);

                    destRow3["TrainST"] = dtGrandTotal.Compute("SUM(TrainST)", string.Empty);
                    destRow3["CertST"] = dtGrandTotal.Compute("SUM(CertST)", string.Empty);
                    destRow3["PlacedST"] = dtGrandTotal.Compute("SUM(PlacedST)", string.Empty);

                    destRow3["TrainOBC"] = dtGrandTotal.Compute("SUM(TrainOBC)", string.Empty);
                    destRow3["CertOBC"] = dtGrandTotal.Compute("SUM(CertOBC)", string.Empty);
                    destRow3["PlacedOBC"] = dtGrandTotal.Compute("SUM(PlacedOBC)", string.Empty);

                    destRow3["TrainPWD"] = dtGrandTotal.Compute("SUM(TrainPWD)", string.Empty);
                    destRow3["CertPWD"] = dtGrandTotal.Compute("SUM(CertPWD)", string.Empty);
                    destRow3["PlacedPWD"] = dtGrandTotal.Compute("SUM(PlacedPWD)", string.Empty);

                    dtGrandTotals.Rows.Add(destRow3);
                    DTFormal.Merge(dtGrandTotals);

                    tblNF.SetWidths(widthss);
                    foreach (DataRow r in DTFormal.Rows)
                    {
                        if (DTFormal.Rows.Count > 0)
                        {
                            tblNF.AddCell(new Phrase(r[0].ToString(), font5));
                            tblNF.AddCell(new Phrase(r[1].ToString(), font5));
                            tblNF.AddCell(new Phrase(r[2].ToString(), font5));
                            tblNF.AddCell(new Phrase(r[3].ToString(), font5));

                            tblNF.AddCell(new Phrase(r[4].ToString(), font5));
                            tblNF.AddCell(new Phrase(r[5].ToString(), font5));
                            tblNF.AddCell(new Phrase(r[6].ToString(), font5));
                            tblNF.AddCell(new Phrase(r[7].ToString(), font5));

                            tblNF.AddCell(new Phrase(r[8].ToString(), font5));
                            tblNF.AddCell(new Phrase(r[9].ToString(), font5));
                            tblNF.AddCell(new Phrase(r[10].ToString(), font5));
                            tblNF.AddCell(new Phrase(r[11].ToString(), font5));

                            tblNF.AddCell(new Phrase(r[12].ToString(), font5));
                            tblNF.AddCell(new Phrase(r[13].ToString(), font5));
                            tblNF.AddCell(new Phrase(r[14].ToString(), font5));
                            tblNF.AddCell(new Phrase(r[15].ToString(), font5));

                            tblNF.AddCell(new Phrase(r[16].ToString(), font5));
                            tblNF.AddCell(new Phrase(r[17].ToString(), font5));

                            tblNF.AddCell(new Phrase(r[18].ToString(), font5));
                            tblNF.AddCell(new Phrase(r[19].ToString(), font5));
                            tblNF.AddCell(new Phrase(r[20].ToString(), font5));
                            tblNF.AddCell(new Phrase(r[21].ToString(), font5));

                        }
                    }



                    tblGrandTotals.SetWidths(widthss);
                    foreach (DataRow r in dtGrandTotals.Rows)
                    {
                        if (dtGrandTotals.Rows.Count > 0)
                        {
                            tblGrandTotals.AddCell(new Phrase(r[0].ToString(), font14));
                            tblGrandTotals.AddCell(new Phrase(r[1].ToString(), font14));
                            tblGrandTotals.AddCell(new Phrase(r[2].ToString(), font14));
                            tblGrandTotals.AddCell(new Phrase(r[3].ToString(), font14));

                            tblGrandTotals.AddCell(new Phrase(r[4].ToString(), font14));
                            tblGrandTotals.AddCell(new Phrase(r[5].ToString(), font14));
                            tblGrandTotals.AddCell(new Phrase(r[6].ToString(), font14));
                            tblGrandTotals.AddCell(new Phrase(r[7].ToString(), font14));

                            tblGrandTotals.AddCell(new Phrase(r[8].ToString(), font14));
                            tblGrandTotals.AddCell(new Phrase(r[9].ToString(), font14));
                            tblGrandTotals.AddCell(new Phrase(r[10].ToString(), font14));
                            tblGrandTotals.AddCell(new Phrase(r[11].ToString(), font14));

                            tblGrandTotals.AddCell(new Phrase(r[12].ToString(), font14));
                            tblGrandTotals.AddCell(new Phrase(r[13].ToString(), font14));
                            tblGrandTotals.AddCell(new Phrase(r[14].ToString(), font14));
                            tblGrandTotals.AddCell(new Phrase(r[15].ToString(), font14));

                            tblGrandTotals.AddCell(new Phrase(r[16].ToString(), font14));
                            tblGrandTotals.AddCell(new Phrase(r[17].ToString(), font14));

                            tblGrandTotals.AddCell(new Phrase(r[18].ToString(), font14));
                            tblGrandTotals.AddCell(new Phrase(r[19].ToString(), font14));
                            tblGrandTotals.AddCell(new Phrase(r[20].ToString(), font14));
                            tblGrandTotals.AddCell(new Phrase(r[21].ToString(), font14));

                        }
                    }
                }
                else
                {
                    DTNFormal.Rows.Clear();
                    DTShortTerm.Rows.Clear();
                    foreach (DataRow sourcerow in dtNF.Rows)
                    {
                        Int32 id = Convert.ToInt32(sourcerow["ID"]);
                        var Center = from p in context.NonAffInstitutes
                                     join c in context.NielitCentres on p.linkedToCentre equals c.ID
                                     where p.ID == id
                                     select new
                                     {
                                         ID = p.ID,
                                         Name = p.Name,
                                         lNKName = c.Name
                                     };

                        foreach (var CenterName in Center.Distinct())
                        {
                            string cName = CenterName.Name.ToString() + " (" + CenterName.lNKName.ToString() + ")";
                            DataRow destRow = DTFormal.NewRow();

                            destRow["Sno"] = j.ToString();
                            destRow["Center"] = cName;
                            destRow["Courses"] = "Formal Courses";

                            destRow["AnnualTarget"] = sourcerow["FAnnualTarget"];
                            destRow["CTrain"] = sourcerow["CFTrain"];
                            destRow["CCert"] = sourcerow["CFCert"];
                            destRow["CPlaced"] = sourcerow["CFPlaced"];


                            destRow["TrainW"] = sourcerow["FTrainW"];
                            destRow["CertW"] = sourcerow["FCertW"];
                            destRow["PlacedW"] = sourcerow["FPlacedW"];

                            destRow["TrainSC"] = sourcerow["FTrainSC"];
                            destRow["CertSC"] = sourcerow["FCertSC"];
                            destRow["PlacedSC"] = sourcerow["FPlacedSC"];

                            destRow["TrainST"] = sourcerow["FTrainST"];
                            destRow["CertST"] = sourcerow["FCertST"];
                            destRow["PlacedST"] = sourcerow["FPlacedST"];

                            destRow["TrainOBC"] = sourcerow["FTrainOBC"];
                            destRow["CertOBC"] = sourcerow["FCertOBC"];
                            destRow["PlacedOBC"] = sourcerow["FPlacedOBC"];

                            destRow["TrainPWD"] = sourcerow["FTrainPWD"];
                            destRow["CertPWD"] = sourcerow["FCertPWD"];
                            destRow["PlacedPWD"] = sourcerow["FPlacedPWD"];

                            DTFormal.Rows.Add(destRow);
                            j = j + 1;
                        }


                        //foreach (DataRow sourcerow in dtNF.Rows)
                        //{
                        DataRow destRowNF = DTNFormal.NewRow();
                        destRowNF["Center"] = "";
                        destRowNF["Courses"] = "Non-Formal Courses";

                        destRowNF["AnnualTarget"] = sourcerow["NFAnnualTarget"];
                        destRowNF["CTrain"] = sourcerow["CNFTrain"];
                        destRowNF["CCert"] = sourcerow["CNFCert"];
                        destRowNF["CPlaced"] = sourcerow["CNFPlaced"];

                        destRowNF["TrainW"] = sourcerow["NFTrainW"];
                        destRowNF["CertW"] = sourcerow["NFCertW"];
                        destRowNF["PlacedW"] = sourcerow["NFPlacedW"];

                        destRowNF["TrainSC"] = sourcerow["NFTrainSC"];
                        destRowNF["CertSC"] = sourcerow["NFCertSC"];
                        destRowNF["PlacedSC"] = sourcerow["NFPlacedSC"];

                        destRowNF["TrainST"] = sourcerow["NFTrainST"];
                        destRowNF["CertST"] = sourcerow["NFCertST"];
                        destRowNF["PlacedST"] = sourcerow["NFPlacedST"];

                        destRowNF["TrainOBC"] = sourcerow["NFTrainOBC"];
                        destRowNF["CertOBC"] = sourcerow["NFCertOBC"];
                        destRowNF["PlacedOBC"] = sourcerow["NFPlacedOBC"];

                        destRowNF["TrainPWD"] = sourcerow["NFTrainPWD"];
                        destRowNF["CertPWD"] = sourcerow["NFCertPWD"];
                        destRowNF["PlacedPWD"] = sourcerow["NFPlacedPWD"];

                        DTNFormal.Rows.Add(destRowNF);
                        //}

                        //foreach (DataRow sourcerow in dtNF.Rows)
                        //{
                        DataRow destRow2 = DTShortTerm.NewRow();
                        destRow2["Center"] = "";
                        destRow2["Courses"] = "Short Term Courses";

                        destRow2["AnnualTarget"] = sourcerow["SAnnualTarget"];
                        destRow2["CTrain"] = sourcerow["CShortTrain"];
                        destRow2["CCert"] = sourcerow["CShortCert"];
                        destRow2["CPlaced"] = sourcerow["CShortPlaced"];

                        destRow2["TrainW"] = sourcerow["ShortTrainW"];
                        destRow2["CertW"] = sourcerow["ShortCertW"];
                        destRow2["PlacedW"] = sourcerow["ShortPlacedW"];

                        destRow2["TrainSC"] = sourcerow["ShortTrainSC"];
                        destRow2["CertSC"] = sourcerow["ShortCertSC"];
                        destRow2["PlacedSC"] = sourcerow["ShortPlacedSC"];

                        destRow2["TrainST"] = sourcerow["ShortTrainST"];
                        destRow2["CertST"] = sourcerow["ShortCertST"];
                        destRow2["PlacedST"] = sourcerow["ShortPlacedST"];

                        destRow2["TrainOBC"] = sourcerow["ShortTrainOBC"];
                        destRow2["CertOBC"] = sourcerow["ShortCertOBC"];
                        destRow2["PlacedOBC"] = sourcerow["ShortPlacedOBC"];

                        destRow2["TrainPWD"] = sourcerow["ShortTrainPWD"];
                        destRow2["CertPWD"] = sourcerow["ShortCertPWD"];
                        destRow2["PlacedPWD"] = sourcerow["ShortPlacedPWD"];

                        DTShortTerm.Rows.Add(destRow2);

                        DTFormal.Merge(DTNFormal);
                        DTFormal.Merge(DTShortTerm);

                        DTNFormal.Rows.Clear();
                        DTShortTerm.Rows.Clear();

                        //}
                    }

                    if (DTFormal.Rows.Count > 0)
                    {
                        DataRow destRowT = DTTotal.NewRow();
                        //destRow1["Center"] = "";
                        destRowT["Courses"] = "Total";

                        destRowT["AnnualTarget"] = DTFormal.Compute("SUM(AnnualTarget)", string.Empty);
                        destRowT["CTrain"] = DTFormal.Compute("SUM(CTrain)", string.Empty);
                        destRowT["CCert"] = DTFormal.Compute("SUM(CCert)", string.Empty);
                        destRowT["CPlaced"] = DTFormal.Compute("SUM(CPlaced)", string.Empty);

                        destRowT["TrainW"] = DTFormal.Compute("SUM(TrainW)", string.Empty);
                        destRowT["CertW"] = DTFormal.Compute("SUM(CertW)", string.Empty);
                        destRowT["PlacedW"] = DTFormal.Compute("SUM(PlacedW)", string.Empty);

                        destRowT["TrainSC"] = DTFormal.Compute("SUM(TrainSC)", string.Empty);
                        destRowT["CertSC"] = DTFormal.Compute("SUM(CertSC)", string.Empty);
                        destRowT["PlacedSC"] = DTFormal.Compute("SUM(PlacedSC)", string.Empty);

                        destRowT["TrainST"] = DTFormal.Compute("SUM(TrainST)", string.Empty);
                        destRowT["CertST"] = DTFormal.Compute("SUM(CertST)", string.Empty);
                        destRowT["PlacedST"] = DTFormal.Compute("SUM(PlacedST)", string.Empty);

                        destRowT["TrainOBC"] = DTFormal.Compute("SUM(TrainOBC)", string.Empty);
                        destRowT["CertOBC"] = DTFormal.Compute("SUM(CertOBC)", string.Empty);
                        destRowT["PlacedOBC"] = DTFormal.Compute("SUM(PlacedOBC)", string.Empty);

                        destRowT["TrainPWD"] = DTFormal.Compute("SUM(TrainPWD)", string.Empty);
                        destRowT["CertPWD"] = DTFormal.Compute("SUM(CertPWD)", string.Empty);
                        destRowT["PlacedPWD"] = DTFormal.Compute("SUM(PlacedPWD)", string.Empty);

                        DTTotal.Rows.Add(destRowT);

                        DTFormal.Merge(DTTotal);

                        dtGrandTotal.Merge(DTTotal);
                        DataTable dtGrandTotals = new DataTable();
                        dtGrandTotals = DTFormal.Clone();
                        DataRow destRow3 = dtGrandTotals.NewRow();

                        destRow3["Sno"] = " ";
                        destRow3["Center"] = " ";
                        destRow3["Courses"] = "Grand Total";

                        destRow3["AnnualTarget"] = dtGrandTotal.Compute("SUM(AnnualTarget)", string.Empty);
                        destRow3["CTrain"] = dtGrandTotal.Compute("SUM(CTrain)", string.Empty);
                        destRow3["CCert"] = dtGrandTotal.Compute("SUM(CCert)", string.Empty);
                        destRow3["CPlaced"] = dtGrandTotal.Compute("SUM(CPlaced)", string.Empty);

                        destRow3["TrainW"] = dtGrandTotal.Compute("SUM(TrainW)", string.Empty);
                        destRow3["CertW"] = dtGrandTotal.Compute("SUM(CertW)", string.Empty);
                        destRow3["PlacedW"] = dtGrandTotal.Compute("SUM(PlacedW)", string.Empty);

                        destRow3["TrainSC"] = dtGrandTotal.Compute("SUM(TrainSC)", string.Empty);
                        destRow3["CertSC"] = dtGrandTotal.Compute("SUM(CertSC)", string.Empty);
                        destRow3["PlacedSC"] = dtGrandTotal.Compute("SUM(PlacedSC)", string.Empty);

                        destRow3["TrainST"] = dtGrandTotal.Compute("SUM(TrainST)", string.Empty);
                        destRow3["CertST"] = dtGrandTotal.Compute("SUM(CertST)", string.Empty);
                        destRow3["PlacedST"] = dtGrandTotal.Compute("SUM(PlacedST)", string.Empty);

                        destRow3["TrainOBC"] = dtGrandTotal.Compute("SUM(TrainOBC)", string.Empty);
                        destRow3["CertOBC"] = dtGrandTotal.Compute("SUM(CertOBC)", string.Empty);
                        destRow3["PlacedOBC"] = dtGrandTotal.Compute("SUM(PlacedOBC)", string.Empty);

                        destRow3["TrainPWD"] = dtGrandTotal.Compute("SUM(TrainPWD)", string.Empty);
                        destRow3["CertPWD"] = dtGrandTotal.Compute("SUM(CertPWD)", string.Empty);
                        destRow3["PlacedPWD"] = dtGrandTotal.Compute("SUM(PlacedPWD)", string.Empty);

                        dtGrandTotals.Rows.Add(destRow3);
                        DTFormal.Merge(dtGrandTotals);


                        tblNF.SetWidths(widthss);
                        foreach (DataRow r in DTFormal.Rows)
                        {
                            if (DTFormal.Rows.Count > 0)
                            {
                                tblNF.AddCell(new Phrase(r[0].ToString(), font5));
                                tblNF.AddCell(new Phrase(r[1].ToString(), font5));
                                tblNF.AddCell(new Phrase(r[2].ToString(), font5));
                                tblNF.AddCell(new Phrase(r[3].ToString(), font5));

                                tblNF.AddCell(new Phrase(r[4].ToString(), font5));
                                tblNF.AddCell(new Phrase(r[5].ToString(), font5));
                                tblNF.AddCell(new Phrase(r[6].ToString(), font5));
                                tblNF.AddCell(new Phrase(r[7].ToString(), font5));

                                tblNF.AddCell(new Phrase(r[8].ToString(), font5));
                                tblNF.AddCell(new Phrase(r[9].ToString(), font5));
                                tblNF.AddCell(new Phrase(r[10].ToString(), font5));
                                tblNF.AddCell(new Phrase(r[11].ToString(), font5));

                                tblNF.AddCell(new Phrase(r[12].ToString(), font5));
                                tblNF.AddCell(new Phrase(r[13].ToString(), font5));
                                tblNF.AddCell(new Phrase(r[14].ToString(), font5));
                                tblNF.AddCell(new Phrase(r[15].ToString(), font5));

                                tblNF.AddCell(new Phrase(r[16].ToString(), font5));
                                tblNF.AddCell(new Phrase(r[17].ToString(), font5));

                                tblNF.AddCell(new Phrase(r[18].ToString(), font5));
                                tblNF.AddCell(new Phrase(r[19].ToString(), font5));
                                tblNF.AddCell(new Phrase(r[20].ToString(), font5));
                                tblNF.AddCell(new Phrase(r[21].ToString(), font5));

                            }
                        }



                        tblGrandTotals.SetWidths(widthss);
                        foreach (DataRow r in dtGrandTotals.Rows)
                        {
                            if (dtGrandTotals.Rows.Count > 0)
                            {
                                tblGrandTotals.AddCell(new Phrase(r[0].ToString(), font14));
                                tblGrandTotals.AddCell(new Phrase(r[1].ToString(), font14));
                                tblGrandTotals.AddCell(new Phrase(r[2].ToString(), font14));
                                tblGrandTotals.AddCell(new Phrase(r[3].ToString(), font14));

                                tblGrandTotals.AddCell(new Phrase(r[4].ToString(), font14));
                                tblGrandTotals.AddCell(new Phrase(r[5].ToString(), font14));
                                tblGrandTotals.AddCell(new Phrase(r[6].ToString(), font14));
                                tblGrandTotals.AddCell(new Phrase(r[7].ToString(), font14));

                                tblGrandTotals.AddCell(new Phrase(r[8].ToString(), font14));
                                tblGrandTotals.AddCell(new Phrase(r[9].ToString(), font14));
                                tblGrandTotals.AddCell(new Phrase(r[10].ToString(), font14));
                                tblGrandTotals.AddCell(new Phrase(r[11].ToString(), font14));

                                tblGrandTotals.AddCell(new Phrase(r[12].ToString(), font14));
                                tblGrandTotals.AddCell(new Phrase(r[13].ToString(), font14));
                                tblGrandTotals.AddCell(new Phrase(r[14].ToString(), font14));
                                tblGrandTotals.AddCell(new Phrase(r[15].ToString(), font14));

                                tblGrandTotals.AddCell(new Phrase(r[16].ToString(), font14));
                                tblGrandTotals.AddCell(new Phrase(r[17].ToString(), font14));

                                tblGrandTotals.AddCell(new Phrase(r[18].ToString(), font14));
                                tblGrandTotals.AddCell(new Phrase(r[19].ToString(), font14));
                                tblGrandTotals.AddCell(new Phrase(r[20].ToString(), font14));
                                tblGrandTotals.AddCell(new Phrase(r[21].ToString(), font14));

                            }
                        }
                    }
                }

               

                Path = Server.MapPath(@"..\\download\\CentreWiseReport\\");

                Path = Path +  "MISReport.pdf";
                imgPath = Server.MapPath(@"..\\images\\");
                pathHead = imgPath + "ShowHead.png";
                //path1 = imgPath + "Nielit.png";
                path2 = imgPath + "NIELIT-Logo.png";
                //path3 = imgPath + "LegendFooter.png";
                MainReportDiv.InnerHtml = StrBunch;

                Response.ContentType = "application/pdf";

                tbl.BorderWidth = System.Web.UI.WebControls.Unit.Pixel(2);
                tbl.BorderColor = System.Drawing.Color.Green;
                tbl.CellPadding = 1;
                tbl.CellSpacing = 1;
                tbl.Width = System.Web.UI.WebControls.Unit.Percentage(100);
                tbl.HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                MainReportDiv.Controls.Add(tbl);

                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                MainReportDiv.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document();
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                string CreateDynamicFileName = Path;
                fsHead = new FileStream(pathHead, FileMode.Open);
                //fsGD = new FileStream(pathHead, FileMode.Open);
                //fs = new FileStream(path1, FileMode.Open);
                fsLogo = new FileStream(path2, FileMode.Open);
                //fsfooter = new FileStream(path3, FileMode.Open);

                iTextSharp.text.Image head = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fsHead), ImageFormat.Png);
                iTextSharp.text.Image headGD = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fsHead), ImageFormat.Png);
                //iTextSharp.text.Image png = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fs), ImageFormat.Png);
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fsLogo), ImageFormat.Png);
                //iTextSharp.text.Image footer = iTextSharp.text.Image.GetInstance(System.Drawing.Image.FromStream(fsfooter), ImageFormat.Png);

                //png.ScaleToFit(760.0F, 100.0F);
                logo.ScaleToFit(80.0F, 80.0F);
                head.ScaleToFit(757.0F, 100.0F);
                headGD.ScaleToFit(757.0F, 100.0F);

                logo.SetAbsolutePosition(30, 530);
                head.SetAbsolutePosition(42, 400);
                headGD.SetAbsolutePosition(42, 397);
                //footer.SetAbsolutePosition(30, 0);

                FontFactory.Register(@"C:\Windows\Fonts\ARIALUNI.TTF", "arial unicode ms");
                iTextSharp.text.html.simpleparser.StyleSheet ST = new iTextSharp.text.html.simpleparser.StyleSheet();
                ST.LoadTagStyle("body", "encoding", "Identity-H");
                ST.LoadTagStyle("body", "size", "2px");
                PdfWriter.GetInstance(pdfDoc, new FileStream(CreateDynamicFileName, FileMode.Create));
                PdfWriter.GetInstance(pdfDoc, Response.OutputStream);

                //For LANDSCAPE
                pdfDoc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());

                #region Common Part

                #endregion

                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    pdfDoc.Open();

                    //pdfDoc.Add(png);


                    //Add line Break
                    Paragraph p = new Paragraph(new Chunk("\n"));
                    Paragraph NF = new Paragraph(new Chunk("Non Accredited CourseWise Report"));
                    Paragraph GD = new Paragraph(new Chunk("Combined Grand Total"));
                    htmlparser.Parse(sr);
                    PdfContentByte content = writer.DirectContent;
                    Rectangle rectangle = new Rectangle(pdfDoc.PageSize);

                    tblFormal.WidthPercentage = 98;
                    tblNF.WidthPercentage = 98;


                    content.MoveTo(10, pdfDoc.PageSize.Height - 50);
                    content.LineTo(pdfDoc.PageSize.Width - 50, pdfDoc.PageSize.Height - 50);
                    if (DTFormal.Rows.Count > 0)
                    {
                        pdfDoc.Add(logo);
                        pdfDoc.Add(head);
                        pdfDoc.Add(p);
                        pdfDoc.Add(p);
                        pdfDoc.Add(p);
                        pdfDoc.Add(tblFormal);
                    }
                    else
                    {
                        strMessage = "No Records found to generate Report.";
                        Response.Redirect("MISDetailReport.aspx?msg=" + strMessage);
                    }
                    rectangle.Left += pdfDoc.LeftMargin;
                    rectangle.Right -= pdfDoc.RightMargin;
                    rectangle.Top -= pdfDoc.TopMargin;
                    rectangle.Bottom += pdfDoc.BottomMargin;
                    content.SetColorStroke(new iTextSharp.text.BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000")));
                    content.SetRGBColorFill(80, 66, 244);
                    content.Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, rectangle.Height);


                    if (dtNF.Rows.Count > 0)
                    {
                        pdfDoc.NewPage();
                        pdfDoc.Add(logo);
                        pdfDoc.Add(p);
                        pdfDoc.Add(p);
                        pdfDoc.Add(p);

                        pdfDoc.Add(NF);
                        pdfDoc.Add(p);
                        pdfDoc.Add(p);
                        pdfDoc.Add(headGD);
                        pdfDoc.Add(p);
                        pdfDoc.Add(p);
                        pdfDoc.Add(p);
                        pdfDoc.Add(tblNF);
                    }
                    content.Stroke();
                    pdfDoc.Close();
                }

                //Response.Write(MainReportDiv);

                pathHead = "";
                Path = "";
                imgPath = "";
                // path1 = "";
                path2 = "";
                //path3 = "";
                StrBunch = "";
                dt.Rows.Clear();

                dt2.Rows.Clear();

                fsHead.Dispose();
                //fs.Dispose();
                fsLogo.Dispose();
                //fsfooter.Dispose();

                //Response.Clear();
            }
            //}
            //}

            //string strSQL1 = "Select distinct b.registration_no from e_ResultSheet a, e_ResultSheet_history b where a.exam_month = b.exam_month and a.exam_year = b.exam_year and a.level_code = b.level_code and a.registration_no = b.registration_no";
            //SqlDataAdapter sda = new SqlDataAdapter(strSQL1, ConfigurationManager.ConnectionStrings("ExamConnectionString").ConnectionString);
            //DataSet dset = new DataSet();
            //string str = "";
            //sda.Fill(dset);
            //DataTable duplicaterecord = dset.Tables("Table");


            //if (duplicaterecord.Rows.Count > 0)
            //{
            //    str = str + duplicaterecord.Rows(0)("registration_no").ToString() + ",";
            //    rkMessageBox("Exam result genrated " + str);
            //    lblSuccess.Text = str;
            //    lblSuccess.Visible = true;
            //}

            //ZipFile zip = new ZipFile();
            // zip As ZipFile = New ZipFile()
            //string[] filenames = System.IO.Directory.GetFiles(Session("ZippedFile").ToString());
            //zip.AddFiles(filenames, "files");
            // zip.Save("Backup.zip")

            // Response.[End]()
            //Response.Charset = "";
            // ClientScript.RegisterStartupScript(Me.[GetType](), "myalert", "alert('Please Chk pdf ');", True)
            // Response.Redirect("menu-doeacc-exam.aspx")
            // '''''''''''Response.Redirect("~/exam_3/e_repExamResultSheet.aspx")
            // Response.Redirect("../exam_3/e_repExamResultSheet.aspx",True)

            // rkMessageBox("Exam result genrated ")
            // lblSuccess.Text = "Yor sklmdfsd "
            //HttpContext.Current.Response.Flush();
            //HttpContext.Current.Response.SuppressContent = true;
            //HttpContext.Current.ApplicationInstance.CompleteRequest();

            //Response.Clear();
            //// Response.OutputStream()
            //Response.Flush();
            //Response.End();
            //Response.Write("window.open('download/CentreWiseReport/MISReport.pdf');");
        }
        catch (Exception ex)
        {
            Path = "";
            imgPath = "";
            path1 = "";
            path2 = "";
            path3 = "";
            StrBunch = "";
            dt.Rows.Clear();

            dt2.Rows.Clear();

            //fsHead.Close();
            //fsLogo.Dispose();

            ShowAlert(ex.Message);
        }
        finally
        {
            con.Close();
        }
    }

    //protected void AddPageNumber()
    //{
    //    byte[] bytes = File.ReadAllBytes(Server.MapPath(@"..\\CentreWiseReport\\MISReport.pdf"));
    //    Font blackFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
    //    using (MemoryStream stream = new MemoryStream())
    //    {
    //        PdfReader reader = new PdfReader(bytes);
    //        PdfStamper stamper = new PdfStamper(reader, stream);
    //            int pages = reader.NumberOfPages;
                
    //            for (int i = 1; i <= pages; i++)
    //            {
    //                ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase(i.ToString(), blackFont), 568f, 15f, 0);
    //            }

    //        bytes = stream.ToArray();
    //    }
    //    File.WriteAllBytes(Server.MapPath(@"..\\CentreWiseReport\\Test1.pdf"), bytes);

    //}

}