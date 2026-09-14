<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="TargetDetails.aspx.cs" Debug="False" Inherits="Admin_TargetDetails" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc4" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">

        .table-containe r {
            overflow-x: auto;
            width: 800px;
            max-width: 100%;
            height: auto;
        }

        .table-container table {
            border-collapse: collapse;
        }

        .table-container th {
            font-family: Arial, sans-serif;
            font-size: 12px;
            /*font-weight: bold;*/
            background: linear-gradient(to bottom, #E0F0FC, #6b849a);
        }

        .table-container tr {
            font-family: Arial, sans-serif;
            font-size: 12px;
            border: none !important;
        }

        .table-container td, .table-container th {
            /*white-space: nowrap;*/
            padding: 5px;
            max-width: 300px;
        }

            .table-container td a,
            .table-container th a {
                text-decoration: none !important;
                color: black !important;
                font-weight: inherit;
            }

        .table-container tr:hover {
            text-decoration: underline !important;
        }


        .my-grid td:nth-child(2) {
            max-width: 300px !important;
            white-space: wrap !important;
        }

        .my-grid td:nth-child(4) {
            max-width: 500px !important;
            white-space: wrap !important;
        }

        .my-grid tr:nth-child(even) {
            background-color: #E6F0F0;
        }

        .my-grid tr:nth-child(odd) {
            background-color: #c9d7e2;
            transition: background-color 0.2s ease;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Target Details 📝"></asp:Label>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />
    <asp:Panel runat="server" ID="pnlFilter">
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
                                        <asp:Label ID="lblfiltercourse" Width="100%" runat="server" Text="Course Name"></asp:Label>
                                        <asp:DropDownList ID="ddlcoursefilter" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblcentrefilter" Width="100%" runat="server" Text="Centre Name"></asp:Label>
                                        <asp:DropDownList ID="ddlcentrefilter" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblfreqfilter" Width="100%" runat="server" Text="Select Frequency"></asp:Label>
                                        <asp:DropDownList ID="ddlfreqfilter" Width="100%" runat="server">
                                            <asp:ListItem Value="0" Text="--All--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblfilterefffrom" Width="100%" runat="server" Text="Effective from Date"></asp:Label>
                                        <asp:TextBox ID="txtfilterefffrom" runat="server" MaxLength="11"></asp:TextBox>
                                        <asp:CalendarExtender ID="Calendarextender1" runat="server" Format="dd-MMM-yyyy"
                                            PopupButtonID="imgdatefrom" TargetControlID="txtfilterefffrom">
                                        </asp:CalendarExtender>
                                        <img id="imgdatefrom" alt="Calender" src="../images/calendaricon.jpg" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblfiltereffto" Width="100%" runat="server" Text="Effective To Date"></asp:Label>
                                        <asp:TextBox ID="txtfiltereffto" runat="server" MaxLength="11"></asp:TextBox>
                                        <asp:CalendarExtender ID="Calendarextender2" runat="server" Format="dd-MMM-yyyy"
                                            PopupButtonID="imgdateto" TargetControlID="txtfiltereffto">
                                        </asp:CalendarExtender>
                                        <img id="imgdateto" alt="Calender" src="../images/calendaricon.jpg" />
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by TargetName"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1"
        AutoCompleteCompletionSetCount="10" AutoCompleteServiceMethod="GetSearchText" />

    <%--AutoCompleteServiceMethod="GetSearchText"--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc4:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphContents" runat="Server">
    <script language="javascript" type="text/javascript">

        function ValidateFormFields() {

<%--            if (!isSelected("<%=ddltarget.ClientID %>", "Target"))
                return false;

            if (!isSelected("<%=ddlproject.ClientID %>", "Project"))
                return false;

            if (!isSelected("<%=ddlCentre.ClientID %>", "Centre"))
                return false;

            if (!isSelected("<%=ddlCourse.ClientID %>", "Course"))
                return false;

            if (!isBlank("<%=txtvaluebudget.ClientID %>", "Value"))
                return false;

            if (!isNumber("<%=txtvaluebudget.ClientID %>", "Value"))
                return false;


            if (!isSelected("<%=ddlunit.ClientID %>", "Unit"))
                return false;

            if (!isBlank("<%=txteffrom.ClientID %>", "Effective From"))
                return false;--%>
        }



        function makeNumber(event) {
            event.value = event.value.replace(/[0-9\.]/g, '');
        }

    </script>
    <asp:MultiView ID="mltvTab" runat="server" ActiveViewIndex="0">
        <asp:View ID="List" runat="server">
            <div id="divGrid" class="table-container" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvMain" CssClass="my-grid" runat="server" EnableTheming="false"
                            DataKeyNames="ID" OnSorting="gvMain_Sorting"
                            OnRowDataBound="gvMain_RowDataBound" AutoGenerateColumns="False" Width="100%">

                            <EmptyDataTemplate>
                                <div style="color: #666; padding: 30px; text-align: center; border: 2px solid #1a364c; border-radius: 12px; margin: 20px 0; background: #f8f9fa; font-family: 'Segoe UI', sans-serif; font-weight: 600; font-size: 1.1rem; letter-spacing: 0.5px; transition: all 0.3s ease; box-shadow: 0 2px 8px rgba(0,0,0,0.1);">
                                    No Data Found
       
                                </div>
                            </EmptyDataTemplate>
                            <Columns>
                                <asp:BoundField HeaderText="#">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>

                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="targetName" HeaderText="Target Name" SortExpression="targetName"
                                    Target="_self">
                                    <HeaderStyle Width="20%" />
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:HyperLinkField>

                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="frequency" HeaderText="Frequency" SortExpression="Frequency"
                                    Target="_self">
                                    <HeaderStyle Width="8%" />
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:HyperLinkField>


                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="projectName" HeaderText="Project Name" SortExpression="projectName" Target="_self">
                                    <HeaderStyle Width="15%" />
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:HyperLinkField>

                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="CentreName" HeaderText="Centre name" SortExpression="CentreName"
                                    Target="_self">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:HyperLinkField>

                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="CourseName" HeaderText="Course Name" SortExpression="CourseName"
                                    Target="_self">
                                    <HeaderStyle Width="22%" />
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:HyperLinkField>

                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="Value" HeaderText="Value" SortExpression="value"
                                    Target="_self">
                                    <HeaderStyle Width="7%" />
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:HyperLinkField>

                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="unitname" HeaderText="Unit"
                                    Target="_self">
                                    <HeaderStyle Width="3%" />
                                    <ItemStyle HorizontalAlign="left" />
                                </asp:HyperLinkField>

                                <asp:BoundField
                                    DataField="efd"
                                    HeaderText="Effective From Date"
                                    SortExpression="QualificationLevel"
                                    DataFormatString="{0:dd-MM-yyyy}"
                                    HtmlEncode="False">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>

                                <asp:BoundField
                                    DataField="etd"
                                    HeaderText="Effective to Date"
                                    SortExpression="QualificationLevel"
                                    DataFormatString="{0:dd-MM-yyyy}"
                                    HtmlEncode="False"
                                    NullDisplayText="&lt;div style='text-align:center;width:100%;'&gt;-&lt;/div&gt;">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>

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
        </asp:View>
        <asp:View ID="New" runat="server">
            <asp:Label ID="lblerror" runat="server" EnableTheming="False" CssClass="error" Visible="False"
                Width="100%"></asp:Label>
             <asp:Label ID="lblstart" SkinID="CaptionLabel" runat="server"><span style='color:red'>*</span> fields are compulsory</asp:Label>
            <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblState" runat="server" SkinID="CaptionLabel" Text="Target ID &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" colspan="2">
                        <asp:Label ID="lblprojectid" runat="server" SkinID="CaptionLabel" Text="Project ID &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <%-- this doesnt affect any control --%>
  
                                <asp:DropDownList ID="ddltarget" Style="width: 100%" AutoPostBack="true" OnSelectedIndexChanged="ddltarget_SelectedIndexChanged" SkinID="ddl250" runat="server">
                                    <asp:ListItem runat="server" Text="--Select One--"></asp:ListItem>
                                </asp:DropDownList>


                    </td>
                    <td valign="top" colspan="2">

                                <%-- this affects centres --%>
                                <asp:DropDownList ID="ddlproject" Style="width: 100%" SkinID="ddl250" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlproject_SelectedIndexChanged">
                                    <asp:ListItem runat="server" Text="--Select One--"></asp:ListItem>
                                </asp:DropDownList>

                    </td>
                </tr>
                <%-- Next row --%>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblcentre" runat="server" SkinID="CaptionLabel" Text="Centre ID &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" colspan="2">
                        <asp:Label ID="lblcourse" runat="server" SkinID="CaptionLabel" Text="Course ID &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>

                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <%--centres affect course--%>
             
                                <asp:DropDownList ID="ddlCentre" Style="width: 100%" SkinID="ddl250" CssClass="ddlNormal" AutoPostBack="true" OnSelectedIndexChanged="ddlcentre_SelectedIndexChanged" runat="server">
                                    <asp:ListItem runat="server" Text="--Select One--"></asp:ListItem>
                                </asp:DropDownList>
                         
                   
                    </td>
                    <td valign="top" colspan="2">
               
                                <asp:DropDownList ID="ddlCourse" Style="width: 100%" SkinID="ddl250" CssClass="ddlNormal" runat="server">
                                    <asp:ListItem runat="server" Text="--Select One--"></asp:ListItem>
                                </asp:DropDownList>
                   

                    </td>
                </tr>
                <%-- Next row --%>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lblvalue" runat="server" SkinID="CaptionLabel" Text="Value &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top" colspan="2">
                        <asp:Label ID="lblunit" runat="server" SkinID="CaptionLabel" Text="Unit &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:TextBox ID="txtvaluebudget" Placeholder="Enter Value " onkeydown="checkNumber(this)" MaxLength="8" runat="server"></asp:TextBox>
                    </td>
                    <td valign="top" colspan="2">
                        <asp:DropDownList ID="ddlunit" Style="width: 100%" SkinID="ddl250" runat="server">
                            <asp:ListItem runat="server" Text="--Select One--"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <%-- Next row --%>
                <tr>
                    <td style="width: 33%;" valign="top">
                        <asp:Label ID="lbleffrom" runat="server" SkinID="CaptionLabel" Text="Effective From &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="lbleffto" runat="server" SkinID="CaptionLabel" Text="Effective To&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                <tr class="even">
                    <td style="width: 33%;" valign="top">

                        <asp:TextBox ID="txteffrom" runat="server" MaxLength="11"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender3" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgfrom" TargetControlID="txteffrom">
                        </asp:CalendarExtender>
                        <img id="imgfrom" alt="Calender" src="../images/calendaricon.jpg" />
                    </td>
                    <td valign="top">
                        <asp:TextBox ID="txteffto" runat="server" MaxLength="11"></asp:TextBox>
                        <asp:CalendarExtender ID="Calendarextender4" runat="server" Format="dd-MMM-yyyy"
                            PopupButtonID="imgto" TargetControlID="txteffto">
                        </asp:CalendarExtender>
                        <img id="imgto" alt="Calender" src="../images/calendaricon.jpg" />
                    </td>
                </tr>
                <%-- Next row 
                    <%-- Next row --%>
                <tr>
                    <td runat="server" id="fluploadlbl" visible="false" style="width: 33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="File Upload &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblfluploaded" Style="color: green" Visible="false" Text="PDF File is Uploaded"></asp:Label>

                    </td>
                </tr>
                <tr runat="server" id="flupload" visible="false" class="even">
                    <td style="width: 33%;" valign="top">
                        <asp:FileUpload ID="fileupload" runat="server"></asp:FileUpload>
                        <br />
                        <asp:Label ID="lblfltext" runat="server" Text="Upload PDF File"></asp:Label>
                    </td>
                    <%--<td></td>--%>
                </tr>
                <%-- Next row --%>
            </table>
            <div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </div>
        </asp:View>
    </asp:MultiView>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cthRightPannel" runat="Server">
</asp:Content>
