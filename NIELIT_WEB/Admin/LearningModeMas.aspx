<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" 
    AutoEventWireup="true" CodeFile="LearningModeMas.aspx.cs" Inherits="Admin_LearningModeMas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Learning Mode Master"></asp:Label>
</asp:content>
<asp:content id="Content3" contentplaceholderid="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />
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
                                        <asp:Label ID="lblFiler1" Width="100%" runat="server" Text="Learning Mode Master" Height="19px"></asp:Label>
                                        <asp:DropDownList ID="ddlLearningName" Width="100%" runat="server">
                                            <asp:ListItem Value ="0">--Select One--</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblFiler2" Width="100%" runat="server" Text="NIELIT Sectors Master " Height="19px"></asp:Label>
                                        <asp:DropDownList ID="ddlCode" Width="100%" runat="server">
                                            <asp:ListItem Value ="0">--Select One--</asp:ListItem>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Learning Mode Code or Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateLogin() {
            if (!isBlank("<%=txtLearningCode.ClientID %>", "Learning Mode Code"))
                return false;

            if (!isBlank("<%=txtLearningCode.ClientID %>", "Learning Mode Code"))
                return false;

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
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">                                  
                                    <HeaderStyle Width="2%" />                                    
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                
                                <%--<asp:TemplateField  HeaderText="Name">
                                    <ItemTemplate >
                                        <%#Eval("StatusName")%>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField  HeaderText="Description">
                                    <ItemTemplate >
                                        <%#Eval("StatusDesc")%>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>

                                <asp:HyperLinkField  DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="LearningCode" HeaderText="Learning Mode Code" SortExpression="Code"
                                    Target="_self" />

                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="LearningName" HeaderText="Learning Mode Name" SortExpression="Name"
                                    Target="_self" />

                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="LearningDescription" HeaderText="Learning Mode Description" SortExpression="Description"
                                    Target="_self" />

                                <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" /></ItemTemplate>
                                    <HeaderStyle Width="2%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="CheckBox1" SkinID="CheckAllInGridView" /></ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
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
            <table cellpadding="4" border="0" width="100%" class="sample3">
                                <tr class="head1">
                                    <td colspan="2" style="text-align:center; font-style:oblique;font-size:20px">
                                       Learning Mode Master
                                    </td>
                                </tr>
                                <%--<tr>
                                    <td colspan="3" align="left">
                                        <asp:Label ID="lblError" runat="server" Text="Please Enter Group Master (For Nielit Sectors)"
                                            CssClass="error" EnableTheming="false" Width="100%" Style="padding-left: 1px;
                                            margin-left: -1px;"></asp:Label>
                                    </td>
                                </tr>--%>
                                <tr class="gdalternate1">
                                    <td colspan="2">
                                        <asp:Label ID="lbl1" Width="70%" runat="server" ForeColor="Red"></asp:Label><a style="float: right;"></a>

                                    </td>
                                </tr>

                                <tr class="gdrow1">
                                    <td class="style2">
                                        <asp:Label ID="lblLearningCode" runat="server" SkinID="CaptionLabel" Text="Learning Mode Code &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                    </td>
                                    <td style="width: 85%">
                                        <asp:TextBox ID="txtLearningCode" runat="server"  MaxLength="10" Height="22px" Width="210px"></asp:TextBox>
                                        
                                        <%--<span style="text-align: left; vertical-align: top; font-size: 8pt;">(Select Current
                                            Course in which you are Registered.)</span>--%>
                                    </td>
                                </tr>

                                <tr class="gdrow1">
                                    <td class="style2">
                                        <asp:Label ID="lblLearningName" runat="server" SkinID="CaptionLabel" Text="Learning Mode Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                    </td>
                                    <td style="width: 85%">
                                        <asp:TextBox ID="txtLearningName" runat="server"  MaxLength="50" Height="22px" Width="210px"></asp:TextBox>
                                        
                                        <%--<span style="text-align: left; vertical-align: top; font-size: 8pt;">(Select Current
                                            Course in which you are Registered.)</span>--%>
                                    </td>
                                </tr>
                                <%--<tr class="gdalternate1">
                                    <td class="style2">
                                        <asp:Label ID="lblAuthSig" runat="server" SkinID="CaptionLabel" Text="Authorization Signature &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                    </td>
                                    <td style="width: 85%">
                                        <asp:TextBox ID="txtAuthsig" MaxLength="60" Width="200px" runat="server" onpaste="return false;"></asp:TextBox>
                                        <%--<span style="text-align: left; vertical-align: top; font-size: 8pt;">(Registration Number
                                            of above selected course.)</span>
                                    </td>
                                </tr>--%>

                                <tr class="gdrow1">
                                    <td class="style2">
                                        <asp:Label ID="lblDesc" runat="server" SkinID="CaptionLabel" Text="Learning Mode Description"></asp:Label>
                                    </td>
                                    <td style="width: 85%">
                                        <asp:TextBox ID="txtDesc" runat="server"  MaxLength="100" Height="22px" Width="210px"></asp:TextBox>
                                        
                                        <%--<span style="text-align: left; vertical-align: top; font-size: 8pt;">(Select Current
                                            Course in which you are Registered.)</span>--%>
                                    </td>
                                </tr>
                                
                                <tr class="gdalternate1">
                                    <td valign="top" class="style2">
                                    </td>
                                    <td align="left" valign="top" style="padding: 0">
                                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <table cellpadding="0" cellspacing="0" style="margin: 0">
                                                    <tr>
                                                        <td width="42%" align="left">
                                                            
                                                        </td>
                                                        <td width="58%">
                                                            <%--<asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                                
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSubmit" runat="server" Text="Save" OnClick="btnSubmit_Click" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />

                <div runat="server" id="MainReportDiv">
                </div>
                <%--<asp:Button ID="btnSave" OnClientClick="return ValidateLogin();" runat="server" Text="Save"
                    OnClick="SaveRecord" />--%>
                <%--<asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />--%>
        </asp:View>
    </asp:MultiView>
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphNavigation" runat="Server">
    
</asp:content>
<asp:content id="Content7" contentplaceholderid="cthRightPannel" runat="Server">

    <table runat="server" visible="false" align="center" class="nav" cellspacing="0"
        cellpadding="0" id="tblNavLinks" width="97%">
        <tr>
            <td>
                <asp:HyperLink ID="hlExamMenu" runat="server" Target="_self">Learning Mode Master</asp:HyperLink>
            </td>
        </tr>
    </table>

</asp:content>
