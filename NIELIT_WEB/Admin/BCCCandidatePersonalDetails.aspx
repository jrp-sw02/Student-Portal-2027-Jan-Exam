<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BCCCandidatePersonalDetails.aspx.cs"
    Inherits="Admin_BCCCandidatePersonalDetails" MasterPageFile="~/MasterPages/main.master"  Debug="false"%>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:content id="Content1" contentplaceholderid="head" runat="Server">
</asp:content>
<asp:content id="Content3" contentplaceholderid="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Personal Detail"></asp:Label>
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"
        Visible="false" />
    <asp:Panel runat="server" ID="pnlFilter" Visible="false">
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
                                        <asp:Label ID="lblFilter1" Width="100%" runat="server" Text="Filter1"></asp:Label>
                                        <asp:DropDownList ID="ddlFilter1" Width="100%" runat="server">
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by object name or parent name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" Visible="false" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function Validate() {

            if (!isSelected("<%=ddlSalutation.ClientID%>", "Salutation"))
                return false;
            if (!isDate("<%=Txt_Dob.ClientID %>", "Invalid Date of Birth", "dd-MMM-yyyy"))
                return false;
            if (!isSelected("<%=ddloccupation.ClientID%>", "Occupation"))
                return false;
            if (!isSelected("<%=ddlCategory.ClientID%>", "Cast Category"))
                return false;
            var GuardianName = document.getElementById("<%=Txt_GName.ClientID %>").value;
            var FatherName = document.getElementById("<%=Txt_Fname.ClientID %>").value;
            var MotherName = document.getElementById("<%=Txt_Mname.ClientID %>").value;
            if ((GuardianName == "" && FatherName == "" && MotherName == "") || (GuardianName != "" && FatherName != "" && MotherName != "")) {
                callErrorMsg("<%=Txt_Fname.ClientID %>", "Please enter either (Father Name and Mother Name) OR  Guardian Name.");
                return false;
            }
            else if (FatherName != "" && MotherName == "") {
                callErrorMsg("<%=Txt_Mname.ClientID %>", "Please enter Mother Name.");
                return false;
            }
            else if (MotherName != "" && FatherName == "") {
                callErrorMsg("<%=Txt_Fname.ClientID %>", "Please enter Father Name.");
                return false;
            }
            //            if (document.getElementById("<%=Txt_Mname.ClientID %>").disabled == false) {
            //                if (!isBlank("<%=Txt_Mname.ClientID %>", "Mother Name"))
            //                    return false;
            //            }
            //            if (document.getElementById("<%=Txt_Fname.ClientID %>").disabled == false) {
            //                if (!isBlank("<%=Txt_Fname.ClientID %>", "Father Name"))
            //                    return false;
            //            }
            //            if (document.getElementById("<%=Txt_GName.ClientID %>").disabled == false) {
            //                if (!isBlank("<%=Txt_GName.ClientID %>", "Guardian Name"))
            //                    return false;
            //            }

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
                                <asp:HyperLinkField HeaderText="Name" DataNavigateUrlFields="ID,appid,Name" DataNavigateUrlFormatString="?key={0}&key1={1}&Name={2}"
                                    DataTextField="Name" SortExpression="Name" Target="_self">
                                    <HeaderStyle Width="26%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Father Name" DataTextField="Fname" SortExpression="Fname"
                                    DataNavigateUrlFields="ID,appid,Name" DataNavigateUrlFormatString="?key={0}&key1={1}&Name={2}">
                                    <HeaderStyle Width="27%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Mother Name" DataTextField="MName" SortExpression="MName"
                                    DataNavigateUrlFields="ID,appid,Name" DataNavigateUrlFormatString="?key={0}&key1={1}&Name={2}">
                                    <HeaderStyle Width="22%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderText="Gender" HeaderStyle-Width="8%" DataTextField="gender"
                                    SortExpression="gender" DataNavigateUrlFields="ID,appid,Name" DataNavigateUrlFormatString="?key={0}&key1={1}&Name={2}">
                                    <HeaderStyle Width="8%" />
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
            <%--<div style="background-color:#A8A8A8; width:98%;">--%>
            <table class="sample2" cellpadding="2" cellspacing="0">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label18" runat="server" SkinID="CaptionLabel" Text="Salutation&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label17" runat="server" SkinID="CaptionLabel" Text=" Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label25" runat="server" SkinID="CaptionLabel" Text="Gender &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlSalutation" runat="server" Height="22" SkinID="ddl250" 
                            Width="102px" AutoPostBack="True" 
                            onselectedindexchanged="ddlSalutation_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem Value="1">Mr.</asp:ListItem>
                            <asp:ListItem Value="2">Ms.</asp:ListItem>
                            <asp:ListItem Value="3">Others</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtName" runat="server" MaxLength="60" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlGender" runat="server" Height="22" SkinID="ddl250" Width="102px"
                                    >
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlSalutation" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblUserNameCaption1" runat="server" SkinID="CaptionLabel" Text="Father Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label24" runat="server" SkinID="CaptionLabel" Text="Mother Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label26" runat="server" SkinID="CaptionLabel" Text="Guardian Name"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <%--<asp:TextBox ID="TxtGender" runat="server" SkinID="txt248" MaxLength="8"></asp:TextBox>--%>
                        <asp:TextBox ID="Txt_Fname" runat="server" MaxLength="60" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="Txt_Mname" runat="server" MaxLength="60" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="Txt_GName" runat="server" MaxLength="60" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label22" runat="server" SkinID="CaptionLabel" Text="DOB &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label21" runat="server" SkinID="CaptionLabel" Text="Occupation &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label20" runat="server" SkinID="CaptionLabel" Text="Caste Category&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                        <asp:TextBox ID="Txt_Dob" runat="server" MaxLength="100" SkinID="txtDate"></asp:TextBox>
                        <img id="imgDob" runat="server" src="../images/calendaricon.jpg" style="width: 20px;
                            height: 22px; vertical-align: top;" />
                        <asp:CalendarExtender ID="ceDOB" TargetControlID="Txt_Dob" PopupPosition="BottomLeft"
                            Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddloccupation" runat="server" Height="22" SkinID="ddl250">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCategory" runat="server" EnableTheming="True" Height="22"
                            SkinID="ddl250" TabIndex="8">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return Validate();" runat="server" Text="Save"
                    OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>
        </asp:View>
    </asp:MultiView>
</asp:content>
<asp:content id="Content7" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content8" contentplaceholderid="cthRightPannel" runat="Server">
</asp:content>
