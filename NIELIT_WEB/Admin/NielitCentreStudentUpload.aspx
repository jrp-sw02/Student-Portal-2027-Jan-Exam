<%@ Page Language="C#" AutoEventWireup="true" MaintainScrollPositionOnPostback="true" CodeFile="NielitCentreStudentUpload.aspx.cs"
    Inherits="Admin_NielitCentreStudentUpload" MasterPageFile="~/MasterPages/main.master" Debug="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:content id="Content3" contentplaceholderid="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Nielit Centre Student Upload"></asp:Label>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />  
     <script src="../Script/GlobalFunction.js" type="text/javascript"></script> 
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server"  visible="false"/>
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
                                        <asp:Label ID="Label13" Width="100%" runat="server" Text="Batch Name"></asp:Label>
                                        <asp:DropDownList ID="ddlName" Width="100%" runat="server"  >
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <caption>
                                    &lt;<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NielitCentreStudent.aspx.cs" Inherits="Admin_NielitCentreStudent" %>--%><%--<%@ Page Language="C#" AutoEventWireup="true" MaintainScrollPositionOnPostback="true"
    CodeFile="NielitCentreStudent.aspx.cs" Inherits="Admin_NielitCentreStudent"  Debug="true" %>--%><tr>
                                        <td><%--<asp:Label ID="Label7" Width="100%" runat="server" Text="Exam Centre Type"></asp:Label>
                                        <asp:DropDownList ID="ddlflexcentretype" Width="100%" runat="server" AutoPostBack="True">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>--%></td>
                                    </tr>
                                </caption>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:content>

<asp:content id="Content5" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>

<asp:content id="Content6" contentplaceholderid="cphContents" runat="Server">    
    <script type="text/javascript">
        function preventBack() { window.history.forward(); }
        setTimeout("preventBack()", 0);
        window.onunload = function () { null };
    </script>
    <script src="../Script/Date.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">

        function ValidateForm() {

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
            if (!isSelected("ddlCenter", "Center")) //jksah
                return false;
            if (!isSelected("ddlCourse", "Course"))
                return false;
            if (!isSelected("ddlBatch", "Batch"))
                return false;
        }

        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1]
            ShowHideMenu(obj, tableid);
        }

    </script> 
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" runat="server">                                    
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
                      <%--  <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="30%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="Name"
                                    HeaderText="Name" SortExpression="Name" Target="_self">
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="10%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="FatherName"
                                    HeaderText="Father Name" SortExpression="FatherName" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="25%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="MotherName"
                                    HeaderText="Mother Name" SortExpression="MotherName" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="GuardianName"
                                    HeaderText="Guardian Name" SortExpression="Course" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextFormatString="{0:dd-MMM-yyyy}" DataTextField="DateOfBirth"
                                    HeaderText="Date Of Birth" SortExpression="DateOfBirth" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="Course"
                                    HeaderText="Course" SortExpression="Batch" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="Batch"
                                    HeaderText="Batch" SortExpression="Batch" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
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
                        </asp:GridView>--%>
                      <%--  <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfFileName" runat="server" />--%>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            
        </asp:View>
        <asp:View ID="New" runat="server"> 
            <div style="text-align: center; margin-top: 10px" >     
                 <strong style="text-align: center" class="headfont">STUDENT DATA UPLOAD
                        <asp:Label ID="Lblhead" runat="server" Text=""></asp:Label></strong>

                 <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblerror" runat="server" EnableTheming="False" CssClass="error" Visible="False"
                                Width="99%"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
        <table class="sample2" cellpadding="2" cellspacing="0" width="100%"> 
            <%--<tr class="even" id="r1" runat="server">
                <td align="center" colspan="3">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                        <ContentTemplate>
                            <asp:Label ID="lblerror" runat="server" EnableTheming="False" CssClass="error" Visible="False"
                                Width="99%"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>--%>         
                     <tr  class="even" id="r2" runat="server">
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Choose  Institute  &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top" colspan="2">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <asp:RadioButtonList ID="RdoAffInstOrNonAffInst" runat="server" RepeatDirection="Horizontal"
                                    TabIndex="2" Width="512px" AutoPostBack="True" OnSelectedIndexChanged="RdoAffInstOrNonAffInst_SelectedIndexChanged"
                                    Style="height: 27px" Font-Bold="True">
                                    <asp:ListItem Value="1">Accredited Centres</asp:ListItem>
                                    <asp:ListItem Value="0">Non Accredited Institute</asp:ListItem>
                                    <asp:ListItem Value="2">NIELIT Centre</asp:ListItem>
                                </asp:RadioButtonList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="RdoAffInstOrNonAffInst" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                </tr>
             <tr id="r1" runat="server">
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Center Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label22" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label8" runat="server" SkinID="CaptionLabel" Text="Batch Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                        <tr class="even" id="r3" runat="server" >
                            <td valign="top" width="30%">                                
                                <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="ddlCenter" Width="100%" runat="server" AutoPostBack="True" SkinID="ddl250"  OnSelectedIndexChanged="ddlCenter_SelectedIndexChanged" >
                                            <asp:ListItem Value="0" Text="--Select One--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </td>
                            <td valign="top" width="30%">                                
                                <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>  
                                <asp:DropDownList ID="ddlCourse" runat="server" Width="100%" SkinID="ddl250" AutoPostBack="True" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged" TabIndex="1">
                                </asp:DropDownList>
                                </ContentTemplate>
                            <Triggers>  
                            </Triggers>
                        </asp:UpdatePanel>
                            </td>                            
                            <td valign="top" width="37%" >
                                <asp:UpdatePanel ID="UpdatePanel9" runat="server">
                            <ContentTemplate>  
                                <asp:DropDownList ID="ddlBatch" runat="server" Width="100%" SkinID="ddl250" TabIndex="1">
                                </asp:DropDownList>
                                </ContentTemplate>
                            <Triggers>  
                            </Triggers>
                        </asp:UpdatePanel>
                            </td>                        
                        </tr>
                        <tr class="even" id="r4" runat="server">
                            <td style="width: 40%;" valign="top">
                                <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Upload MS-Excel File (.xlsm) &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                <br />
                                 <asp:Label ID="lblmsg" ForeColor="Red" Font-Italic ="True" runat="server" Text="File must be .xlsm extension. "></asp:Label>
                                <asp:HyperLink ID="hylDownload" runat="server" NavigateUrl="~/download/UploadData.xlsx" Font-Italic ="true" ForeColor="Red">Download Format</asp:HyperLink>
                                </td>
                            <td style="width: 60%;" align="left" colspan="2" valign="middle">
                                <asp:FileUpload ID="fileUpload" runat ="server" />
                                <br />
                               <%-- <asp:Button ID="btnUpload" runat="server" Text="Upload file" TabIndex="53" OnClick="btnUpload_Click"
                                    OnClientClick="return ValidateForm();" />
                                <asp:Button ID="btnback" runat="server" Text="Reset" TabIndex="54" OnClick="btnback_Click" />--%>
                                <br /> <br />
                                <asp:Label ID="lblCount" runat="server" Text="Label" Visible ="false" Font-Bold ="true"></asp:Label>
                            </td>
                        </tr>                                
                    </table>                                    
               <div style="text-align: right; margin-top: 10px">
                   <asp:Button ID="btnUpload" runat="server" Text="Upload file" TabIndex="53" OnClick="btnUpload_Click"
                                    OnClientClick="return ValidateForm();" />
                                <asp:Button ID="btnback" runat="server" Text="Reset" TabIndex="54" OnClick="btnback_Click" />
                   </div>
    </div> 
           <asp:HiddenField ID="HiddenField2" runat="server" />   
             <div id="divValidateData" runat="server" visible="false" height="150px" width="600px"
        style="overflow: scroll;">
        <table class="sample3" id="tblValidateData" style="width: 100%; text-align: left"
            border="0" cellpadding="3" cellspacing="1">
            <tr class="head1">
                <td align="left" colspan="2">
                    Validate Data
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="20%">
                    Total Records
                </td>
                <td width="80%">
                    <asp:Label ID="lblTotalRecords" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdalternate1">
                <td width="40%">
                    Successful Records
                </td>
                <td>
                    <asp:Label ID="lblValidateRecords" runat="server"></asp:Label>
                </td>
            </tr>
            <tr class="gdrow1">
                <td width="40%" valign="top">
                    Failed Records
                </td>
                <td>
                    <asp:Label ID="lblFailedRecords" runat="server"></asp:Label>
                </td>
            </tr>
             <tr class="head1">
                <td align="left" colspan="2">
                  Details of Failed Records !!
                </td>
            </tr>
             <tr class="gdalternate1">
                <td width="40%" valign="top" colspan="2">
                    <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                     <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:HyperLinkField HeaderStyle-Width="7%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="ID"
                                    HeaderText="Sr. No." SortExpression="Sr. No." Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                    </asp:HyperLinkField>
                                <asp:HyperLinkField HeaderStyle-Width="15%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="Name"
                                    HeaderText="Name" SortExpression="Name" Target="_self">
                                </asp:HyperLinkField>
                                 
                                <asp:HyperLinkField HeaderStyle-Width="30%" DataNavigateUrlFields="ID"
                                    DataNavigateUrlFormatString="?Key={0}" DataTextField="Remarks"
                                    HeaderText="Remarks" SortExpression="Remarks" Target="_self">
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:HyperLinkField>                                                       
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                          <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfFileName" runat="server" />
                        </ContentTemplate>
                        </asp:UpdatePanel>
                    <div id="divNavigation" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
                    <br />  
                     <div style="text-align: right; margin-top: 10px">
                   <asp:Button ID="Button1" runat="server"  
            Text="Export To PDF" OnClick="Button1_Click" /> 
                         </div>
                </td>
                 </tr>
            <tr class="gdrow1">
                <td width="40%" colspan="2">
                    <asp:Label ID="Label12" runat="server" Text="The unsaved records may be due to data discrepancies or duplicacy."
                        ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
            <div style="text-align: right; margin-top: 10px">
                 <asp:Button ID="btnCancel" runat="server" Text="Reset" OnClick="btnCancel_Click" visible="false" />
                </div>
        </asp:View>
    </asp:MultiView>
</asp:content>
<asp:content id="Content7" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>
<asp:content id="Content8" contentplaceholderid="cthRightPannel" runat="Server">
     <table runat="server" visible="false" align="center" class="nav" cellspacing="0"
        cellpadding="0" id="tblNavLinks" width="97%">
        <tr><td></td></tr>
    </table>
</asp:content>