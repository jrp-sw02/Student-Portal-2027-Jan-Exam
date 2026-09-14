using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using EConnect;
using EConnect.DAL;
using EConnect.NIELIT;
using EConnect.URM;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.OleDb;
using System.IO;
using EConnect.Utils.Data;
using System.Data.SqlClient;
using System.Configuration;

using System.Security.Cryptography;

public partial class MISProjectStudentsReport : BasePage
{
    string cs = ConfigurationManager.ConnectionStrings["NIELITMISContext"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                LoadProjects();
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    private void LoadProjects()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("nfillprojects", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                ddlProjects.DataSource = rdr;
                ddlProjects.DataTextField = "projectName";
                ddlProjects.DataValueField = "ID";
                ddlProjects.DataBind();
                con.Close();
            }

            ddlProjects.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Project--", "0"));
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void ddlProjects_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            chkCentres.Items.Clear();
            lblNoCentres.Visible = false;

            if (ddlProjects.SelectedValue != "0")
            {
                using (SqlConnection con = new SqlConnection(cs))
                using (SqlCommand cmd = new SqlCommand("nfillCentres", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@project_id", SqlDbType.Int).Value = Convert.ToInt32(ddlProjects.SelectedValue);

                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();
                    chkCentres.DataSource = rdr;
                    chkCentres.DataTextField = "Name";
                    chkCentres.DataValueField = "ID";
                    chkCentres.DataBind();
                    con.Close();
                }
                if (chkCentres.Items.Count == 0)
                {
                    lblNoCentres.Text = "No centres found for the selected project.";
                    lblNoCentres.Visible = true;
                }
            }
            btnToggleSelect.Text = "Select All";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }


    protected void btnToggleSelect_Click(object sender, EventArgs e)
    {
        try
        {
            bool selectAll = btnToggleSelect.Text == "Select All";

            foreach (System.Web.UI.WebControls.ListItem item in chkCentres.Items)
            {
                item.Selected = selectAll;
            }

            // Toggle button text
            btnToggleSelect.Text = selectAll ? "Deselect All" : "Select All";
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }


    protected void btnShowData_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlProjects.SelectedValue == "0")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select a project.');", true);
                return;
            }

            // Collect selected centres
            StringBuilder selectedCentres = new StringBuilder();
            foreach (System.Web.UI.WebControls.ListItem item in chkCentres.Items)
            {
                if (item.Selected)
                {
                    if (selectedCentres.Length > 0)
                        selectedCentres.Append(",");
                    selectedCentres.Append(item.Value);
                }
            }

            if (selectedCentres.Length == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select at least one centre.');", true);
                return;
            }

            using (SqlConnection con = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("getMIS_StudentdataforProjects", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@project_id", SqlDbType.Int).Value = Convert.ToInt32(ddlProjects.SelectedValue);

                DateTime start, end;


                if (!string.IsNullOrWhiteSpace(txtStartDate.Text))
                {
                    if (!DateTime.TryParse(txtStartDate.Text, out start))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter a valid Start Date.');", true);
                        return;
                    }
                    if (start > DateTime.Today)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Start Date cannot be greater than today.');", true);
                        return;
                    }
                    cmd.Parameters.Add("@batch_start", SqlDbType.Date).Value = start;
                }
                else
                {
                    start = DateTime.MinValue;
                    cmd.Parameters.Add("@batch_start", SqlDbType.Date).Value = DBNull.Value;
                }


                if (!string.IsNullOrWhiteSpace(txtEndDate.Text))
                {
                    if (!DateTime.TryParse(txtEndDate.Text, out end))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please enter a valid End Date.');", true);
                        return;
                    }


                    if (start != DateTime.MinValue && start > end)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Start Date cannot be greater than End Date.');", true);
                        return;
                    }

                    cmd.Parameters.Add("@batch_end", SqlDbType.Date).Value = end;
                }
                else
                {
                    cmd.Parameters.Add("@batch_end", SqlDbType.Date).Value = DBNull.Value;
                }

                cmd.Parameters.Add("@CentreIDs", SqlDbType.NVarChar).Value = selectedCentres.ToString();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvReport.DataSource = dt;
                gvReport.DataBind();
                gvReport.Style["display"] = "none";

                //btnExportExcel.Visible = dt.Rows.Count > 0;
                //btnExportPDF.Visible = dt.Rows.Count > 0;
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }



    //protected void btnConfirm_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (ddlProjects.SelectedValue == "0")
    //        {
    //            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select a project.');", true);
    //            return;
    //        }

    //        StringBuilder selectedCentres = new StringBuilder();
    //        foreach (System.Web.UI.WebControls.ListItem item in chkCentres.Items)
    //        {
    //            if (item.Selected)
    //            {
    //                if (selectedCentres.Length > 0)
    //                    selectedCentres.Append(",");
    //                selectedCentres.Append(item.Value);
    //            }
    //        }

    //        if (selectedCentres.Length == 0)
    //        {
    //            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select at least one centre.');", true);
    //            return;
    //        }

    //        //ShowAlert("CentreIDs param: " + selectedCentres.ToString());


    //        // Call existing data-binding logic
    //        btnShowData_Click(null, null);

    //        // Show export buttons only if data exists
    //        if (gvReport.Rows.Count > 0)
    //        {
    //            btnExportExcel.Visible = true;
    //            btnExportPDF.Visible = true;
    //        }
    //        else
    //        {
    //            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No data available for your selection.');", true);
    //            btnExportExcel.Visible = false;
    //            btnExportPDF.Visible = false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ShowAlert(ex.Message);
    //    }
    //}


    protected void btnExportExcel_Click(object sender, EventArgs e)
    {
        try
        {
            gvReport.AllowPaging = false;
            btnShowData_Click(null, null);

            if (gvReport.Rows.Count == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No data available to export.');", true);
                return;
            }


            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=MIS_ProjectStudentReport.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Charset = "";

            using (System.IO.StringWriter sw = new System.IO.StringWriter())
            {
                using (System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw))
                {
                    hw.Write("<h2 style='text-align:Left;'>MIS Project Student Report for Project "
    + ddlProjects.SelectedItem.Text
    + " and Batch Starting Between "
    + Convert.ToDateTime(txtStartDate.Text).ToString("dd-MM-yyyy")
    + " and "
    + Convert.ToDateTime(txtEndDate.Text).ToString("dd-MM-yyyy")
    + " (DD-MM-YYYY)"
    + "</h2>");

                    gvReport.RenderControl(hw);

                    Response.Write(sw.ToString());
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void btnExportPDF_Click(object sender, EventArgs e)
    {
        try
        {
            // Rebind data before exporting
            gvReport.AllowPaging = false;
            btnShowData_Click(null, null);

            if (gvReport.Rows.Count == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('No data available to export.');", true);
                return;
            }


            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=MIS_ProjectStudentReport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            // Use A3 landscape instead of A4 (wider page)
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            Font headerFont = FontFactory.GetFont("Arial", 12f, Font.BOLD, BaseColor.BLACK);
            Paragraph heading = new Paragraph(
                "MIS Project Student Report for Project "
                + ddlProjects.SelectedItem.Text
                + " and Batch Starting Between "
                + Convert.ToDateTime(txtStartDate.Text).ToString("dd-MM-yyyy")
                + " and "
                + Convert.ToDateTime(txtEndDate.Text).ToString("dd-MM-yyyy")
                + " (DD-MM-YYYY)",
                headerFont
            );
            heading.Alignment = Element.ALIGN_CENTER;
            heading.SpacingAfter = 10f;
            pdfDoc.Add(heading);

            // Create table with same number of columns as GridView
            PdfPTable table = new PdfPTable(gvReport.Columns.Count);
            table.WidthPercentage = 100;
            table.HorizontalAlignment = Element.ALIGN_LEFT;

            float[] widths = Enumerable.Repeat(1f, gvReport.Columns.Count).ToArray();
            table.SetWidths(widths);

            Font font = FontFactory.GetFont("Arial", 6f, Font.NORMAL, BaseColor.BLACK);

            foreach (System.Web.UI.WebControls.DataControlField col in gvReport.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(HttpUtility.HtmlDecode(col.HeaderText), font));
                cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(cell);
            }

            foreach (System.Web.UI.WebControls.GridViewRow row in gvReport.Rows)
            {
                foreach (System.Web.UI.WebControls.TableCell gridViewCell in row.Cells)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(HttpUtility.HtmlDecode(gridViewCell.Text), font));
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    table.AddCell(cell);
                }
            }

            pdfDoc.Add(table);
            pdfDoc.Close();
            Response.End();
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
    {
        try
        {
            // Required override
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        try
        {
            ddlProjects.SelectedIndex = 0;
            chkCentres.Items.Clear();
            txtStartDate.Text = string.Empty;
            txtEndDate.Text = string.Empty;
            gvReport.DataSource = null;

            gvReport.DataBind();
            gvReport.Visible = false;
        }
        catch (Exception ex)
        {
            ShowAlert(ex.Message);
        }
    }
}