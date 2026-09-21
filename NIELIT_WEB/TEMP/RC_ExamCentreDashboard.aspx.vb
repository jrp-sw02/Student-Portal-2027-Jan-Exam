Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Partial Class RC_ExamCentreDashboard
    Inherits System.Web.UI.Page

    Private connStr As String = ConfigurationManager.ConnectionStrings("ExamConnectionString").ConnectionString
    Private Const IAS_COURSE_IDS As String = "1,2,3,4"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            BindExamCycleDropdown()
            BindCityPreferenceGrid()
            phVenuePanel.Visible = False
            phPrevCentresPanel.Visible = False
        End If

    End Sub

    ' ---------------------------------------------------------------
    ' RC scoping — Session("rc_code") from RC_Login.aspx is still DUMMY
    ' data and does not match Regional_Center.Code. Until RC_Login is
    ' rewritten to authenticate against Regional_Center, there is no
    ' real Regional_Center.ID to key off. This reads Session("rc_id")
    ' (expected to hold Regional_Center.ID once real login is wired up)
    ' and fails safely (shows "not linked" instead of wrong/empty data)
    ' if it isn't present yet.
    ' ---------------------------------------------------------------
    Private ReadOnly Property CurrentRCId As Integer?
        Get
            Dim v = Session("rc_id")
            If v IsNot Nothing Then
                Dim id As Integer
                If Integer.TryParse(v.ToString(), id) Then Return id
            End If
            Return Nothing
        End Get
    End Property

    ' ---- Exam cycle dropdown (used only for tagging a venue's cycle) ----
    Private Sub BindExamCycleDropdown()
        Dim dt As New DataTable()
        Using conn As New SqlConnection(connStr)
            Dim sql As String =
                "SELECT DISTINCT Exam_Month, Exam_Year " &
                "FROM NIELIT.dbo.Exam " &
                "WHERE Course_ID IN (" & IAS_COURSE_IDS & ") " &
                "ORDER BY Exam_Year DESC, Exam_Month DESC"
            Using cmd As New SqlCommand(sql, conn)
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dt)
                End Using
            End Using
        End Using

        ddlExamCycle.Items.Clear()
        For Each row As DataRow In dt.Rows
            Dim mn As Integer = Convert.ToInt32(row("Exam_Month"))
            Dim yr As Integer = Convert.ToInt32(row("Exam_Year"))
            Dim cycleLabel As String = MonthCode(mn) & "-" & yr.ToString()
            ddlExamCycle.Items.Add(New ListItem(cycleLabel, cycleLabel))
        Next

        If ddlExamCycle.Items.Count > 0 Then
            ddlExamCycle.ClearSelection()
            ddlExamCycle.Items(0).Selected = True ' most recent cycle by default
        End If
    End Sub

    Private Function MonthCode(ByVal m As Integer) As String
        Return MonthName(m, True).ToUpper()
    End Function

    Private ReadOnly Property SelectedExamCycle As String
        Get
            If ddlExamCycle.SelectedValue IsNot Nothing AndAlso ddlExamCycle.SelectedValue <> "" Then
                Return ddlExamCycle.SelectedValue
            End If
            Return String.Empty
        End Get
    End Property

    ' ---- Resolves "current" cycle for the top grid (hardcoded to July 2026) ----
    Private Function GetCurrentExamCycle() As Tuple(Of Integer, Integer)
        Dim mn As Integer = 7
        Dim yr As Integer = 2026
        Return Tuple.Create(mn, yr)
    End Function

    ' ---- City preference grid — built from real production data ----
    ' ---- City preference grid — built from real production data (no RC scoping) ----
    Private Sub BindCityPreferenceGrid()

        Dim cycle = GetCurrentExamCycle()
        Dim mn As Integer = cycle.Item1
        Dim yr As Integer = cycle.Item2
        If mn = 0 OrElse yr = 0 Then
            gvCityPreference.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "No exam cycle found for this course group."
            Return
        End If

        ' 1. Real city list for the current cycle — ALL cities, no RC filter
        Dim dtCities As New DataTable()
        Using conn As New SqlConnection(connStr)
            Dim sql As String =
                "SELECT DISTINCT ec.ID AS pref_id, ec.Code AS city_code, ec.Name AS city_name " &
                "FROM NIELIT.dbo.Exam_Wise_Exam_Center wec " &
                "INNER JOIN NIELIT.dbo.Exam_Center ec ON ec.ID = wec.Exam_Center_ID " &
                "INNER JOIN NIELIT.dbo.Exam ex ON ex.id = wec.Exam_ID " &
                "WHERE ex.Exam_Month = @exam_month AND ex.Exam_Year = @exam_year " &
                "  AND ex.Course_ID IN (" & IAS_COURSE_IDS & ") " &
                "ORDER BY ec.Code"
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@exam_month", mn)
                cmd.Parameters.AddWithValue("@exam_year", yr)
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dtCities)
                End Using
            End Using
        End Using

        ' 2. Last-3-cycles peak capacity, per city
        Dim dtLast3 As New DataTable()
        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand("NIELIT.dbo.USP_City_MaxCapacity_LastN", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Course_IDs", IAS_COURSE_IDS)
                cmd.Parameters.AddWithValue("@NumCycles", 3)
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dtLast3)
                End Using
            End Using
        End Using

        ' 3. Current cycle's peak filled-so-far, per city
        Dim dtCurrent As New DataTable()
        Using conn As New SqlConnection(connStr)
            Using cmd As New SqlCommand("NIELIT.dbo.USP_City_CurrentCycle_Filled", conn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@Exam_Month", mn)
                cmd.Parameters.AddWithValue("@Exam_Year", yr)
                Using da As New SqlDataAdapter(cmd)
                    da.Fill(dtCurrent)
                End Using
            End Using
        End Using

        ' 4. Merge in memory — city list drives the rows, capacity is looked up per city_code
        Dim last3ByCity As New Dictionary(Of String, Integer)()
        For Each row As DataRow In dtLast3.Rows
            Dim code As String = row("city_code").ToString()
            Dim capVal As Integer = Convert.ToInt32(row("max_applied_capacity"))
            If Not last3ByCity.ContainsKey(code) OrElse capVal > last3ByCity(code) Then
                last3ByCity(code) = capVal
            End If
        Next

        Dim currentByCity As New Dictionary(Of String, Integer)()
        For Each row As DataRow In dtCurrent.Rows
            Dim code As String = row("city_code").ToString()
            currentByCity(code) = Convert.ToInt32(row("current_filled_per_session"))
        Next

        Dim result As New DataTable()
        result.Columns.Add("pref_id", GetType(Integer))
        result.Columns.Add("city_code", GetType(String))
        result.Columns.Add("city_name", GetType(String))
        result.Columns.Add("PerSessionMaxLast3", GetType(Integer))
        result.Columns.Add("PerSessionMaxCurrent", GetType(Integer))

        For Each row As DataRow In dtCities.Rows
            Dim code As String = row("city_code").ToString()
            Dim nr As DataRow = result.NewRow()
            nr("pref_id") = row("pref_id")
            nr("city_code") = code
            nr("city_name") = row("city_name")
            nr("PerSessionMaxLast3") = If(last3ByCity.ContainsKey(code), last3ByCity(code), 0)
            nr("PerSessionMaxCurrent") = If(currentByCity.ContainsKey(code), currentByCity(code), 0)
            result.Rows.Add(nr)
        Next

        If result.Rows.Count > 0 Then
            gvCityPreference.Visible = True
            lblNoData.Visible = False
            gvCityPreference.DataSource = result
            gvCityPreference.DataBind()
        Else
            gvCityPreference.Visible = False
            lblNoData.Visible = True
            lblNoData.Text = "No city preference data found."
        End If

    End Sub

    Protected Sub gvCityPreference_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim args As String() = e.CommandArgument.ToString().Split("|"c)
        Dim examCenterId As Integer = Convert.ToInt32(args(0))
        Dim cityCode As String = args(1)

        If e.CommandName = "ToggleVenues" Then
            hdnPrefId.Value = examCenterId.ToString()
            hdnVenueId.Value = "0"
            lblActiveCityCode.Text = cityCode
            litFormTitle.Text = "Add Venue"

            ClearVenueForm()
            BindExamCycleDropdown()
            BindVenues(examCenterId)

            Using conn As New SqlConnection(connStr)
                Dim sql As String =
                    "SELECT loc.Name AS district_name " &
                    "FROM NIELIT.dbo.Exam_Center ec " &
                    "INNER JOIN NIELIT.dbo.Location loc ON loc.ID = ec.District_ID " &
                    "WHERE ec.ID = @exam_center_id"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@exam_center_id", examCenterId)
                    conn.Open()
                    Dim districtResult = cmd.ExecuteScalar()
                    If districtResult IsNot Nothing AndAlso districtResult IsNot DBNull.Value Then
                        txtDistrict.Text = districtResult.ToString()
                    End If
                End Using
            End Using

            phVenuePanel.Visible = True
            phPrevCentresPanel.Visible = False

        ElseIf e.CommandName = "TogglePrevCentres" Then
            hdnPrefId.Value = examCenterId.ToString()
            lblPrevCityCode.Text = cityCode

            BindPreviousCentres(examCenterId)
            phPrevCentresPanel.Visible = True
            phVenuePanel.Visible = False
        End If
    End Sub

    Protected Sub ddlExamCycle_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim examCenterId As Integer
        If Integer.TryParse(hdnPrefId.Value, examCenterId) Then
            ClearVenueForm()
            BindVenues(examCenterId)
        End If
        phVenuePanel.Visible = True
    End Sub

    Protected Sub btnCancelVenue_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        phVenuePanel.Visible = False
    End Sub

    Private Sub BindVenues(ByVal examCenterId As Integer)
        Dim dt As New DataTable()
        Using conn As New SqlConnection(connStr)
            Dim sql As String =
                "SELECT venue_id, venue_code, es_name, es_phone, es_mail, centre_name, district_name " &
                "FROM tblVenue " &
                "WHERE pref_id = @pref_id AND is_active = 1 AND exam_cycle = @exam_cycle " &
                "ORDER BY venue_id"
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@pref_id", examCenterId)
                cmd.Parameters.AddWithValue("@exam_cycle", SelectedExamCycle)
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using

        If dt.Rows.Count > 0 Then
            gvVenues.Visible = True
            lblNoVenues.Visible = False
            gvVenues.DataSource = dt
            gvVenues.DataBind()
        Else
            gvVenues.Visible = False
            lblNoVenues.Visible = True
        End If
    End Sub

    Protected Sub gvVenues_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        Dim venueId As Integer = Convert.ToInt32(e.CommandArgument)
        Dim examCenterId As Integer
        Integer.TryParse(hdnPrefId.Value, examCenterId)

        If e.CommandName = "EditVenue" Then
            Using conn As New SqlConnection(connStr)
                Dim sql As String = "SELECT es_name, es_phone, es_mail, centre_name, district_name FROM tblVenue WHERE venue_id = @venue_id"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@venue_id", venueId)
                    conn.Open()
                    Using rdr As SqlDataReader = cmd.ExecuteReader()
                        If rdr.Read() Then
                            txtEsName.Text = rdr("es_name").ToString()
                            txtEsPhone.Text = rdr("es_phone").ToString()
                            txtEsMail.Text = rdr("es_mail").ToString()
                            txtCentreName.Text = rdr("centre_name").ToString()
                            txtDistrict.Text = rdr("district_name").ToString()
                            hdnVenueId.Value = venueId.ToString()
                            litFormTitle.Text = "Edit Venue"
                        End If
                    End Using
                End Using
            End Using
            phVenuePanel.Visible = True

        ElseIf e.CommandName = "DeleteVenue" Then
            Using conn As New SqlConnection(connStr)
                Dim sql As String = "UPDATE tblVenue SET is_active = 0 WHERE venue_id = @venue_id"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@venue_id", venueId)
                    conn.Open()
                    cmd.ExecuteNonQuery()
                End Using
            End Using
            BindVenues(examCenterId)
            phVenuePanel.Visible = True
        End If
    End Sub

    Private Sub BindPreviousCentres(ByVal examCenterId As Integer)
        Dim dt As New DataTable()
        Using conn As New SqlConnection(connStr)
            Dim sql As String =
                "SELECT venue_id, venue_code, es_name, es_phone, es_mail, centre_name, district_name, exam_cycle " &
                "FROM tblVenue " &
                "WHERE pref_id = @pref_id AND is_active = 1 AND (exam_cycle <> @exam_cycle OR exam_cycle IS NULL) " &
                "ORDER BY exam_cycle DESC, venue_id"
            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@pref_id", examCenterId)
                cmd.Parameters.AddWithValue("@exam_cycle", SelectedExamCycle)
                Dim da As New SqlDataAdapter(cmd)
                da.Fill(dt)
            End Using
        End Using

        If dt.Rows.Count > 0 Then
            gvPrevCentres.Visible = True
            lblNoPrevCentres.Visible = False
            gvPrevCentres.DataSource = dt
            gvPrevCentres.DataBind()
        Else
            gvPrevCentres.Visible = False
            lblNoPrevCentres.Visible = True
        End If
    End Sub

    Protected Sub btnClosePrevCentres_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        phPrevCentresPanel.Visible = False
    End Sub

    Protected Sub gvPrevCentres_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        If e.CommandName = "UsePrevCentre" Then
            Dim venueId As Integer = Convert.ToInt32(e.CommandArgument)
            Dim examCenterId As Integer
            Integer.TryParse(hdnPrefId.Value, examCenterId)

            Using conn As New SqlConnection(connStr)
                Dim sql As String =
                    "SELECT v.es_name, v.es_phone, v.es_mail, v.centre_name, v.district_name, ec.Code AS city_code " &
                    "FROM tblVenue v " &
                    "INNER JOIN NIELIT.dbo.Exam_Center ec ON ec.ID = v.pref_id " &
                    "WHERE v.venue_id = @venue_id"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@venue_id", venueId)
                    conn.Open()
                    Using rdr As SqlDataReader = cmd.ExecuteReader()
                        If rdr.Read() Then
                            txtEsName.Text = rdr("es_name").ToString()
                            txtEsPhone.Text = rdr("es_phone").ToString()
                            txtEsMail.Text = rdr("es_mail").ToString()
                            txtCentreName.Text = rdr("centre_name").ToString()
                            txtDistrict.Text = rdr("district_name").ToString()
                            lblActiveCityCode.Text = rdr("city_code").ToString()
                        End If
                    End Using
                End Using
            End Using

            hdnPrefId.Value = examCenterId.ToString()
            hdnVenueId.Value = "0"
            litFormTitle.Text = "Add Venue"
            BindExamCycleDropdown()
            BindVenues(examCenterId)
            phPrevCentresPanel.Visible = False
            phVenuePanel.Visible = True
        End If
    End Sub

    Private Function GenerateNextVenueCode(ByVal conn As SqlConnection, ByVal cityCode As String) As String
        Dim sql As String = "SELECT venue_code FROM tblVenue WHERE venue_code LIKE @prefix + '%'"
        Dim maxSeq As Integer = 0

        Using cmd As New SqlCommand(sql, conn)
            cmd.Parameters.AddWithValue("@prefix", cityCode)
            Using rdr As SqlDataReader = cmd.ExecuteReader()
                While rdr.Read()
                    Dim code As String = rdr("venue_code").ToString()
                    Dim suffix As String = code.Substring(cityCode.Length)
                    Dim seqNum As Integer
                    If Integer.TryParse(suffix, seqNum) Then
                        If seqNum > maxSeq Then maxSeq = seqNum
                    End If
                End While
            End Using
        End Using

        Dim nextSeq As Integer = maxSeq + 1
        Dim seqStr As String = nextSeq.ToString()
        If nextSeq < 10 Then
            seqStr = "0" & seqStr
        End If

        Return cityCode & seqStr
    End Function

    Protected Sub btnSaveVenue_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim examCenterId As Integer
        If Not Integer.TryParse(hdnPrefId.Value, examCenterId) Then Exit Sub

        Dim venueId As Integer
        Integer.TryParse(hdnVenueId.Value, venueId)

        If txtEsName.Text.Trim() = "" Then
            BindVenues(examCenterId)
            phVenuePanel.Visible = True
            Exit Sub
        End If

        Dim cityCode As String = lblActiveCityCode.Text.Trim()

        Using conn As New SqlConnection(connStr)
            conn.Open()
            Dim sql As String

            If venueId > 0 Then
                sql = "UPDATE tblVenue SET es_name=@es_name, es_phone=@es_phone, es_mail=@es_mail, centre_name=@centre_name, district_name=@district_name WHERE venue_id=@venue_id"
            Else
                sql = "INSERT INTO tblVenue (pref_id, venue_code, es_name, es_phone, es_mail, centre_name, district_name, created_by, created_on, exam_cycle, is_active) " &
                      "VALUES (@pref_id, @venue_code, @es_name, @es_phone, @es_mail, @centre_name, @district_name, @created_by, GETDATE(), @exam_cycle, 1)"
            End If

            Using cmd As New SqlCommand(sql, conn)
                cmd.Parameters.AddWithValue("@es_name", txtEsName.Text.Trim())

                If txtEsPhone.Text.Trim() = "" Then
                    cmd.Parameters.AddWithValue("@es_phone", DBNull.Value)
                Else
                    cmd.Parameters.AddWithValue("@es_phone", txtEsPhone.Text.Trim())
                End If

                If txtEsMail.Text.Trim() = "" Then
                    cmd.Parameters.AddWithValue("@es_mail", DBNull.Value)
                Else
                    cmd.Parameters.AddWithValue("@es_mail", txtEsMail.Text.Trim())
                End If

                If txtCentreName.Text.Trim() = "" Then
                    cmd.Parameters.AddWithValue("@centre_name", DBNull.Value)
                Else
                    cmd.Parameters.AddWithValue("@centre_name", txtCentreName.Text.Trim())
                End If

                If txtDistrict.Text.Trim() = "" Then
                    cmd.Parameters.AddWithValue("@district_name", DBNull.Value)
                Else
                    cmd.Parameters.AddWithValue("@district_name", txtDistrict.Text.Trim())
                End If

                If venueId > 0 Then
                    cmd.Parameters.AddWithValue("@venue_id", venueId)
                Else
                    Dim newVenueCode As String = GenerateNextVenueCode(conn, cityCode)
                    cmd.Parameters.AddWithValue("@venue_code", newVenueCode)
                    cmd.Parameters.AddWithValue("@pref_id", examCenterId)
                    cmd.Parameters.AddWithValue("@created_by", "ADMIN")
                    cmd.Parameters.AddWithValue("@exam_cycle", SelectedExamCycle)
                End If

                cmd.ExecuteNonQuery()
            End Using
        End Using

        Dim venueIdForLink As Integer = venueId
        If venueIdForLink = 0 Then
            Using conn As New SqlConnection(connStr)
                Dim sql As String = "SELECT MAX(venue_id) FROM tblVenue WHERE pref_id = @pref_id"
                Using cmd As New SqlCommand(sql, conn)
                    cmd.Parameters.AddWithValue("@pref_id", examCenterId)
                    conn.Open()
                    Dim maxResult = cmd.ExecuteScalar()
                    If maxResult IsNot Nothing AndAlso maxResult IsNot DBNull.Value Then
                        venueIdForLink = Convert.ToInt32(maxResult)
                    End If
                End Using
            End Using
        End If

        txtConsentLink.Text = ResolveUrl("~/ExamCentreConsent.aspx") & "?venue_id=" & venueIdForLink
        lblCopyStatus.Visible = False
        hdnShowConsentPopup.Value = "1"

        ClearVenueForm()
        BindVenues(examCenterId)
        BindCityPreferenceGrid()
        phVenuePanel.Visible = True
    End Sub

    Private Sub ClearVenueForm()
        txtEsName.Text = ""
        txtEsPhone.Text = ""
        txtEsMail.Text = ""
        txtCentreName.Text = ""
        txtDistrict.Text = ""
        hdnVenueId.Value = "0"
        litFormTitle.Text = "Add Venue"
    End Sub

End Class