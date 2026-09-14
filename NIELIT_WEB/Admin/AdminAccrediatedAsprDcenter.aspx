<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="AdminAccrediatedAsprDcenter.aspx.cs" Inherits="Admin_AdminAccrediatedAsprDcenter"
    Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/Address.ascx" TagName="Address" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Institute Accredited For Aspirational District"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />
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
                                        <asp:Label ID="lblUserType" Width="100%" runat="server" Text="States"></asp:Label>
                                        <asp:DropDownList ID="ddlAccentre" Width="100%" runat="server" AutoPostBack="True">
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Accredited Centre"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc5:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {

            if (!isBlank("<%=txtaccentre.ClientID %>", "Institute Name"))
                return false;
            if (!isBlankNumber("<%=txtmobile.ClientID %>", "Mobile Number"))
                return false;
            if (!isNumber("<%=txtmobile.ClientID %>"))
                return false;
            if (!chekMobNo("<%=txtmobile.ClientID %>"))
                return false;

            if (!isBlank("<%=txtadd1.ClientID %>", "Address Line-1")) 
                return false;
            if (!isBlank("<%=txtadd2.ClientID %>", "Address Line-2"))
                return false;
            if (!isBlank("<%=txtcity.ClientID %>", "City Name"))
                return false;
            if (!isSelected("<%=ddlstate.ClientID %>", "State Name"))
                return false;
            if (!isSelected("<%=ddldistrict.ClientID %>", "District Name"))
                return false;
				 //Added for city type
            if (!isSelected("<%=ddlCityType.ClientID %>", "City Type"))
                return false;

            if (!isBlank("<%=txtemail1.ClientID %>", "Email Address-1")) 
            return false;
            //

            // --------------------------------------------------------------------
            var p1;
            p1 = document.getElementById("<%=txtphone1.ClientID %>").value;
            if (p1 != "") {

                if (!isBlankNumber("<%=txtstdno.ClientID %>", "STD Number"))
                    return false;
                if (!isNumber("<%=txtstdno.ClientID %>"))
                    return false;
                if (!isNumber("<%=txtphone1.ClientID %>"))
                    return false;
                if (!isValidTeliphone("<%=txtstdno.ClientID %>", "<%=txtphone1.ClientID %>"))
                    return false;
            }
            var phoneno2;
            phoneno2 = document.getElementById("<%=txtphone2.ClientID %>").value;
            if (phoneno2 != "") {

                if (!isBlankNumber("<%=txtstdno.ClientID %>", "STD Number"))
                    return false;
                if (!isNumber("<%=txtstdno.ClientID %>"))
                    return false;
                if (!isNumber("<%=txtphone2.ClientID %>"))
                    return false;

                if (!isValidTeliphone("<%=txtstdno.ClientID %>", "<%=txtphone2.ClientID %>"))
                    return false;
            }
            var fx;
            fx = document.getElementById("<%=txtfaxno.ClientID %>").value;
            if (fx != "") {

                if (!isBlankNumber("<%=txtstdno.ClientID %>", "STD Number"))
                    return false;
                if (!isNumber("<%=txtstdno.ClientID %>"))
                    return false;
                if (!isNumber("<%=txtfaxno.ClientID %>"))
                    return false;

                if (!isValidTeliphone("<%=txtstdno.ClientID %>", "<%=txtfaxno.ClientID %>"))
                    return false;
            }
            // ----------------------------------------------------------------------

            if (!isBlankNumber("<%=txtpinno.ClientID %>", "Pin Code"))
                return false;
            if (!isNumber("<%=txtpinno.ClientID %>"))
                return false;
            if (!IsValidMinMaxLenght("<%=txtpinno.ClientID %>", 6, 6, "Invalid Pin Code"))
                return false;
            // ----------------------------------------------------------------------
            if (!isSelected("<%=ddlstate.ClientID %>", "State"))
                return false;
            if (!isSelected("<%=ddldistrict.ClientID %>", "District"))
                return false;

            // ----------------------------------------------------------------------
            var e1;
            e1 = document.getElementById("<%=txtemail1.ClientID %>").value;
            if (e1 != "") {
                if (!isValidEmail("<%=txtemail1.ClientID %>", "Not A Valid Email Address"))
                    return false;
            }
            var e2;
            e2 = document.getElementById("<%=txtemail2.ClientID %>").value;
            if (e2 != "") {
                if (!isValidEmail("<%=txtemail2.ClientID %>", "Not A Valid Email Address"))
                    return false;
            }
            if (!IsValidMinMaxLenght("txtpinno", 6, 6, "Invalid Pin Code"))
                return false;
        }

        function CheckSelectedDept() {

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
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server"></asp:Label>
                        <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 100px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                        runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                        OnClick="PerformPopupAction"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="60%" DataNavigateUrlFields="ID,Name" DataNavigateUrlFormatString="?Key={0}&Name={1}"
                                    DataTextField="Name" HeaderText="Accredited Centre" SortExpression="Name" Target="_self">
                                    <HeaderStyle Width="60%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="20%" DataNavigateUrlFields="ID,Name" DataNavigateUrlFormatString="?Key={0}&Name={1}"
                                    DataTextField="Location" HeaderText="Location" SortExpression="Location" Target="_self">
                                    <HeaderStyle Width="20%" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID,Name" DataNavigateUrlFormatString="?Key={0}&Name={1}"
                                    DataTextField="AccreditationNumber" HeaderText="ACCR Number" SortExpression="AccreditationNumber"
                                    Target="_self">
                                    <HeaderStyle Width="15%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:HyperLinkField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" /></ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfAccID" runat="server" />
                        <asp:HiddenField ID="hfName" runat="server" />
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
            <table class="sample2" cellpadding="0" cellspacing="1" width="100%">
               
                  <tr>
                    <td style="width: 33%;" valign="top">

                        <asp:Label ID="Label23" runat="server" SkinID="CaptionLabel" Text="Already Accredited Institute  &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top" colspan="2">
                       
                                <asp:RadioButtonList ID="RdoYesNo" runat="server" RepeatDirection="Horizontal"
                                    TabIndex="2" Width="312px" AutoPostBack="True" OnSelectedIndexChanged="RdoYesNo_SelectedIndexChanged" Enabled="false"
                                    Style="height:27px "  Font-Bold="True">
                                    <asp:ListItem Value="1">YES</asp:ListItem>
                                    <asp:ListItem Value="0">NO</asp:ListItem> 
                                    
                                </asp:RadioButtonList>
                            
                    </td>
                </tr>
                
                <tr id="YesNo" runat="server" visible="false">
                    <td valign="top">
                        <asp:Label ID="Label22" runat="server" SkinID="CaptionLabel" Text="Enter Accr No &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAccrNo" runat="server" SkinID="txt248"></asp:TextBox>

                    </td>
                    <td>
                        <asp:Button ID="btnSearch"  runat="server" Text="Search" OnClick="SearchRecord" />

                    </td>
                </tr>
                 <tr  id="RowError19" runat="server" visible="false">
                    <td colspan="3">
                        <asp:Label ID="lblerrorAccr" runat="server" ForeColor="Red" Font-Size="Medium" Font-Italic="true" Width="63%"></asp:Label>
                    </td>
                </tr>
                 <tr class="heading" id="row18" runat="server">
                    <td colspan="3">
                        Institute Details
                    </td>
                </tr>
                <tr id="row1" runat="server">
                    <td>
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Institute ID &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td>
                         
                    </td>
                    <td valign="top">
                       
                    </td>
                    
                </tr>
                <tr class="even" id="row2" runat="server">
                     <td>
                          <asp:textbox id="txtInstituteID" runat="server" skinid="txt248" maxlength="10" Enabled="false" onkeypress="checkNumber(this,10,0,event)"></asp:TextBox>
                    </td>
                    <td>
                         
                    </td>
                    <td valign="top">
                      
                    </td>
                   
                </tr>
                <tr id="row3" runat="server">
                    <td valign="top" colspan="2">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Institute Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Contact Person"></asp:Label>
                    </td>
                </tr>
                <tr class="even" id="row4" runat="server">
                    <td colspan="2" valign="top">
                        <asp:TextBox ID="txtaccentre" runat="server" SkinID="txt502"></asp:TextBox>
                    </td>
                    <td valign="top">
                        <asp:TextBox ID="txtcontactperson" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>
                <tr id="row5" runat="server">
                    <td valign="top">
                        <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Contact Person Designation"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="STD Code"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Phone Number 1"></asp:Label>
                    </td>
                </tr>
                <tr class="even" id="row6" runat="server">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtdesignation" SkinID="txt248" runat="server"></asp:TextBox>
                    </td>
                    <td valign="top" style="height: 25%;">
                        <asp:TextBox ID="txtstdno" runat="server" SkinID="txt248" MaxLength="6" onkeypress="checkNumber(this,6,0,event)"></asp:TextBox>
                    </td>
                    <td style="height: 25%;" valign="top">
                        <asp:TextBox ID="txtphone1" SkinID="txt248" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr id="row7" runat="server">
                    <td valign="top">
                        <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Phone Number 2"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Fax Number"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Mobile Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even" id="row8" runat="server">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtphone2" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td valign="top" style="height: 25%;">
                        <asp:TextBox ID="txtfaxno" runat="server" SkinID="txt248" onkeypress="checkNumber(this,6,0,event)"></asp:TextBox>
                    </td>
                    <td style="height: 25%;" valign="top">
                        <asp:TextBox ID="txtmobile" SkinID="txt248" runat="server" MaxLength="10" onkeypress="checkNumber(this,10,0,event)"></asp:TextBox>
                    </td>
                </tr>
                <tr id="row9" runat="server">
                    <td valign="top">
                        <asp:Label ID="Label12" runat="server" SkinID="CaptionLabel" Text="Email Address-1 &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label13" runat="server" SkinID="CaptionLabel" Text="Email Address-2"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label14" runat="server" SkinID="CaptionLabel" Text="Web Address"></asp:Label>
                    </td>
                </tr>
                <tr class="even" id="row10" runat="server">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtemail1" SkinID="txt248" runat="server"></asp:TextBox>
                    </td>
                    <td valign="top" style="height: 25%;">
                        <asp:TextBox ID="txtemail2" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="height: 25%;" valign="top">
                        <asp:TextBox ID="txtwebaddress" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>
                <tr class="heading" id="row11" runat="server">
                    <td colspan="3">
                        Contact Details
                    </td>
                </tr>
                <tr id="row12" runat="server">
                    <td valign="top">
                        <asp:Label ID="Label15" runat="server" SkinID="CaptionLabel" Text="Address Line-1 &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label16" runat="server" SkinID="CaptionLabel" Text="Address Line-2 &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label17" runat="server" SkinID="CaptionLabel" Text="Address Line-3"></asp:Label>
                    </td>
                </tr >
                <tr class="even" id="row13" runat="server">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtadd1" SkinID="txt248" runat="server"></asp:TextBox>
                    </td>
                    <td valign="top" style="height: 25%;">
                        <asp:TextBox ID="txtadd2" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                    <td style="height: 25%;" valign="top">
                        <%--<asp:DropDownList ID="ddlcity" runat="server" SkinID="ddl250" 
                                    OnSelectedIndexChanged="ddlstate_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>--%>
                        <asp:TextBox ID="txtadd3" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
                </tr>
                <tr id="row14" runat="server">
                    <td valign="top">
                        <asp:Label ID="Label18" runat="server" SkinID="CaptionLabel" Text="City Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
					<td valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="City Type &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label19" runat="server" SkinID="CaptionLabel" Text="State &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                   
                </tr>
                <tr class="even" id="row15" runat="server">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtcity" runat="server" SkinID="txt248"></asp:TextBox>
                    </td>
					<td valign="top" style="height: 25%;">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlCityType" runat="server" SkinID="ddl250" 
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </ContentTemplate>
                           
                        </asp:UpdatePanel>
                    </td>
                    <td valign="top" style="height: 25%;">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlstate" runat="server" SkinID="ddl250" OnSelectedIndexChanged="ddlstate_SelectedIndexChanged"
                                    AutoPostBack="True">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlstate" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
					</tr>
					<tr id="row16" runat="server">
                    <td valign="top">
                        <asp:Label ID="Label20" runat="server" SkinID="CaptionLabel" Text="District &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="Label21" runat="server" SkinID="CaptionLabel" Text="Pin Number &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                    </td>
                    <td valign="top">
                    </td>
                </tr>
                <tr class="even" id="row17" runat="server">
                     <td style="height: 25%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddldistrict" runat="server" SkinID="ddl250" AutoPostBack="True">
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddldistrict" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtpinno" runat="server" SkinID="txt248" MaxLength="6" onkeypress="checkNumber(this,6,0,event)"></asp:TextBox>
                    </td>
                    <td valign="top" style="height: 25%;">
                    </td>
                    <td style="height: 25%;" valign="top">
                    </td>
                </tr>
            </table>
            <div  style="text-align: right; margin-top: 10px" id="btnview" runat="server">
                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
    <table runat="server" align="center" class="nav" cellspacing="0" cellpadding="0"
        id="tblNavLinks" width="97%" visible="false">
        <tr>
            <td style="border: 1px solid #2c5070; background-color: #FFFFFF;" width="100%">
                <%--  <a href="accrediationdetails.aspx?key1=<%=  hfAccID.Value %> &name=<%=  hfName.Value  %>" " target="_self">Accreditation Details</a>--%>
                <asp:HyperLink ID="hl1" runat="server" Target="_self">Accreditation Details</asp:HyperLink>
            </td>
        </tr>
    </table>
</asp:Content>
