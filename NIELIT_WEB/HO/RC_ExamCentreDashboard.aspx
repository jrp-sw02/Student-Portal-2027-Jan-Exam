<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RC_ExamCentreDashboard.aspx.cs"
    Inherits="HO_RC_ExamCentreDashboard" MasterPageFile="~/MasterPages/MyInfo.master" Debug="false" %>

<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .rcVenueLinks a,
        .rcVenueLinks input,
        .rcVenueLinks button
        {
            display: inline-block;
            margin-bottom: 4px;
        }
        .rcVenueSubText
        {
            display: block;
            color: #666666;
            font-size: 11px;
        }
        .consentOverlay
        {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.45);
            z-index: 9999;
        }
        .consentBox
        {
            background: #ffffff;
            max-width: 460px;
            width: 90%;
            margin: 100px auto;
            padding: 20px;
            border-radius: 4px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    Exam Centre Identification &mdash; RC Login
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc1:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script type="text/javascript" language="javascript">
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
            document.getElementById('consentOverlay').style.display = 'block';
        }
        function closeConsentModal() {
            document.getElementById('consentOverlay').style.display = 'none';
        }
        window.onload = function () {
            var flagBoxes = document.querySelectorAll("input[id$='hdnShowConsentPopup']");
            if (flagBoxes.length > 0 && flagBoxes[0].value === '1') {
                showConsentModal();
                flagBoxes[0].value = '0';
            }
        };
    </script>

    <asp:Label ID="lblNoData" runat="server" SkinID="CaptionLabel" Text="No city preference data found for this RC." Visible="false" />

    <div id="divGrid" runat="server">
        <asp:GridView ID="gvCityPreference" runat="server" AutoGenerateColumns="false"
            CssClass="sample2" Width="100%" DataKeyNames="pref_id"
            OnRowCommand="gvCityPreference_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="City Code">
                    <ItemTemplate>
                        <%# System.Web.UI.DataBinder.Eval(Container.DataItem, "city_code") %>
                        <span class="rcVenueSubText">(<%# System.Web.UI.DataBinder.Eval(Container.DataItem, "city_name") %>)</span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Per Session (Max) - Last 3 Exams">
                    <ItemTemplate>
                        <%# System.Web.UI.DataBinder.Eval(Container.DataItem, "PerSessionMaxLast3") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Per Session (Max) - Current, Filled">
                    <ItemTemplate>
                        <%# System.Web.UI.DataBinder.Eval(Container.DataItem, "PerSessionMaxCurrent") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Venues">
                    <ItemTemplate>
                        <div class="rcVenueLinks">
                            <asp:LinkButton ID="btnToggle" runat="server"
                                CommandName="ToggleVenues"
                                CommandArgument='<%# Convert.ToString(System.Web.UI.DataBinder.Eval(Container.DataItem, "pref_id")) + "|" + Convert.ToString(System.Web.UI.DataBinder.Eval(Container.DataItem, "city_code")) %>'
                                Text="View / Add Venues" />
                            <br />
                            <asp:LinkButton ID="btnPrevCentres" runat="server"
                                CommandName="TogglePrevCentres"
                                CommandArgument='<%# Convert.ToString(System.Web.UI.DataBinder.Eval(Container.DataItem, "pref_id")) + "|" + Convert.ToString(System.Web.UI.DataBinder.Eval(Container.DataItem, "city_code")) %>'
                                Text="Previous Venues" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <asp:Panel ID="phPrevCentresPanel" runat="server" Visible="false">
        <br />
        <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Previous Venues &mdash; City Code:" />
                    <asp:Label ID="lblPrevCityCode" runat="server" SkinID="CaptionLabel" />
                </td>
            </tr>
        </table>

        <asp:GridView ID="gvPrevCentres" runat="server" AutoGenerateColumns="false"
            CssClass="sample2" Width="100%" DataKeyNames="venue_id"
            OnRowCommand="gvPrevCentres_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Venue Code" DataField="venue_code" />
                <asp:BoundField HeaderText="ES Name" DataField="es_name" />
                <asp:BoundField HeaderText="ES Phone" DataField="es_phone" />
                <asp:BoundField HeaderText="ES Email" DataField="es_mail" />
                <asp:BoundField HeaderText="Centre Name" DataField="centre_name" />
                <asp:BoundField HeaderText="District" DataField="district_name" />
                <asp:BoundField HeaderText="Exam Cycle" DataField="exam_cycle" />
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkUseCentre" runat="server"
                            CommandName="UsePrevCentre" CommandArgument='<%# System.Web.UI.DataBinder.Eval(Container.DataItem, "venue_id") %>'
                            Text="Add Venue" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Label ID="lblNoPrevCentres" runat="server" SkinID="CaptionLabel" Text="No previous venues found for this city code." Visible="false" />

        <div style="text-align: right; margin-top: 10px">
            <asp:Button ID="btnClosePrevCentres" runat="server" Text="Close" OnClick="btnClosePrevCentres_Click" CausesValidation="false" />
        </div>
    </asp:Panel>

    <asp:Panel ID="phVenuePanel" runat="server" Visible="false">
        <br />
        <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Venues for City Code:" />
                    <asp:Label ID="lblActiveCityCode" runat="server" SkinID="CaptionLabel" />
                </td>
                <td style="text-align: right;">
                    <asp:Label ID="Label12" runat="server" SkinID="CaptionLabel" Text="Exam Cycle:" />
                    <asp:DropDownList ID="ddlExamCycle" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlExamCycle_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
            </tr>
        </table>

        <asp:HiddenField ID="hdnPrefId" runat="server" />
        <asp:HiddenField ID="hdnVenueId" runat="server" Value="0" />
        <asp:HiddenField ID="hdnShowConsentPopup" runat="server" Value="0" />

        <asp:GridView ID="gvVenues" runat="server" AutoGenerateColumns="false"
            CssClass="sample2" Width="100%" DataKeyNames="venue_id"
            OnRowCommand="gvVenues_RowCommand">
            <Columns>
                <asp:BoundField HeaderText="Venue Code" DataField="venue_code" />
                <asp:BoundField HeaderText="ES Name" DataField="es_name" />
                <asp:BoundField HeaderText="Phone" DataField="es_phone" />
                <asp:BoundField HeaderText="Email" DataField="es_mail" />
                <asp:BoundField HeaderText="Centre Name" DataField="centre_name" />
                <asp:BoundField HeaderText="District" DataField="district_name" />
                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkEditVenue" runat="server" CommandName="EditVenue" CommandArgument='<%# System.Web.UI.DataBinder.Eval(Container.DataItem, "venue_id") %>' Text="Edit" />
                        &nbsp;|&nbsp;
                        <asp:LinkButton ID="lnkDeleteVenue" runat="server" CommandName="DeleteVenue" CommandArgument='<%# System.Web.UI.DataBinder.Eval(Container.DataItem, "venue_id") %>' Text="Delete"
                            OnClientClick="return confirm('Delete this venue?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Label ID="lblNoVenues" runat="server" SkinID="CaptionLabel" Text="No venues added yet for this city code." Visible="false" />

        <asp:Label ID="litFormTitle" runat="server" SkinID="CaptionLabel" Text="Add Venue" />

        <table class="sample2" width="100%" border="0" cellpadding="2" cellspacing="0">
            <tr>
                <td style="width: 33%;" valign="top">
                    <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel" Text="ES Name &lt;b class='mandatory'&gt;*&lt;/b&gt;" />
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Phone &lt;b class='mandatory'&gt;*&lt;/b&gt;" />
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Email &lt;b class='mandatory'&gt;*&lt;/b&gt;" />
                </td>
            </tr>
            <tr class="even">
                <td style="width: 33%;" valign="top">
                    <asp:TextBox ID="txtEsName" runat="server" MaxLength="200" SkinID="txt248" />
                    <asp:RequiredFieldValidator ID="rfvEsName" runat="server" ControlToValidate="txtEsName"
                        ErrorMessage="ES name is required." CssClass="error" Display="Dynamic" ValidationGroup="VenueForm" />
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:TextBox ID="txtEsPhone" runat="server" MaxLength="10" SkinID="txt248" />
                    <asp:RequiredFieldValidator ID="rfvEsPhone" runat="server" ControlToValidate="txtEsPhone"
                        ErrorMessage="Phone number is required." CssClass="error" Display="Dynamic" ValidationGroup="VenueForm" />
                    <asp:RegularExpressionValidator ID="revEsPhone" runat="server" ControlToValidate="txtEsPhone"
                        ValidationExpression="^[6-9]\d{9}$" ErrorMessage="Enter a valid 10-digit mobile number."
                        CssClass="error" Display="Dynamic" ValidationGroup="VenueForm" />
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:TextBox ID="txtEsMail" runat="server" MaxLength="150" SkinID="txt248" />
                    <asp:RequiredFieldValidator ID="rfvEsMail" runat="server" ControlToValidate="txtEsMail"
                        ErrorMessage="Email is required." CssClass="error" Display="Dynamic" ValidationGroup="VenueForm" />
                    <asp:RegularExpressionValidator ID="revEsMail" runat="server" ControlToValidate="txtEsMail"
                        ValidationExpression="^[\w\.\-]+@[a-zA-Z\d\-]+(\.[a-zA-Z\d\-]+)*\.[a-zA-Z]{2,}$"
                        ErrorMessage="Enter a valid email like name@example.com." CssClass="error" Display="Dynamic" ValidationGroup="VenueForm" />
                </td>
            </tr>
            <tr>
                <td style="width: 33%;" valign="top">
                    <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;" />
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="District &lt;b class='mandatory'&gt;*&lt;/b&gt;" />
                </td>
                <td style="width: 33%;" valign="top"></td>
            </tr>
            <tr class="even">
                <td style="width: 33%;" valign="top">
                    <asp:TextBox ID="txtCentreName" runat="server" MaxLength="200" SkinID="txt248" />
                    <asp:RequiredFieldValidator ID="rfvCentreName" runat="server" ControlToValidate="txtCentreName"
                        ErrorMessage="Centre name is required." CssClass="error" Display="Dynamic" ValidationGroup="VenueForm" />
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:TextBox ID="txtDistrict" runat="server" MaxLength="150" SkinID="txt248" ReadOnly="true" />
                    <asp:RequiredFieldValidator ID="rfvDistrict" runat="server" ControlToValidate="txtDistrict"
                        ErrorMessage="District is required." CssClass="error" Display="Dynamic" ValidationGroup="VenueForm" />
                </td>
                <td style="width: 33%;" valign="top"></td>
            </tr>
        </table>

        <div style="text-align: right; margin-top: 10px">
            <asp:Button ID="btnCancelVenue" runat="server" Text="Close" OnClick="btnCancelVenue_Click" CausesValidation="false" />
            <asp:Button ID="btnSaveVenue" runat="server" Text="Save Venue" OnClick="btnSaveVenue_Click" ValidationGroup="VenueForm" />
        </div>
    </asp:Panel>

    <div id="consentOverlay" class="consentOverlay">
        <div class="consentBox">
            <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Venue Saved" Font-Bold="true" /><br /><br />
            <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Copy this consent form link and send it to the concerned ES:" /><br /><br />
            <asp:TextBox ID="txtConsentLink" runat="server" ReadOnly="true" SkinID="txt756" Width="70%" />
            <input type="button" value="Copy Link" onclick="copyConsentLink(this);" />
            <br />
            <asp:Label ID="lblCopyStatus" runat="server" SkinID="CaptionLabel" Text="Link copied to clipboard." Visible="false" />
            <div style="text-align: right; margin-top: 10px">
                <input type="button" value="Close" onclick="closeConsentModal();" />
            </div>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>