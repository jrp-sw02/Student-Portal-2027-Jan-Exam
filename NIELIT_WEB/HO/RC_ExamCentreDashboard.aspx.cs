using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI.WebControls;

public partial class HO_RC_ExamCentreDashboard : System.Web.UI.Page
{
    private readonly string connStr = GetConnectionString();
    private const string IAS_COURSE_IDS = "1,2,3,4";

    private static string GetConnectionString()
    {
        ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings["ExamConnectionString"];
        if (connectionString == null || string.IsNullOrWhiteSpace(connectionString.ConnectionString))
        {
            connectionString = ConfigurationManager.ConnectionStrings["EConnectContext"];
        }
        if (connectionString == null || string.IsNullOrWhiteSpace(connectionString.ConnectionString))
        {
            throw new ConfigurationErrorsException("A valid database connection string for RC_ExamCentreDashboard was not found.");
        }
        return connectionString.ConnectionString;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindExamCycleDropdown();
            BindCityPreferenceGrid();
            phVenuePanel.Visible = false;
            phPrevCentresPanel.Visible = false;
        }
    }

    private int? CurrentRCId
    {
        get
        {
            object value = Session["rc_id"];
            if (value != null)
            {
                int id;
                if (int.TryParse(value.ToString(), out id))
                {
                    return id;
                }
            }
            return null;
        }
    }

    private void BindExamCycleDropdown()
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql =
                "SELECT DISTINCT Exam_Month, Exam_Year " +
                "FROM NIELIT.dbo.Exam " +
                "WHERE Course_ID IN (" + IAS_COURSE_IDS + ") " +
                "ORDER BY Exam_Year DESC, Exam_Month DESC";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
        }

        ddlExamCycle.Items.Clear();
        foreach (DataRow row in dt.Rows)
        {
            int month = Convert.ToInt32(row["Exam_Month"]);
            int year = Convert.ToInt32(row["Exam_Year"]);
            string cycleLabel = MonthCode(month) + "-" + year;
            ddlExamCycle.Items.Add(new ListItem(cycleLabel, cycleLabel));
        }

        if (ddlExamCycle.Items.Count > 0)
        {
            ddlExamCycle.ClearSelection();
            ddlExamCycle.Items[0].Selected = true;
        }
    }

    private string MonthCode(int month)
    {
        return new DateTime(2000, month, 1).ToString("MMM", CultureInfo.InvariantCulture).ToUpperInvariant();
    }

    private string SelectedExamCycle
    {
        get
        {
            if (!string.IsNullOrEmpty(ddlExamCycle.SelectedValue))
            {
                return ddlExamCycle.SelectedValue;
            }
            return string.Empty;
        }
    }

    /// <summary>
    /// Parses "JUL-2026" style cycle into month and year
    /// </summary>
    private bool TryParseExamCycle(string cycle, out int month, out int year)
    {
        month = 0;
        year = 0;

        if (string.IsNullOrWhiteSpace(cycle))
            return false;

        string[] parts = cycle.Split('-');
        if (parts.Length != 2)
            return false;

        DateTime dt;
        if (!DateTime.TryParseExact(parts[0].Trim(), "MMM", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            return false;

        month = dt.Month;
        return int.TryParse(parts[1].Trim(), out year);
    }

    private Tuple<int, int> GetCurrentExamCycle()
    {
        // Prefer the currently selected dropdown value
        int month, year;
        if (TryParseExamCycle(SelectedExamCycle, out month, out year))
        {
            return Tuple.Create(month, year);
        }

        // Fallback (should rarely be used)
        return Tuple.Create(7, 2026);
    }

    private void BindCityPreferenceGrid()
    {
        Tuple<int, int> cycle = GetCurrentExamCycle();
        int month = cycle.Item1;
        int year = cycle.Item2;

        if (month == 0 || year == 0)
        {
            gvCityPreference.Visible = false;
            lblNoData.Visible = true;
            lblNoData.Text = "No exam cycle found for this course group.";
            return;
        }

        DataTable dtCities = new DataTable();
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql =
                "SELECT DISTINCT ec.ID AS pref_id, ec.Code AS city_code, ec.Name AS city_name " +
                "FROM NIELIT.dbo.Exam_Wise_Exam_Center wec " +
                "INNER JOIN NIELIT.dbo.Exam_Center ec ON ec.ID = wec.Exam_Center_ID " +
                "INNER JOIN NIELIT.dbo.Exam ex ON ex.ID = wec.Exam_ID " +
                "WHERE ex.Exam_Month = @exam_month AND ex.Exam_Year = @exam_year " +
                "  AND ex.Course_ID IN (" + IAS_COURSE_IDS + ") " +
                "ORDER BY ec.Code";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@exam_month", month);
                cmd.Parameters.AddWithValue("@exam_year", year);
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dtCities);
                }
            }
        }

        // Capacity stored procedures – change to NIELIT.dbo if they exist in Project 1
        DataTable dtLast3 = new DataTable();
        using (SqlConnection conn = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand("NIELIT.dbo.USP_City_MaxCapacity_LastN", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Course_IDs", IAS_COURSE_IDS);
            cmd.Parameters.AddWithValue("@NumCycles", 3);
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dtLast3);
            }
        }

        DataTable dtCurrent = new DataTable();
        using (SqlConnection conn = new SqlConnection(connStr))
        using (SqlCommand cmd = new SqlCommand("NIELIT.dbo.USP_City_CurrentCycle_Filled", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Exam_Month", month);
            cmd.Parameters.AddWithValue("@Exam_Year", year);
            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
            {
                da.Fill(dtCurrent);
            }
        }

        Dictionary<string, int> last3ByCity = new Dictionary<string, int>();
        foreach (DataRow row in dtLast3.Rows)
        {
            string code = row["city_code"].ToString();
            int capVal = Convert.ToInt32(row["max_applied_capacity"]);
            if (!last3ByCity.ContainsKey(code) || capVal > last3ByCity[code])
            {
                last3ByCity[code] = capVal;
            }
        }

        Dictionary<string, int> currentByCity = new Dictionary<string, int>();
        foreach (DataRow row in dtCurrent.Rows)
        {
            string code = row["city_code"].ToString();
            currentByCity[code] = Convert.ToInt32(row["current_filled_per_session"]);
        }

        DataTable result = new DataTable();
        result.Columns.Add("pref_id", typeof(int));
        result.Columns.Add("city_code", typeof(string));
        result.Columns.Add("city_name", typeof(string));
        result.Columns.Add("PerSessionMaxLast3", typeof(int));
        result.Columns.Add("PerSessionMaxCurrent", typeof(int));

        foreach (DataRow row in dtCities.Rows)
        {
            string code = row["city_code"].ToString();
            DataRow newRow = result.NewRow();
            newRow["pref_id"] = row["pref_id"];
            newRow["city_code"] = code;
            newRow["city_name"] = row["city_name"];
            newRow["PerSessionMaxLast3"] = last3ByCity.ContainsKey(code) ? last3ByCity[code] : 0;
            newRow["PerSessionMaxCurrent"] = currentByCity.ContainsKey(code) ? currentByCity[code] : 0;
            result.Rows.Add(newRow);
        }

        if (result.Rows.Count > 0)
        {
            gvCityPreference.Visible = true;
            lblNoData.Visible = false;
            gvCityPreference.DataSource = result;
            gvCityPreference.DataBind();
        }
        else
        {
            gvCityPreference.Visible = false;
            lblNoData.Visible = true;
            lblNoData.Text = "No city preference data found.";
        }
    }

    protected void gvCityPreference_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string[] args = e.CommandArgument.ToString().Split('|');
        int examCenterId = Convert.ToInt32(args[0]);
        string cityCode = args[1];

        if (e.CommandName == "ToggleVenues")
        {
            hdnPrefId.Value = examCenterId.ToString();
            hdnVenueId.Value = "0";
            lblActiveCityCode.Text = cityCode;
            litFormTitle.Text = "Add Venue";
            ClearVenueForm();
            BindExamCycleDropdown();
            BindVenues(examCenterId);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql =
                    "SELECT loc.Name AS district_name " +
                    "FROM NIELIT.dbo.Exam_Center ec " +
                    "INNER JOIN NIELIT.dbo.Location loc ON loc.ID = ec.District_ID " +
                    "WHERE ec.ID = @exam_center_id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@exam_center_id", examCenterId);
                    conn.Open();
                    object districtResult = cmd.ExecuteScalar();
                    if (districtResult != null && districtResult != DBNull.Value)
                    {
                        txtDistrict.Text = districtResult.ToString();
                    }
                }
            }

            phVenuePanel.Visible = true;
            phPrevCentresPanel.Visible = false;
        }
        else if (e.CommandName == "TogglePrevCentres")
        {
            hdnPrefId.Value = examCenterId.ToString();
            lblPrevCityCode.Text = cityCode;
            BindPreviousCentres(examCenterId);
            phPrevCentresPanel.Visible = true;
            phVenuePanel.Visible = false;
        }
    }

    protected void ddlExamCycle_SelectedIndexChanged(object sender, EventArgs e)
    {
        int examCenterId;
        if (int.TryParse(hdnPrefId.Value, out examCenterId))
        {
            ClearVenueForm();
            BindVenues(examCenterId);
        }
        phVenuePanel.Visible = true;
    }

    protected void btnCancelVenue_Click(object sender, EventArgs e)
    {
        phVenuePanel.Visible = false;
    }

    private void BindVenues(int examCenterId)
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql =
                "SELECT ID AS venue_id, " +
                "       Venue_ID AS venue_code, " +
                "       ES_Name AS es_name, " +
                "       ES_Phone AS es_phone, " +
                "       ES_Mail AS es_mail, " +
                "       City_Name AS centre_name, " +
                "       District_Name AS district_name " +
                "FROM NIELIT.dbo.Exam_Venues " +
                "WHERE Pref_Id = @pref_id " +
                "  AND Is_Active = 1 " +
                "  AND Exam_Month = @exam_month " +
                "  AND Exam_Year = @exam_year " +
                "ORDER BY ID";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@pref_id", examCenterId);

                int month, year;
                if (TryParseExamCycle(SelectedExamCycle, out month, out year))
                {
                    cmd.Parameters.AddWithValue("@exam_month", month);
                    cmd.Parameters.AddWithValue("@exam_year", year);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@exam_month", 0);
                    cmd.Parameters.AddWithValue("@exam_year", 0);
                }

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
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
        int examCenterId;
        int.TryParse(hdnPrefId.Value, out examCenterId);

        if (e.CommandName == "EditVenue")
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql =
                    "SELECT ES_Name AS es_name, " +
                    "       ES_Phone AS es_phone, " +
                    "       ES_Mail AS es_mail, " +
                    "       City_Name AS centre_name, " +
                    "       District_Name AS district_name " +
                    "FROM NIELIT.dbo.Exam_Venues " +
                    "WHERE ID = @venue_id";

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
                string sql = "UPDATE NIELIT.dbo.Exam_Venues SET Is_Active = 0 WHERE ID = @venue_id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@venue_id", venueId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            BindVenues(examCenterId);
            phVenuePanel.Visible = true;
        }
    }

    private void BindPreviousCentres(int examCenterId)
    {
        DataTable dt = new DataTable();
        using (SqlConnection conn = new SqlConnection(connStr))
        {
            string sql =
                "SELECT ID AS venue_id, " +
                "       Venue_ID AS venue_code, " +
                "       ES_Name AS es_name, " +
                "       ES_Phone AS es_phone, " +
                "       ES_Mail AS es_mail, " +
                "       City_Name AS centre_name, " +
                "       District_Name AS district_name, " +
                "       CONCAT(LEFT(DATENAME(MONTH, DATEFROMPARTS(Exam_Year, Exam_Month, 1)), 3), '-', Exam_Year) AS exam_cycle " +
                "FROM NIELIT.dbo.Exam_Venues " +
                "WHERE Pref_Id = @pref_id " +
                "  AND Is_Active = 1 " +
                "  AND (Exam_Month <> @exam_month OR Exam_Year <> @exam_year) " +
                "ORDER BY Exam_Year DESC, Exam_Month DESC, ID";

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@pref_id", examCenterId);

                int month, year;
                if (TryParseExamCycle(SelectedExamCycle, out month, out year))
                {
                    cmd.Parameters.AddWithValue("@exam_month", month);
                    cmd.Parameters.AddWithValue("@exam_year", year);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@exam_month", 0);
                    cmd.Parameters.AddWithValue("@exam_year", 0);
                }

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
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
            int examCenterId;
            int.TryParse(hdnPrefId.Value, out examCenterId);

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql =
                    "SELECT v.ES_Name AS es_name, " +
                    "       v.ES_Phone AS es_phone, " +
                    "       v.ES_Mail AS es_mail, " +
                    "       v.City_Name AS centre_name, " +
                    "       v.District_Name AS district_name, " +
                    "       ec.Code AS city_code " +
                    "FROM NIELIT.dbo.Exam_Venues v " +
                    "INNER JOIN NIELIT.dbo.Exam_Center ec ON ec.ID = v.Pref_Id " +
                    "WHERE v.ID = @venue_id";

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

            hdnPrefId.Value = examCenterId.ToString();
            hdnVenueId.Value = "0";
            litFormTitle.Text = "Add Venue";
            BindExamCycleDropdown();
            BindVenues(examCenterId);
            phPrevCentresPanel.Visible = false;
            phVenuePanel.Visible = true;
        }
    }

    private string GenerateNextVenueCode(SqlConnection conn, string cityCode)
    {
        string sql = "SELECT Venue_ID AS venue_code FROM NIELIT.dbo.Exam_Venues WHERE Venue_ID LIKE @prefix + '%'";
        int maxSeq = 0;

        using (SqlCommand cmd = new SqlCommand(sql, conn))
        {
            cmd.Parameters.AddWithValue("@prefix", cityCode);
            using (SqlDataReader rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    string code = rdr["venue_code"].ToString();
                    if (code.Length <= cityCode.Length) continue;

                    string suffix = code.Substring(cityCode.Length);
                    int seqNum;
                    if (int.TryParse(suffix, out seqNum) && seqNum > maxSeq)
                    {
                        maxSeq = seqNum;
                    }
                }
            }
        }

        int nextSeq = maxSeq + 1;
        string seqStr = nextSeq < 10 ? "0" + nextSeq.ToString() : nextSeq.ToString();
        return cityCode + seqStr;
    }

    protected void btnSaveVenue_Click(object sender, EventArgs e)
    {
        int examCenterId;
        if (!int.TryParse(hdnPrefId.Value, out examCenterId))
        {
            return;
        }

        int venueId;
        int.TryParse(hdnVenueId.Value, out venueId);

        if (string.IsNullOrWhiteSpace(txtEsName.Text))
        {
            BindVenues(examCenterId);
            phVenuePanel.Visible = true;
            return;
        }

        string cityCode = lblActiveCityCode.Text.Trim();

        using (SqlConnection conn = new SqlConnection(connStr))
        {
            conn.Open();
            string sql;

            if (venueId > 0)
            {
                // UPDATE
                sql = "UPDATE NIELIT.dbo.Exam_Venues " +
                      "SET ES_Name = @es_name, " +
                      "    ES_Phone = @es_phone, " +
                      "    ES_Mail = @es_mail, " +
                      "    City_Name = @centre_name, " +
                      "    District_Name = @district_name " +
                      "WHERE ID = @venue_id";
            }
            else
            {
                // INSERT
                sql = "INSERT INTO NIELIT.dbo.Exam_Venues " +
                      "(Pref_Id, Venue_ID, ES_Name, ES_Phone, ES_Mail, City_Name, District_Name, " +
                      " Created_By, Created_On, Exam_Month, Exam_Year, Is_Active) " +
                      "VALUES " +
                      "(@pref_id, @venue_code, @es_name, @es_phone, @es_mail, @centre_name, @district_name, " +
                      " @created_by, GETDATE(), @exam_month, @exam_year, 1)";
            }

            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@es_name", txtEsName.Text.Trim());
                cmd.Parameters.AddWithValue("@es_phone", string.IsNullOrWhiteSpace(txtEsPhone.Text) ? (object)DBNull.Value : txtEsPhone.Text.Trim());
                cmd.Parameters.AddWithValue("@es_mail", string.IsNullOrWhiteSpace(txtEsMail.Text) ? (object)DBNull.Value : txtEsMail.Text.Trim());
                cmd.Parameters.AddWithValue("@centre_name", string.IsNullOrWhiteSpace(txtCentreName.Text) ? (object)DBNull.Value : txtCentreName.Text.Trim());
                cmd.Parameters.AddWithValue("@district_name", string.IsNullOrWhiteSpace(txtDistrict.Text) ? (object)DBNull.Value : txtDistrict.Text.Trim());

                if (venueId > 0)
                {
                    cmd.Parameters.AddWithValue("@venue_id", venueId);
                }
                else
                {
                    string newVenueCode = GenerateNextVenueCode(conn, cityCode);
                    cmd.Parameters.AddWithValue("@venue_code", newVenueCode);
                    cmd.Parameters.AddWithValue("@pref_id", examCenterId);
                    cmd.Parameters.AddWithValue("@created_by", "ADMIN");

                    int month, year;
                    if (TryParseExamCycle(SelectedExamCycle, out month, out year))
                    {
                        cmd.Parameters.AddWithValue("@exam_month", month);
                        cmd.Parameters.AddWithValue("@exam_year", year);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@exam_month", 0);
                        cmd.Parameters.AddWithValue("@exam_year", 0);
                    }
                }

                cmd.ExecuteNonQuery();
            }
        }

        // Get the newly inserted ID for consent link
        int venueIdForLink = venueId;
        if (venueIdForLink == 0)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string sql = "SELECT MAX(ID) FROM NIELIT.dbo.Exam_Venues WHERE Pref_Id = @pref_id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@pref_id", examCenterId);
                    conn.Open();
                    object maxResult = cmd.ExecuteScalar();
                    if (maxResult != null && maxResult != DBNull.Value)
                    {
                        venueIdForLink = Convert.ToInt32(maxResult);
                    }
                }
            }
        }

        txtConsentLink.Text = ResolveUrl("~/ExamCentreConsent.aspx") + "?venue_id=" + venueIdForLink;
        lblCopyStatus.Visible = false;
        hdnShowConsentPopup.Value = "1";

        ClearVenueForm();
        BindVenues(examCenterId);
        BindCityPreferenceGrid();
        phVenuePanel.Visible = true;
    }

    private void ClearVenueForm()
    {
        txtEsName.Text = string.Empty;
        txtEsPhone.Text = string.Empty;
        txtEsMail.Text = string.Empty;
        txtCentreName.Text = string.Empty;
        txtDistrict.Text = string.Empty;
        hdnVenueId.Value = "0";
        litFormTitle.Text = "Add Venue";
    }
}