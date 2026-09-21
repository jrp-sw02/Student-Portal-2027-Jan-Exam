<%@ Page Title="RC Dashboard" Language="C#" MasterPageFile="~/MasterPages/DashBoard.master" AutoEventWireup="false" CodeFile="RC_ExamCentreDashboard.aspx.cs" Inherits="HO_RC_ExamCentreDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/tabler-icons/2.47.0/iconfont/tabler-icons.min.css" />

    <style>
        .rc-wrap { max-width: 1000px; margin: 20px auto; font-family: "Segoe UI", Arial, sans-serif; }
        .rc-panel { border: 1px solid #b0b8c1; background: #fff; }
        .rc-panel-header { background: #003366; color: #fff; padding: 10px 16px; font-size: 15px; font-weight: 600; overflow: hidden; }
        .rc-toolbar { padding: 12px 16px; border-bottom: 1px solid #d7dce2; background: #f4f6f8; display: flex; align-items: center; gap: 10px; }
        .rc-toolbar label { font-size: 13px; color: #333; font-weight: 600; }
        .rc-toolbar select { padding: 5px 8px; border: 1px solid #9aa5b1; }

        .rc-table { width: 100%; border-collapse: collapse; font-size: 13px; }
        .rc-table th { text-align: left; padding: 10px 12px; background: #e8edf2; border-bottom: 2px solid #b0b8c1; border-right: 1px solid #d7dce2; font-weight: 600; color: #1a2530; }
        .rc-table th:last-child, .rc-table td:last-child { border-right: none; }
        .rc-table td { padding: 10px 12px; border-bottom: 1px solid #e2e6ea; border-right: 1px solid #eceff2; vertical-align: top; }
        .rc-table tbody tr:hover { background: #f5f8fb; }
        .rc-table .sub { font-weight: 400; color: #6b7684; font-size: 11px; display: block; }
        .rc-citycode-btn { background: none; border: none; padding: 0; color: #0b4f8a; font-weight: 600; font-size: 13px; cursor: pointer; text-decoration: underline; }
        .rc-venue-links { display: flex; flex-direction: column; gap: 4px; }
        .rc-prevcentres-btn { background: none; border: none; padding: 0; color: #6b4fa0; font-weight: 600; font-size: 12.5px; cursor: pointer; text-decoration: underline; }

        .venue-row td { background: #fbfcfd; border-top: none; padding: 0; }
        .venue-inner { padding: 16px 20px; border-left: 3px solid #003366; }
        .venue-inner h4 { margin: 0 0 10px; font-size: 13px; color: #003366; text-transform: uppercase; letter-spacing: .3px; }

        .venue-cycle-bar { display: flex; align-items: center; gap: 10px; margin-bottom: 14px; }
        .venue-cycle-bar label { font-size: 12.5px; font-weight: 600; color: #333; }
        .venue-cycle-bar select { padding: 5px 8px; border: 1px solid #9aa5b1; font-size: 12.5px; }

        .venue-table { width: 100%; border-collapse: collapse; font-size: 12.5px; margin-bottom: 14px; }
        .venue-table th { text-align: left; padding: 7px 10px; background: #eef1f4; border: 1px solid #d7dce2; font-weight: 600; }
        .venue-table td { padding: 7px 10px; border: 1px solid #e2e6ea; }
        .venue-table .action-link { color: #0b4f8a; text-decoration: underline; cursor: pointer; font-size: 12px; margin-right: 8px; }
        .venue-code-tag { display: inline-block; background: #003366; color: #fff; font-size: 11px; font-weight: 600; padding: 2px 7px; border-radius: 3px; letter-spacing: .3px; }

        .prev-inner { padding: 16px 20px; border-left: 3px solid #6b4fa0; background: #fbfaff; }
        .prev-inner h4 { margin: 0 0 10px; font-size: 13px; color: #6b4fa0; text-transform: uppercase; letter-spacing: .3px; }
        .prev-table { width: 100%; border-collapse: collapse; font-size: 12.5px; margin-bottom: 4px; }
        .prev-table th { text-align: left; padding: 7px 10px; background: #eee8f7; border: 1px solid #d9cdec; font-weight: 600; }
        .prev-table td { padding: 7px 10px; border: 1px solid #e6def2; }
        .prev-table .cycle-tag { display: inline-block; background: #6b4fa0; color: #fff; font-size: 10.5px; padding: 2px 6px; border-radius: 3px; }
        .prev-use-btn { background: #6b4fa0; color: #fff; border: 1px solid #6b4fa0; padding: 4px 12px; font-size: 12px; cursor: pointer; }
        .prev-use-btn:hover { background: #7d5cb5; }
        .prev-close-row { text-align: right; margin-top: 8px; }

        .venue-form-title { font-size: 12.5px; font-weight: 600; color: #333; margin: 4px 0 10px; }
        .venue-form-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 10px 16px; }
        .venue-form-grid label { font-size: 12px; color: #444; display: block; margin-bottom: 3px; font-weight: 600; }
        .venue-form-grid input[type=text] { width: 100%; padding: 6px 8px; box-sizing: border-box; border: 1px solid #9aa5b1; font-size: 13px; }
        .rc-error { color: #a32d2d; font-size: 11.5px; margin-top: 3px; display: block; }

        .btn-gov { background: #003366; color: #fff; border: 1px solid #003366; padding: 6px 16px; font-size: 13px; cursor: pointer; }
        .btn-gov:hover { background: #024a8f; }
        .btn-gov-outline { background: #fff; color: #003366; border: 1px solid #003366; padding: 6px 16px; font-size: 13px; cursor: pointer; }
        .btn-gov-outline:hover { background: #eef3f8; }
        .venue-actions { display: flex; gap: 8px; justify-content: flex-end; margin-top: 12px; }

        .no-data-msg { padding: 14px 16px; font-size: 13px; color: #666; }

        .rc-logout-link { float: right; color: #fff; text-decoration: underline; font-size: 12px; }

        .readonly-field { background-color: #f0f0f0; color: #555; cursor: not-allowed; }

        .finalize-overlay {
            display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%;
            background: rgba(0,0,0,0.45); z-index: 9999; align-items: center; justify-content: center;
        }
        .finalize-box {
            background: #fff; max-width: 460px; width: 90%; padding: 22px 24px; border-radius: 4px;
            box-shadow: 0 8px 30px rgba(0,0,0,0.25); font-family: "Segoe UI", Arial, sans-serif;
        }
        .finalize-box h3 { margin: 0 0 10px; font-size: 15.5px; color: #1a7a3c; }
        .finalize-box p { font-size: 13.5px; color: #333; line-height: 1.5; margin: 0 0 10px; }
        .finalize-box .fin-actions { text-align: right; }
    </style>

    <div class="rc-wrap">
        <div class="rc-panel">
            <div class="rc-panel-header">
                Exam Centre Identification &mdash; <asp:Label ID="lblRCLoginTitle" runat="server" Text="RC Login" />
            </div>

            <asp:GridView ID="gvCityPreference" runat="server" AutoGenerateColumns="false"
                CssClass="rc-table" GridLines="None" DataKeyNames="pref_id"
                OnRowCommand="gvCityPreference_RowCommand" ShowHeader="true">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            City Code<span class="sub">Candidate 1st Preference</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# System.Web.UI.DataBinder.Eval(Container.DataItem, "city_code") %>
                            <span class="sub"><%# System.Web.UI.DataBinder.Eval(Container.DataItem, "city_name") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Per Session (Max)<span class="sub"> Last 3 Exams</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# System.Web.UI.DataBinder.Eval(Container.DataItem, "PerSessionMaxLast3") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            Per Session (Max)<span class="sub">Current, Filled So Far</span>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# System.Web.UI.DataBinder.Eval(Container.DataItem, "PerSessionMaxCurrent") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Venues">
                        <ItemTemplate>
                            <div class="rc-venue-links">
                                <asp:LinkButton ID="btnToggle" runat="server" CssClass="rc-citycode-btn"
                                    CommandName="ToggleVenues"
                                    CommandArgument='<%# Convert.ToString(System.Web.UI.DataBinder.Eval(Container.DataItem, "pref_id")) + "|" + Convert.ToString(System.Web.UI.DataBinder.Eval(Container.DataItem, "city_code")) %>'
                                    Text="View / Add Venues" />
                                <asp:LinkButton ID="btnPrevCentres" runat="server" CssClass="rc-prevcentres-btn"
                                    CommandName="TogglePrevCentres"
                                    CommandArgument='<%# Convert.ToString(System.Web.UI.DataBinder.Eval(Container.DataItem, "pref_id")) + "|" + Convert.ToString(System.Web.UI.DataBinder.Eval(Container.DataItem, "city_code")) %>'
                                    Text="Previous Venues" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:PlaceHolder ID="phPrevCentresPanel" runat="server" Visible="false">
                <table class="rc-table" style="border-top: none;">
                    <tr>
                        <td colspan="4" style="padding:0;">
                            <div class="prev-inner">
                                <h4>Previous Venues &mdash; City Code: <asp:Label ID="lblPrevCityCode" runat="server" /></h4>

                                <asp:GridView ID="gvPrevCentres" runat="server" AutoGenerateColumns="false"
                                    CssClass="prev-table" GridLines="None" DataKeyNames="venue_id"
                                    OnRowCommand="gvPrevCentres_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Venue Code">
                                            <ItemTemplate>
                                                <span class="cycle-tag"><%# System.Web.UI.DataBinder.Eval(Container.DataItem, "venue_code") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="ES Name" DataField="es_name" />
                                        <asp:BoundField HeaderText="ES Phone" DataField="es_phone" />
                                        <asp:BoundField HeaderText="ES Email" DataField="es_mail" />
                                        <asp:BoundField HeaderText="Centre Name" DataField="centre_name" />
                                        <asp:BoundField HeaderText="District" DataField="district_name" />
                                        <asp:TemplateField HeaderText="Exam Cycle">
                                            <ItemTemplate>
                                                <span class="cycle-tag"><%# System.Web.UI.DataBinder.Eval(Container.DataItem, "exam_cycle") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkUseCentre" runat="server" CssClass="prev-use-btn"
                                                    CommandName="UsePrevCentre" CommandArgument='<%# System.Web.UI.DataBinder.Eval(Container.DataItem, "venue_id") %>'
                                                    Text="Add Venue" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Label ID="lblNoPrevCentres" runat="server" Text="No previous venues found for this city code." CssClass="no-data-msg" Visible="false" />

                                <div class="prev-close-row">
                                    <asp:Button ID="btnClosePrevCentres" runat="server" Text="Close" CssClass="btn-gov-outline" OnClick="btnClosePrevCentres_Click" CausesValidation="false" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:PlaceHolder>

            <asp:PlaceHolder ID="phVenuePanel" runat="server" Visible="false">
                <table class="rc-table" style="border-top: none;">
                    <tr class="venue-row">
                        <td colspan="4">
                            <div class="venue-inner">
                                <h4>Venues for City Code: <asp:Label ID="lblActiveCityCode" runat="server" /></h4>

                                <asp:HiddenField ID="hdnPrefId" runat="server" />
                                <asp:HiddenField ID="hdnVenueId" runat="server" Value="0" />
                                <asp:HiddenField ID="hdnShowConsentPopup" runat="server" Value="0" />

                                <div class="venue-cycle-bar">
                                    <label>Exam Cycle:</label>
                                    <asp:DropDownList ID="ddlExamCycle" runat="server" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlExamCycle_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>

                                <asp:GridView ID="gvVenues" runat="server" AutoGenerateColumns="false"
                                    CssClass="venue-table" GridLines="None" DataKeyNames="venue_id"
                                    OnRowCommand="gvVenues_RowCommand">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Venue Code">
                                            <ItemTemplate>
                                                <span class="venue-code-tag"><%# System.Web.UI.DataBinder.Eval(Container.DataItem, "venue_code") %></span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="ES Name" DataField="es_name" />
                                        <asp:BoundField HeaderText="Phone" DataField="es_phone" />
                                        <asp:BoundField HeaderText="Email" DataField="es_mail" />
                                        <asp:BoundField HeaderText="Centre Name" DataField="centre_name" />
                                        <asp:BoundField HeaderText="District" DataField="district_name" />
                                        <asp:TemplateField HeaderText="Action">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEditVenue" runat="server" CssClass="action-link" CommandName="EditVenue" CommandArgument='<%# System.Web.UI.DataBinder.Eval(Container.DataItem, "venue_id") %>' Text="Edit" />
                                                <asp:LinkButton ID="lnkDeleteVenue" runat="server" CssClass="action-link" CommandName="DeleteVenue" CommandArgument='<%# System.Web.UI.DataBinder.Eval(Container.DataItem, "venue_id") %>' Text="Delete"
                                                    OnClientClick="return confirm('Delete this venue?');" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Label ID="lblNoVenues" runat="server" Text="No venues added yet for this city code." CssClass="no-data-msg" Visible="false" />

                                <div class="venue-form-title"><asp:Literal ID="litFormTitle" runat="server" Text="Add Venue" /></div>

                                <div class="venue-form-grid">
                                    <div>
                                        <label>ES Name</label>
                                        <asp:TextBox ID="txtEsName" runat="server" placeholder="Exam Superintendent name" />
                                        <asp:RequiredFieldValidator ID="rfvEsName" runat="server" ControlToValidate="txtEsName"
                                            ErrorMessage="ES name is required." CssClass="rc-error" Display="Dynamic" ValidationGroup="VenueForm" />
                                    </div>
                                    <div>
                                        <label>Phone</label>
                                        <asp:TextBox ID="txtEsPhone" runat="server" placeholder="10-digit mobile number" MaxLength="10" />
                                        <asp:RequiredFieldValidator ID="rfvEsPhone" runat="server" ControlToValidate="txtEsPhone"
                                            ErrorMessage="Phone number is required." CssClass="rc-error" Display="Dynamic" ValidationGroup="VenueForm" />
                                        <asp:RegularExpressionValidator ID="revEsPhone" runat="server" ControlToValidate="txtEsPhone"
                                            ValidationExpression="^[6-9]\d{9}$" ErrorMessage="Enter a valid 10-digit mobile number."
                                            CssClass="rc-error" Display="Dynamic" ValidationGroup="VenueForm" />
                                    </div>
                                    <div>
                                        <label>Email</label>
                                        <asp:TextBox ID="txtEsMail" runat="server" placeholder="name@example.com" />
                                        <asp:RequiredFieldValidator ID="rfvEsMail" runat="server" ControlToValidate="txtEsMail"
                                            ErrorMessage="Email is required." CssClass="rc-error" Display="Dynamic" ValidationGroup="VenueForm" />
                                        <asp:RegularExpressionValidator ID="revEsMail" runat="server" ControlToValidate="txtEsMail"
                                            ValidationExpression="^[\w\.\-]+@[a-zA-Z\d\-]+(\.[a-zA-Z\d\-]+)*\.[a-zA-Z]{2,}$"
                                            ErrorMessage="Enter a valid email like name@example.com." CssClass="rc-error" Display="Dynamic" ValidationGroup="VenueForm" />
                                    </div>
                                    <div>
                                        <label>Centre Name</label>
                                        <asp:TextBox ID="txtCentreName" runat="server" placeholder="e.g. Govt Sr Sec School" />
                                        <asp:RequiredFieldValidator ID="rfvCentreName" runat="server" ControlToValidate="txtCentreName"
                                            ErrorMessage="Centre name is required." CssClass="rc-error" Display="Dynamic" ValidationGroup="VenueForm" />
                                    </div>
                                    <div>
                                        <label>District</label>
                                        <asp:TextBox ID="txtDistrict" runat="server" placeholder="e.g. South West Delhi" ReadOnly="true" CssClass="readonly-field" />
                                        <asp:RequiredFieldValidator ID="rfvDistrict" runat="server" ControlToValidate="txtDistrict"
                                            ErrorMessage="District is required." CssClass="rc-error" Display="Dynamic" ValidationGroup="VenueForm" />
                                    </div>
                                </div>

                                <div class="venue-actions">
                                    <asp:Button ID="btnCancelVenue" runat="server" Text="Close" CssClass="btn-gov-outline" OnClick="btnCancelVenue_Click" CausesValidation="false" />
                                    <asp:Button ID="btnSaveVenue" runat="server" Text="Save Venue" CssClass="btn-gov" OnClick="btnSaveVenue_Click"
                                        ValidationGroup="VenueForm" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </asp:PlaceHolder>

            <asp:Label ID="lblNoData" runat="server" Text="No city preference data found for this RC." CssClass="no-data-msg" Visible="false" />
        </div>
    </div>

    <div class="finalize-overlay" id="consentOverlay">
        <div class="finalize-box">
            <h3>Venue Saved</h3>
            <p>Copy this consent form link and send it to the concerned ES:</p>
            <div style="display:flex; gap:8px; align-items:center; margin-bottom:10px;">
                <asp:TextBox ID="txtConsentLink" runat="server" ReadOnly="true" style="flex:1; padding:6px 8px; font-size:12.5px; border:1px solid #9aa5b1; background:#fff;" />
                <button type="button" class="btn-gov" onclick="copyConsentLink(this)">Copy Link</button>
            </div>
            <asp:Label ID="lblCopyStatus" runat="server" style="display:block; margin-bottom:10px; font-size:12.5px; color:#1a7a3c;" Visible="false" Text="Link copied to clipboard." />
            <div class="fin-actions">
                <button type="button" class="btn-gov-outline" onclick="closeConsentModal();">Close</button>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function copyConsentLink(btn) {
            var boxes = document.querySelectorAll("input[id$='txtConsentLink']");
            if (boxes.length === 0) return;
            var input = boxes[0];
            input.select();
            input.setSelectionRange(0, 99999);
            navigator.clipboard.writeText(input.value).then(function () {
                var statusLabels = document.querySelectorAll("span[id$='lblCopyStatus']");
                if (statusLabels.length > 0) { statusLabels[0].style.display = 'block'; }
            });
        }
        function showConsentModal() {
            document.getElementById('consentOverlay').style.display = 'flex';
        }
        function closeConsentModal() {
            document.getElementById('consentOverlay').style.display = 'none';
        }
        window.addEventListener('DOMContentLoaded', function () {
            var flagBoxes = document.querySelectorAll("input[id$='hdnShowConsentPopup']");
            if (flagBoxes.length > 0 && flagBoxes[0].value === '1') {
                showConsentModal();
                flagBoxes[0].value = '0';
            }
        });
    </script>

</asp:Content>