<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="AdminCandidatePersonaldetail.aspx.cs" Inherits="AdminCandidatePersonaldetail"
    Debug="True" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style type="text/css">
        .declaration-box {
            padding: 6px 10px;
            margin: 6px 0;
            border: 1px solid #ffeeba;
            background-color: #fff3cd;
            border-radius: 4px;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 12px;
            line-height: 1.4;
            color: #856404;
        }

            .declaration-box input[type="checkbox"] {
                vertical-align: top;
                margin-top: 1px;
                margin-right: 4px;
            }

        .declaration-text {
            display: inline;
        }

        .declaration-title {
            font-weight: bold;
            color: #856404;
        }

        .required-star {
            color: #dc3545;
            font-weight: bold;
        }

        .declaration-highlight {
            font-weight: bold;
        }
    </style>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Personal Detail"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
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
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function Validate() {
            ////vishal
            //if (!isBlankNumber("TextBoxAadharNo", "Aadhar Number"))
            //    return false;
            //if (!IsValidMinMaxLenght("TextBoxAadharNo", 12, 12, "Invalid Aadhar Number"))
            //    return false;
            //if (!isNumber("TextBoxAadharNo"))
            //    return false;
            ////vishal



            if (!isSelected("<%=ddlSalutation.ClientID%>", "Salutation"))
                return false;
            if (!isDate("<%=Txt_Dob.ClientID %>", "Invalid Date of Birth", "dd-MMM-yyyy"))
                return false;
            if (!isSelected("<%=ddlMaritalStatus.ClientID%>", "Marital Status"))
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




            if (!isSelected("<%=ddlGender.ClientID%>", "Gender"))
                return false;
            if (!isSelected("<%=ddlIsHandi.ClientID%>", "Is Handicapped"))
                return false;
            if (!isSelected("<%=ddlIsExService.ClientID%>", "Is Ex-Serviceman"))
                return false;



            // amit_apaar_api_changes_may_2026_start

            if (!isBlank("<%=txtapaar.ClientID%>", "Apaar ID"))
                return false;

            if (!isNumber("<%=txtapaar.ClientID%>", "Apaar ID"))
                return false;

            if (!IsValidMinLength("<%=txtapaar.ClientID%>", "Apaar ID", 12))
                return false;

            if (!isSelected("<%=ddlConsentRelation.ClientID%>", "Consent Relation"))
                return false;

            if (!isSelected("<%=ddlAuthMode.ClientID%>", "Authentication Mode"))
                return false;

            if (!isBlank("<%=txtAuthenticationIdNo.ClientID%>", "Authentication ID No"))
                return false;

            if (!IsValidMinLength("<%=txtAuthenticationIdNo.ClientID%>", "Authentication ID No", 6))
                return false;

            function IsValidMinLength(ctrl, msg, minLen) {
                if (document.getElementById(ctrl)) {
                    var value = trim(document.getElementById(ctrl).value, "");
                    var upperValue = value.toUpperCase();

                    if (value.length < minLen) {
                        CallDiv(ctrl, msg + " must contain " + minLen + " characters");
                        document.getElementById(ctrl).focus();
                        return false;
                    }
                }
                return true;
            }


            if (!isBlank("<%=txtConsentPlace.ClientID%>", "Consent Place"))
                return false;

            if (!ischecked("<%=chkApaarDeclaration.ClientID%>", "Apaar Declaration"))
                return false;

            if (!ischecked("<%=chkDeclaration2.ClientID%>", "Final Declaration"))
                return false;


            // amit_apaar_api_changes_may_2026_end



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
                                <asp:HyperLinkField HeaderText="Effective Date" HeaderStyle-Width="15%" DataTextField="effdate"
                                    SortExpression="effdate" DataTextFormatString="{0:dd-MMM-yyyy}" DataNavigateUrlFields="ID,appid,Name"
                                    DataNavigateUrlFormatString="?key={0}&key1={1}&Name={2}">
                                    <HeaderStyle Width="15%" />
                                    <ItemStyle HorizontalAlign="Right" />
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
                    <%--      // amit_apaar_api_changes_may_2026_start--%>
                    <td align="center" colspan="3">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblerror" runat="server" EnableTheming="False" CssClass="error" Visible="False"
                                    Width="99%"></asp:Label>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                    <%--      // amit_apaar_api_changes_may_2026_end--%>
                </tr>
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
                            OnSelectedIndexChanged="ddlSalutation_SelectedIndexChanged">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem Value="1">Mr.</asp:ListItem>
                            <asp:ListItem Value="2">Ms.</asp:ListItem>
                            <asp:ListItem Value="3">Others</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtName" OnTextChanged="txtAppName_TextChanged" AutoPostBack="true" runat="server" MaxLength="60" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlGender" runat="server" Height="22" SkinID="ddl250" Width="102px">
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
                        <asp:Label ID="Label26" runat="server" SkinID="CaptionLabel" Text="Guardian Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
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
                        <asp:Label ID="Label21" runat="server" SkinID="CaptionLabel" Text="Marital Status &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label20" runat="server" SkinID="CaptionLabel" Text="Category&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                        <asp:TextBox ID="Txt_Dob" AutoPostBack="true" OnTextChanged="txtDob_TextChanged" runat="server" MaxLength="100" SkinID="txtDate"></asp:TextBox>
                        <img id="imgDob" runat="server" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                        <asp:CalendarExtender ID="ceDOB" TargetControlID="Txt_Dob" PopupPosition="BottomLeft"
                            Format="dd-MMM-yyyy" PopupButtonID="imgDob" runat="server">
                        </asp:CalendarExtender>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlMaritalStatus" runat="server" Height="22" SkinID="ddl250">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCategory" runat="server" EnableTheming="True" Height="22"
                            SkinID="ddl250" TabIndex="8">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label28" runat="server" SkinID="CaptionLabel"
                            Text="Is Handicapped &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label29" runat="server" SkinID="CaptionLabel"
                            Text="Is Ex-Serviceman&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label27" runat="server" SkinID="CaptionLabel"
                            Text="Effective Date"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                        <asp:DropDownList ID="ddlIsHandi" runat="server" Height="22" SkinID="ddl250"
                            Width="102px">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem Value="1">No</asp:ListItem>
                            <asp:ListItem Value="2">Yes</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:DropDownList ID="ddlIsExService" runat="server" Height="22"
                            SkinID="ddl250" Width="102px">
                            <asp:ListItem Value="0">--Select One--</asp:ListItem>
                            <asp:ListItem Value="1">No</asp:ListItem>
                            <asp:ListItem Value="2">Yes</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:TextBox ID="Txt_EffDate" runat="server" MaxLength="100" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>




                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="LabelAadharNo" runat="server" SkinID="CaptionLabel"
                            Text="Aadhar Card No"></asp:Label>
                    </td>
                    <%--// amit_apaar_api_changes_may_2026_start--%>
                    <td style="width: 33%;" valign="top">

                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel"
                            Text="Apaar No"></asp:Label>

                        <a href="https://www.abc.gov.in/" target="_blank">Generate Apaar ID</a>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel"
                            Text="Is Provider Present"></asp:Label>
                    </td>

                </tr>

                <tr class="even">
                    <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                        <asp:TextBox ID="TextBoxAadharNo" runat="server" MaxLength="12" SkinID="txt248"
                            onkeypress="checkNumber(this,15,0,event);">
                        </asp:TextBox>
                    </td>
                    <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                        <asp:TextBox ID="txtapaar" runat="server" MaxLength="12" SkinID="txt248" OnTextChanged="txtapaar_TextChanged" AutoPostBack="true"
                            onkeypress="checkNumber(this,15,0,event);">
                        </asp:TextBox>
                    </td>
                    <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                        <asp:TextBox ID="txtIsProviderPresent" runat="server" SkinID="txt248"
                            Text="True" Enabled="false">
                        </asp:TextBox>
                    </td>
                </tr>

                <%-- row 2 --%>


                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel"
                            Text="Consent Relation"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel"
                            Text="Provider Name"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label5" runat="server" SkinID="CaptionLabel"
                            Text="Auth Mode"></asp:Label>
                    </td>

                </tr>

                <tr class="even">
                    <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                        <asp:DropDownList ID="ddlConsentRelation" runat="server"
                            Width="100px" AutoPostBack="true" SkinID="ddl250"
                            OnSelectedIndexChanged="ddlConsentRelation_SelectedIndexChanged">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Self" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Guardian" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Father" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Mother" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                        <asp:TextBox ID="txtproviderName" SkinID="txt248" runat="server"
                            MaxLength="100">
                        </asp:TextBox>

                    </td>
                    <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                        <asp:DropDownList ID="ddlAuthMode" runat="server" SkinID="ddl250"
                            AutoPostBack="true" OnSelectedIndexChanged="ddlAuthMode_SelectedIndexChanged">
                            <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <asp:Label ID="lblauthidrules" Text="" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>


                <%-- row 3 --%>


                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel"
                            Text="Auth Id No"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel"
                            Text="Consent Date and Time"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel"
                            Text="Consent Place"></asp:Label>
                    </td>

                </tr>

                <tr class="even">
                    <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ID="updAuth">
                            <ContentTemplate>
                                <asp:TextBox ID="txtAuthenticationIdNo" runat="server"
                                    MaxLength="50" SkinID="txt248" Placeholder="Enter here..."
                                    onkeypress="return isNumberKey(event);"
                                    onpaste="return false;" oncopy="return false;" oncut="return false;">
                                </asp:TextBox>
                                <br />
                                <small runat="server" id="authidrule">Rule for entering Authentication ID</small>

                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlAuthMode" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                        <asp:TextBox ID="txtConsentDate" runat="server"
                            Width="90px" Enabled="false">
                        </asp:TextBox>
                        <asp:TextBox ID="txtConsentTime" runat="server"
                            Width="90px" Enabled="false">
                        </asp:TextBox>
                    </td>
                    <td style="width: 33%; border: 1px solid #A8A8A8;" valign="top">
                        <asp:TextBox ID="txtConsentPlace" Placeholder="Enter here..." SkinID="txt248" MaxLength="30" onpaste="return false" runat="server">
                        </asp:TextBox>
                        <br />
                        <small>Maximum 30 characters. Only alphabets and hyphens (-) are allowed. Do not enter a full address.
                        </small>
                    </td>
                </tr>


                <tr>
                    <td id="tdApaarDeclaration" runat="server" colspan="3">
                        <div class="declaration-box">

                            <asp:CheckBox ID="chkApaarDeclaration"
                                ClientIDMode="Static"
                                runat="server" />

                            <span class="declaration-text">
                                <span class="required-star">*</span>
                                <span class="declaration-title">APAAR Consent:</span>

                                <asp:Label ID="lblApaarDeclaration"
                                    runat="server"
                                    Text=" I, hereby voluntarily give my consent to NIELIT to use APAAR ID of [Applicant Name] with APAAR ID as for validation of personal details. I understand that the APAAR ID may be used and shared only for limited, authorized purposes, and that the information provided by me shall be kept confidential. The information w.r.t authentication document no. provided by me, is correct and valid to the best of my knowledge.">
            </asp:Label>
                            </span>

                        </div>
                    </td>
                </tr>
                <%--      // amit_apaar_api_changes_may_2026_end--%>
            </table>

            <div class="declaration-box">

                <asp:CheckBox ID="chkDeclaration2"
                    runat="server" />

                <span class="required-star">*</span>

                <span class="declaration-text">
                    <span class="declaration-title">Declaration:</span>
                  Please ensure that the consent is available with the registration section before submitting the form for updation.
                 </span>
            </div>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return Validate();" runat="server" Text="Save"
                    OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </div>

            <br />
            <br />
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
