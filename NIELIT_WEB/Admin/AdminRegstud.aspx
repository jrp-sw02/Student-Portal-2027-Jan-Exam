<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="AdminRegstud.aspx.cs" Inherits="Admin_AdminRegstude"  Debug="false"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/CourseApplication.ascx" TagName="CourseApplication"
    TagPrefix="uc4" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc5" %>
<%@ Register Src="../UserControl/SideLink.ascx" TagName="SideLink" TagPrefix="uc6" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Registered Students"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"
        Visible="False" />
    <asp:Panel runat="server" ID="pnlFilter" Visible="true">
        <div id="filterContainer">
            <a href="#" id="filterButton"><span></span><em></em></a>
            <div style="clear: both">
            </div>
            <div id="filterBox" align="left">
                <div id="filterPannel">
                    <asp:UpdatePanel EnableViewState="true" RenderMode="Inline" ID="filterPnal_upnlFilter"
                        UpdateMode="Conditional" runat="server">
                        <ContentTemplate>
                            <table cellpadding="0" id="body1" cellspacing="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFiler" Width="60%" runat="server" Font-Bold="true" Font-Size="12pt"
                                            Text="Filter Panel"></asp:Label>
                                        <asp:Button runat="server" ID="btnReset" ToolTip="Reset Filter" ClientIDMode="Static"
                                            Text="" OnClick="ResetFilterPanel" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Application Type"></asp:Label>
                                        <asp:DropDownList ID="ddlapptype" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <script language="javascript" type="text/javascript">
                    var box = $('#filterBox');
                    shortcut.add("Ctrl+Shift+F", function () {
                        box.show();
                    });
                    shortcut.add("Esc", function () {
                        box.hide();
                    });
                </script>
            </div>
        </div>
    </asp:Panel>
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Candidate name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc5:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateLogin() {

            return true;

        }

        var dtgp = "<%= gvMain.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
            ShowHideMenu(obj, tableid);
        }
    </script>
    <asp:Label Width="99%" Style="background-color: #EACFCE; padding-top: 4px; padding-bottom: 4px;
        padding-left: 4px; color: Red; border: 1px solid maroon; font-size: 11pt; font-variant: normal;"
        ID="Lblerror" Visible="false" runat="server" Text=""></asp:Label>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            AutoGenerateColumns="False" Width="100%" OnRowDataBound="gvMain_RowDataBound">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderText="Course" HeaderStyle-Width="10%" DataTextField="CourseName"
                                    SortExpression="CourseName" DataNavigateUrlFields="ApplID,ApplTypeID,courseID,Status,Regno"
                                    DataNavigateUrlFormatString="?ApplID={0}&ApplTypeID={1}&courseID={2}&Status={3}&Regno={4}">
                                    <HeaderStyle Width="10%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="13%" HeaderText="Registration No" DataNavigateUrlFields="ApplID,ApplTypeID,courseID,Status,Regno"
                                    DataNavigateUrlFormatString="?ApplID={0}&ApplTypeID={1}&courseID={2}&Status={3}&Regno={4}"
                                    DataTextField="Regno" SortExpression="Regno" Target="_self">
                                    <HeaderStyle Width="13%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Registration Date" HeaderStyle-Width="14%" DataTextField="date"
                                    DataNavigateUrlFields="ApplID,ApplTypeID,courseID,Status,Regno" DataNavigateUrlFormatString="?ApplID={0}&ApplTypeID={1}&courseID={2}&Status={3}&Regno={4}"
                                    SortExpression="date" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <HeaderStyle Width="14%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Student Name" HeaderStyle-Width="27%" DataTextField="StudentName"
                                    DataNavigateUrlFields="ApplID,ApplTypeID,courseID,Status,Regno" DataNavigateUrlFormatString="?ApplID={0}&ApplTypeID={1}&courseID={2}&Status={3}&Regno={4}"
                                    SortExpression="StudentName">
                                    <HeaderStyle Width="27%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Father Name" HeaderStyle-Width="30%" DataTextField="FatherName"
                                    DataNavigateUrlFields="ApplID,ApplTypeID,courseID,Status,Regno" DataNavigateUrlFormatString="?ApplID={0}&ApplTypeID={1}&courseID={2}&Status={3}&Regno={4}"
                                    SortExpression="FatherName">
                                    <HeaderStyle Width="30%" />
                                </asp:HyperLinkField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divNavigation" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </asp:View>
        <asp:View ID="New" runat="server">
            <div style="text-align: right; margin-top: 10px">
                <table style="text-align: left" class="sample3" border="0" cellpadding="3" cellspacing="1"
                    width="100%">
                    <tr class="head1">
                        <td colspan="3">
                            <asp:Label ID="Label69" runat="server" Text="Personal Details"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td style="width: 36%">
                            <asp:Label ID="Label70" runat="server" Text=" Full Name"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="LblAppName" runat="server" Text=""></asp:Label>
                        </td>
                        <td style="width: 20%" align="center" rowspan="10" valign="top">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table width="90%" cellspacing="3">
                                        <tr>
                                            <td class="box" align="center" valign="middle" style="background-color: #c9d7e2;
                                                height: 140px;">
                                                <asp:Image ID="ImgCandidatePhoto" ImageUrl="../images/photo.jpg" Width="112" Height="130"
                                                    runat="server" Style="text-align: center;" ImageAlign="Middle" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" id="TdPhotoCaption" class="box" style="background-color: #c9d7e2;">
                                                <asp:ImageButton ID="ImgBtnPrevious" runat="server" Style="float: left; width: 20px;"
                                                    ImageUrl="~/images/previous.gif" CommandArgument="P" Enabled="False" OnClick="ToggleImage" />
                                                <asp:Label ID="LblPhotoCaption" runat="server" Text="Photograph"></asp:Label>
                                                <asp:ImageButton ID="ImgBtnNext" ToolTip="Click to view signature" runat="server"
                                                    ImageUrl="~/images/next.gif" Style="float: right;" CommandArgument="N" OnClick="ToggleImage" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:LinkButton ID="LnkBtnPhotoDetail" runat="server" ForeColor="#003366" ToolTip="Please click to change the Images"
                                                    OnClick="LnkBtnPhotoDetail_Click" Visible="true">Change Images</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr class="gdrow1" id="TrGardianName" runat="server" visible="false">
                        <td style="width: 36%">
                            <asp:Label ID="Label9" runat="server" Text="Guardian's Name"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="LblGuardianName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1" id="TrFatherName" runat="server" visible="false">
                        <td style="width: 36%">
                            <asp:Label ID="Label2" runat="server" Text="Father's Name"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="LblFatherName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1" runat="server" id="TrMotherName" visible="false">
                        <td style="width: 36%">
                            <asp:Label ID="Label3" runat="server" Text="Mother's Name"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="LblMotherName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1" runat="server" id="TrName">
                        <td style="width: 36%">
                            <asp:Label ID="Label4" runat="server" Text="Gender"></asp:Label>
                        </td>
                        <td align="left" style="width: 44%">
                            <asp:Label ID="LblGender" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td style="width: 36%">
                            <asp:Label ID="Label5" runat="server" Text="Marital Status"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="LblMaritalStatus" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td style="width: 36%">
                            <asp:Label ID="Label6" runat="server" Text="Date of Birth "></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="LblDob" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td style="width: 36%">
                            <asp:Label ID="Label7" runat="server" Text="Cast Category"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="LblCategory" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td>
                            <asp:Label ID="Label84" runat="server" Text="Occuption"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="LblOccuption" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td>
                            <asp:Label ID="Label85" runat="server" Text="Is Handicaped"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="LblIsHandicaped" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td>
                            <asp:Label ID="Label86" runat="server" Text="Is Ex-Servicemane"></asp:Label>
                        </td>
                        <td style="width: 44%">
                            <asp:Label ID="LblIsExServicemane" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="3">
                            <asp:Label ID="Label83" runat="server" Text="Course Registration Details"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td style="width: 36%">
                            Registration Number
                        </td>
                        <td style="width: 44%" colspan="2">
                            <asp:Label ID="LblRegNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td style="width: 36%">
                            Registration Date
                        </td>
                        <td style="width: 44%" colspan="2">
                            <asp:Label ID="LblRegDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td style="width: 36%">
                            Commencement Date
                        </td>
                        <td style="width: 44%" colspan="2">
                            <asp:Label ID="LblComDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td style="width: 36%">
                            Valid Upto Date
                        </td>
                        <td style="width: 44%" colspan="2">
                            <asp:Label ID="LblValidUpto" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td style="width: 36%">
                            Current Course
                        </td>
                        <td style="width: 44%" colspan="2">
                            <asp:Label ID="LblCourse" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td style="width: 36%">
                            Candidate Type
                        </td>
                        <td style="width: 44%" colspan="2">
                            <asp:Label ID="LblCandidateType" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1" id="TrAccCentre" runat="server">
                        <td style="width: 36%">
                            Accredited Centre
                        </td>
                        <td style="width: 44%" colspan="2">
                            <asp:Label ID="LblAccCentre" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trstatus" runat="server">
                        <td style="width: 36%">
                            Status
                        </td>
                        <td style="width: 44%" colspan="2">
                            <asp:Label ID="Lbregstatus" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="3">
                            <asp:Label ID="Label8" runat="server" Text="Contact Details"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td>
                            <asp:Label ID="Label18" runat="server" Text="Phone Number with STD Code"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LblPhone" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td style="width: 36%">
                            <asp:Label ID="Label40" runat="server" Text="Mobile Number"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LblMobile" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td style="width: 36%">
                            <asp:Label ID="Label21" runat="server" EnableTheming="True" Text="Email Address"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LblEmail" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="3">
                            <asp:Label ID="Label74" runat="server" Text="Correspondence Address Details"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td style="width: 36%">
                            <asp:Label ID="Label10" runat="server" Text="Address Line1 "></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LblCorAddressLine1" runat="server">NA</asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td style="width: 36%">
                            <asp:Label ID="Label17" runat="server" Text="Address Line2"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LbCorlAddressLine2" runat="server">NA</asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td style="width: 36%">
                            <asp:Label ID="Label19" runat="server" Text="Address Line3"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LblCorAddressLine3" runat="server">NA</asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td style="width: 36%">
                            <asp:Label ID="Label23" runat="server" Text="City "></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LblCityName" runat="server">NA</asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td style="width: 36%">
                            <asp:Label ID="Label24" runat="server" Text="District"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblDistrictName" runat="server" Text="NA"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td style="width: 36%">
                            <asp:Label ID="Label26" runat="server" Text="State "></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblStateName" runat="server" Text="NA"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td style="width: 36%">
                            <asp:Label ID="Label28" runat="server" Text="Pin Code"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblPinCodeNumber" runat="server" Text="NA"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="3">
                            <asp:Label ID="Labelp" runat="server" Text="Permanent Address  Details"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td style="width: 36%">
                            <asp:Label ID="Label20" runat="server" Text="Address Line1"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LblPerAddressLine1" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td style="width: 36%">
                            <asp:Label ID="Label22" runat="server" Text="Address Line2"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LblPerAddressLine2" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td style="width: 36%">
                            <asp:Label ID="Label25" runat="server" Text="Address Line3"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LblPerAddressLine3" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td style="width: 36%">
                            City
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LblPerCityName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td style="width: 36%">
                            <asp:Label ID="Label27" runat="server" Text="District"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LblPerDistrictName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td style="width: 36%">
                            <asp:Label ID="Label31" runat="server" Text="State"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LblPerStateName" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="gdalternate1">
                        <td style="width: 36%">
                            <asp:Label ID="Label33" runat="server" Text="Pin Code"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LblPerPinCodeNumber" runat="server" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="3">
                            Educational/Qualification Details
                        </td>
                    </tr>
                    <%-- <tr class="gdrow1">
                        <td valign="top" style="width: 36%">
                            <asp:Label ID="Label10" runat="server" Text="DOEACC Qualification"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="LblDoeaccQualification" runat="server"></asp:Label>
                        </td>
                    </tr>--%>
                    <tr valign="top" class="gdalternate1">
                        <td style="width: 36%">
                            <asp:Label ID="Label60" runat="server" Text="Highest Educational Qualification"></asp:Label>
                        </td>
                        <td colspan="2" style="width: 44%">
                            <asp:Label ID="LblHeighestEducation" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top" class="gdrow1">
                        <td style="width: 36%">
                            <asp:Label ID="Label82" runat="server" Text="Year of Passing"></asp:Label>
                        </td>
                        <td colspan="2" style="width: 44%">
                            <asp:Label ID="LblYrOfPassing" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr class="head1">
                        <td colspan="3">
                            Identification Details
                        </td>
                    </tr>
                    <tr class="gdrow1">
                        <td align="left" valign="top" style="width: 36%">
                            <asp:Label ID="Label30" runat="server" Text="Body Mark"></asp:Label>
                        </td>
                        <td colspan="2" align="left" valign="top" style="width: 44%">
                            <asp:Label ID="LblBodyMark" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="btnSave" OnClientClick="return ValidateLogin();" runat="server" Text="Save"
                    OnClick="SaveRecord" Visible="False" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <uc6:SideLink ID="SideLink1" runat="server" />
</asp:Content>
