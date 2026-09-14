<%@ Page Language="C#" AutoEventWireup="true" CodeFile="projectSubCentre.aspx.cs"
    Inherits="Admin_projectSubCentre" MasterPageFile="~/MasterPages/main.master"
    Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="NIELIT Projects Sub Centre"></asp:Label>
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
                                        <asp:Label ID="Label1" Width="100%" runat="server" Text="Project Name"></asp:Label>
                                        <asp:DropDownList ID="ddlprojectName" Width="100%" runat="server"  >
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label3" Width="100%" runat="server" Text="Budget Allocated"></asp:Label>

                                    </td>
                                </tr>
                                <tr>
                                    <td>
<asp:TextBox ID="txtBudgetAllocatedSearch" runat="server"  onkeypress="return numeric(event)" onpaste="return false;" Width="95%" MaxLength="9" BorderStyle="Double">0</asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>

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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Project Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {
           
            if (!isvalidateRadioButtonList("RdoAffInstOrNonAffInst", "Choose  Institute"))
                return false;

            var list = document.getElementById("RdoAffInstOrNonAffInst");
            var listItemArray = list.getElementsByTagName("input");
            var Itemvalue = "";
            for (var i = 0; i < listItemArray.length; i++) {
                var listItem = listItemArray[i];
                if (listItem.checked) {
                    Itemvalue = listItem.value;
                }
            }
            if (!isSelected("<%=ddlProjName.ClientID  %>", "Project Name"))
                return false;
            if (!isSelected("<%=ddlcentreName.ClientID  %>", "Centre Name"))
                return false;
            if (!isBlankDate("<%=txtallocatedFromDate.ClientID %>", "Allocated From Date", "dd-MMM-yyyy"))
                return false;
            if (!isBlankDate("<%=txtallocatedTodate.ClientID %>", "Allocated To Date", "dd-MMM-yyyy"))
                return false;
           
            if (!isBlank("<%=txtbudgetAllocated.ClientID  %>", "Budget Allocated"))
                return false;
            if (!isSelected("<%=ddlwhetherAffiliated.ClientID  %>", "WhetherAccredited"))
                return false;

            if (!isDate("txtallocatedFromDate", "Invalid date", "dd-MMM-yyyy"))
                return false;
            if (!isDate("txtallocatedTodate", "Invalid date", "dd-MMM-yyyy"))
                return false;           

            if (!isNumber("txtbudgetAllocated"))
                return false;

            var frdate = document.getElementById("<%=txtallocatedFromDate.ClientID %>").value;
            var todate = document.getElementById("<%=txtallocatedTodate.ClientID %>").value;
            
            if (!CompareDates(frdate, todate, "From date should be less then To date", true))
                return false;

            }


        var dtgp = "<%= gvMain.ClientID %>"
        
            function CheckAll(Sender, CheckBoxName) {
                CheckUncheckAll(dtgp, Sender, CheckBoxName)
            }

            function PerformAction(obj, tableid) {
             document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
             ShowHideMenu(obj, tableid);
            }
        var dtgp = "<%= gvMainNonAflInt.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }


    </script>

     <script type="text/javascript" language="javascript">
         function numeric(evt) {
             var charCode = (evt.which) ? evt.which : event.keyCode
             if (charCode > 31 && ((charCode >= 48 && charCode <= 57) || charCode == 46))
                 return true;
             else {
                 alert('Please Enter Numeric values.');
                 return false;
             }
         }
    </script>
 
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">
                <br />
                <asp:Label ID="lblAffiliatedInst"  runat="server" ForeColor="blue" Font-Bold="true"
                      Text="  Projectwise Accredited Centres List" Visible="true"   ></asp:Label>
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <table id="popup" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 132px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lbDelteteOne" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                        runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                        OnClick="PerformPopupAction"></asp:LinkButton>                               
                                </td>
                            </tr>
                        </table>
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblErrMsg" Visible="false"
                runat="server"></asp:Label>
                    <br />
                        <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="30%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="projectName" 
                                    HeaderText="Project Name" SortExpression="projectName" Target="_self">
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="20%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="centreName"
                                    HeaderText="Centre Name" SortExpression="centreName" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>

                               

                            <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="budgetAllocated"
                                    HeaderText="Budget Allocated" SortExpression="budgetAllocated" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="allocatedFromDate" 
                                    HeaderText="Allocated From Date" SortExpression="allocatedFromDate" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="allocatedTodate"
                                    HeaderText="Allocated To date" SortExpression="allocatedTodate" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
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
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" /></ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfcode" runat="server" />
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
            <%--deep--%>
             
            <div id="div1" runat="server">
                <br /><br />
                <asp:Label ID="Label2"  runat="server" ForeColor="blue" Font-Bold="true"
                      Text="  Projectwise Non Accredited Institute List " Visible="true"   ></asp:Label>
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <table id="Table1" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 132px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lbDelteteOne1" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                        runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"
                                       OnClick="PerformPopupAction" ></asp:LinkButton>                               
                                </td>
                            </tr>
                        </table>
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblErrMsg1" Visible="false"
                runat="server"></asp:Label>
                    <br />
                        <asp:GridView ID="gvMainNonAflInt" runat="server" DataKeyNames="ID" OnSorting="gvMainNonAflInt_Sorting"
                            OnRowDataBound="gvMainNonAflInt_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="30%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="projectName" 
                                    HeaderText="Project Name" SortExpression="projectName" Target="_self">
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="20%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="centreName"
                                    HeaderText="Centre Name" SortExpression="centreName" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="budgetAllocated"
                                    HeaderText="Budget Allocated" SortExpression="budgetAllocated" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="allocatedFromDate" 
                                    HeaderText="Allocated From Date" SortExpression="allocatedFromDate" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="allocatedTodate"
                                    HeaderText="Allocated To date" SortExpression="allocatedTodate" Target="_self" DataTextFormatString="{0:dd-MMM-yyyy}">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:TemplateField HeaderStyle-Width="2%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction1(this,'popup')"
                                            runat="server" ID="imgAction1" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" /></ItemTemplate>
                                    <HeaderStyle Width="2%" />
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
                        <asp:HiddenField ID="hfActionID1" runat="server" Value="" />
                        <asp:HiddenField ID="hfcode1" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="divNavigation1" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation1" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar2" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <%--deep--%>
        </asp:View>
        <asp:View ID="New" runat="server">
             <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                            <ContentTemplate>
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
               <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Choose  Institute  &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                   <td style="width: 66%;" valign="top" colspan="2">
                       <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                       <asp:RadioButtonList ID="RdoAffInstOrNonAffInst" runat="server" RepeatDirection="Horizontal"
                                    TabIndex="2" Width="412px" AutoPostBack="True" OnSelectedIndexChanged="RdoAffInstOrNonAffInst_SelectedIndexChanged"
                                    Style="height: 27px" Font-Bold="True">
                                    <asp:ListItem Value="1" Selected="True">Accredited Centres</asp:ListItem>
                                    <asp:ListItem Value="0">Non Accredited Institute</asp:ListItem>
                                </asp:RadioButtonList>
                                </ContentTemplate>
                           <Triggers>
          <asp:AsyncPostBackTrigger ControlID="RdoAffInstOrNonAffInst" EventName="SelectedIndexChanged" />
                                            </Triggers>
                    </asp:UpdatePanel>
                    </td>
                   
                </tr>
                <tr>
                    <td style="width: 33%;" valign="top">
                       <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Project Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Centre Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label11" runat="server" SkinID="CaptionLabel" Text="Budget Allocated &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>                       
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">                       
                         <asp:DropDownList ID="ddlProjName" Width="100%" runat="server"  >
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>                               
                                <asp:DropDownList ID="ddlcentreName" Width="100%" runat="server"  >
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>  
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                    <td style="width: 33%;" valign="top">                       
                     <asp:TextBox ID="txtbudgetAllocated" runat="server" SkinID="txt248" onkeypress="return numeric(event)" onpaste="return false;" Width="200px" MaxLength="9"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td style="width: 33%;" valign="top">
                       <asp:Label ID="Label9" runat="server" SkinID="CaptionLabel" Text="Allocated From Date &lt;b class='mandatory'&gt;*&lt;/b&gt;" ></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                       <asp:Label ID="Label10" runat="server" SkinID="CaptionLabel" Text="Allocated To Date &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                         <asp:Label ID="Label12" runat="server" SkinID="CaptionLabel" Text="Whether Accredited &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                          <asp:TextBox ID="txtallocatedFromDate" runat="server" MaxLength="11" SkinID="txtDate" onpaste="return false;" Width="200px"></asp:TextBox>
                         <img id="img2" runat="server" src="../images/calendaricon.jpg" alt="Calandar" style="width: 20px;
                                    height: 22px; vertical-align: top;" />
                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtallocatedFromDate">
                        </asp:CalendarExtender>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtallocatedTodate" runat="server" MaxLength="11" SkinID="txtDate" onpaste="return false;" Width="200px"></asp:TextBox>
                        <img id="img3"  runat="server" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" />
                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txtallocatedTodate">
                        </asp:CalendarExtender>
                        
                    </td>
                    <td style="width: 33%;" valign="top">  
                         <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                        <asp:DropDownList ID="ddlwhetherAffiliated" runat="server" SkinID="ddl250" Enabled="false">
                            <asp:ListItem>---Select One---</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="2" >No</asp:ListItem>
                        </asp:DropDownList>
                                </ContentTemplate>
                    </asp:UpdatePanel>
                    </td>
                </tr>               
                <tr>
                     <td colspan="3">
                         <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblerror" runat="server" EnableTheming="False" CssClass="error" Visible="False"
                                Width="99%"></asp:Label>
                             <asp:HiddenField ID="hddnielitId" runat="server" />
                        </ContentTemplate>
                    </asp:UpdatePanel>

                    </td>
                </tr>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" /></div>
                                                        </ContentTemplate>
                    </asp:UpdatePanel>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="cthRightPannel" runat="Server">
     <table runat="server" visible="false" align="center" class="nav" cellspacing="0"
        cellpadding="0" id="tblNavLinks" width="97%">
        <tr>
            <td>   
            </td>
        </tr>
    </table>
</asp:Content>
