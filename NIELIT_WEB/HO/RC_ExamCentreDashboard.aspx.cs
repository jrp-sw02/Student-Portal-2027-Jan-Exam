using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class RC_ExamCentreDashboard : System.Web.UI.Page
{
    private string connStr = ConfigurationManager.ConnectionStrings["EConnectContext"].ConnectionString;
    private const string CURRENT_EXAM_CYCLE = "JUL-2026";

    protected void Page_Load(object sender, EventArgs e)
    {
        Session["rc_code"] = "GO";
        Session["rc_name"] = "RC Gorakhpur";

        if (!IsPostBack)
        {
            BindCityPreferenceGrid();
            phVenuePanel.Visible = false;
            phPrevCentresPanel.Visible = false;
        }
    }

    private void BindCityPreferenceGrid()
    {
        string rcCode = Session["rc_code"].ToString();
        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql =
                "SELECT cp.pref_id, cp.city_code, cp.city_name, " +
                "       es.last3_avg_per_session AS PerSessionMaxLast3, " +
                "       es.current_filled_per_session AS PerSessionMaxCurrent " +
                "FROM tblCityPreference cp " +
                "INNER JOIN tblExamSession es ON es.pref_id = cp.pref_id " +
                "INNER JOIN tblRCMaster rc ON rc.rc_id = cp.rc_id " +
                "WHERE rc.rc_code = @rc_code AND cp.is_active = 1 " +
                "ORDER BY cp.city_code";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@rc_code", rcCode);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
        }

        if (dt.Rows.Count > 0)
        {
            divGrid.Visible = true;
            lblNoData.Visible = false;
            gvCityPreference.DataSource = dt;
            gvCityPreference.DataBind();
        }
        else
        {
            divGrid.Visible = false;
            lblNoData.Visible = true;
        }
    }

    protected void gvCityPreference_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string[] args = e.CommandArgument.ToString().Split('|');
        int prefId = Convert.ToInt32(args[0]);
        string cityCode = args[1];

        if (e.CommandName == "ToggleVenues")
        {
            hdnPrefId.Value = prefId.ToString();
            hdnVenueId.Value = "0";
            lblActiveCityCode.Text = cityCode;
            litFormTitle.Text = "Add Venue";

            ClearVenueForm();
            BindVenues(prefId);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT district_name FROM tblCityPreference WHERE pref_id = @pref_id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@pref_id", prefId);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        txtDistrict.Text = result.ToString();
                }
            }

            phVenuePanel.Visible = true;
            phPrevCentresPanel.Visible = false;
        }
        else if (e.CommandName == "TogglePrevCentres")
        {
            hdnPrefId.Value = prefId.ToString();
            lblPrevCityCode.Text = cityCode;
            BindPreviousCentres(prefId);
            phPrevCentresPanel.Visible = true;
            phVenuePanel.Visible = false;
        }
    }

    protected void btnCancelVenue_Click(object sender, EventArgs e)
    {
        phVenuePanel.Visible = false;
    }

    private void BindVenues(int prefId)
    {
        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql =
                "SELECT venue_id, es_name, es_phone, es_mail, centre_name, district_name " +
                "FROM tblVenue " +
                "WHERE pref_id = @pref_id AND is_active = 1 AND exam_cycle = @exam_cycle " +
                "ORDER BY venue_id";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@pref_id", prefId);
                cmd.Parameters.AddWithValue("@exam_cycle", CURRENT_EXAM_CYCLE);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
        }

        if (dt.Rows.Count > 0)
        {
            gvVenues.Visible = true;
            lblNoVenues.Visible = false;
            gvVenues.DataSource = dt;
            gvVenues.DataBind();
        }
        else
        {
            gvVenues.Visible = false;
            lblNoVenues.Visible = true;
        }
    }

    protected void gvVenues_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int venueId = Convert.ToInt32(e.CommandArgument);
        int prefId;
        int.TryParse(hdnPrefId.Value, out prefId);

        if (e.CommandName == "EditVenue")
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT es_name, es_phone, es_mail, centre_name, district_name FROM tblVenue WHERE venue_id = @venue_id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@venue_id", venueId);
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            txtEsName.Text = rdr["es_name"].ToString();
                            txtEsPhone.Text = rdr["es_phone"].ToString();
                            txtEsMail.Text = rdr["es_mail"].ToString();
                            txtCentreName.Text = rdr["centre_name"].ToString();
                            txtDistrict.Text = rdr["district_name"].ToString();
                            hdnVenueId.Value = venueId.ToString();
                            litFormTitle.Text = "Edit Venue";
                        }
                    }
                }
            }
            phVenuePanel.Visible = true;
        }
        else if (e.CommandName == "DeleteVenue")
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "UPDATE tblVenue SET is_active = 0 WHERE venue_id = @venue_id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@venue_id", venueId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            BindVenues(prefId);
            phVenuePanel.Visible = true;
        }
    }

    private void BindPreviousCentres(int prefId)
    {
        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql =
                "SELECT venue_id, es_name, es_phone, es_mail, centre_name, district_name, exam_cycle " +
                "FROM tblVenue " +
                "WHERE pref_id = @pref_id AND is_active = 1 AND (exam_cycle <> @exam_cycle OR exam_cycle IS NULL) " +
                "ORDER BY exam_cycle DESC, venue_id";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@pref_id", prefId);
                cmd.Parameters.AddWithValue("@exam_cycle", CURRENT_EXAM_CYCLE);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
        }

        if (dt.Rows.Count > 0)
        {
            gvPrevCentres.Visible = true;
            lblNoPrevCentres.Visible = false;
            gvPrevCentres.DataSource = dt;
            gvPrevCentres.DataBind();
        }
        else
        {
            gvPrevCentres.Visible = false;
            lblNoPrevCentres.Visible = true;
        }
    }

    protected void btnClosePrevCentres_Click(object sender, EventArgs e)
    {
        phPrevCentresPanel.Visible = false;
    }

    protected void gvPrevCentres_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "UsePrevCentre")
        {
            int venueId = Convert.ToInt32(e.CommandArgument);
            int prefId;
            int.TryParse(hdnPrefId.Value, out prefId);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT es_name, es_phone, es_mail, centre_name, district_name, city_code_lookup.city_code " +
                             "FROM tblVenue v " +
                             "CROSS APPLY (SELECT cp.city_code FROM tblCityPreference cp WHERE cp.pref_id = v.pref_id) AS city_code_lookup " +
                             "WHERE v.venue_id = @venue_id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@venue_id", venueId);
                    conn.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            txtEsName.Text = rdr["es_name"].ToString();
                            txtEsPhone.Text = rdr["es_phone"].ToString();
                            txtEsMail.Text = rdr["es_mail"].ToString();
                            txtCentreName.Text = rdr["centre_name"].ToString();
                            txtDistrict.Text = rdr["district_name"].ToString();
                            lblActiveCityCode.Text = rdr["city_code"].ToString();
                        }
                    }
                }
            }

            hdnPrefId.Value = prefId.ToString();
            hdnVenueId.Value = "0";
            litFormTitle.Text = "Add Venue";

            BindVenues(prefId);

            phPrevCentresPanel.Visible = false;
            phVenuePanel.Visible = true;
        }
    }

    protected void btnSaveVenue_Click(object sender, EventArgs e)
    {
        int prefId;
        if (!int.TryParse(hdnPrefId.Value, out prefId)) return;

        int venueId;
        int.TryParse(hdnVenueId.Value, out venueId);

        if (txtEsName.Text.Trim() == "")
        {
            BindVenues(prefId);
            phVenuePanel.Visible = true;
            return;
        }

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql;

            if (venueId > 0)
                sql = "UPDATE tblVenue SET es_name=@es_name, es_phone=@es_phone, es_mail=@es_mail, centre_name=@centre_name, district_name=@district_name WHERE venue_id=@venue_id";
            else
                sql = " INSERT INTO tblVenue (pref_id, es_name, es_phone, es_mail, centre_name, district_name, created_by, created_on, exam_cycle, is_active) " +
                      "VALUES (@pref_id, @es_name, @es_phone, @es_mail, @centre_name, @district_name, @created_by, GETDATE(), @exam_cycle, 1)";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@es_name", txtEsName.Text.Trim());
                cmd.Parameters.AddWithValue("@es_phone", txtEsPhone.Text.Trim() == "" ? (object)DBNull.Value : txtEsPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@es_mail", txtEsMail.Text.Trim() == "" ? (object)DBNull.Value : txtEsMail.Text.Trim());
                cmd.Parameters.AddWithValue("@centre_name", txtCentreName.Text.Trim() == "" ? (object)DBNull.Value : txtCentreName.Text.Trim());
                cmd.Parameters.AddWithValue("@district_name", txtDistrict.Text.Trim() == "" ? (object)DBNull.Value : txtDistrict.Text.Trim());

                if (venueId > 0)
                {
                    cmd.Parameters.AddWithValue("@venue_id", venueId);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@pref_id", prefId);
                    cmd.Parameters.AddWithValue("@created_by", "ADMIN");
                    cmd.Parameters.AddWithValue("@exam_cycle", CURRENT_EXAM_CYCLE);
                }

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        int venueIdForLink = venueId;
        if (venueIdForLink == 0)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT MAX(venue_id) FROM tblVenue WHERE pref_id = @pref_id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@pref_id", prefId);
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                        venueIdForLink = Convert.ToInt32(result);
                }
            }
        }

        txtConsentLink.Text = ResolveUrl("~/HO/ExamCentreConsent_v2.aspx") + "?venue_id=" + venueIdForLink;
        lblCopyStatus.Visible = false;
        hdnShowConsentPopup.Value = "1";

        ClearVenueForm();
        BindVenues(prefId);
        BindCityPreferenceGrid();

        phVenuePanel.Visible = true;
    }

    private void ClearVenueForm()
    {
        txtEsName.Text = "";
        txtEsPhone.Text = "";
        txtEsMail.Text = "";
        txtCentreName.Text = "";
        txtDistrict.Text = "";
        hdnVenueId.Value = "0";
        litFormTitle.Text = "Add Venue";
    }
}