<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AffInstitute.aspx.cs" Inherits="Admin_AffInstitute" %>--%>

<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NonAffInstitute.aspx.cs" Inherits="Admin_NonAffInstitute" %>--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="NIELITProjectCourses.aspx.cs" Inherits="HO_NIELITProjectCourses"
    Debug="false" %>   

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/Address.ascx" TagName="Address" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Project Courses"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" Visible="false" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />
    <asp:Panel runat="server" ID="pnlFilter" Visible="True">
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
                                        <asp:Label ID="lblFiler" Width="60%" Visible="true" runat="server" Font-Bold="true" Font-Size="12pt"
                                            Text="Filter Panel"></asp:Label>
                                        <asp:Button runat="server" ID="btnReset" Visible="true" ToolTip="Reset Filter" ClientIDMode="Static"
                                            Text="" OnClick="ResetFilterPanel" />
                                        <asp:Button runat="server" ToolTip="Apply Filter" Visible="true" ID="btnFilter" ClientIDMode="Static"
                                            Text="" OnClick="AllyFilter" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblUserType" Width="100%" runat="server" Text="Project Courses"></asp:Label>
                                        <asp:DropDownList ID="ddlProjectFilter" Width="100%" runat="server" AutoPostBack="True">
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Project"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" Visible="true" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cphBreadScrum" runat="Server">
    <uc5:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="cphContents" runat="Server">
    
    <%--<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css" integrity="sha384-BVYiiSIFeK1dGmJRAkycuHAHRg32OmUcww7on3RYdg4Va+PmSTsz/K68vbdEjh4u" crossorigin="anonymous">
<link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap-theme.min.css" integrity="sha384-rHyoN1iRsVXV4nD0JutlnGaslCJuC7uwjduW9SVrLvRYooPp2bWYgmgJQIXwl/Sp" crossorigin="anonymous">
<link rel="stylesheet" href="bootstrap-multiselect.css" type="text/css"/>
 
<script   src="https://code.jquery.com/jquery-3.2.1.min.js"   integrity="sha256-hwg4gsxgFZhOsEEamdOYGBf13FyQuiTwlAQgxVSNgt4="   crossorigin="anonymous"></script>
<script src="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js" integrity="sha384-Tc5IQib027qvyjSMfHjOMaLkfuWVxZxUPnCJA7l2mCWNIpG9mGCD8wGNIcPD7Txa" crossorigin="anonymous"></script> 
<script type="text/javascript" src="bootstrap-multiselect.js"></script>--%>


    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.0/jquery.min.js"></script>
    <script type="text/javascript">
        function SearchEmployees(txtSearch, cbCourses) {
            if ($(txtSearch).val() != "") {
                var count = 0;
                $(cbCourses).children('tbody').children('tr').each(function () {
                    var match = false;
                    $(this).children('td').children('label').each(function () {
                        if ($(this).text().toUpperCase().indexOf($(txtSearch).val().toUpperCase()) > -1)
                            match = true;
                    });
                    if (match) {
                        $(this).show();
                        count++;
                    }
                    else { $(this).hide(); }
                });
                $('#spnCount').html((count) + ' match');
            }
            else {
                $(cbCourses).children('tbody').children('tr').each(function () {
                    $(this).show();
                });
                $('#spnCount').html('');
            }
        }


    </script>


    <script  type="text/javascript">
        $(function () {
            $('#multiselect').multiselect();
        });
    </script>

    <script type="text/javascript">
        $(function () {
            $('#multiselect').multiselect({
                includeSelectAllOption: true,
                enableFiltering: true,
                enableCaseInsensitiveFiltering: true,
                filterPlaceholder: 'Search for something...'
            });
        });
</script>

    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">


        function ValidateFormFields() {
            if (!isSelected("<%=ddlProjects.ClientID %>", "Projects"))
                return false;
            return true;
        }

        function CheckSelectedDept() {

        }
        var dtgp = "<%= gridProjectCourses.ClientID %>"
        function CheckAll(Sender, CheckBoxName) {
            CheckUncheckAll(dtgp, Sender, CheckBoxName)
        }
        function PerformAction(obj, tableid) {
            document.getElementById("<%=hfActionID.ClientID %>").value = obj.id.split("_")[1];
            ShowHideMenu(obj, tableid);
        }

        function Check_Click(objRef) {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            if (objRef.checked) {
                //If checked change color to Aqua
                row.style.backgroundColor = "aqua";
            }
            else {
                //If not checked change back to original color
                if (row.rowIndex % 2 == 0) {
                    //Alternating Row Color
                    row.style.backgroundColor = "#C2D69B";
                }
                else {
                    row.style.backgroundColor = "white";
                }
            }

            //Get the reference of GridView
            var GridView = row.parentNode;

            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");

            for (var i = 0; i < inputList.length; i++) {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];

                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
            //headerCheckBox.checked = checked;

        }
        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        row.style.backgroundColor = "aqua";
                        inputList[i].checked = true;
                    }
                    else {
                        //If the header checkbox is checked
                        //uncheck all checkboxes
                        //and change rowcolor back to original 
                        if (row.rowIndex % 2 == 0) {
                            //Alternating Row Color
                            row.style.backgroundColor = "#C2D69B";
                        }
                        else {
                            row.style.backgroundColor = "white";
                        }
                        inputList[i].checked = false;
                    }
                }
            }
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
                                        runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>

                        <div id="div1" runat="server">
                            <asp:UpdatePanel EnableViewState="true" ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="Label1" Visible="false"
                                        runat="server"></asp:Label>
                                    <table id="Table1" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                                        class="ActionPopup" style="width: 100px; height: 40px;">
                                        <tr>
                                            <td align="left" colspan ="2">
                                                <asp:LinkButton ID="LinkButton1" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
                                                    runat="server" Text="Delete" ToolTip="click to delete this record" SkinID="lnkbtnAction"></asp:LinkButton>
                                               
                                            </td>
                                        </tr>
                                       
                                    </table>
                                    <table class="sample2" cellpadding="2" cellspacing="0" width="100%">
                                         <tr>
                                            <td style="width: 50%;" valign="top">
                                                <asp:Label ID="lblProjects" runat="server" SkinID="CaptionLabel" Text="Projects&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>

                                            </td>
                                            <td style="width: 50%;" valign="top">
                                                 <asp:Label ID="lblCourse" runat="server" SkinID="CaptionLabel" Text="Course&lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                            </td>
                                          </tr>
                                            <tr class="even">
                                                <td style="width: 33%;" valign="top" id="ddlDegree">
                                                    <asp:DropDownList style="width: 370px;" ID="ddlProjects" runat="server"  AutoPostBack="True"  OnSelectedIndexChanged="ddlProjects_SelectedIndexChanged"  SkinID="ddl251">
                                                    </asp:DropDownList>
                                                </td>
                                            <td style="width: 33%;" valign="top">

                                            <div style=" width: 100%" >

                                                <asp:TextBox ID="TextBox1" runat="server" style="width: 370px;" SkinID="ddl250" onkeyup="SearchEmployees(this,'#cbCourses');"
                                                    placeholder="Search courses">
                                                </asp:TextBox>
                                                <span id="spnCount"></span>
                                                <div style="height: 70px; overflow-y: auto; overflow-x: hidden">
                                                    <asp:CheckBox ID="allChkBox" Text="Select all" AutoPostBack="True" oncheckedchanged="allChkBox_CheckedChanged" runat="server" visible="false"  />
                                                    <asp:CheckBoxList ID="cbCourses" runat="server" visible="false" RepeatColumns="1"
                                                        RepeatDirection="Vertical" Width="360px" ClientIDMode="Static" >
                                                    
                                                    </asp:CheckBoxList>
                                                    
                                                    
                                            <%--<asp:ListBox ID="lstFruits" runat="server"   SelectionMode="Multiple"> </asp:ListBox>--%>
                                             
                                                    <%--<select id="multiselect" runat="server"  meta:multiple="multiple" name="property_typeid" class="w290">
                                                    </select>--%>

                                                </div>
                                                </div>
                                                </td>  
                                             </tr>
                                         <tr >
                    
                    <td style="width: 33%;" valign="top" colspan="3">
                        <asp:Label ID="lblIsActive" runat="server"  SkinID="CaptionLabel" Text="Is Active &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                </tr>
                                         <tr class="even">
                    
                    <td style="width: 33%;" valign="top" colspan="3">
                         <asp:DropDownList ID="ddlActive" runat="server" SkinID="ddl250" >
                            <asp:ListItem Value="-1" Text="--Select One--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                            <asp:ListItem Value="0" Text="No"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <%--<div id="divNavigation" runat="server">
                            <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                                runat="server">
                                <ContentTemplate>
                                    <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>--%>

                        <div style="text-align: right; margin-top: 10px">
                            <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                                Text="Save" OnClick="SaveRecord" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
                        </div>

                        

                        <div>

                           <asp:GridView ID="gridProjectCourses" runat="server" DataKeyNames="ID" OnSorting="gridProjectCourses_Sorting"
                            OnRowDataBound="gridProjectCourses_RowDataBound" AutoGenerateColumns="False">
                            <Columns>
                                <asp:BoundField HeaderStyle-Width="2%" HeaderText="#">
                                    <HeaderStyle Width="2%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="ProjectName" HeaderText="Project Name" SortExpression="ProjectName"
                                    Target="_self" />
                                <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="CourseName" HeaderText="Course Name" SortExpression="CourseName"
                                    Target="_self" />
                                  <asp:HyperLinkField DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="IsActive" HeaderText="IsActive" SortExpression="IsActive"
                                    Target="_self" />
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif" Visible="False">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" /></ItemTemplate>
                                    <HeaderStyle Width="3%" />
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

                            <%--<asp:GridView ID="gridProjectCourses" runat="server"
                                AutoGenerateColumns="false" DataKeyNames="ID" Font-Names="Arial"
                                Font-Size="11pt"
                                AllowPaging="true" OnRowDataBound="RowDataBound"
                                PageSize="10">
                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label runat="server" Visible="false" ID="lblID" Text='<%# Eval("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="60%" DataField="ProjectName" HeaderText="Project Name" />
                                    <asp:BoundField ItemStyle-Width="20%" DataField="CourseName" HeaderText="Course Name" />
                                </Columns>
                            </asp:GridView>--%>
                        </div>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfAccID" runat="server" />
                        <asp:HiddenField ID="hfName" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div id="div2" runat="server">
        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                         runat="server">
            <ContentTemplate>
                <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

            </div>


            

        </asp:View>
        <asp:View ID="New" runat="server">

            <%--<div id="div1" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="Label1" Visible="false"
                            runat="server"></asp:Label>
                        <table id="Table1" clientidmode="Static" runat="server" cellpadding="2" cellspacing="0"
                            class="ActionPopup" style="width: 100px; height: 40px;">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="LinkButton1" OnClientClick="return ConfirmAction('Are you sure you want to delete this record!');"
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
                                        <asp:CheckBox runat="server" ID="chkInstitutes" SkinID="CheckAllInGridView" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderStyle-Width="3%" Visible="false" HeaderText="" HeaderImageUrl="~/images/fleche_down_sel.gif">
                                    <ItemTemplate>
                                        <asp:Image ClientIDMode="Static" ToolTip="Action" onclick="PerformAction(this,'popup')"
                                            runat="server" ID="imgAction" ImageUrl="~/images/fleche_down_sel.gif" ImageAlign="Middle"
                                            Style="cursor: pointer; border: 1px solid transparent;" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="3%" HeaderText="" Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chk" SkinID="CheckAllInGridView" />
                                    </ItemTemplate>
                                    <HeaderStyle Width="3%" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Visible="False" />
                        </asp:GridView>
                        <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                        <asp:HiddenField ID="HiddenField2" runat="server" />
                        <asp:HiddenField ID="HiddenField3" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>--%>
            <%--<div style="text-align: right; margin-top: 10px">
                <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                    Text="Save" OnClick="SaveRecord" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" />
            </div>--%>
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
                <asp:HyperLink ID="hl1" runat="server" Target="_self">Project Courses Details</asp:HyperLink>
            </td>
        </tr>
    </table>
</asp:Content>
