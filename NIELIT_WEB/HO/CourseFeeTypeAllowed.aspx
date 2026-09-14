<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AffInstitute.aspx.cs" Inherits="Admin_AffInstitute" %>--%>

<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NonAffInstitute.aspx.cs" Inherits="Admin_NonAffInstitute" %>--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/main.master" AutoEventWireup="true"
    CodeFile="CourseFeeTypeAllowed.aspx.cs" Inherits="HO_CourseFeeTypeAllowed"
    Debug="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="../UserControl/SearchBar.ascx" TagName="SearchBar" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ToggleView.ascx" TagName="ToggleView" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/PagingBar.ascx" TagName="PagingBar" TagPrefix="uc3" %>
<%@ Register Src="../UserControl/Address.ascx" TagName="Address" TagPrefix="uc4" %>
<%@ Register Src="../UserControl/BreadCrumb.ascx" TagName="BreadCrumb" TagPrefix="uc5" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .auto-style1 {
            width: 33%;
            height: 28px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Course Fee Type Allowed"></asp:Label>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cphAddNew" runat="Server">
    <uc2:ToggleView ID="btnMode" Visible="true" OnBtnMode_Click="ToggleViewMode_Changed" runat="server" />
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
                                        <asp:Label ID="lblUserType" Width="100%" runat="server" Text="Courses"></asp:Label>
                                        <asp:DropDownList ID="ddlCourseFilter" Width="100%" runat="server" AutoPostBack="True">
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Course"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" Visible="True" AutoCompleteFirstRowSelected="True"
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

           
                        <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
                            runat="server"></asp:Label>
    <asp:Panel id="pnlNew" runat="server" Visible="false">
                       <table width ="100%" border ="1px">
                                         <tr>
                                             <td valign="top" class="auto-style1">
                        <asp:Label ID="Label3" runat="server" SkinID="CaptionLabel" Text="Course Category &lt;b class='mandatory'&gt;*&lt;/b&gt;"
                            Width="100%"></asp:Label>
                    </td>
                    
                                            <td valign="top" class="auto-style1">
                        <%-- <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>--%>
                        <asp:DropDownList ID="ddlcoursecategory" runat="server" AutoPostBack="True"
                            OnSelectedIndexChanged="ddlcoursecategory_SelectedIndexChanged">
                            <asp:ListItem>--Select One--</asp:ListItem>
                        </asp:DropDownList>
                        <%--</ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label4" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                    </td>
                                            <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlcourse" runat="server"  AutoPostBack="True" 
                                     OnSelectedIndexChanged="ddlcourse_SelectedIndexChanged">
                                      <asp:ListItem Value="--Select One--"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="ddlcoursecategory" EventName="SelectedIndexChanged" />
                               
                            </Triggers>
                        </asp:UpdatePanel>
                    </td>
                                        </tr>
                           
                                    </table>
        </asp:Panel> 
    <asp:Panel id="pnlEdit" runat="server" Visible="false">
        <table width="100%" border="1px">
            <tr>
                                <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Fee Type &lt;b class='mandatory'&gt;*&lt;/b&gt;" enabled="false"></asp:Label>
                    </td>
                                            <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <asp:DropDownList ID="ddlFeeType" runat="server" Enabled="False" >
                                      <asp:ListItem Value="--Select One--"></asp:ListItem>
                                </asp:DropDownList>
                            </ContentTemplate>
                            
                        </asp:UpdatePanel>
                    </td>
                </tr><tr>
                 <td style="width: 33%;" valign="top">
                        <asp:Label ID="Label2" runat="server" SkinID="CaptionLabel" Text="Is Active &lt;b class='mandatory'&gt;*&lt;/b&gt;" enabled="false"></asp:Label>
                    </td>
                                            <td style="width: 33%;" valign="top">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                            <ContentTemplate>
                                 <asp:DropDownList ID="ddlIsActive" runat="server" SkinID="ddl250" >
                            <asp:ListItem Selected="True">---Select One---</asp:ListItem>
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="2" >No</asp:ListItem>
                        </asp:DropDownList>  
                            </ContentTemplate>
                            
                        </asp:UpdatePanel>
                    </td>
            </tr>
        </table>
    </asp:Panel>
    <br />
     <div id="divGrid" runat="server">
                <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        
                                   
                                    
                                    <div>
                                        <asp:GridView ID="gvMain" runat="server"
                                            AutoGenerateColumns="false" DataKeyNames="ID" Font-Names="Arial"
                                            Font-Size="11pt" caption="<b>Select Fee Types for Course</b>"
                                            AllowPaging="true" OnRowDataBound="RowDataBound" Visible="false"
                                            PageSize="10" Width="217px">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkAll" runat="server" onclick="checkAll(this);" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        <asp:CheckBox ID="chkCourse" runat="server" onclick="Check_Click(this)" TextAlign="Left"  />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">

                                                    <ItemTemplate>
                                                        <asp:Label runat="server" Visible="true" ID="lblID" Text='<%# Eval("ID") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <%--<asp:BoundField ItemStyle-Width="5%" Visible="true" DataField="ID" HeaderText="ID" />--%>
                                                <asp:BoundField ItemStyle-Width="60%" DataField="feeType" HeaderText="Allowed Fee Type" >
                                                    <ItemStyle Width="60%" />
                                                </asp:BoundField>
                                               </Columns>

                                        </asp:GridView>
                                    </div>
                                    <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                                    <asp:HiddenField ID="HiddenField2" runat="server" />
                                    <asp:HiddenField ID="HiddenField3" runat="server" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="divNavigation" runat="server">
                            <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
                                runat="server">
                                <ContentTemplate>
                                    <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" Visible="False" />

                                

                        <div style="text-align: right; margin-top: 10px">
                            <asp:Button ID="btnSave" OnClientClick="return ValidateFormFields();" runat="server"
                                Text="Save" OnClick="SaveRecord"  />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click"  />
                        </div>
    </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        
                    <div id="div1" runat="server">
                            <asp:UpdatePanel EnableViewState="true" ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                        <div>
                            <asp:GridView ID="gridCourseFeeType" runat="server"
                                AutoGenerateColumns="false" DataKeyNames="ID" Font-Names="Arial" ScrollBars="Both"
                                Font-Size="11pt" Caption="<b>Coursewise Fee Type</b>"
                                AllowPaging="true" OnRowDataBound="RowDataBound"
                                PageSize="10">
                                <Columns>
                                    <%--<asp:BoundField HeaderStyle-Width="5%" HeaderText="#">
                                        <HeaderStyle Width="5%" />
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>--%>

                                    <asp:TemplateField Visible="false">

                                        <ItemTemplate>
                                            <asp:Label runat="server" Visible="false" ID="lblID" Text='<%# Eval("ID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                     <asp:HyperLinkField HeaderStyle-Width="22%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="course" HeaderText="Course name" SortExpression="course"
                                    Target="_self" >
                                    <HeaderStyle Width="22%" />
                                </asp:HyperLinkField>
                                    <asp:HyperLinkField HeaderStyle-Width="22%" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="?Key={0}"
                                    DataTextField="feeType" HeaderText="Fee type" SortExpression="feeType"
                                    Target="_self" >
                                    <HeaderStyle Width="22%" />
                                </asp:HyperLinkField>
                                    <asp:BoundField ItemStyle-Width="5%" Visible="true" DataField="isActive" HeaderText="Active" />
                                    <%--<asp:BoundField ItemStyle-Width="5%" Visible="true" DataField="ID" HeaderText="ID" />
                                    <asp:BoundField ItemStyle-Width="40%" DataField="course" HeaderText="Course Name" />--%>
                                    <%--<asp:BoundField ItemStyle-Width="20%" DataField="Location" HeaderText="Location" />
                                    <asp:BoundField ItemStyle-Width="40%" DataField="feeType" HeaderText="Fee type" />--%>

                                </Columns>

                            </asp:GridView>
                        </div>
                        <asp:HiddenField ID="hfActionID" runat="server" Value="" />
                        <asp:HiddenField ID="hfAccID" runat="server" />
                        <asp:HiddenField ID="hfName" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>


            <div id="divNavigation2" runat="server">
                <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation2" UpdateMode="Conditional"
                    runat="server">
                    <ContentTemplate>
                        <uc3:PagingBar ID="PagingBar2" runat="server" OnPageIndexChanged="PageIndexChanged2" Visible="True" />

                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="cphNavigation" runat="Server">
</asp:Content>

