<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPages/main.master" CodeFile="SemesterFeeMaster.aspx.cs" Inherits="HO_SemesterFeeMaster" EnableEventValidation="false" %>

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
<asp:content id="Content3" contentplaceholderid="chpHeading" runat="Server">
    <asp:Label ID="lblHeading" runat="server" Text="Semester Fee Master"></asp:Label>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.6/jquery.min.js" type="text/javascript"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js" type="text/javascript"></script>
    <link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="Stylesheet" type="text/css" />
</asp:content>
<asp:content id="Content4" contentplaceholderid="cphAddNew" runat="Server">
 
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
                                        <asp:Label ID="Label2" Width="100%" runat="server" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                        <asp:DropDownList ID="ddlCourseName" Width="100%" runat="server"
                                            AutoPostBack="true" OnSelectedIndexChanged="ddlCourseName_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label5" Width="100%" runat="server" Text="Batch Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                        <asp:DropDownList ID="ddlbatchname" Width="100%" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlbatchname_SelectedIndexChanged">
                                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label4" Width="100%" runat="server" Text="Semester &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                                        <asp:DropDownList ID="ddlsem" Width="100%" runat="server">
                                          <%--  <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>--%>
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
    <uc1:SearchBar ID="ucSearchBar" OnLnkBtnResetSearch="SearchBar_Reset" SearchTextToolTip="Search by Batch Code/Batch Name"
        OnLnkBtnGO="SearchBar_ApplySearch" runat="server" Visible="True" AutoCompleteFirstRowSelected="True"
        AutoCompleteMinimumPrefixLength="1" AutoCompleteServiceMethod="GetSearchText"
        AutoCompleteCompletionSetCount="10" />
</asp:content>
<asp:content id="Content5" contentplaceholderid="cphBreadScrum" runat="Server">
    <uc5:BreadCrumb ID="BreadCrumb1" runat="server" />
</asp:content>
<asp:content id="Content6" contentplaceholderid="cphContents" runat="Server">
    <script src="../Script/GlobalFunction.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function ValidateFormFields() {
            if (!isSelected("<%=ddlCourse.ClientID  %>", "Course Name"))
                return false;
            if (!isSelected("<%=ddlbatchSession.ClientID  %>", "Batch Name"))
                return false;

            if (!isSelected("<%=ddlsemester.ClientID  %>", "Semester"))
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

        function Validate() {
            //Reference the GridView.
            var grid = document.getElementById("<%=gvMain.ClientID %>");

            //Reference all INPUT elements.
            var inputs = grid.getElementsByTagName("INPUT");

            //Set the Validation Flag to True.
            var isValid = true;
            for (var i = 0; i < inputs.length; i++) {
                //If TextBox.
                if (inputs[i].type == "text") {
                    //Reference the Error Label.
                    var label = inputs[i].parentNode.getElementsByTagName("SPAN")[0];

                    //If Blank, display Error Label.
                    if (inputs[i].value == "") {
                        label.style.display = "block";
                        isValid = false;
                    } else {
                        label.style.display = "none";
                    }
                }
            }

            return isValid;
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

                var headerCheckBox = inputList[0];
                var checked = true;
                if (inputList[i].type == "checkbox" && inputList[i] != headerCheckBox) {
                    if (!inputList[i].checked) {
                        checked = false;
                        break;
                    }
                }
            }
        }

        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {

                        row.style.backgroundColor = "aqua";
                        inputList[i].checked = true;
                    }
                    else {

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


 
    <asp:Panel ID="pnlNew" runat="server">
         <asp:Label Width="99%" EnableTheming="false" CssClass="error" ID="lblError" Visible="false"
        runat="server"></asp:Label>
         <table class="sample2" cellpadding="2" cellspacing="0" width="100%" id="trpage" runat="server">

            <tr>
            <td colspan="2" style="width: 66%;" valign="top">
                <asp:Label ID="Label6" runat="server" SkinID="CaptionLabel" Text="Institutes"
                    Width="100%"></asp:Label>
            </td>

            <td style="width: 33%;" valign="top">&nbsp;</td>
        </tr>
        <tr class="even">
            <td colspan="2" style="width: 66%;" valign="top">
                <asp:TextBox Style="width: 501px;" ID="txtInstitute" runat="server" Enabled="false" SkinID="txt248" Width="100%" ToolTip="Institute"></asp:TextBox>
            </td>
            <td style="width: 33%;" valign="top"></td>
        </tr>
        <tr id="trinstitute" runat="server" visible="false">
            <td style="width: 33%;" valign="top">
                <asp:Label ID="Label7" runat="server" SkinID="CaptionLabel" Text="Choose  Institute  &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
            </td>
            <td style="width: 33%;" valign="top" colspan="2">
                <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                    <ContentTemplate>
                        <asp:RadioButtonList ID="RdoAffInstOrNonAffInst" runat="server" RepeatDirection="Horizontal"
                            TabIndex="2" Width="412px" AutoPostBack="True"  OnSelectedIndexChanged="RdoAffInstOrNonAffInst_SelectedIndexChanged"
                            Style="height: 27px" Font-Bold="True">
                            <asp:ListItem Value="1">Accredited Centre</asp:ListItem>
                            <asp:ListItem Value="0">Non Accredited Centre</asp:ListItem>
                            <asp:ListItem Value="2" Selected="True">Nielit Centre</asp:ListItem>
                        </asp:RadioButtonList>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="RdoAffInstOrNonAffInst" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </td>
            </tr>
           
            <tr>
                <td valign="top">
                    <asp:Label ID="Label12" runat="server" SkinID="CaptionLabel" Text="Course Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="Label13" runat="server" SkinID="CaptionLabel" Text="Batch Name &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                </td>
                <td style="width: 33%;" valign="top">
                    <asp:Label ID="Label1" runat="server" SkinID="CaptionLabel" Text="Semester &lt;b class='mandatory'&gt;*&lt;/b&gt;"></asp:Label>
                </td>
            </tr>
            <tr class="even">
                <td valign="top" class="style1">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlCourse" runat="server" AutoPostBack="True" Width="200px" SkinID="ddl760" OnSelectedIndexChanged="ddlCourse_SelectedIndexChanged">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                <td class="style1">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlbatchSession" runat="server" SkinID="ddl250" AutoPostBack="true" OnSelectedIndexChanged="ddlbatchSession_SelectedIndexChanged">
                                <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                </td>
                <td valign="top" style="width: 33%;">
                    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                        <ContentTemplate>
                            <asp:DropDownList ID="ddlsemester" runat="server" Width="200px" TabIndex="1" AutoPostBack="True" OnSelectedIndexChanged="ddlsemester_SelectedIndexChanged">
                                 <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lblsemerror" runat="server" ForeColor="Red" Font-Size="Small" Font-Italic="true" Width="100%"></asp:Label>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
            </tr>

        </table>
        <div style="text-align: right; margin-top: 10px">
            <asp:Button ID="btnShowRecord" runat="server" Text="Show" OnClick="ShowRecord" OnClientClick="return ValidateFormFields();" />
              <asp:Button ID="btnBack" runat="server" Text="Back" OnClick="btnBack_Click" />
        </div>

        <div id="msg">
            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                    <asp:HiddenField ID="hcentreID" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </asp:Panel>
    <br />
  
    <div id="divGrid" runat="server" visible="false">

        <asp:UpdatePanel EnableViewState="true" ID="uPnlGrid" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                                          
                <asp:Label ID="lblsemsterfeelist"  runat="server" forecolor="black" Font-Bold="true"
                      Text="Semester Fee Type List" Visible="false"></asp:Label>
                <asp:GridView ID="gvMain" runat="server" DataKeyNames="ID" OnSorting="gvMain_Sorting"
                    AutoGenerateColumns="False" Width="600px" ShowHeader="true" >
                    <RowStyle Height="40px" />
                    <Columns>
                        
                        <asp:HyperLinkField HeaderStyle-Width="16%"  DataNavigateUrlFields="ID"
                            DataTextField="feetype" HeaderText="Fee Type" SortExpression="Fee" Target="_self">
                            <HeaderStyle Width="16%" />
                        </asp:HyperLinkField>

                        <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="110px">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("Amount") %>' Width="110px" ></asp:Label>

                                <asp:TextBox ID="txtAmount" runat="server" Text='<%# Eval("Amount") %>'
                                    Width="70px" Visible="false" MaxLength="7"></asp:TextBox>
                                <asp:Label ID="lblamount" runat="server" ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Effective From" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px">
                            <ItemTemplate>
                                <asp:Label ID="lbleffectivefrom" runat="server" Text='<%# String.Format("{0:dd-MMM-yyyy}", Eval("effectivefrom")) %>' Width="150px"></asp:Label>
                                <asp:TextBox ID="txteffectivefrom" runat="server" Text='<%#  Eval("effectivefrom","{0:dd-MMM-yyyy}")  %>'
                                    SkinID="txtDate" Width="100px" Visible="false" MaxLength="11"></asp:TextBox>
                                
                                <img id="img3" runat="server" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" visible="false" />
                                <asp:CalendarExtender ID="CalendarExtender3" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txteffectivefrom">
                                </asp:CalendarExtender>
                                
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Effective To" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px"><ItemTemplate>
                                <asp:Label ID="lbleffectiveto" runat="server" Text='<%# String.Format("{0:dd-MMM-yyyy}", Eval("effectiveto")) %>' Width="150px"></asp:Label>
                                <asp:TextBox ID="txteffectiveto" runat="server" Text='<%#  Eval("effectiveto","{0:dd-MMM-yyyy}")  %>'
                                    SkinID="txtDate" Width="100px" Visible="false" MaxLength="11"></asp:TextBox>
                                <img id="img4" runat="server" src="../images/calendaricon.jpg" style="width: 20px; height: 22px; vertical-align: top;" visible="false" />
                                <asp:CalendarExtender ID="CalendarExtender4" runat="server" Format="dd-MMM-yyyy" PopupButtonID="imgEFrm" PopupPosition="BottomLeft" TargetControlID="txteffectiveto">
                                </asp:CalendarExtender>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField ItemStyle-Width="20px">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" OnCheckedChanged="OnCheckedChanged" />
                               
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false" HeaderText="lblIdVisFalse">
                            <ItemTemplate>
                                <asp:Label runat="server" Visible="true" ID="lblID" Text='<%# Eval("ID") %>'></asp:Label>
                                 <asp:Label ID="lblfeetypeid" runat="server" Visible="true" Text='<%# Eval("ID")%>'></asp:Label>

                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <PagerSettings Visible="False" />
                </asp:GridView>
                <asp:HiddenField ID="HiddenField1" runat="server" Value="" />
                <asp:HiddenField ID="hfcode" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <di0v id="divNavigation" runat="server">
        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation" UpdateMode="Conditional"
            runat="server">
            <ContentTemplate>
                <uc3:PagingBar ID="PagingBar1" runat="server" OnPageIndexChanged="PageIndexChanged" Visible="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </di0v>
    <div style="text-align: right; margin-top: 10px">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>

                <asp:Button ID="btnSave" runat="server" OnClientClick="return Validate();"
                    Text="Save" OnClick="SaveRecord" Visible="false" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Visible="false" OnClick="btnCancel_Click" />
              
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
                <asp:HiddenField ID="NIELITCentreId" runat="server" />
                <asp:HiddenField ID="HNANFL" runat="server" />
                <asp:HiddenField ID="HNonAfflAfflInst" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="div1" runat="server">
        <asp:UpdatePanel EnableViewState="true" ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <div>

                    <asp:GridView ID="gridSemesterFeeType" runat="server" OnSorting="gridSemesterFeeType_Sorting"
                            OnRowDataBound="gridSemesterFeeType_RowDataBound"
                        AutoGenerateColumns="false"  Font-Names="Arial"
                        Font-Size="11pt"
                        AllowPaging="true"
                        PageSize="10">
                        <Columns>
                           <asp:BoundField HeaderStyle-Width="5%" HeaderText="Sr No.">
                                    <HeaderStyle Width="5%" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                            <%--<asp:TemplateField HeaderText="Sr" ItemStyle-HorizontalAlign="Center" >
                            <ItemTemplate>
                                <asp:Label ID="lblsr" runat="server" Text='<%# Bind("Id") %>' ></asp:Label>                           
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                            <asp:HyperLinkField HeaderStyle-Width="10%" 
                                DataTextField="CourseName" HeaderText="Course Name" SortExpression="Course" Target="_self">
                                <HeaderStyle Width="10%" />
                            </asp:HyperLinkField>

                            <asp:HyperLinkField HeaderStyle-Width="10%" 
                                DataTextField="BatchName" HeaderText="Batch Name" SortExpression="Batch" Target="_self">
                                <HeaderStyle Width="10%" />
                            </asp:HyperLinkField>

                            <asp:HyperLinkField HeaderStyle-Width="5%" 
                                DataTextField="SemId" HeaderText="Semester" SortExpression="Sem" Target="_self">
                                <HeaderStyle Width="10%" />
                            </asp:HyperLinkField>

                             <asp:HyperLinkField HeaderStyle-Width="10%" 
                            DataTextField="feeType" HeaderText="Fee Type" SortExpression="Fee" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>
                         <asp:HyperLinkField HeaderStyle-Width="10%" 
                            DataTextField="feeAmount" HeaderText="Amount" SortExpression="Amount" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>

                         <asp:HyperLinkField HeaderStyle-Width="10%" 
                            DataTextField="effectiveFromDate" HeaderText="Effective Date" SortExpression="effrom" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>  

                            <asp:HyperLinkField HeaderStyle-Width="10%" 
                            DataTextField="effectiveToDate" HeaderText="Effective To" SortExpression="efto" Target="_self">
                            <HeaderStyle Width="15%" />
                        </asp:HyperLinkField>  
                       
                        </Columns>

                    </asp:GridView>
 <uc3:PagingBar ID="PagingBar2" runat="server" Visible="false" OnPageIndexChanged="PageIndexChangedOld" />
                </div>
                <asp:HiddenField ID="hfActionID" runat="server" Value="" />              
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


 <%--   <div id="divNavigation2" runat="server"  visible="false">
        <asp:UpdatePanel RenderMode="Inline" ID="uPnlNavigation2" UpdateMode="Conditional"
            runat="server">
            <ContentTemplate>
                <uc3:PagingBar ID="PagingBar2" runat="server"  />

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>--%>

</asp:content>
<asp:content id="Content7" contentplaceholderid="cphNavigation" runat="Server">
</asp:content>


